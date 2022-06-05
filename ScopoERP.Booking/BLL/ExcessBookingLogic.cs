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
    public class ExcessBookingLogic
    {
        private UnitOfWork unitOfWork;
        private excessbooking excessBooking;
        private piinfo piInfo;

        public ExcessBookingLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void CreateExcessBooking(ExcessBookingViewModel excessBookingVM)
        {
            int piID = 0;

            var pi = (from s in unitOfWork.PIRepository.Get()
                      where s.PINo == excessBookingVM.ProformaInvoiceNo
                      select s).SingleOrDefault();

            if (pi == null)
            {
                piInfo = new piinfo
                {
                    ReferenceNo = GetNewReferenceNo(),
                    PINo = excessBookingVM.ProformaInvoiceNo,
                    Status = 1
                };

                unitOfWork.PIRepository.Insert(piInfo);

                piID = piInfo.PIID;
            }
            else
            {
                piID = pi.PIID;
            }

            excessBooking = new excessbooking
            {
                JobID = excessBookingVM.JobID,
                ProformaInvoiceID = piID,
                ItemID = excessBookingVM.ItemID,
                ItemColor = excessBookingVM.ItemColor,
                ItemSize = excessBookingVM.ItemSize,
                TotalQuantity = excessBookingVM.TotalQuantity,
                TotalPrice = excessBookingVM.TotalPrice,
                Status = true
            };

            unitOfWork.ExcessBookingRepository.Insert(excessBooking);
            unitOfWork.Save();
        }

        public string GetNewReferenceNo()
        {
            string newReferenceNo = string.Empty;

            var result = (from c in unitOfWork.PIRepository.Get()
                          orderby c.PIID descending
                          select c.ReferenceNo).FirstOrDefault();

            if (result == null)
            {
                newReferenceNo = "REF-" + DateTime.Now.Year.ToString() + "-00001";
            }
            else
            {
                string newReferenceNoInDigit = (Convert.ToInt32(result.Split('-').Last()) + 1).ToString().PadLeft(5, '0');

                newReferenceNo = "REF-" + DateTime.Now.Year.ToString() + "-" + newReferenceNoInDigit;
            }

            return newReferenceNo;
        }

        public void UpdateExcessBooking(ExcessBookingViewModel excessBookingVM)
        {
            excessBooking = new excessbooking
            {
                ExcessBookingID = excessBookingVM.ExcessBookingID,
                JobID = excessBookingVM.JobID,
                ProformaInvoiceID = excessBookingVM.ProformaInvoiceID,
                ItemID = excessBookingVM.ItemID,
                ItemColor = excessBookingVM.ItemColor,
                ItemSize = excessBookingVM.ItemSize,
                TotalQuantity = excessBookingVM.TotalQuantity,
                TotalPrice = excessBookingVM.TotalPrice,
                Status = true
            };

            unitOfWork.ExcessBookingRepository.Update(excessBooking);
            unitOfWork.Save();
        }

        public List<ExcessBookingViewModel> GetAllExcessBooking()
        {
            var result = (from s in unitOfWork.ExcessBookingRepository.Get()
                          join j in unitOfWork.JobRepository.Get() on s.JobID equals j.JobInfoId
                          join p in unitOfWork.PIRepository.Get() on s.ProformaInvoiceID equals p.PIID
                          join i in unitOfWork.ItemRepository.Get() on s.ItemID equals i.ItemId
                          where s.Status == true
                          select new ExcessBookingViewModel
                          {
                              ExcessBookingID = s.ExcessBookingID,
                              JobID = s.JobID,
                              JobNo = j.JobNo,
                              ProformaInvoiceID = s.ProformaInvoiceID,
                              ProformaInvoiceNo = p.PINo,
                              ItemID = s.ItemID,
                              ItemDescription = i.ItemDescription,
                              ItemColor = s.ItemColor,
                              ItemSize = s.ItemSize,
                              TotalQuantity = s.TotalQuantity,
                              TotalPrice = s.TotalPrice,
                              Status = true
                          }).ToList();

            return result;
        }

        public ExcessBookingViewModel GetExcessBookingByID(int id)
        {
            var result = (from s in unitOfWork.ExcessBookingRepository.Get()
                          join j in unitOfWork.JobRepository.Get() on s.JobID equals j.JobInfoId
                          join p in unitOfWork.PIRepository.Get() on s.ProformaInvoiceID equals p.PIID
                          join i in unitOfWork.ItemRepository.Get() on s.ItemID equals i.ItemId
                          where s.ExcessBookingID == id
                          select new ExcessBookingViewModel
                          {
                              ExcessBookingID = s.ExcessBookingID,
                              JobID = s.JobID,
                              JobNo = j.JobNo,
                              ProformaInvoiceID = s.ProformaInvoiceID,
                              ProformaInvoiceNo = p.PINo,
                              ItemID = s.ItemID,
                              ItemDescription = i.ItemDescription,
                              ItemColor = s.ItemColor,
                              ItemSize = s.ItemSize,
                              TotalQuantity = s.TotalQuantity,
                              TotalPrice = s.TotalPrice,
                              Status = true
                          }).SingleOrDefault();

            return result;
        }
    }
}
