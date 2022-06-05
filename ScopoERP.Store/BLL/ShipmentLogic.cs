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
    public class ShipmentLogic
    {
        private UnitOfWork unitOfWork;

        public ShipmentLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<ShipmentViewModel> GetAllShipment()
        {
            var result = (from s in unitOfWork.ShipmentRepository.Get()
                          join ch in unitOfWork.ChalanExportRepository.Get() on s.ChalanID equals ch.ChalanID into cg
                          join p in unitOfWork.PurchaseOrderRepository.Get() on s.PurchaseOrderID equals p.PoStyleId
                          join f in unitOfWork.FactoryRepository.Get() on p.FactoryId equals f.FactoryId
                          join iv in unitOfWork.ExportInvoiceRepository.Get() on s.InvoiceID equals iv.InvoiceId into g
                          from c in cg.DefaultIfEmpty()
                          from i in g.DefaultIfEmpty()
                          orderby s.ShipmentID descending
                          select new ShipmentViewModel
                          {
                              ShipmentID = s.ShipmentID,

                              PurchaseOrderID = s.PurchaseOrderID,
                              PONo = p.PoNo,
                              OrderQuantity = p.OrderQuantity,
                              FactoryName = f.FactoryName,

                              ChalanID = s.ChalanID,
                              ChalanNo = c.ChalanNo,
                              ChalanQuantity = s.ChalanQuantity,
                              ChalanDate = s.ChalanDate,

                              CBM = s.CBM,
                              CartoonQuantity = s.CartoonQuantity,

                              InvoiceID = s.InvoiceID,
                              InvoiceNo = i.InvoiceNo,
                              InvoiceFOB = s.InvoiceFOB,
                              Destination = s.Destination,

                              ShippedFOB = s.ChalanQuantity * p.Fob,

                              UserID = s.UserID,
                              SetupDate = s.SetupDate
                          }).AsQueryable();

            return result;
        }

        public List<ShipmentViewModel> GetAllShipmentByChalan(int chalanID)
        {
            var result = (from s in unitOfWork.ShipmentRepository.Get()
                          join p in unitOfWork.PurchaseOrderRepository.Get() on s.PurchaseOrderID equals p.PoStyleId
                          join f in unitOfWork.FactoryRepository.Get() on p.FactoryId equals f.FactoryId
                          where s.ChalanID == chalanID
                          select new ShipmentViewModel
                          {
                              ShipmentID = s.ShipmentID,

                              PurchaseOrderID = s.PurchaseOrderID,
                              PONo = p.PoNo,
                              OrderQuantity = p.OrderQuantity,
                              FactoryName = f.FactoryName,

                              ChalanID = s.ChalanID,
                              ChalanQuantity = s.ChalanQuantity,
                              ChalanDate = s.ChalanDate,

                              CBM = s.CBM,
                              CartoonQuantity = s.CartoonQuantity,

                              InvoiceID = s.InvoiceID,
                              InvoiceFOB = s.InvoiceFOB,
                              Destination = s.Destination,

                              ShippedFOB = s.ChalanQuantity * p.Fob,

                              UserID = s.UserID,
                              SetupDate = s.SetupDate
                          }).ToList();

            return result;
        }

        public List<ShipmentViewModel> GetAllShipmentByInvoice(int invoiceID)
        {
            var result = (from s in unitOfWork.ShipmentRepository.Get()
                          join p in unitOfWork.PurchaseOrderRepository.Get() on s.PurchaseOrderID equals p.PoStyleId
                          join f in unitOfWork.FactoryRepository.Get() on p.FactoryId equals f.FactoryId
                          where s.InvoiceID == invoiceID
                          select new ShipmentViewModel
                          {
                              ShipmentID = s.ShipmentID,

                              PurchaseOrderID = s.PurchaseOrderID,
                              PONo = p.PoNo,
                              OrderQuantity = p.OrderQuantity,
                              FactoryName = f.FactoryName,

                              ChalanID = s.ChalanID,
                              ChalanQuantity = s.ChalanQuantity,
                              ChalanDate = s.ChalanDate,

                              CBM = s.CBM,
                              CartoonQuantity = s.CartoonQuantity,

                              InvoiceID = s.InvoiceID,
                              InvoiceFOB = s.InvoiceFOB,
                              Destination = s.Destination,

                              ShippedFOB = s.ChalanQuantity * p.Fob,

                              UserID = s.UserID,
                              SetupDate = s.SetupDate
                          }).ToList();

            return result;
        }

        public ShipmentViewModel GetShipmentByPurchaseOrder(int purchaseOrderID)
        {
            var result = (from s in unitOfWork.ShipmentRepository.Get()
                          join p in unitOfWork.PurchaseOrderRepository.Get() on s.PurchaseOrderID equals p.PoStyleId
                          join f in unitOfWork.FactoryRepository.Get() on p.FactoryId equals f.FactoryId
                          join iv in unitOfWork.ExportInvoiceRepository.Get() on s.InvoiceID equals iv.InvoiceId into ig
                          from i in ig.DefaultIfEmpty()
                          where s.PurchaseOrderID == purchaseOrderID
                          select new ShipmentViewModel
                          {
                              ShipmentID = s.ShipmentID,

                              PurchaseOrderID = s.PurchaseOrderID,
                              PONo = p.PoNo,
                              OrderQuantity = p.OrderQuantity,
                              FactoryName = f.FactoryName,

                              ChalanID = s.ChalanID,
                              ChalanQuantity = s.ChalanQuantity,
                              ChalanDate = s.ChalanDate,

                              CBM = s.CBM,
                              CartoonQuantity = s.CartoonQuantity,

                              InvoiceID = s.InvoiceID,
                              InvoiceNo = i.InvoiceNo,
                              InvoiceFOB = s.InvoiceFOB,
                              Destination = s.Destination,

                              ShippedFOB = s.ChalanQuantity * p.Fob,                              

                              UserID = s.UserID,
                              SetupDate = s.SetupDate
                          }).FirstOrDefault();

            return result;
        }

        public ShipmentViewModel GetPurchaseOrderByID(int purchaseOrderID)
        {
            var result = (from s in unitOfWork.PurchaseOrderRepository.Get()
                          join f in unitOfWork.FactoryRepository.Get() on s.FactoryId equals f.FactoryId
                          where s.PoStyleId == purchaseOrderID
                          select new ShipmentViewModel
                          {
                              PurchaseOrderID = s.PoStyleId,
                              PONo = s.PoNo,
                              OrderQuantity = s.OrderQuantity,
                              FactoryName = f.FactoryName
                          }).SingleOrDefault();

            return result;
        }

        public ShipmentViewModel GetShipmentByPurchaseOrderID(int purchaseOrderID)
        {
            var result = (from s in unitOfWork.ShipmentRepository.Get()
                          join p in unitOfWork.PurchaseOrderRepository.Get() on s.PurchaseOrderID equals p.PoStyleId
                          where s.PurchaseOrderID == purchaseOrderID
                          select new ShipmentViewModel
                          {
                              ShipmentID = s.ShipmentID,
                              ChalanQuantity = s.ChalanQuantity,
                              ShippedFOB = s.ChalanQuantity * p.Fob,
                              PurchaseOrderID = s.PurchaseOrderID,
                              PONo = p.PoNo,
                              InvoiceFOB = s.InvoiceFOB
                          }).SingleOrDefault();
            return result;
        }
    }
}
