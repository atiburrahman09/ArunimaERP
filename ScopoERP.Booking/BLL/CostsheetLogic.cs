using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using ScopoERP.MaterialManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.MaterialManagement.BLL
{
    public class CostsheetLogic
    {
        private UnitOfWork unitOfWork;
        private initialcostsheet costsheet;


        public CostsheetLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public string CreateCostsheet(List<CostsheetViewModel> costsheetVM)
        {
            string costsheetNo = string.Empty;

            // Check that CostsheetNo is empty or not. If empty then Generate new CostsheetNo.
            if (costsheetVM[0].CostSheetNo == null)
            {
                costsheetNo = GenerateCostsheetNo(costsheetVM[0].StyleID);
            }
            else
            {
                costsheetNo = costsheetVM[0].CostSheetNo;

                // Delete existing Costsheet information
                DeleteCostsheetByCostsheetNo(costsheetNo);
            }
              
            // Insert Costsheet information
            foreach (var item in costsheetVM)
            {
                costsheet = new initialcostsheet
                {
                    InitialCostsheetId = item.CostsheetID,
                    StyleId = item.StyleID,
                    CostSheetNo = costsheetNo,
                    ItemCategoryId = item.ItemCategoryID,
                    ItemId = item.ItemID,
                    Consumption = item.Consumption,
                    ConsumptionUnitId = item.ConsumptionUnitID,
                    Wastage = item.Wastage,
                    ActualPrice = item.UnitPrice,
                    ConversionQuantity = item.ConversionQuantity,
                    ConversionUnit = item.ConversionUnitID,
                    Status = 1
                };
                unitOfWork.CostsheetRepository.Insert(costsheet);
            }
            unitOfWork.Save();

            return costsheetNo;
        }


        public string CopyCostSheet(string costSheetNo, int toStyleID)
        {
            List<initialcostsheet> costSheet = (from c in unitOfWork.CostsheetRepository.Get()
                                         where c.CostSheetNo == costSheetNo
                                         select c).ToList();

            costSheetNo = GenerateCostsheetNo(toStyleID);

            foreach (var item in costSheet)
            {
                item.StyleId = toStyleID;
                item.CostSheetNo = costSheetNo;

                unitOfWork.CostsheetRepository.Insert(item);
            }
            unitOfWork.Save();

            return costSheetNo;
        }

        public object GetCostSheetByStyleIDPOID(int styleID, int pOID)
        {
            var result = from cs in unitOfWork.CostsheetRepository.Get()
                          join pocs in unitOfWork.POCostsheetRepository.Get() on cs.CostSheetNo equals pocs.CostSheetNo
                          join i in unitOfWork.ItemRepository.Get() on cs.ItemId equals i.ItemId
                          join it in unitOfWork.ItemCategoryRepository.Get() on i.ItemCategoryId equals it.ItemCategoryId
                          join cn in unitOfWork.ConsumptionUnitRepository.Get() on cs.ConsumptionUnitId equals cn.ConsumptionUnitId
                          join cv in unitOfWork.ConsumptionUnitRepository.Get() on cs.ConsumptionUnitId equals cv.ConsumptionUnitId
                          where cs.StyleId == styleID && pocs.PoStyleId == pOID
                          select new CostsheetViewModel
                          {

                              CostsheetID = cs.InitialCostsheetId,
                              StyleID = cs.StyleId,
                              CostSheetNo = cs.CostSheetNo,

                              ItemCategoryID = cs.ItemCategoryId,
                              ItemCategory = it.Name,
                              ItemID = cs.ItemId,
                              ItemDescription = i.ItemDescription,
                              ItemCode = i.ItemCode,

                              Consumption = Math.Round(cs.Consumption, 4),
                              ConsumptionUnitID = cs.ConsumptionUnitId,
                              ConsumptionUnit = cn.UnitName,

                              ConversionQuantity = Math.Round((decimal)cs.ConversionQuantity, 4),
                              ConversionUnitID = cs.ConversionUnit,
                              ConversionUnit = cv.UnitName,

                              Wastage = Math.Round(cs.Wastage, 4),
                              UnitPrice = Math.Round(cs.ActualPrice, 4)
                          };

            return result.OrderBy(x => x.ItemCategoryID).ToList();
        }

        public void DeleteCostSheet(string costSheetNo)
        {
            DeleteCostsheetByCostsheetNo(costSheetNo);
            DeletePOCostSheetMapping(costSheetNo);

            unitOfWork.Save();
        }


        private void DeletePOCostSheetMapping(string costSheetNo)
        {
            unitOfWork.CostsheetRepository.RawQuery("DELETE FROM POCostSheet WHERE CostsheetNo = '" + costSheetNo + "'");
        }


        private void DeleteCostsheetByCostsheetNo(string costsheetNo)
        {
            unitOfWork.CostsheetRepository.RawQuery("DELETE FROM initialcostsheet WHERE CostsheetNo = '" + costsheetNo + "'");
        }


        public List<CostsheetViewModel> GetCostSheetByCostsheetNo(string costsheetNo)
        {
            var results = from cs in unitOfWork.CostsheetRepository.Get()
                          join i in unitOfWork.ItemRepository.Get() on cs.ItemId equals i.ItemId
                          join it in unitOfWork.ItemCategoryRepository.Get() on i.ItemCategoryId equals it.ItemCategoryId
                          join cn in unitOfWork.ConsumptionUnitRepository.Get() on cs.ConsumptionUnitId equals cn.ConsumptionUnitId
                          join cv in unitOfWork.ConsumptionUnitRepository.Get() on cs.ConsumptionUnitId equals cv.ConsumptionUnitId
                          where cs.CostSheetNo == costsheetNo && cs.Status != 0
                          select new CostsheetViewModel
                          {
                              CostsheetID = cs.InitialCostsheetId,
                              StyleID = cs.StyleId,
                              CostSheetNo = cs.CostSheetNo,

                              ItemCategoryID = cs.ItemCategoryId,
                              ItemCategory = it.Name,
                              ItemID = cs.ItemId,
                              ItemDescription = i.ItemDescription,
                              ItemCode = i.ItemCode,

                              Consumption = Math.Round(cs.Consumption, 4),
                              ConsumptionUnitID = cs.ConsumptionUnitId,
                              ConsumptionUnit = cn.UnitName,

                              ConversionQuantity = Math.Round((decimal)cs.ConversionQuantity, 4),
                              ConversionUnitID = cs.ConversionUnit,
                              ConversionUnit = cv.UnitName,

                              Wastage = Math.Round(cs.Wastage, 4),
                              UnitPrice = Math.Round(cs.ActualPrice, 4)
                          };

            return results.OrderBy(x => x.ItemCategoryID).ToList();
        }


        public CostsheetViewModel GetCostSheet(string costsheetNo, int itemID)
            {
            var results = (from cs in unitOfWork.CostsheetRepository.Get()
                           where cs.CostSheetNo == costsheetNo && cs.ItemId == itemID && cs.Status != 0
                           select new CostsheetViewModel
                           {
                               CostsheetID = cs.InitialCostsheetId,
                               StyleID = cs.StyleId,
                               CostSheetNo = cs.CostSheetNo,
                               ItemCategoryID = cs.ItemCategoryId,
                               ItemCategory = cs.itemcategory.Name,
                               ItemID = cs.ItemId,
                               ItemDescription = cs.item.ItemDescription,
                               ItemCode = cs.item.ItemCode,
                               Consumption = cs.Consumption,
                               ConsumptionUnitID = cs.ConsumptionUnitId,
                               ConversionQuantity = cs.ConversionQuantity,
                               ConversionUnitID = cs.ConversionUnit,
                               Wastage = cs.Wastage,
                               UnitPrice = cs.ActualPrice
                           }).FirstOrDefault();

            return results;
        }


        public string GenerateCostsheetNo(int styleID)
        {
            string generatedCsNo = "";

            string csNo = (from ic in unitOfWork.CostsheetRepository.Get()
                           where ic.StyleId == styleID
                           orderby ic.CostSheetNo descending
                           select ic.CostSheetNo).FirstOrDefault();
            if (csNo == null)
            {
                string styleNo = (from s in unitOfWork.StyleRepository.Get()
                                  where s.StyleId == styleID
                                  select s.StyleNo).SingleOrDefault();

                generatedCsNo = "CS-" + styleNo + "-001";

            }
            else
            {
                string[] splittedCsNo = csNo.Split('-');
                int slNo = Convert.ToInt32(splittedCsNo[2]) + 1;
                if (slNo < 10)
                {
                    generatedCsNo = splittedCsNo[0] + "-" + splittedCsNo[1] + "-00" + slNo.ToString();
                }
                else if (slNo >= 10 && slNo <= 99)
                {
                    generatedCsNo = splittedCsNo[0] + "-" + splittedCsNo[1] + "-0" + slNo.ToString();
                }
                else if (slNo >= 100 && slNo <= 999)
                {
                    generatedCsNo = splittedCsNo[0] + "-" + splittedCsNo[1] + "-" + slNo.ToString();
                }
            }
            return generatedCsNo;
        }


        public List<DropDownListViewModel> GetCostsheetNoByStyle(int[] styleID)
        {
            var results = (from c in unitOfWork.CostsheetRepository.Get()
                           where styleID.Contains(c.StyleId)
                           select new DropDownListViewModel{
                               ValueString = c.CostSheetNo,
                               Text = c.CostSheetNo
                           }).Distinct();

            return results.ToList();
        }


        public List<string> GetCostsheetNoByStyle(int styleID)
        {
            var results = (from ic in unitOfWork.CostsheetRepository.Get()
                           where ic.StyleId == styleID && ic.Status == 1
                           select ic.CostSheetNo
                           ).Distinct();

            return results.ToList();
        }


        public List<DropDownListViewModel> GetCostSheetDropDown(int styleID)
        {
            var results = (from s in unitOfWork.CostsheetRepository.Get()
                           where s.StyleId == styleID
                           select new DropDownListViewModel
                           {
                               Text = s.CostSheetNo,
                               ValueString = s.CostSheetNo
                           }).Distinct().ToList();            

            return results;
        }


        public List<DropDownListViewModel> GetItemByCostsheet(string[] costsheetNo)
        {
            var results = (from c in unitOfWork.CostsheetRepository.Get()
                           join d in unitOfWork.ItemRepository.Get()
                               on c.ItemId equals d.ItemId
                           where costsheetNo.Contains(c.CostSheetNo)
                           select new DropDownListViewModel
                           {
                               Value = d.ItemId,
                               Text = d.ItemDescription
                           }).Distinct();

            return results.ToList();
        }


        public List<DropDownListViewModel> GetItemByCostsheet(string costsheetNo)
        {
            var results = (from c in unitOfWork.CostsheetRepository.Get()
                           join d in unitOfWork.ItemRepository.Get()
                               on c.ItemId equals d.ItemId
                           where costsheetNo == c.CostSheetNo
                           select new DropDownListViewModel
                           {
                               Value = d.ItemId,
                               Text = d.ItemDescription
                           }).Distinct();

            return results.ToList();
        }


        public IQueryable<CostSheetSummaryViewModel> GetAllCostSheet(int accountID)
        {
            IQueryable<CostSheetSummaryViewModel> result = (from s in unitOfWork.StyleRepository.Get()
                                                            join cs in unitOfWork.CostsheetRepository.Get() on s.StyleId equals cs.StyleId into cg
                                                            from c in cg.DefaultIfEmpty()
                                                            where s.AccountId == accountID
                                                            select new CostSheetSummaryViewModel
                                                            {
                                                                CostSheetNo = String.IsNullOrEmpty(c.CostSheetNo) ? "No CostSheet" : c.CostSheetNo,
                                                                StyleNo = s.StyleNo
                                                            }).Distinct();

            return result;
        }
    }
}
