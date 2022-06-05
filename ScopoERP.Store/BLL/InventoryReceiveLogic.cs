using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using ScopoERP.Store.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Store.BLL
{
    public class InventoryReceiveLogic
    {
        private UnitOfWork unitOfWork;
        private bldetails blDetails;
        private bl bl;

        public InventoryReceiveLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public List<InventoryReceiveViewModel> GetImportChalanDetails(int blID, int purchaseOrderID, int itemID)
        {
            var result = (from s in unitOfWork.BLDetailsRepository.Get()
                          join b in unitOfWork.BookingRepository.Get() on s.BookingID equals b.BookingID
                          join p in unitOfWork.PurchaseOrderRepository.Get() on b.PurchaseOrderID equals p.PoStyleId
                          join i in unitOfWork.ItemRepository.Get() on b.ItemID equals i.ItemId
                          join c in unitOfWork.ConsumptionUnitRepository.Get() on b.ConsumptionUnitID equals c.ConsumptionUnitId
                          where s.BLID == blID && b.PurchaseOrderID == purchaseOrderID && b.ItemID == itemID
                          select new InventoryReceiveViewModel
                          {
                              BLDetailsID = s.BLDetailsID,
                              BLID = s.BLID,
                              BookingID = s.BookingID,
                              InvoiceQuantity = s.InvoiceQuantity,
                              UserID = s.UserID,
                              SetupDate = s.SetupDate,

                              PONo = p.PoNo,
                              ItemDescription = i.ItemDescription,
                              ItemColor = b.ItemColor,
                              ItemSize = b.ItemSize,
                              BookingQuantity = b.TotalQuantity,
                              ConsumpsionUnit = c.UnitName
                          }).ToList();

            if (result.Count <= 0)
            {
                result = (from b in unitOfWork.BookingRepository.Get()
                          join p in unitOfWork.PurchaseOrderRepository.Get() on b.PurchaseOrderID equals p.PoStyleId
                          join i in unitOfWork.ItemRepository.Get() on b.ItemID equals i.ItemId
                          join c in unitOfWork.ConsumptionUnitRepository.Get() on b.ConsumptionUnitID equals c.ConsumptionUnitId
                          where b.PurchaseOrderID == purchaseOrderID && b.ItemID == itemID
                          select new InventoryReceiveViewModel
                          {
                              BLID = blID,
                              BookingID = b.BookingID,

                              PONo = p.PoNo,
                              ItemDescription = i.ItemDescription,
                              ItemColor = b.ItemColor,
                              ItemSize = b.ItemSize,
                              BookingQuantity = b.TotalQuantity,
                              ConsumpsionUnit = c.UnitName
                          }).ToList();
            }

            return result;
        }

        public int CreateImportChalan(ImportChalanViewModel importChalanVM)
        {
            this.bl = new bl()
            {
                BLNo = importChalanVM.BLNo,
                BLDate = importChalanVM.BLDate,
                IsChalan = true,
                Status = importChalanVM.Status
            };

            unitOfWork.BLRepository.Insert(bl);
            unitOfWork.Save();

            return bl.BLID;
        }

        public void CreateImportChalanDetails(List<InventoryReceiveViewModel> importChalanDetailsList)
        {
            foreach (var item in importChalanDetailsList)
            {
                blDetails = new bldetails
                {
                    BLID = item.BLID,
                    BookingID = item.BookingID,
                    InvoiceQuantity = item.InvoiceQuantity,
                    ReceivedQuantity = item.InvoiceQuantity,
                    UserID = item.UserID,
                    SetupDate = item.SetupDate
                };

                unitOfWork.BLDetailsRepository.Insert(blDetails);

                unitOfWork.Save();
            }
        }


        public void UpdateImportChalanDetails(List<InventoryReceiveViewModel> blDetailsList)
        {
            foreach (var item in blDetailsList)
            {
                blDetails = new bldetails
                {
                    BLDetailsID = item.BLDetailsID,
                    BLID = item.BLID,
                    BookingID = item.BookingID,
                    InvoiceQuantity = item.InvoiceQuantity,
                    ReceivedQuantity = item.InvoiceQuantity,
                    UserID = item.UserID,
                    SetupDate = item.SetupDate
                };

                unitOfWork.BLDetailsRepository.Update(blDetails);
            }

            unitOfWork.Save();
        }

        public List<DropDownListViewModel> GetImportChalanDropDown()
        {
            var results = (from c in unitOfWork.BLRepository.Get()
                           where c.IsChalan == true
                           select new DropDownListViewModel
                           {
                               Value = c.BLID,
                               Text = c.BLNo
                           }).ToList();

            return results;
        }
        
    }
}
