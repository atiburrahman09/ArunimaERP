using ScopoERP.Commercial.ViewModel;
using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using ScopoERP.Store.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using ScopoERP.Reports.ViewModel;

namespace ScopoERP.Commercial.BLL
{
    public class ExportInvoiceLogic
    {
        private UnitOfWork unitOfWork;
        private invoice invoice;
        
        public ExportInvoiceLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        
        public void CreateExportInvoice(ExportInvoiceViewModel exportInvoiceVM)
        {
            byte[] fileBytes = null;

            if (exportInvoiceVM.InvoiceFile != null)
            {
                System.IO.Stream InvoiceFileInputStream = exportInvoiceVM.InvoiceFile.InputStream;
                fileBytes = new byte[InvoiceFileInputStream.Length];
            }

            invoice invoice = new invoice
            {
                InvoiceNo = exportInvoiceVM.InvoiceNo,
                InvoiceDate = exportInvoiceVM.InvoiceDate,
                InvoiceValue = exportInvoiceVM.InvoiceValue,

                JobInfoId = exportInvoiceVM.JobID,

                DocumentSentDate = exportInvoiceVM.DocumentSentDate,
                TradeCardInPutDate = exportInvoiceVM.TradeCardInPutDate,

                BL = exportInvoiceVM.BL,
                B_LDate = exportInvoiceVM.B_LDate,
                BLToBeEndorsedTo = exportInvoiceVM.BLToBeEndorsedTo,

                EXP = exportInvoiceVM.EXP,
                EXPDate = exportInvoiceVM.EXPDate,

                FCR = exportInvoiceVM.FCR,
                FCRDate = exportInvoiceVM.FCRDate,

                BookingExFactoryDate = exportInvoiceVM.BookingExFactoryDate,
                ExFactoryDate = exportInvoiceVM.ExFactoryDate,

                BankNegoDate = exportInvoiceVM.BankNegoDate,
                FDBP_No = exportInvoiceVM.FDBP_No,
                OnBoardDate = exportInvoiceVM.OnBoardDate,
                CODate = exportInvoiceVM.CODate,
                ICDate = exportInvoiceVM.ICDate,

                BLRealeaseDate = exportInvoiceVM.BLRealeaseDate,
                DocDespatchDate = exportInvoiceVM.DocDespatchDate,
                DocsPaymentApprovalDate=exportInvoiceVM.DocsPaymentApprovalDate,

                Courier = exportInvoiceVM.Courier,
                PaymentReceiveDate = exportInvoiceVM.PaymentReceiveDate,

                ShippingBill = exportInvoiceVM.ShippingBill,
                ShippingBillDate = exportInvoiceVM.ShippingBillDate,
                PortOfLoading = exportInvoiceVM.PortOfLoading,
                FinalDestination = exportInvoiceVM.FinalDestination,
                CountryName = exportInvoiceVM.CountryName,
                

                InvoiceFile = fileBytes
            };

           if(exportInvoiceVM.ShipmentList != null && exportInvoiceVM.ShipmentList.Count > 0)
            {
                foreach (var item in exportInvoiceVM.ShipmentList)
                {
                    var shipment = unitOfWork.ShipmentRepository.Get().Where(x => x.ShipmentID == item.ShipmentID).SingleOrDefault();
                    shipment.InvoiceID = item.InvoiceID;
                    shipment.InvoiceFOB = item.InvoiceFOB;

                    invoice.shipment.Add(shipment);
                }
            }
           

            unitOfWork.ExportInvoiceRepository.Insert(invoice);

            unitOfWork.Save();            
        }
        
        public void UpdateExportInvoice(ExportInvoiceViewModel exportInvoiceVM)
        {
            byte[] fileBytes = null;

            if (exportInvoiceVM.InvoiceFile != null)
            {
                System.IO.Stream InvoiceFileInputStream = exportInvoiceVM.InvoiceFile.InputStream;
                fileBytes = new byte[InvoiceFileInputStream.Length];
            }

            invoice = unitOfWork.ExportInvoiceRepository.GetById(exportInvoiceVM.InvoiceId);
            
            invoice.InvoiceId = exportInvoiceVM.InvoiceId;
            invoice.InvoiceNo = exportInvoiceVM.InvoiceNo;
            invoice.InvoiceDate = exportInvoiceVM.InvoiceDate;
            invoice.InvoiceValue = exportInvoiceVM.InvoiceValue;

            invoice.JobInfoId = exportInvoiceVM.JobID;

            invoice.DocumentSentDate = exportInvoiceVM.DocumentSentDate;
            invoice.TradeCardInPutDate = exportInvoiceVM.TradeCardInPutDate;

            invoice.BL = exportInvoiceVM.BL;
            invoice.B_LDate = exportInvoiceVM.B_LDate;
            invoice.BLToBeEndorsedTo = exportInvoiceVM.BLToBeEndorsedTo;

            invoice.EXP = exportInvoiceVM.EXP;
            invoice.EXPDate = exportInvoiceVM.EXPDate;

            invoice.FCR = exportInvoiceVM.FCR;
            invoice.FCRDate = exportInvoiceVM.FCRDate;

            invoice.BookingExFactoryDate = exportInvoiceVM.BookingExFactoryDate;
            invoice.ExFactoryDate = exportInvoiceVM.ExFactoryDate;

            invoice.BankNegoDate = exportInvoiceVM.BankNegoDate;
            invoice.FDBP_No = exportInvoiceVM.FDBP_No;
            invoice.OnBoardDate = exportInvoiceVM.OnBoardDate;
            invoice.CODate = exportInvoiceVM.CODate;
            invoice.ICDate = exportInvoiceVM.ICDate;

            invoice.BLRealeaseDate = exportInvoiceVM.BLRealeaseDate;
            invoice.DocDespatchDate = exportInvoiceVM.DocDespatchDate;
            invoice.DocsPaymentApprovalDate = exportInvoiceVM.DocsPaymentApprovalDate;

            invoice.Courier = exportInvoiceVM.Courier;
            invoice.PaymentReceiveDate = exportInvoiceVM.PaymentReceiveDate;

            invoice.ShippingBill = exportInvoiceVM.ShippingBill;
            invoice.ShippingBillDate = exportInvoiceVM.ShippingBillDate;
            invoice.PortOfLoading = exportInvoiceVM.PortOfLoading;
            invoice.FinalDestination = exportInvoiceVM.FinalDestination;
            invoice.CountryName = exportInvoiceVM.CountryName;


            if (exportInvoiceVM.InvoiceFile != null)
            {
                invoice.InvoiceFile = fileBytes;
            }

            unitOfWork.ExportInvoiceRepository.Update(invoice);

            var shipmentList = unitOfWork.ShipmentRepository.Get().Where(x => x.InvoiceID == exportInvoiceVM.InvoiceId).ToList();

            foreach (var item in shipmentList)
            {
                item.InvoiceID = null;
                item.InvoiceFOB = null;

                unitOfWork.ShipmentRepository.Update(item);

            }

            if (exportInvoiceVM.ShipmentList != null)
            {
                foreach (var item in exportInvoiceVM.ShipmentList)
                {
                    var shipment = unitOfWork.ShipmentRepository.Get().Where(x => x.ShipmentID == item.ShipmentID).SingleOrDefault();
                    shipment.InvoiceID = exportInvoiceVM.InvoiceId;
                    shipment.InvoiceFOB = item.InvoiceFOB;

                    unitOfWork.ShipmentRepository.Update(shipment);
                }
            }

            unitOfWork.Save();
        }
        
        public List<ExportInvoiceViewModel> GetAllExportInvoice()
        {
            var result = (from s in unitOfWork.ExportInvoiceRepository.Get()
                          join bfrw in unitOfWork.BankForwardingRepository.Get() on s.BankForwardingID equals bfrw.BankForwardingID into bg
                          from bf in bg.DefaultIfEmpty()
                          join rz in unitOfWork.RealizationRepository.Get() on bf.BankForwardingID equals rz.BankForwardingID into rg
                          from r in rg.DefaultIfEmpty()
                          orderby s.InvoiceId descending
                          select new ExportInvoiceViewModel
                          {
                              InvoiceId = s.InvoiceId,
                              InvoiceNo = s.InvoiceNo,
                              InvoiceDate = s.InvoiceDate,
                              InvoiceValue = s.InvoiceValue,
                              JobID = s.JobInfoId,

                              DocumentSentDate = s.DocumentSentDate,
                              TradeCardInPutDate = s.TradeCardInPutDate,

                              BL = s.BL,
                              BLRealeaseDate = s.BLRealeaseDate,
                              B_LDate = s.B_LDate,
                              BLToBeEndorsedTo = s.BLToBeEndorsedTo,

                              EXP = s.EXP,
                              EXPDate = s.EXPDate,

                              FCR = s.FCR,
                              FCRDate = s.FCRDate,

                              BookingExFactoryDate = s.BookingExFactoryDate,
                              ExFactoryDate = s.ExFactoryDate,
                              BankNegoDate = s.BankNegoDate,
                              FDBP_No = string.IsNullOrEmpty(bf.FDBPNo) ? s.FDBP_No : bf.FDBPNo,
                              OnBoardDate = s.OnBoardDate,
                              CODate = s.CODate,
                              ICDate = s.ICDate,
                              DocDespatchDate = s.DocDespatchDate,
                              DocsPaymentApprovalDate=s.DocsPaymentApprovalDate,
                              Courier = s.Courier,
                              PaymentReceiveDate = r.RealizationDate == null ? s.PaymentReceiveDate : r.RealizationDate,

                              ShippingBill = s.ShippingBill,
                              ShippingBillDate = s.ShippingBillDate,
                              PortOfLoading = s.PortOfLoading,
                              FinalDestination = s.FinalDestination,
                              CountryName = s.CountryName
                          }).ToList();

            return result;
        }
        


        public List<ExportInvoiceViewModel> GetAllExportInvoiceByJob(int jobID)
        {
            var result = (from s in unitOfWork.ExportInvoiceRepository.Get()
                          where s.JobInfoId == jobID
                          orderby s.InvoiceId descending
                          select new ExportInvoiceViewModel
                          {
                              InvoiceId = s.InvoiceId,
                              InvoiceNo = s.InvoiceNo,
                              InvoiceDate = s.InvoiceDate,
                              InvoiceValue = s.InvoiceValue,

                              JobID = s.JobInfoId,

                              DocumentSentDate = s.DocumentSentDate,
                              TradeCardInPutDate = s.TradeCardInPutDate,

                              BL = s.BL,
                              BLRealeaseDate = s.BLRealeaseDate,
                              B_LDate = s.B_LDate,
                              BLToBeEndorsedTo = s.BLToBeEndorsedTo,

                              EXP = s.EXP,
                              EXPDate = s.EXPDate,

                              FCR = s.FCR,
                              FCRDate = s.FCRDate,

                              BookingExFactoryDate = s.BookingExFactoryDate,
                              ExFactoryDate = s.ExFactoryDate,
                              BankNegoDate = s.BankNegoDate,
                              
                              OnBoardDate = s.OnBoardDate,
                              CODate = s.CODate,
                              ICDate = s.ICDate,
                              DocDespatchDate = s.DocDespatchDate,    
                              DocsPaymentApprovalDate=s.DocsPaymentApprovalDate,                          

                              ShippingBill = s.ShippingBill,
                              ShippingBillDate = s.ShippingBillDate,
                              PortOfLoading = s.PortOfLoading,
                              FinalDestination = s.FinalDestination,
                              CountryName = s.CountryName,
                              PaymentReceiveDate =s.PaymentReceiveDate 
                          }).Distinct().ToList();

            return result;
        }

        public ExportInvoiceViewModel GetExportInvoiceByID(int invoiceID)
        {
            var result = (from s in unitOfWork.ExportInvoiceRepository.Get()
                          join bfrw in unitOfWork.BankForwardingRepository.Get() on s.BankForwardingID equals bfrw.BankForwardingID into bg
                          from bf in bg.DefaultIfEmpty()
                          where s.InvoiceId == invoiceID
                          select new ExportInvoiceViewModel
                          {
                              InvoiceId = s.InvoiceId,
                              InvoiceNo = s.InvoiceNo,
                              InvoiceDate = s.InvoiceDate,
                              InvoiceValue = s.InvoiceValue,

                              JobID = s.JobInfoId,

                              DocumentSentDate = s.DocumentSentDate,
                              TradeCardInPutDate = s.TradeCardInPutDate,

                              BL = s.BL,
                              BLRealeaseDate = s.BLRealeaseDate,
                              B_LDate = s.B_LDate,
                              BLToBeEndorsedTo = s.BLToBeEndorsedTo,

                              EXP = s.EXP,
                              EXPDate = s.EXPDate,

                              FCR = s.FCR,
                              FCRDate = s.FCRDate,

                              BookingExFactoryDate = s.BookingExFactoryDate,
                              ExFactoryDate = s.ExFactoryDate,
                              BankNegoDate = s.BankNegoDate,
                              FDBP_No = string.IsNullOrEmpty(bf.FDBPNo) ? s.FDBP_No : bf.FDBPNo,
                              OnBoardDate = s.OnBoardDate,
                              CODate = s.CODate,
                              ICDate = s.ICDate,
                              DocDespatchDate = s.DocDespatchDate,
                              DocsPaymentApprovalDate=s.DocsPaymentApprovalDate,
                              Courier = s.Courier,
                              PaymentReceiveDate = s.PaymentReceiveDate,

                              ShippingBill = s.ShippingBill,
                              ShippingBillDate = s.ShippingBillDate,
                              PortOfLoading = s.PortOfLoading,
                              FinalDestination = s.FinalDestination,
                              CountryName = s.CountryName,

                              BankForwardingID = s.BankForwardingID
                          }).SingleOrDefault();

           
            DateTime? realizationDate = (from s in unitOfWork.RealizationRepository.Get()
                                         where s.BankForwardingID == result.BankForwardingID
                                         select s.RealizationDate).FirstOrDefault();

            result.PaymentReceiveDate = realizationDate == null ? result.PaymentReceiveDate : realizationDate;

            return result;
        }
        
        public bool IsUniqueInvoiceNo(string invoiceNo, Nullable<int> invoiceID = null)
        {
            IQueryable<int> result;

            if (invoiceID == null)
            {
                result = from s in unitOfWork.ExportInvoiceRepository.Get()
                         where s.InvoiceNo == invoiceNo
                         select s.InvoiceId;
            }
            else
            {
                result = from s in unitOfWork.ExportInvoiceRepository.Get()
                         where s.InvoiceNo == invoiceNo & s.InvoiceId != invoiceID
                         select s.InvoiceId;
            }

            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }
                
        public List<DropDownListViewModel> GetInvoiceDropDown(int jobID)
        {
            var result = (from i in unitOfWork.ExportInvoiceRepository.Get()                          
                          where i.JobInfoId == jobID
                          select new DropDownListViewModel
                          {
                              Value = i.InvoiceId,
                              Text = i.InvoiceNo
                          }).Distinct().ToList();

            return result;
        }

       
        //----New Method

        public List<InvoiceSummary> GetInvoiceSummaryListByJob(int jobID)
        {
            var result = (from inv in unitOfWork.ExportInvoiceRepository.Get()
                          join sp in unitOfWork.ShipmentRepository.Get() on inv.InvoiceId equals sp.InvoiceID
                          join p in unitOfWork.PurchaseOrderRepository.Get() on sp.PurchaseOrderID equals p.PoStyleId
                          where p.JobId == jobID
                          select new InvoiceSummary
                          {
                              InvoiceID = inv.InvoiceId,
                              InvoiceNo = inv.InvoiceNo,
                              InvoiceDate = inv.InvoiceDate
                          }).Distinct().ToList();
            return result;
        }
        
        public List<InvoiceSummary> GetInvoiceListByBankForwardingID(int bankForwardingID)
        {
            var result = (from inv in unitOfWork.ExportInvoiceRepository.Get()
                          where inv.BankForwardingID == bankForwardingID
                          select new InvoiceSummary
                          {
                              InvoiceID = inv.InvoiceId,
                              InvoiceNo = inv.InvoiceNo
                          }).ToList();
            return result;
                        
        }

        public List<ExportInvoiceViewModel> GetAllExportInvoiceForBankPRC(string exp)
        {
            var result = (from s in unitOfWork.ExportInvoiceRepository.Get()
                          join bfrw in unitOfWork.BankForwardingRepository.Get() on s.BankForwardingID equals bfrw.BankForwardingID into bg
                          from bf in bg.DefaultIfEmpty()
                          join rz in unitOfWork.RealizationRepository.Get() on bf.BankForwardingID equals rz.BankForwardingID into rg
                          from r in rg.DefaultIfEmpty()
                          orderby s.InvoiceId descending
                          where s.EXP.Contains(exp)
                          select new ExportInvoiceViewModel
                          {
                              InvoiceId = s.InvoiceId,
                              InvoiceNo = s.InvoiceNo,
                              InvoiceDate = s.InvoiceDate,
                              InvoiceValue = s.InvoiceValue,
                              JobID = s.JobInfoId,

                              EXP = s.EXP,
                              EXPDate = s.EXPDate,

                              FDBP_No = string.IsNullOrEmpty(bf.FDBPNo) ? s.FDBP_No : bf.FDBPNo
                          }).ToList();

            return result;
        }

        public List<CommercialInvoiceReportVM> GetInvoiceSummaryByInvoiceId(int invoiceId)
        {
            var result = (from inv in unitOfWork.ExportInvoiceRepository.Get()
                          join job in unitOfWork.JobRepository.Get() on inv.JobInfoId equals job.JobInfoId
                          join bank in unitOfWork.BankRepository.Get() on job.BankID equals bank.BankID
                          where inv.InvoiceId==invoiceId
                          select new CommercialInvoiceReportVM
                          {
                              InvoiceNo=inv.InvoiceNo,
                              InvoiceDate=inv.InvoiceDate,
                              ExportDate=inv.EXPDate,
                              ExportNo=inv.EXP,
                              Destination=inv.FinalDestination,
                              ContractNo=job.ContractNo,
                              BankName=bank.BankName,
                              BankAddress=bank.BankAddress,
                              PortOfLoading=inv.PortOfLoading,
                              AccountNo=bank.AccountNo,
                              SwiftCode=bank.SwiftCode,
                              BuyerAddress = job.ShippedTo,
                              NotifyParty = job.NotifyParty,
                              AlsoNotifyParty = job.AlsoNotifyParty,
                              CATNO = job.CATNO
                          }).ToList();

            var buyerData = (from inv in unitOfWork.ExportInvoiceRepository.Get()
                             join sp in unitOfWork.ShipmentRepository.Get() on inv.InvoiceId equals sp.InvoiceID
                             join p in unitOfWork.PurchaseOrderRepository.Get() on sp.PurchaseOrderID equals p.PoStyleId
                             join s in unitOfWork.StyleRepository.Get() on p.StyleId equals s.StyleId
                             join b in unitOfWork.BuyerRepository.Get() on s.BuyerId equals b.BuyerId
                             where inv.InvoiceId == invoiceId
                             select new CommercialInvoiceReportVM
                             {
                                 BuyerName=b.BuyerName,
                                
                             }).Take(1).ToList();

            result[0].BuyerName = buyerData[0].BuyerName;

            return result;
        }

        public List<CommercialInvoiceDescriptionVM> GetDetailsDataByInvoiceId(int invoiceId)
        {
            var result = (from inv in unitOfWork.ExportInvoiceRepository.Get()
                          join sp in unitOfWork.ShipmentRepository.Get() on inv.InvoiceId equals sp.InvoiceID
                          join p in unitOfWork.PurchaseOrderRepository.Get() on sp.PurchaseOrderID equals p.PoStyleId
                          join s in unitOfWork.StyleRepository.Get() on p.StyleId equals s.StyleId
                          where inv.InvoiceId == invoiceId
                          select new CommercialInvoiceDescriptionVM
                          {
                              PONo=p.PoNo,
                              QTY=p.OrderQuantity,
                              StyleNo=s.StyleNo,
                              UnitPrice=sp.InvoiceFOB      ,
                              CTNQuantity=sp.CartoonQuantity                       
                              
                          }).ToList();
            return result;
        }
    }
}
