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
    public class RequisitionLogic
    {
        private UnitOfWork unitOfWork;
        private requisition requisition;
        private piinfo piInfo;

        public RequisitionLogic(UnitOfWork unitOfWork, requisition requisition)
        {
            this.unitOfWork = unitOfWork;
        }

        public List<RequisitionViewModel> GetAllRequisition() {
            var result = from c in unitOfWork.RequisitionRepository.Get()
                         join j in unitOfWork.JobRepository.Get()
                         on c.JobID equals j.JobInfoId
                         select new RequisitionViewModel
                         {
                             RequisitionID = c.RequisitionID,
                             RequisitionNo = c.RequisitionNo,
                             RequisitionSerial = c.RequisitionSerial,
                             RequisitionDate = c.RequisitionDate,
                             JobID = c.JobID,
                             JobNo = j.JobNo,
                             AccountID = c.AccountID,
                             UserID = c.UserID
                         };

            return result.ToList();
        }

        public RequisitionViewModel GetRequisitionByID(int requisitionID) {
            var result = (from c in unitOfWork.RequisitionRepository.Get()
                         where c.RequisitionID == requisitionID
                         select new RequisitionViewModel
                         {
                             RequisitionID = c.RequisitionID,
                             RequisitionNo = c.RequisitionNo,
                             RequisitionSerial = c.RequisitionSerial,
                             RequisitionDate = c.RequisitionDate,
                             JobID = c.JobID,
                             AccountID = c.AccountID,
                             UserID = c.UserID,
                             SetupDate = c.SetupDate
                         }).SingleOrDefault();

            var piList = (from p in unitOfWork.PIRepository.Get()
                          join b in unitOfWork.BookingRepository.Get()
                          on p.PIID equals b.PIId
                          where p.RequisitionID == requisitionID
                          group b by new { p.PIID, p.PINo } into s
                          select new PISummary
                          {
                              PIID = s.Key.PIID,
                              PINo = s.Key.PINo,
                              PIValue = s.Sum(x => x.TotalQuantity * x.UnitPrice)
                          }).ToList();

            var piListFromExcessBooking = (from p in unitOfWork.PIRepository.Get()
                                           join b in unitOfWork.ExcessBookingRepository.Get()
                                           on p.PIID equals b.ProformaInvoiceID
                                           where p.RequisitionID == requisitionID
                                           group b by new { p.PIID, p.PINo } into s
                                           select new PISummary
                                           {
                                               PIID = s.Key.PIID,
                                               PINo = s.Key.PINo,
                                               PIValue = s.Sum(x => x.TotalPrice)
                                           }).ToList();

            piList.AddRange(piListFromExcessBooking);

            result.PIList = piList;

            return result;
        }

        public string CreateRequisition(RequisitionViewModel requisitionVM, int accountID, int userID)
        {
            this.requisition = new requisition()
            {
                RequisitionNo = GetNewReferenceNo(),
                RequisitionSerial = requisitionVM.RequisitionSerial,
                RequisitionDate = requisitionVM.RequisitionDate,
                JobID = requisitionVM.JobID,
                AccountID = accountID,
                Status = true,
                UserID = userID,
                SetupDate = DateTime.Now,
                SupplierID=requisitionVM.SupplierID
            };

            unitOfWork.RequisitionRepository.Insert(requisition);
            unitOfWork.Save();

            if (requisitionVM.PIList != null)
            {
                foreach (var item in requisitionVM.PIList)
                {
                    piInfo = unitOfWork.PIRepository.Get().SingleOrDefault(x => x.PIID == item.PIID);
                    piInfo.RequisitionID = requisition.RequisitionID;

                    unitOfWork.PIRepository.Update(piInfo);
                }
            }

            unitOfWork.Save();

            return requisition.RequisitionNo;
        }

        public List<PISummary> GetPISummaryByReqID(int requisitionID)
        {
            var result = (from pi in unitOfWork.PIRepository.Get()
                          where pi.RequisitionID == requisitionID
                          select new PISummary
                          {
                              PIID = pi.PIID,
                              PINo = pi.PINo
                          }).ToList();

            return result;
        }

        public List<PISummary> GetPISummaryByJobID(int jobID)
        {
            var result = (from s in unitOfWork.PIRepository.Get()
                          join w in unitOfWork.BookingRepository.Get() on s.PIID equals w.PIId
                          join p in unitOfWork.PurchaseOrderRepository.Get() on w.PurchaseOrderID equals p.PoStyleId
                          where s.PINo != null && s.PINo != "" && p.JobId == jobID
                          select new PISummary
                          {
                              PIID = s.PIID,
                              PINo = s.PINo
                          }).Distinct().ToList();
            return result;
        }

        public List<RequisitionViewModel> GetRequisitionByJobID(int jobID)
        {
            var result = from c in unitOfWork.RequisitionRepository.Get()
                         join j in unitOfWork.JobRepository.Get()
                         on c.JobID equals j.JobInfoId
                         where c.JobID == jobID
                         select new RequisitionViewModel
                         {
                             RequisitionID = c.RequisitionID,
                             RequisitionNo = c.RequisitionNo,
                             RequisitionSerial = c.RequisitionSerial,
                             RequisitionDate = c.RequisitionDate,
                             JobID = c.JobID,
                             JobNo = j.JobNo,
                             AccountID = c.AccountID,
                             UserID = c.UserID,
                             SupplierID=c.SupplierID
                         };

            return result.ToList();
        }

        public string UpdateRequisition(RequisitionViewModel requisitionVM)
        {
            this.requisition = new requisition()
            {
                RequisitionID = requisitionVM.RequisitionID,
                RequisitionNo = requisitionVM.RequisitionNo,
                RequisitionSerial = requisitionVM.RequisitionSerial,
                RequisitionDate = requisitionVM.RequisitionDate,
                JobID = requisitionVM.JobID,
                AccountID = requisitionVM.AccountID,
                Status = true,
                UserID = requisitionVM.UserID,
                SetupDate = DateTime.Now,
                SupplierID=requisitionVM.SupplierID
            };
            unitOfWork.RequisitionRepository.Update(requisition);

            var piList = (from s in unitOfWork.PIRepository.Get()
                          where s.RequisitionID == requisitionVM.RequisitionID
                          select s).ToList();

            foreach (var item in piList)
            {
                piInfo = unitOfWork.PIRepository.Get().SingleOrDefault(x => x.PIID == item.PIID);
                piInfo.RequisitionID = null;
                unitOfWork.PIRepository.Update(piInfo);
            }

            if (requisitionVM.PIList != null)
            {
                foreach (var item in requisitionVM.PIList)
                {
                    piInfo = unitOfWork.PIRepository.Get().SingleOrDefault(x => x.PIID == item.PIID);
                    piInfo.RequisitionID = requisitionVM.RequisitionID;
                    unitOfWork.PIRepository.Update(piInfo);
                }
            }

            unitOfWork.Save();

            return requisition.RequisitionNo;
        }

        public string GetNewReferenceNo()
        {
            string newRequisitionNo = string.Empty;

            var result = (from c in unitOfWork.RequisitionRepository.Get()
                          orderby c.RequisitionID descending
                          select c.RequisitionNo).FirstOrDefault();

            if (result == null)
            {
                newRequisitionNo = "REQ-" + DateTime.Now.Year.ToString() + "-00001";
            }
            else
            {
                string newRequisitionNoInDigit = (Convert.ToInt32(result.Split('-').Last()) + 1).ToString().PadLeft(5, '0');

                newRequisitionNo = "REQ-" + DateTime.Now.Year.ToString() + "-" + newRequisitionNoInDigit;
            }
            return newRequisitionNo;
        }

        public List<DropDownListViewModel> GetRequisitionDropDown()
        {
            var result = (from s in unitOfWork.RequisitionRepository.Get()
                          select new DropDownListViewModel
                          {
                              Value = s.RequisitionID,
                              Text = s.RequisitionNo
                          }).ToList();

            return result;
        }

        public List<DropDownListViewModel> GetRequisitionDropDown(int jobID)
        {
            var result = (from s in unitOfWork.RequisitionRepository.Get()
                          where s.JobID == jobID
                          select new DropDownListViewModel
                          {
                              Value = s.RequisitionID,
                              Text = s.RequisitionNo
                          }).ToList();

            return result;
        }

        public DateTime GetLastRequisitionDate(int jobID, int supplierID)
        {
            DateTime? requisitionDate = (from s in unitOfWork.RequisitionRepository.Get()
                                         where s.JobID == jobID & s.SupplierID == supplierID
                                         select s.RequisitionDate).Max();

            return DateTime.Now;
        }
    }
}
