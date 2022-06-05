using ScopoERP.Commercial.ViewModel;
using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Commercial.BLL
{
    public class BLDetailsLogic
    {
        private UnitOfWork unitOfWork;
        private bldetails blDetails;        

        public BLDetailsLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void CreateBLDetails(List<BLDetailsViewModel> blDetailsList)
        {
            foreach (var item in blDetailsList)
            {
                blDetails = new bldetails
                {                                 
                    BLID = item.BLID,
                    BookingID = item.BookingID,
                    InvoiceQuantity = item.InvoiceQuantity,
                    ReceivedQuantity = item.ReceivedQuantity,
                    UserID = item.UserID,
                    SetupDate = DateTime.Now,
                    ActualItemDescription = item.ActualItemDescription
                };

                unitOfWork.BLDetailsRepository.Insert(blDetails);

                unitOfWork.Save();
            }
        }

        public void UpdateBLDetails(List<BLDetailsViewModel> blDetailsList)
        {
            int blID = blDetailsList[0].BLID;

            var oldBLDetails = (from bl in unitOfWork.BLDetailsRepository.Get()
                                where bl.BLID == blID
                                select bl).ToList();


            if(oldBLDetails != null || oldBLDetails.Count() > 0)
            {
                unitOfWork.BLDetailsRepository.DeleteRange(oldBLDetails);
            }

            CreateBLDetails(blDetailsList);
            
        }

        public List<BLDetailsViewModel> GetBLDetails(int blID, int? piID, int? itemID)
        {
            var result = (from s in unitOfWork.BLDetailsRepository.Get()
                          join b in unitOfWork.BookingRepository.Get() on s.BookingID equals b.BookingID
                          join p in unitOfWork.PurchaseOrderRepository.Get() on b.PurchaseOrderID equals p.PoStyleId
                          join i in unitOfWork.ItemRepository.Get() on b.ItemID equals i.ItemId
                          join c in unitOfWork.ConsumptionUnitRepository.Get() on b.ConsumptionUnitID equals c.ConsumptionUnitId
                          where s.BLID == blID && b.PIId == piID && b.ItemID == itemID
                          orderby p.ExitDate
                          select new BLDetailsViewModel
                          {
                              BLDetailsID = s.BLDetailsID,
                              BLID = s.BLID,
                              BookingID = s.BookingID,
                              InvoiceQuantity = s.InvoiceQuantity,
                              ReceivedQuantity = s.ReceivedQuantity,
                              UserID = s.UserID,
                              SetupDate = s.SetupDate,

                              PONo = p.PoNo,
                              ItemDescription = i.ItemDescription,
                              ItemColor = b.ItemColor,
                              ItemSize = b.ItemSize,
                              BookingQuantity = b.TotalQuantity,
                              ConsumpsionUnit = c.UnitName,
                              ActualItemDescription = s.ActualItemDescription
                          }).ToList();

            if (result.Count <= 0)
            {
                result = (from b in unitOfWork.BookingRepository.Get()
                          join p in unitOfWork.PurchaseOrderRepository.Get() on b.PurchaseOrderID equals p.PoStyleId
                          join i in unitOfWork.ItemRepository.Get() on b.ItemID equals i.ItemId
                          join c in unitOfWork.ConsumptionUnitRepository.Get() on b.ConsumptionUnitID equals c.ConsumptionUnitId
                          where b.PIId == piID && b.ItemID == itemID
                          orderby p.ExitDate
                          select new BLDetailsViewModel
                          {
                              BLID = blID,
                              BookingID = b.BookingID,

                              PONo = p.PoNo,
                              ItemDescription = i.ItemDescription,
                              ItemColor = b.ItemColor,
                              ItemSize = b.ItemSize,
                              BookingQuantity = b.TotalQuantity,
                              BLBalanceQuantity = b.TotalQuantity,
                              ConsumpsionUnit = c.UnitName
                          }).ToList();
            }
            else 
            {
                var TotalInvoiceQuantity = (from c in unitOfWork.BLDetailsRepository.Get()
                                         group c by c.BookingID into g
                                         select new
                                         {
                                             BookingID = g.Key,
                                             TotalInvoiceQuantity = g.Sum(c => c.InvoiceQuantity)
                                         }).ToList();

                result = (from a in result
                          join b in TotalInvoiceQuantity on a.BookingID equals b.BookingID
                         select new BLDetailsViewModel
                          {
                              BLDetailsID = a.BLDetailsID,
                              BLID = blID,
                              BookingID = a.BookingID,
                              InvoiceQuantity = a.InvoiceQuantity,
                              UserID = a.UserID,
                              SetupDate = a.SetupDate,

                              PONo = a.PONo,
                              ItemDescription = a.ItemDescription,
                              ItemColor = a.ItemColor,
                              ItemSize = a.ItemSize,
                              BookingQuantity = a.BookingQuantity,
                              BLBalanceQuantity = a.BookingQuantity - b.TotalInvoiceQuantity,
                              ConsumpsionUnit = a.ConsumpsionUnit,
                              ActualItemDescription = a.ActualItemDescription
                          }).ToList();
            }

            return result;
        }

        

        public List<BLDetailsViewModel> GetBLDetailsForChalan(int blID, int piID)
        {
            var result = GetBLDetailsForInventory(blID, piID, null);

            if(result == null || result.Count <= 0)
            {
                result = (from b in unitOfWork.BookingRepository.Get()
                          join p in unitOfWork.PurchaseOrderRepository.Get() on b.PurchaseOrderID equals p.PoStyleId
                          join i in unitOfWork.ItemRepository.Get() on b.ItemID equals i.ItemId
                          join c in unitOfWork.ConsumptionUnitRepository.Get() on b.ConsumptionUnitID equals c.ConsumptionUnitId
                          where b.PIId == piID //&& b.ItemID == itemID
                          orderby p.ExitDate
                          select new BLDetailsViewModel
                          {
                              BLID = blID,
                              BookingID = b.BookingID,
                              PONo = p.PoNo,
                              ItemDescription = i.ItemDescription,
                              ItemColor = b.ItemColor,
                              ItemSize = b.ItemSize,
                              BookingQuantity = b.TotalQuantity,
                              BLBalanceQuantity = b.TotalQuantity,
                              ConsumpsionUnit = c.UnitName
                          }).ToList();
            }

            

            return result;
        }

        public List<BLDetailsViewModel> GetBLDetailsForInventory(int blID, int? piID, int? itemID)
        {
            var result = (from s in unitOfWork.BLDetailsRepository.Get()
                          join b in unitOfWork.BookingRepository.Get() on s.BookingID equals b.BookingID
                          join p in unitOfWork.PurchaseOrderRepository.Get() on b.PurchaseOrderID equals p.PoStyleId
                          join i in unitOfWork.ItemRepository.Get() on b.ItemID equals i.ItemId
                          join c in unitOfWork.ConsumptionUnitRepository.Get() on b.ConsumptionUnitID equals c.ConsumptionUnitId
                          where s.BLID == blID && b.PIId == piID //&& b.ItemID == itemID
                          select new BLDetailsViewModel
                          {
                              BLDetailsID = s.BLDetailsID,
                              BLID = s.BLID,
                              BookingID = s.BookingID,
                              InvoiceQuantity = s.InvoiceQuantity,
                              ReceivedQuantity = s.ReceivedQuantity,
                              UserID = s.UserID,
                              SetupDate = s.SetupDate,

                              PONo = p.PoNo,
                              ItemDescription = i.ItemDescription,
                              ItemColor = b.ItemColor,
                              ItemSize = b.ItemSize,
                              BookingQuantity = b.TotalQuantity,
                              ConsumpsionUnit = c.UnitName
                          }).ToList();

            return result;
        }

        public List<DropDownListViewModel> GetItemDropDownByBL(int blID)
        {
            var results = (from s in unitOfWork.BLDetailsRepository.Get()
                           join b in unitOfWork.BookingRepository.Get() on s.BookingID equals b.BookingID
                           join i in unitOfWork.ItemRepository.Get() on b.ItemID equals i.ItemId
                           where s.BLID == blID
                           select new DropDownListViewModel
                           {
                               Value = i.ItemId,
                               Text = i.ItemDescription
                           }).ToList();

            return results;
        }
    }
}
