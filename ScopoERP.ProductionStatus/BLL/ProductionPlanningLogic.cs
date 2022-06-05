using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using ScopoERP.Production.ViewModel;
using ScopoERP.ProductionStatus.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.ProductionStatus.BLL
{
    public class ProductionPlanningLogic
    {
        private UnitOfWork unitOfWork;
        private productionplanning productionPlanning;

        public ProductionPlanningLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        
        public List<ProductionPlanViewModel> GetAllProductionPlanByDate(DateTime startDate, DateTime endDate)
        {
            var results = (from pp in unitOfWork.ProductionPlanningRepository.Get()
                           join po in unitOfWork.PurchaseOrderRepository.Get()
                           on pp.PoStyleID equals po.PoStyleId
                           join s in unitOfWork.StyleRepository.Get()
                           on po.StyleId equals s.StyleId
                           where (pp.StartDate >= startDate && pp.StartDate <= endDate)
                           || (pp.EndDate >= startDate && pp.EndDate <= endDate)
                           select new ProductionPlanViewModel
                           {
                               ProductionPlanningID = pp.PoductionPlanningID,
                               PoStyleID = pp.PoStyleID,
                               PurchaseOrderNo = po.PoNo,
                               StartDate = pp.StartDate,
                               EndDate = pp.EndDate,
                               FloorLineID = pp.FloorLineID,
                               Quantity = pp.Quantity,
                               Capacity = pp.Capacity,
                               StyleID = s.StyleId,
                               StyleNo = s.StyleNo,
                               ExitDate = po.ExitDate
                           }).ToList();
            return results;
        }
        
        public string GetStyleCapacityOrderQty(int purchaseOrderID)
        {
            var result = (from a in unitOfWork.PurchaseOrderRepository.Get()
                         join b in unitOfWork.StyleRepository.Get()
                         on a.StyleId equals b.StyleId
                         where a.PoStyleId == purchaseOrderID
                         select new
                         {
                             StyleCapacity = b.Capacity,
                             OrderQuantity = a.OrderQuantity
                         }).SingleOrDefault();

            return result.StyleCapacity.ToString()+","+result.OrderQuantity.ToString();
        }
        
        public Boolean CheckValidPlan(int productionPlanningID, int purchaseOrderID, DateTime startDate, int floorLineID)
        {
            var result = (from a in unitOfWork.ProductionPlanningRepository.Get()
                         where a.StartDate < startDate && a.EndDate >= startDate && a.FloorLineID == floorLineID
                         && a.PoductionPlanningID != productionPlanningID
                         select a.PoductionPlanningID).ToList();

            if (result.Count > 0)
                return false;
            return true;
        }
        
        public void CreateProductionPlan(int purchaseOrderID, DateTime startDate, int floorLineID, int lineQuantity, int lineCapacity)
        {
            this.productionPlanning = new productionplanning()
            {
                PoStyleID = purchaseOrderID,
                StartDate = startDate,
                EndDate = GetEndDate(lineCapacity, lineQuantity, startDate),
                FloorLineID = floorLineID,
                Quantity = lineQuantity,
                Capacity = lineCapacity
            };

            unitOfWork.ProductionPlanningRepository.Insert(this.productionPlanning);
            unitOfWork.Save();

            ReschedulePlan(this.productionPlanning.PoductionPlanningID, startDate, this.productionPlanning.EndDate, floorLineID, 0);
        }
        
        public void ReschedulePlan(int productionPlanningID, DateTime startDate, DateTime endDate, int floorLineID, int isResize)
        {
            var changedProductionPlan = (from a in unitOfWork.ProductionPlanningRepository.Get()
                                        where a.PoductionPlanningID == productionPlanningID
                                        select a).SingleOrDefault();

            DateTime beforeStartDate = changedProductionPlan.StartDate;
            DateTime beforeEndDate = changedProductionPlan.EndDate;

            changedProductionPlan.StartDate = startDate;
            changedProductionPlan.EndDate = endDate;
            changedProductionPlan.FloorLineID = floorLineID;

            unitOfWork.ProductionPlanningRepository.Update(changedProductionPlan);
            
            var isIntersactAnyPurchaseOrder = (from a in unitOfWork.ProductionPlanningRepository.Get()
                                               where a.StartDate >= startDate && a.StartDate <= endDate
                                               && a.PoductionPlanningID != productionPlanningID
                                               && a.FloorLineID == floorLineID
                                               select a).ToList();
            if (isIntersactAnyPurchaseOrder.Count > 0)
            {
                double diffIntervalDays;
                if (isResize == 1)
                {
                    double beforeIntervalDays = (beforeEndDate - beforeStartDate).TotalDays;
                    double changedIntervalDays = (endDate - startDate).TotalDays;
                    diffIntervalDays = changedIntervalDays - beforeIntervalDays;
                }
                else
                    diffIntervalDays = (endDate - startDate).TotalDays + 1;

                if (diffIntervalDays > 0)
                {
                    var impactedProductionPlan = (from a in unitOfWork.ProductionPlanningRepository.Get()
                                                  where a.StartDate >= startDate
                                                  && a.PoductionPlanningID != productionPlanningID
                                                  && a.FloorLineID == floorLineID
                                                  select a).ToList();

                    foreach (var i in impactedProductionPlan)
                    {
                        i.StartDate = i.StartDate.AddDays(diffIntervalDays);
                        i.EndDate = i.EndDate.AddDays(diffIntervalDays);

                        unitOfWork.ProductionPlanningRepository.Update(i);
                    }

                }
            }
            unitOfWork.Save();
        }
        
        public Boolean DeleteProductionPlan(int ProductionPlanningID)
        {
            unitOfWork.ProductionPlanningRepository.RawQuery("DELETE FROM productionplanning WHERE PoductionPlanningID = " + ProductionPlanningID);
            return true;
        }
        
        public Boolean SplitProductionPlan(int productionPlanningID)
        {
            productionplanning productionPlanning = (from c in unitOfWork.ProductionPlanningRepository.Get()
                         where c.PoductionPlanningID == productionPlanningID
                         select c).SingleOrDefault();

            productionPlanning.Quantity /= 2;

            unitOfWork.ProductionPlanningRepository.Insert(productionPlanning);
            unitOfWork.Save();

            ReschedulePlan(productionPlanning.PoductionPlanningID, productionPlanning.StartDate, productionPlanning.EndDate, productionPlanning.FloorLineID, 0);

            return true;
        }
        
        public DateTime GetEndDate(int lineCapacity, int lineQuantity, DateTime startDate)
        {
            var requiredDays = (lineQuantity / lineCapacity) + ((lineQuantity % lineCapacity) == 0 ? 0 : 1);
            DateTime endDate = startDate.AddDays(requiredDays - 1);
            TimeSpan diff = endDate - startDate;
            int days = diff.Days;
            for (var i = 0; i <= days; i++)
            {
                var everyday = startDate.AddDays(i);
                switch (everyday.DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        endDate = endDate.AddDays(1);
                        break;
                }
            }
            return endDate;
        }
        
        public List<RawMaterialStatusViewModel> GetRawMaterialStatus(int purchaseOrderID)
        {
            var result = from a in unitOfWork.WorksheetRepository.Get()
                         join b in unitOfWork.ItemRepository.Get()
                         on a.ItemId equals b.ItemId
                         join c in unitOfWork.ItemCategoryRepository.Get()
                         on b.ItemCategoryId equals c.ItemCategoryId
                         where a.PoStyleId == purchaseOrderID
                         group a by new { a.ItemId, b.ItemDescription, c.ItemCategoryId, c.Name }
                             into g
                             select new RawMaterialStatusViewModel
                             {
                                 ItemCategoryId = g.Key.ItemCategoryId,
                                 ItemCategoryName = g.Key.Name,
                                 ItemID = g.Key.ItemId,
                                 ItemDescription = g.Key.ItemDescription,
                                 TotalQuantity = g.Sum(a => a.TotalQuantity)
                             };

            return result.ToList();
        }
        
        public List<ProductionStatusViewModel> GetDailyProductionStatus(int purchaseOrderID)
        {
            var result = from a in unitOfWork.ProductionStatusRepository.Get()
                         where a.PoStyleId == purchaseOrderID
                         select new ProductionStatusViewModel
                         {
                             Date = a.Date,
                             TodaySewing = a.TodaySewing,
                             Cutting = a.Cutting
                         };
            return result.ToList();
        }
    }
}
