using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using ScopoERP.OrderManagement.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ScopoERP.OrderManagement.BLL
{
    public class PurchaseOrderLogic
    {
        private UnitOfWork unitOfWork;
        private postyle poStyle;

        public PurchaseOrderLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void CreatePurchaseOrder(PurchaseOrderViewModel purchaseOrderVM)
        {
            poStyle = new postyle
            {
                JobId = purchaseOrderVM.JobID,
                StyleId = purchaseOrderVM.StyleID,
                PoNo = purchaseOrderVM.PurchaseOrderNo,
                Fob = purchaseOrderVM.FOB,
                AgreedCm = purchaseOrderVM.AgreedCM,
                OrderQuantity = purchaseOrderVM.OrderQuantity,
                ExitDate = purchaseOrderVM.ExitDate,
                OriginalCRD = purchaseOrderVM.OriginalCRD,
                FactoryId = purchaseOrderVM.FactoryID,
                SubContractRate = purchaseOrderVM.SubContractRate,
                FactoryCM = purchaseOrderVM.FactoryCM,
                SeasonId = purchaseOrderVM.SeasonID,

                ShipMode = purchaseOrderVM.ShipMode,
                DCCode = purchaseOrderVM.DCCode,

                CurrentStatus = purchaseOrderVM.CurrentStatus,
                Remarks = purchaseOrderVM.Remarks,

                CostSheetNo = purchaseOrderVM.CostSheetNo
            };

            unitOfWork.PurchaseOrderRepository.Insert(poStyle);
            unitOfWork.Save();
        }

        public List<PurchaseOrderViewModel> GetAllPurchaseOrderByStyleID(int styleID)
        {
            var result = (from p in unitOfWork.PurchaseOrderRepository.Get()
                          where p.StyleId == styleID
                          orderby p.PoStyleId descending
                          select new PurchaseOrderViewModel
                          {
                              PurchaseOrderID = p.PoStyleId,

                              JobID = p.JobId,

                              StyleID = p.StyleId,

                              PurchaseOrderNo = p.PoNo,
                              FOB = p.Fob,
                              AgreedCM = p.AgreedCm,
                              OrderQuantity = p.OrderQuantity,
                              ExitDate = p.ExitDate,
                              OriginalCRD = p.OriginalCRD,

                              ShipMode = p.ShipMode,
                              ShipModeName =
                              (
                                p.ShipMode == 1 ? "Sea" :
                                p.ShipMode == 2 ? "Air" :
                                ""
                              ),
                              DCCode = p.DCCode,

                              FactoryID = p.FactoryId,

                              SubContractRate = p.SubContractRate,
                              FactoryCM = p.FactoryCM,

                              SeasonID = p.SeasonId,

                              CurrentStatus = p.CurrentStatus,
                              Remarks = p.Remarks,

                              CostSheetNo = p.CostSheetNo
                          }).ToList();

            return result;
        }

        public void UpdatePurchaseOrder(PurchaseOrderViewModel purchaseOrderVM)
        {
            var poStyle = unitOfWork.PurchaseOrderRepository.Get().SingleOrDefault(x => x.PoStyleId == purchaseOrderVM.PurchaseOrderID);

            poStyle.PoStyleId = purchaseOrderVM.PurchaseOrderID;
            poStyle.JobId = purchaseOrderVM.JobID;
            poStyle.StyleId = purchaseOrderVM.StyleID;
            poStyle.PoNo = purchaseOrderVM.PurchaseOrderNo;
            poStyle.Fob = purchaseOrderVM.FOB;
            poStyle.AgreedCm = purchaseOrderVM.AgreedCM;
            poStyle.OrderQuantity = purchaseOrderVM.OrderQuantity;
            poStyle.ExitDate = purchaseOrderVM.ExitDate;
            poStyle.OriginalCRD = purchaseOrderVM.OriginalCRD;
            poStyle.FactoryId = purchaseOrderVM.FactoryID;
            poStyle.SubContractRate = purchaseOrderVM.SubContractRate;
            poStyle.FactoryCM = purchaseOrderVM.FactoryCM;
            poStyle.SeasonId = purchaseOrderVM.SeasonID;

            poStyle.ShipMode = purchaseOrderVM.ShipMode;
            poStyle.DCCode = purchaseOrderVM.DCCode;

            poStyle.CurrentStatus = purchaseOrderVM.CurrentStatus;
            poStyle.Remarks = purchaseOrderVM.Remarks;

            poStyle.CostSheetNo = purchaseOrderVM.CostSheetNo;

            unitOfWork.PurchaseOrderRepository.Update(poStyle);
            unitOfWork.Save();
        }

        public void UpdatePurchaseOrder(List<PurchaseOrderViewModel> bulkPurchaseOrderVM)
        {
            foreach (var purchaseOrderVM in bulkPurchaseOrderVM)
            {
                var poStyle = unitOfWork.PurchaseOrderRepository.Get().SingleOrDefault(x => x.PoStyleId == purchaseOrderVM.PurchaseOrderID);

                poStyle.PoStyleId = purchaseOrderVM.PurchaseOrderID;
                poStyle.JobId = purchaseOrderVM.JobID;
                poStyle.StyleId = purchaseOrderVM.StyleID;
                poStyle.PoNo = purchaseOrderVM.PurchaseOrderNo;
                poStyle.Fob = purchaseOrderVM.FOB;
                poStyle.AgreedCm = purchaseOrderVM.AgreedCM;
                poStyle.OrderQuantity = purchaseOrderVM.OrderQuantity;
                poStyle.ExitDate = purchaseOrderVM.ExitDate;
                poStyle.OriginalCRD = purchaseOrderVM.OriginalCRD;
                poStyle.FactoryId = purchaseOrderVM.FactoryID;
                poStyle.SubContractRate = purchaseOrderVM.SubContractRate;
                poStyle.FactoryCM = purchaseOrderVM.FactoryCM;
                poStyle.SeasonId = purchaseOrderVM.SeasonID;

                poStyle.ShipMode = purchaseOrderVM.ShipMode;
                poStyle.DCCode = purchaseOrderVM.DCCode;

                poStyle.CurrentStatus = purchaseOrderVM.CurrentStatus;
                poStyle.Remarks = purchaseOrderVM.Remarks;

                poStyle.CostSheetNo = purchaseOrderVM.CostSheetNo;

                unitOfWork.PurchaseOrderRepository.Update(poStyle);
            }
            unitOfWork.Save();
        }

        public List<PurchaseOrderViewModel> GetAllPurchaseOrder(int accountID)
        {
            var result = (from p in unitOfWork.PurchaseOrderRepository.Get()
                          join s in unitOfWork.StyleRepository.Get() on p.StyleId equals s.StyleId
                          join jb in unitOfWork.JobRepository.Get() on p.JobId equals jb.JobInfoId into g
                          join sn in unitOfWork.SeasonRepository.Get() on p.SeasonId equals sn.SeasonId
                          join f in unitOfWork.FactoryRepository.Get() on p.FactoryId equals f.FactoryId
                          from j in g.DefaultIfEmpty()
                          where s.AccountId == accountID
                          orderby p.PoStyleId descending
                          select new PurchaseOrderViewModel
                          {
                              PurchaseOrderID = p.PoStyleId,

                              JobID = p.JobId,
                              JobNo = j.JobNo,

                              StyleID = p.StyleId,
                              StyleNo = s.StyleNo,

                              PurchaseOrderNo = p.PoNo,
                              FOB = p.Fob,
                              AgreedCM = p.AgreedCm,
                              OrderQuantity = p.OrderQuantity,
                              ExitDate = p.ExitDate,
                              OriginalCRD = p.OriginalCRD,

                              ShipMode = p.ShipMode,
                              ShipModeName =
                              (
                                p.ShipMode == 1 ? "Sea" :
                                p.ShipMode == 2 ? "Air" :
                                ""
                              ),
                              DCCode = p.DCCode,

                              FactoryID = p.FactoryId,
                              FactoryName = f.FactoryName,

                              SubContractRate = p.SubContractRate,
                              FactoryCM = p.FactoryCM,

                              SeasonID = p.SeasonId,
                              SeasonName = sn.SeasonName,

                              CurrentStatus = p.CurrentStatus,
                              Remarks = p.Remarks,

                              CostSheetNo = p.CostSheetNo
                          }).ToList();

            return result;
        }

        public List<PurchaseOrderViewModel> GetAllPurchaseOrderByJobId(int jobId)
        {
            var result = (from p in unitOfWork.PurchaseOrderRepository.Get()
                          join s in unitOfWork.StyleRepository.Get() on p.StyleId equals s.StyleId
                          join jb in unitOfWork.JobRepository.Get() on p.JobId equals jb.JobInfoId into g
                          join sn in unitOfWork.SeasonRepository.Get() on p.SeasonId equals sn.SeasonId
                          join f in unitOfWork.FactoryRepository.Get() on p.FactoryId equals f.FactoryId
                          from j in g.DefaultIfEmpty()
                          where p.JobId == jobId
                          orderby p.PoStyleId descending
                          select new PurchaseOrderViewModel
                          {
                              PurchaseOrderID = p.PoStyleId,

                              JobID = p.JobId,
                              JobNo = j.JobNo,

                              StyleID = p.StyleId,
                              StyleNo = s.StyleNo,

                              PurchaseOrderNo = p.PoNo,
                              FOB = p.Fob,
                              AgreedCM = p.AgreedCm,
                              OrderQuantity = p.OrderQuantity,
                              ExitDate = p.ExitDate,
                              OriginalCRD = p.OriginalCRD,

                              ShipMode = p.ShipMode,
                              ShipModeName =
                              (
                                p.ShipMode == 1 ? "Sea" :
                                p.ShipMode == 2 ? "Air" :
                                ""
                              ),
                              DCCode = p.DCCode,

                              FactoryID = p.FactoryId,
                              FactoryName = f.FactoryName,

                              SubContractRate = p.SubContractRate,
                              FactoryCM = p.FactoryCM,

                              SeasonID = p.SeasonId,
                              SeasonName = sn.SeasonName,

                              CurrentStatus = p.CurrentStatus,
                              Remarks = p.Remarks,

                              CostSheetNo = p.CostSheetNo
                          }).ToList();

            return result;
        }

        public PurchaseOrderViewModel GetPurchaseOrderByID(int id)
        {
            var result = (from s in unitOfWork.PurchaseOrderRepository.Get()
                          where s.PoStyleId == id
                          select new PurchaseOrderViewModel
                          {
                              PurchaseOrderID = s.PoStyleId,
                              JobID = s.JobId,
                              StyleID = s.StyleId,
                              PurchaseOrderNo = s.PoNo,
                              FOB = s.Fob,
                              AgreedCM = s.AgreedCm,
                              OrderQuantity = s.OrderQuantity,
                              ExitDate = s.ExitDate,
                              OriginalCRD = s.OriginalCRD,
                              FactoryID = s.FactoryId,
                              SubContractRate = s.SubContractRate,
                              FactoryCM = s.FactoryCM,
                              SeasonID = s.SeasonId,

                              ShipMode = s.ShipMode,
                              DCCode = s.DCCode,

                              CurrentStatus = s.CurrentStatus,
                              Remarks = s.Remarks,

                              CostSheetNo = s.CostSheetNo
                          }).SingleOrDefault();

            return result;
        }

        public int GetOrderQuantityByPurchaseOrder(int purchaseOrderID)
        {
            return unitOfWork.PurchaseOrderRepository.Get().SingleOrDefault(x => x.PoStyleId == purchaseOrderID).OrderQuantity;
        }

        public List<DropDownListViewModel> GetPurchaseOrderDropDown()
        {
            var result = (from s in unitOfWork.PurchaseOrderRepository.Get()
                          select new DropDownListViewModel
                          {
                              Text = s.PoNo,
                              Value = s.PoStyleId
                          }).ToList();

            return result;
        }

        public List<DropDownListViewModel> GetPurchaseOrderDropDown(int[] styleIDs)
        {
            var results = (from c in unitOfWork.PurchaseOrderRepository.Get()
                           where styleIDs.Contains(c.StyleId)
                           select new DropDownListViewModel
                           {
                               Value = c.PoStyleId,
                               Text = c.PoNo
                           }).Distinct().OrderBy(x => x.Text);

            return results.ToList();
        }

        public List<DropDownListViewModel> GetPurchaseOrderDropDown(int styleID)
        {
            var result = (from s in unitOfWork.PurchaseOrderRepository.Get()
                          where s.StyleId == styleID
                          select new DropDownListViewModel
                          {
                              Text = s.PoNo,
                              Value = s.PoStyleId
                          }).ToList();

            return result;
        }

        public List<DropDownListViewModel> GetPurchaseOrderDropDownByJob(int jobID)
        {
            /*var result = (from p in unitOfWork.PurchaseOrderRepository.Get()
                          join s in unitOfWork.SizeColorRepository.Get() on p.PoStyleId equals s.PoStyleId into ps
                          from s in ps.DefaultIfEmpty()
                          where p.JobId == jobID
                          select new DropDownListViewModel
                          {
                              Text = p.PoNo,
                              Value = p.PoStyleId
                          }).ToList();*/


            var result = (from p in unitOfWork.PurchaseOrderRepository.Get()
                          //join s in unitOfWork.SizeColorRepository.Get() on p.PoStyleId equals s.PoStyleId
                          where p.JobId == jobID
                          select new DropDownListViewModel
                          {
                              Text = p.PoNo,
                              Value = p.PoStyleId
                          }).Distinct().ToList();

            return result;
        }

        public List<DropDownListViewModel> GetPurchaseOrderDropDown(int styleID, int factoryID, DateTime fromDate, DateTime toDate)
        {
            var productionPlanList = (from c in unitOfWork.ProductionPlanningRepository.Get()
                                      select c.PoStyleID).ToList();

            var temp = (from s in unitOfWork.PurchaseOrderRepository.Get()
                        where s.StyleId == styleID && s.FactoryId == factoryID && !productionPlanList.Contains(s.PoStyleId)
                        && s.ExitDate >= fromDate && s.ExitDate <= toDate
                        select s).ToList();

            var result = (from s in temp
                          orderby s.ExitDate
                          select new DropDownListViewModel
                          {
                              Text = s.PoNo,
                              Value = s.PoStyleId
                          }).ToList();

            return result;
        }

        public bool IsUniquePurchaseOrder(string poNo, Nullable<int> poStyleID = null)
        {
            IQueryable<int> result;

            if (poStyleID == null)
            {
                result = from s in unitOfWork.PurchaseOrderRepository.Get()
                         where s.PoNo == poNo
                         select s.PoStyleId;
            }
            else
            {
                result = from s in unitOfWork.PurchaseOrderRepository.Get()
                         where s.PoNo == poNo & s.PoStyleId != poStyleID
                         select s.PoStyleId;
            }

            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }

        public List<SplitPONo> GetPurchaseOrderForSplit(int styleID, int purchaeOrderID)
        {
            return (from s in unitOfWork.PurchaseOrderRepository.Get()
                    where s.StyleId == styleID && s.PoStyleId != purchaeOrderID
                    select new SplitPONo
                    {
                        IsChecked = false,
                        PurchaseOrderID = s.PoStyleId,
                        PONo = s.PoNo,
                        OrderQuantity = s.OrderQuantity,
                        ExitDate = s.ExitDate,
                        FOB=s.Fob,
                        AgreedCM=s.AgreedCm
                    }).ToList();
        }

        public void Split(SplitViewModel splitVM)
        {
            // Get all information of Master PO
            var productionList = unitOfWork.ProductionStatusRepository.Get().Where(x => x.PoStyleId == splitVM.MasterPOID).ToList();
            var bookingList = unitOfWork.BookingRepository.Get().Where(x => x.PurchaseOrderID == splitVM.MasterPOID).ToList();
            var masterPO = unitOfWork.PurchaseOrderRepository.Get().Where(x => x.PoStyleId == splitVM.MasterPOID).SingleOrDefault();

            // Get Splitted PO list
            List<int> poList = splitVM.SplitList.Where(x => x.IsChecked == true).Select(x => x.PurchaseOrderID).ToList();

            // Calculate Total Quantity of splitted POs
            int totalQuantity = unitOfWork.PurchaseOrderRepository.Get().Where(x => poList.Contains(x.PoStyleId)).Sum(x => x.OrderQuantity);

            decimal ratio;
            postyle splittedPO;
            int _purchaseOrderID;

            for (int i = 0; i < poList.Count(); i++)
            {
                // Get Splitted PO information
                _purchaseOrderID = poList[i];
                splittedPO = unitOfWork.PurchaseOrderRepository.Get().Where(x => x.PoStyleId == _purchaseOrderID).SingleOrDefault();

                // Calculate Ration
                if (masterPO.OrderQuantity < totalQuantity)
                    ratio = splittedPO.OrderQuantity / masterPO.OrderQuantity;
                else
                    ratio = (decimal)splittedPO.OrderQuantity / (decimal)totalQuantity;

                // Split Production Information
                foreach (var item in productionList)
                {
                    var dpr = new productiondailyreport
                    {
                        PoStyleId = splittedPO.PoStyleId,
                        Date = item.Date,
                        Floor = item.Floor,
                        Line = item.Line,
                        Hour = item.Hour,
                        Cutting = item.Cutting != null ? (long)(item.Cutting * ratio) : 0,
                        SewingInput = item.SewingInput != null ? (long)(item.SewingInput * ratio) : 0,
                        TodaySewing = item.TodaySewing != null ? (long)(item.TodaySewing * ratio) : 0,
                        SentPrintEmb = item.SentPrintEmb != null ? (long)(item.SentPrintEmb * ratio) : 0,
                        ReceivedPrintEmb = item.ReceivedPrintEmb != null ? (long)(item.ReceivedPrintEmb * ratio) : 0,
                        SentWash = item.SentWash != null ? (long)(item.SentWash * ratio) : 0,
                        ReceivedWash = item.ReceivedWash != null ? (long)(item.ReceivedWash * ratio) : 0,
                        TodayFinish = item.TodayFinish != null ? (long)(item.TodayFinish * ratio) : 0
                    };

                    unitOfWork.ProductionStatusRepository.Insert(dpr);
                }

                // Split Booking Information
                foreach (var item in bookingList)
                {
                    var bookings = new booking
                    {
                        PurchaseOrderID = splittedPO.PoStyleId,
                        ItemID = item.ItemID,
                        ItemSize = item.ItemSize,
                        ItemColor = item.ItemColor,
                        TotalQuantity = item.TotalQuantity * ratio,
                        ConsumptionUnitID = item.ConsumptionUnitID,
                        UnitPrice = item.UnitPrice,
                        PIId = item.PIId,
                        RevisionNo = item.RevisionNo,
                        UserID = item.UserID,
                        SetDate = item.SetDate
                    };

                    unitOfWork.BookingRepository.Insert(bookings);
                }
            }

            // Setting Master PO Quantity to zero
            masterPO.OrderQuantity = 0;
            unitOfWork.PurchaseOrderRepository.Update(masterPO);

            // Delete Booking information of Master PO
            unitOfWork.BookingRepository.DeleteRange(bookingList);

            // Delete Production information of Master PO
            unitOfWork.ProductionStatusRepository.DeleteRange(productionList);

            unitOfWork.Save();
        }
        
        public List<DropDownListViewModel> GetUnAssignedPurchaseOrderDropDownByJob(int jobID)
        {
            string query = $@"SELECT * FROM shipment s 
                            INNER JOIN postyle p on s.PurchaseOrderID = p.PoStyleId 
                            WHERE s.InvoiceID IS NULL AND p.JobId = {jobID}";

            var result = unitOfWork.PurchaseOrderRepository.SelectQuery<postyle>(query);

            var data = (from r in result
                        select new DropDownListViewModel
                        {
                            Text = r.PoNo,
                            Value = r.PoStyleId
                        }).ToList();

            return data;
        }

        public List<DropDownListViewModel> GetPurchaseOrderDropDownByStyleID(int styleID)
        {
            List<DropDownListViewModel> result = (from p in unitOfWork.PurchaseOrderRepository.Get()
                          where p.StyleId == styleID
                          orderby p.PoStyleId descending
                          select new DropDownListViewModel
                          {
                             Value=p.PoStyleId,
                             Text=p.PoNo
                          }).ToList();

            return result;
        }
    }
}
