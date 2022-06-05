using ScopoERP.Commercial.ViewModel;
using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Commercial.BankForwardingL
{
    public class BankForwardingLogic
    {
        private UnitOfWork unitOfWork;
        private bankforwarding bankForwarding;
        private invoice invoice;

        public BankForwardingLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public List<BankForwardingViewModel> GetAllBankForwarding()
        {
            var result = from c in unitOfWork.BankForwardingRepository.Get()
                         join j in unitOfWork.JobRepository.Get()
                         on c.JobID equals j.JobInfoId
                         select new BankForwardingViewModel
                         {
                             BankForwardingID = c.BankForwardingID,
                             BankForwardingNo = c.BankForwardingNo,
                             BankForwardingDate = c.BankForwardingDate,
                             FDBPNo = c.FDBPNo,
                             Courier = c.Courier,
                             JobID = c.JobID,
                             JobNo = j.JobNo,
                             UserID = c.UserID,
                             SetupDate = c.SetupDate
                         };

            return result.ToList();
        }

        public List<BankForwardingViewModel> GetAllBankForwarding(int jobID)
        {
            var result = from c in unitOfWork.BankForwardingRepository.Get()
                         join j in unitOfWork.JobRepository.Get() on c.JobID equals j.JobInfoId
                         where c.JobID == jobID
                         select new BankForwardingViewModel
                         {
                             BankForwardingID = c.BankForwardingID,
                             BankForwardingNo = c.BankForwardingNo,
                             BankForwardingDate = c.BankForwardingDate,
                             Courier=c.Courier,
                             FDBPNo = c.FDBPNo,
                             JobID = c.JobID,
                             JobNo = j.JobNo,
                             UserID = c.UserID,
                             SetupDate = c.SetupDate
                         };

            return result.ToList();
        }

        public object GetFDBPDropDownByJobID(int jobID)
        {
            var result = (from s in unitOfWork.BankForwardingRepository.Get()
                          where s.JobID == jobID
                          select new DropDownListViewModel
                          {
                              Value = s.BankForwardingID,
                              Text = s.FDBPNo
                          }).ToList();

            return result;
        }

        public BankForwardingViewModel GetBankForwardingByID(int bankForwardingID)
        {
            var result = (from c in unitOfWork.BankForwardingRepository.Get()
                          where c.BankForwardingID == bankForwardingID
                          select new BankForwardingViewModel
                          {
                              BankForwardingID = c.BankForwardingID,
                              BankForwardingNo = c.BankForwardingNo,
                              BankForwardingDate = c.BankForwardingDate,
                              FDBPNo = c.FDBPNo,
                              Courier = c.Courier,
                              JobID = c.JobID,
                              UserID = c.UserID,
                              SetupDate = c.SetupDate
                          }).SingleOrDefault();

            var invoiceList = (from p in unitOfWork.ExportInvoiceRepository.Get()
                               where p.BankForwardingID == bankForwardingID
                               select new InvoiceSummary
                               {
                                   InvoiceID = p.InvoiceId,
                                   InvoiceNo = p.InvoiceNo
                               }).ToList();

            result.InvoiceList = invoiceList;

            return result;
        }

        public string CreateBankForwarding(BankForwardingViewModel bankForwardingVM, int userID)
        {
            this.bankForwarding = new bankforwarding()
            {
                BankForwardingNo = GetNewForwardingNo(),
                BankForwardingDate = bankForwardingVM.BankForwardingDate,
                FDBPNo = bankForwardingVM.FDBPNo,
                JobID = bankForwardingVM.JobID,
                Courier=bankForwardingVM.Courier,
                Status = true,
                UserID = userID,
                SetupDate = DateTime.Now
            };

            unitOfWork.BankForwardingRepository.Insert(bankForwarding);
            unitOfWork.Save();

            if (bankForwardingVM.InvoiceList != null)
            {
                foreach (var item in bankForwardingVM.InvoiceList)
                {
                    invoice = unitOfWork.ExportInvoiceRepository.Get().SingleOrDefault(x => x.InvoiceId == item.InvoiceID);
                    invoice.BankForwardingID = bankForwarding.BankForwardingID;

                    unitOfWork.ExportInvoiceRepository.Update(invoice);
                }
            }

            unitOfWork.Save();

            return bankForwarding.BankForwardingNo;
        }

        public string UpdateBankForwarding(BankForwardingViewModel bankForwardingVM)
        {
            this.bankForwarding = new bankforwarding()
            {
                BankForwardingID = bankForwardingVM.BankForwardingID,
                BankForwardingNo = bankForwardingVM.BankForwardingNo,
                BankForwardingDate = bankForwardingVM.BankForwardingDate,
                FDBPNo = bankForwardingVM.FDBPNo,
                Courier = bankForwardingVM.Courier,
                JobID = bankForwardingVM.JobID,
                Status = true,
                UserID = bankForwardingVM.UserID,
                SetupDate = bankForwardingVM.SetupDate
            };
            unitOfWork.BankForwardingRepository.Update(bankForwarding);

            var invoiceList = (from s in unitOfWork.ExportInvoiceRepository.Get()
                               where s.BankForwardingID == bankForwardingVM.BankForwardingID
                               select s).ToList();



            foreach (var item in invoiceList)
            {
                invoice = unitOfWork.ExportInvoiceRepository.Get().SingleOrDefault(x => x.InvoiceId == item.InvoiceId);
                invoice.BankForwardingID = null;

                unitOfWork.ExportInvoiceRepository.Update(invoice);
            }

            if (bankForwardingVM.InvoiceList != null)
            {
                foreach (var item in bankForwardingVM.InvoiceList)
                {
                    invoice = unitOfWork.ExportInvoiceRepository.Get().SingleOrDefault(x => x.InvoiceId == item.InvoiceID);
                    invoice.BankForwardingID = bankForwarding.BankForwardingID;

                    unitOfWork.ExportInvoiceRepository.Update(invoice);
                }
            }

            unitOfWork.Save();

            return bankForwarding.BankForwardingNo;
        }

        public string GetNewForwardingNo()
        {
            string newBankForwardingNo = string.Empty;

            var result = (from c in unitOfWork.BankForwardingRepository.Get()
                          orderby c.BankForwardingID descending
                          select c.BankForwardingNo).FirstOrDefault();

            if (result == null)
            {
                newBankForwardingNo = "FRD-" + DateTime.Now.Year.ToString() + "-00001";
            }
            else
            {
                string newBankForwardingNoInDigit = (Convert.ToInt32(result.Split('-').Last()) + 1).ToString().PadLeft(5, '0');

                newBankForwardingNo = "FRD-" + DateTime.Now.Year.ToString() + "-" + newBankForwardingNoInDigit;
            }

            return newBankForwardingNo;
        }

        public List<DropDownListViewModel> GetBankForwardingDropDown()
        {
            var result = (from s in unitOfWork.BankForwardingRepository.Get()
                          select new DropDownListViewModel
                          {
                              Value = s.BankForwardingID,
                              Text = s.BankForwardingNo
                          }).ToList();

            return result;
        }

        public List<DropDownListViewModel> GetFDBPDropDown()
        {
            var result = (from s in unitOfWork.BankForwardingRepository.Get()
                          where !string.IsNullOrEmpty(s.FDBPNo)
                          select new DropDownListViewModel
                          {
                              Value = s.BankForwardingID,
                              Text = s.FDBPNo
                          }).ToList();

            return result;
        }

        public List<DropDownListViewModel> GetFDBPDropDown(int jobID)
        {
            var result = (from s in unitOfWork.BankForwardingRepository.Get()
                          where s.JobID == jobID && !string.IsNullOrEmpty(s.FDBPNo)
                          select new DropDownListViewModel
                          {
                              Value = s.BankForwardingID,
                              Text = s.FDBPNo
                          }).ToList();

            return result;
        }

        public decimal? GetTotalInvoiceValue(int bankForwardingID)
        {
            var result = (from s in unitOfWork.ExportInvoiceRepository.Get()
                          join sp in unitOfWork.ShipmentRepository.Get() on s.InvoiceId equals sp.InvoiceID
                          where s.BankForwardingID == bankForwardingID
                          select sp.InvoiceFOB).Sum();
            return result;
        }


        //----New Method
        // this method uses existing Create and Update method 
        public string SaveBankForwarding(BankForwardingViewModel bankForwardingVM, int userID)
        {
            if(bankForwardingVM.BankForwardingID != 0)
            {
                //update
                UpdateBankForwarding(bankForwardingVM);
                return bankForwardingVM.BankForwardingNo;
                
            }
            else
            {
                //create
                string bankforwardingNo= CreateBankForwarding(bankForwardingVM, userID);
                return bankforwardingNo;
            }
        }


    }
}
