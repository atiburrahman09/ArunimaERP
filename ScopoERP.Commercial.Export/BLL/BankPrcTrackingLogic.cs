using ScopoERP.Commercial.ViewModel;
using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ScopoERP.Commercial.ViewModel.BankPrcTrackingViewModel;

namespace ScopoERP.Commercial.BLL
{
    public class BankPrcTrackingLogic
    {
        private UnitOfWork unitOfWork;
        private BankPRCTracking bankPRC;
        private invoice invoice;

        public BankPrcTrackingLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public List<BankPrcTrackingViewModel> GetAllBankPRC()
        {
            var result = from c in unitOfWork.BankPRCTrackingRepository.Get()                                                  
                         select new BankPrcTrackingViewModel
                         {
                             Id=c.Id,
                             TrackingNo=c.TrackingNo,
                             TrackingDate=c.TrackingDate,                             
                             CreatedBy = c.CreatedBy,
                             CreatedOn = c.CreatedOn,
                             UpdatedBy = c.UpdatedBy,
                             UpdatedOn = c.UpdatedOn
                         };

            return result.ToList();
        }
        
        public BankPrcTrackingViewModel GetBankPrcTrackingByID(int bankPrcTrackingID)
        {
            var result = (from c in unitOfWork.BankPRCTrackingRepository.Get()
                          where c.Id == bankPrcTrackingID
                          select new BankPrcTrackingViewModel
                          {
                              Id = c.Id,
                              TrackingNo = c.TrackingNo,
                              TrackingDate = c.TrackingDate,
                              
                              CreatedBy = c.CreatedBy,
                              CreatedOn = c.CreatedOn
                          }).SingleOrDefault();

            var invoiceList = (from p in unitOfWork.ExportInvoiceRepository.Get()
                               where p.BankPrcTrackingID == bankPrcTrackingID
                               select new InvoiceSummaries
                               {
                                   InvoiceID = p.InvoiceId,
                                   InvoiceNo = p.InvoiceNo
                               }).ToList();

            result.InvoiceList = invoiceList;

            return result;
        }

        public string CreateBankPRC(BankPrcTrackingViewModel bankPRCVM)
        {
            bankPRC = new BankPRCTracking()
            {
                TrackingNo = bankPRCVM.TrackingNo,
                TrackingDate = bankPRCVM.TrackingDate,
                CreatedBy = bankPRCVM.CreatedBy,
                CreatedOn = bankPRCVM.CreatedOn,                 
            };

            unitOfWork.BankPRCTrackingRepository.Insert(bankPRC);
            unitOfWork.Save();

            if (bankPRCVM.InvoiceList != null)
            {
                foreach (var item in bankPRCVM.InvoiceList)
                {
                    invoice = unitOfWork.ExportInvoiceRepository.Get().SingleOrDefault(x => x.InvoiceId == item.InvoiceID);
                    invoice.BankPrcTrackingID = bankPRC.Id;
                    unitOfWork.ExportInvoiceRepository.Update(invoice);
                }
            }

            unitOfWork.Save();

            return bankPRC.TrackingNo;
        }

        public string UpdateBankPRC(BankPrcTrackingViewModel bankPRCVM)
        {

            bankPRC = unitOfWork.BankPRCTrackingRepository.Get()
                .Where(x => x.Id == bankPRCVM.Id).SingleOrDefault();

            bankPRC.TrackingDate = bankPRCVM.TrackingDate;
            bankPRC.TrackingNo = bankPRCVM.TrackingNo;
            bankPRC.UpdatedBy = bankPRC.UpdatedBy;
            bankPRC.UpdatedOn = bankPRC.UpdatedOn;

            unitOfWork.BankPRCTrackingRepository.Update(bankPRC);

            var invoiceList = (from s in unitOfWork.ExportInvoiceRepository.Get()
                               where s.BankPrcTrackingID == bankPRC.Id
                               select s).ToList();
            foreach (var item in invoiceList)
            {
                invoice = unitOfWork.ExportInvoiceRepository.Get().SingleOrDefault(x => x.InvoiceId == item.InvoiceId);
                invoice.BankPrcTrackingID = null;

                unitOfWork.ExportInvoiceRepository.Update(invoice);
            }

            if (bankPRCVM.InvoiceList != null)
            {
                foreach (var item in bankPRCVM.InvoiceList)
                {
                    invoice = unitOfWork.ExportInvoiceRepository.Get().SingleOrDefault(x => x.InvoiceId == item.InvoiceID);
                    invoice.BankPrcTrackingID = bankPRC.Id;
                    unitOfWork.ExportInvoiceRepository.Update(invoice);
                }
            }

            unitOfWork.Save();

            return bankPRC.TrackingNo;
        }        

        public List<DropDownListViewModel> GetBankPrcTrackingDropDown()
        {
            var result = (from s in unitOfWork.BankPRCTrackingRepository.Get()
                          select new DropDownListViewModel
                          {
                              Value = s.Id,
                              Text = s.TrackingNo
                          }).ToList();

            return result;
        }
        
        public decimal? GetTotalInvoiceValue(int bankPrcTrackingID)
        {
            var result = (from s in unitOfWork.ExportInvoiceRepository.Get()
                          join sp in unitOfWork.ShipmentRepository.Get() on s.InvoiceId equals sp.InvoiceID
                          where s.BankPrcTrackingID == bankPrcTrackingID
                          select sp.InvoiceFOB).Sum();
            return result;
        }
           

        public List<ExportInvoiceViewModel> GetInvoiceListByBankPrcTrackingID(int bankPrcTrackingID)
        {
            var result = (from inv in unitOfWork.ExportInvoiceRepository.Get()
                          where inv.BankPrcTrackingID == bankPrcTrackingID
                          select new ExportInvoiceViewModel
                          {
                              InvoiceId = inv.InvoiceId,
                              InvoiceNo = inv.InvoiceNo,
                              InvoiceDate = inv.InvoiceDate,
                              EXP = inv.EXP,
                              EXPDate = inv.EXPDate,
                              InvoiceValue = inv.InvoiceValue,
                              FDBP_No = inv.FDBP_No
                          }).ToList();
            return result;

        }
    }
}
