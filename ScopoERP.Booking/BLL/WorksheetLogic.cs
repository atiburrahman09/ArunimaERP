using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using ScopoERP.MaterialManagement.ViewModel;
using ScopoERP.OrderManagement.BLL;
using ScopoERP.OrderManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.MaterialManagement.BLL
{
    public class WorksheetLogic
    {
        private UnitOfWork unitOfWork;
        private SizeColorLogic sizeColorLogic;
        private CostsheetLogic costsheetLogic;
        private worksheets worksheet;
        private pocostsheet pocostsheet;

        public WorksheetLogic(UnitOfWork unitOfWork, SizeColorLogic sizeColorLogic, CostsheetLogic costsheetLogic)
        {
            this.unitOfWork = unitOfWork;
            this.sizeColorLogic = sizeColorLogic;
            this.costsheetLogic = costsheetLogic;
        }


        public List<WorksheetViewModel> TryToGetWorksheets (string costSheetNo, int purchaseOrderID)
        {
            var result = (from w in unitOfWork.WorksheetRepository.Get()
                          join i in unitOfWork.ItemRepository.Get() on w.ItemId equals i.ItemId
                          join ic in unitOfWork.ItemCategoryRepository.Get() on i.ItemCategoryId equals ic.ItemCategoryId
                          join co in unitOfWork.ConsumptionUnitRepository.Get() on w.ConsumptionUnitId equals co.ConsumptionUnitId
                          where w.PoStyleId == purchaseOrderID
                          select new WorksheetViewModel
                          {
                              WorksheetId = w.WorksheetId,
                              PoStyleId = w.PoStyleId,
                              ItemCategoryID = ic.ItemCategoryId,
                              ItemCategoryName = ic.Name,
                              ItemCategory=ic.Name,
                              ItemDescription=i.ItemDescription,
                              ItemCode = i.ItemCode,
                              ItemColor = w.ItemColor,
                              ItemId = w.ItemId,
                              ItemName = i.ItemDescription,
                              ItemSize = w.ItemSize,
                              Color = w.Color,
                              Consumption = w.Consumption,
                              ConsumptionUnitId = w.ConsumptionUnitId,
                              ConsumptionUnitName = co.UnitName,
                              ConsumptionUnit=co.UnitName,
                              
                              Formula = w.Formula,
                              Size = w.Size,
                              Status = w.Status,
                              TotalQuantity = w.TotalQuantity,
                              Wastage = w.Wastage,
                              UnitPrice = w.UnitPrice
                          }).ToList();


            return result;
        }

        public List<WorksheetViewModel> GetWorksheetsCreateIfEmpty(string costSheetNo, int purchaseOrderID)
        {
            var result = TryToGetWorksheets(costSheetNo, purchaseOrderID);
                
            if(result == null || result.Count <= 0)
            {
                List<CostsheetViewModel> csList = costsheetLogic.GetCostSheetByCostsheetNo(costSheetNo);

                foreach(var cs in csList)
                {
                    this.worksheet = new worksheets
                    {
                        PoStyleId = purchaseOrderID,
                        ItemId = cs.ItemID,
                        Consumption = cs.ActualConsumption ?? 0,
                        ConsumptionUnitId = cs.ConsumptionUnitID,
                        Wastage = cs.Wastage,
                        UnitPrice = cs.UnitPrice,
                        Formula = 4,
                        Status = 1
                    };
                    unitOfWork.WorksheetRepository.Insert(this.worksheet);
                }
                unitOfWork.Save();

            }

            result = TryToGetWorksheets(costSheetNo, purchaseOrderID);

            result.ForEach(x =>
            {
                if (x.Formula == 1) { x.FormulaText = "Color"; }
                else if (x.Formula == 2) { x.FormulaText = "Size"; }
                else if (x.Formula == 3) { x.FormulaText = "Size & Color"; }
                else { x.FormulaText = "N/A"; }
            });

            return result;
            
        }

        public void CreateWorksheet(string costsheetNo, int purchaseOrderID, int itemID, int formula)
        {
            var sizeColorList = sizeColorLogic.GetSizeColorByFormula(purchaseOrderID, formula);

            var costsheet = costsheetLogic.GetCostSheet(costsheetNo, itemID);

            decimal _totalQuantity;
            decimal _wastage;

            var result = from c in unitOfWork.POCostsheetRepository.Get()
                         where c.CostSheetNo == costsheetNo && c.PoStyleId == purchaseOrderID
                         select c;

            if(result.ToList().Count != 1)
            {
                pocostsheet = new pocostsheet
                {
                    CostSheetNo = costsheetNo,
                    PoStyleId = purchaseOrderID
                };

                unitOfWork.POCostsheetRepository.Insert(pocostsheet);
            }

            foreach (var item in sizeColorList)
            {
                _wastage = (decimal)costsheet.ActualConsumption + ((decimal)costsheet.ActualConsumption * costsheet.Wastage) / 100;
                _totalQuantity = item.Quantity * _wastage;

                worksheet = new worksheets
                {
                    PoStyleId = purchaseOrderID,
                    ItemId = costsheet.ItemID,
                    Size = item.Size,
                    Color = item.Color,
                    Consumption = costsheet.ActualConsumption ?? 0,
                    ConsumptionUnitId = costsheet.ConsumptionUnitID,
                    Wastage = costsheet.Wastage,
                    UnitPrice = costsheet.UnitPrice,
                    Formula = formula,
                    TotalQuantity = _totalQuantity,
                    Status = 1
                };

                unitOfWork.WorksheetRepository.Insert(worksheet);
            }

            unitOfWork.Save();
        }

        public void UpdateWorksheet(List<WorksheetViewModel> worksheetVM)
        {
            foreach(var item in worksheetVM)
            {
                worksheet = new worksheets
                {
                    WorksheetId = item.WorksheetId,
                    PoStyleId = item.PoStyleId,
                    ItemId = item.ItemId,
                    Size = item.Size,
                    Color = item.Color,
                    ItemColor = item.ItemColor,
                    ItemSize = item.ItemSize,
                    Consumption = item.Consumption,
                    ConsumptionUnitId = item.ConsumptionUnitId,
                    Wastage = item.Wastage,
                    UnitPrice = item.UnitPrice,
                    Formula = item.Formula,
                    TotalQuantity = item.TotalQuantity,
                    Status = 1
                };

                unitOfWork.WorksheetRepository.Update(worksheet);
            }

            unitOfWork.Save();
        }

        public List<WorksheetViewModel> GetWorksheet(int purchaseOrderID, int itemID)
        {
            var result = (from s in unitOfWork.WorksheetRepository.Get()
                          join i in unitOfWork.ItemRepository.Get() on s.ItemId equals i.ItemId
                          join ic in unitOfWork.ItemCategoryRepository.Get() on i.ItemCategoryId equals ic.ItemCategoryId
                          join c in unitOfWork.ConsumptionUnitRepository.Get() on s.ConsumptionUnitId equals c.ConsumptionUnitId
                          where s.PoStyleId == purchaseOrderID && s.ItemId == itemID
                          select new WorksheetViewModel
                          {
                              WorksheetId = s.WorksheetId,
                              PoStyleId = s.PoStyleId,
                              ItemId = s.ItemId,
                              ItemCode = i.ItemCode,
                              ItemName = i.ItemDescription,
                              ItemCategoryID = ic.ItemCategoryId,
                              ItemCategoryName = ic.Name,
                              Size = s.Size,
                              Color = s.Color,
                              ItemColor = s.ItemColor,
                              ItemSize = s.ItemSize,
                              Consumption = s.Consumption,
                              ConsumptionUnitId = s.ConsumptionUnitId,
                              ConsumptionUnitName = c.UnitName,
                              Wastage = s.Wastage,
                              UnitPrice = s.UnitPrice,
                              Formula = s.Formula,
                              TotalQuantity = s.TotalQuantity,
                              Status = 1
                          }).ToList();

            result.ForEach(x => 
            { 
                if (x.Formula == 1) x.FormulaText = "Color";
                else if (x.Formula == 2) x.FormulaText = "Size";
                else if (x.Formula == 3) x.FormulaText = "Color & Size";
                else if (x.Formula == 4) x.FormulaText = "N/A";
            });
            
            return result;
        }

        public void DeleteWorksheet(int purchaseOrderID, int itemID)
        {
            unitOfWork.WorksheetRepository.RawQuery("DELETE FROM Worksheets WHERE PoStyleID = " + purchaseOrderID + " AND ItemID = " + itemID);
        }

        public Boolean IsItemExistsInWorksheet(int itemID, int purchaseOrderID)
        {
            var result = from s in unitOfWork.WorksheetRepository.Get()
                         where s.PoStyleId == purchaseOrderID && s.ItemId == itemID
                         select s;

            if (result == null)
                return false;

            return true;
        }

        public List<DropDownListViewModel> GetFormulaDropDown() 
        {
            List<DropDownListViewModel> FormulaList = new List<DropDownListViewModel>();
            FormulaList.Add(new DropDownListViewModel { Value = 1, Text = "Color" });
            FormulaList.Add(new DropDownListViewModel { Value = 2, Text = "Size" });
            FormulaList.Add(new DropDownListViewModel { Value = 3, Text = "Color & Size" });
            FormulaList.Add(new DropDownListViewModel { Value = 4, Text = "N/A" });

            return FormulaList;
        }

        public List<DropDownListViewModel> GetItemFromWorksheetByPO(int[] poIDs)
        {
            var results = (from c in unitOfWork.WorksheetRepository.Get()
                           join d in unitOfWork.ItemRepository.Get()
                               on c.ItemId equals d.ItemId
                           where poIDs.Contains(c.PoStyleId)
                           select new DropDownListViewModel
                           {
                               Value = d.ItemId,
                               Text = d.ItemDescription
                           }).Distinct().OrderBy(x => x.Text);

            return results.ToList();
        }

        public Boolean isValidCostsheetForPO(string costsheetNo, int purchaseOrderID)
        {
            var result = (from c in unitOfWork.POCostsheetRepository.Get()
                          where c.PoStyleId == purchaseOrderID
                          select c).FirstOrDefault();

            if (result != null)
            {
                if (result.CostSheetNo != costsheetNo)
                    return false;
            }
            return true;
        }

        public object GetItemByAllPO(List<DropDownListViewModel> poList)
        {
            int[] poArray= poList.Select(x => x.Value).ToArray();

            var results = (from c in unitOfWork.WorksheetRepository.Get()
                           join d in unitOfWork.ItemRepository.Get()
                               on c.ItemId equals d.ItemId
                           where poArray.Contains(c.PoStyleId)
                           select new DropDownListViewModel
                           {
                               Value = d.ItemId,
                               Text = d.ItemDescription
                           }).Distinct().OrderBy(x => x.Text);

            return results.ToList();
        }

        public string GetReferenceNo(int purchaseOrderID, int itemID)
        {
            var result = (from s in unitOfWork.BookingRepository.Get()
                          join p in unitOfWork.PIRepository.Get() on s.PIId equals p.PIID
                          where s.PurchaseOrderID == purchaseOrderID && s.ItemID == itemID
                          select p.ReferenceNo).Distinct().ToList();

            return result == null ? "" : string.Join(", ", result);
        }
    }
}
