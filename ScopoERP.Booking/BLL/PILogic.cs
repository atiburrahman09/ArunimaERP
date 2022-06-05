using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using ScopoERP.MaterialManagement.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.MaterialManagement.BLL
{
    public class PILogic
    {
        private UnitOfWork unitOfWork;
        private piinfo pi;

        public PILogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void CreatePI(PIViewModel piVM)
        {
            pi = new piinfo
            {
                ReferenceNo = piVM.ReferenceNo,
                PINo = piVM.PINo,
                PIDate = piVM.PIDate,
                BackToBackLCID = piVM.BackToBackLCID,
                Status = piVM.Status,
                ApproximateInhouseDate=piVM.ApproximateInHouseDate,
                LoanFromJobID=piVM.LoanFromJobID,
                SupplierID=piVM.SupplierID
            };

            unitOfWork.PIRepository.Insert(pi);
            unitOfWork.Save();
        }

        public void UpdatePI(PIViewModel piVM)
        {
            var pi = unitOfWork.PIRepository.Get().SingleOrDefault(x => x.PIID == piVM.PIID);

            pi.PINo = piVM.PINo;
            pi.PIDate = piVM.PIDate;
            pi.SupplierID = piVM.SupplierID;
            pi.LoanFromJobID = piVM.LoanFromJobID;
            pi.ApproximateInhouseDate = piVM.ApproximateInHouseDate;

            unitOfWork.PIRepository.Update(pi);
            unitOfWork.Save();
        }

        public void DeletePI(int piID)
        {
            unitOfWork.PIRepository.Delete(new piinfo { PIID = piID });
            unitOfWork.Save();
        }

        public List<PIViewModel> GetAllPI(int accountID)
        {
            var piList = (from c in unitOfWork.PIRepository.Get()
                          join bk in unitOfWork.BookingRepository.Get() on c.PIID equals bk.PIId
                          join p in unitOfWork.PurchaseOrderRepository.Get() on bk.PurchaseOrderID equals p.PoStyleId
                          join st in unitOfWork.StyleRepository.Get() on p.StyleId equals st.StyleId
                          join j in unitOfWork.JobRepository.Get() on p.JobId equals j.JobInfoId
                          join sp in unitOfWork.SupplierRepository.Get() on c.SupplierID equals sp.SupplierId into sg
                          join bc in unitOfWork.BackToBackLCRepository.Get() on c.BackToBackLCID equals bc.BackToBackLCID into g
                          join jb in unitOfWork.JobRepository.Get() on c.LoanFromJobID equals jb.JobInfoId into jg
                          join rq in unitOfWork.RequisitionRepository.Get() on c.RequisitionID equals rq.RequisitionID into rg
                          from s in sg.DefaultIfEmpty()
                          from b in g.DefaultIfEmpty()
                          from jl in jg.DefaultIfEmpty()
                          from r in rg.DefaultIfEmpty()
                          select new PIViewModel
                          {
                              JobNo = j.JobNo,
                              PIID = c.PIID,
                              ReferenceNo = c.ReferenceNo,

                              RequisitionNo = r.RequisitionNo,
                              RequisitionDate = r.RequisitionDate,

                              PINo = c.PINo,
                              PIDate = c.PIDate,
                              SupplierID = c.SupplierID,
                              SupplierName = s.SupplierName,
                              BackToBackLCNo = b.BackToBackLC1,
                              AccountID = st.AccountId,
                              LoanFromJobNo = jl.JobNo,
                              ApproximateInHouseDate = c.ApproximateInhouseDate
                          }).Distinct()
                                .OrderByDescending(x => x.PIID)
                                    .AsQueryable();

            var result = accountID == 0 ? piList.ToList() : piList.Where(x => x.AccountID == accountID).ToList();

            var piListFromExcessBooking = (from e in unitOfWork.ExcessBookingRepository.Get()
                                           join p in unitOfWork.PIRepository.Get() on e.ProformaInvoiceID equals p.PIID
                                           join bc in unitOfWork.BackToBackLCRepository.Get() on p.BackToBackLCID equals bc.BackToBackLCID into g
                                           join j in unitOfWork.JobRepository.Get() on e.JobID equals j.JobInfoId
                                           join sp in unitOfWork.SupplierRepository.Get() on p.SupplierID equals sp.SupplierId into sg
                                           join rq in unitOfWork.RequisitionRepository.Get() on p.RequisitionID equals rq.RequisitionID into rg
                                           from b in g.DefaultIfEmpty()
                                           from s in sg.DefaultIfEmpty()
                                           from r in rg.DefaultIfEmpty()
                                           select new PIViewModel
                                           {
                                               JobNo = j.JobNo,
                                               PIID = p.PIID,
                                               ReferenceNo = p.ReferenceNo,
                                               RequisitionNo = r.RequisitionNo,
                                               RequisitionDate = r.RequisitionDate,
                                               PINo = p.PINo,
                                               PIDate = p.PIDate,
                                               SupplierID = p.SupplierID,
                                               SupplierName = s.SupplierName,
                                               BackToBackLCNo = b.BackToBackLC1,
                                               ApproximateInHouseDate = p.ApproximateInhouseDate
                                           }).ToList();

            result.AddRange(piListFromExcessBooking);

            return result;
        }

        public List<DropDownListViewModel> GetPIDropDownByDateRange()
        {
           DateTime threeMonthBackDate = DateTime.Today.AddDays(-90);
            DateTime oneMonthAfterDate = DateTime.Today.AddDays(30);
            var result = (from s in unitOfWork.PIRepository.Get()
                          where s.PINo != null && s.PINo != ""  &&
                          s.PIDate >= threeMonthBackDate && s.PIDate <= oneMonthAfterDate
                          select new DropDownListViewModel
                          {
                              Value = s.PIID,
                              Text = s.PINo
                          }).ToList();

            return result;
        }

        public PIViewModel GetPIByID(int id)
        {
            var result = (from s in unitOfWork.PIRepository.Get()
                          where s.PIID == id
                          select new PIViewModel
                          {
                              PIID = s.PIID,
                              ReferenceNo = s.ReferenceNo,
                              PINo = s.PINo,
                              PIDate = s.PIDate,
                              BackToBackLCID = s.BackToBackLCID,                              
                              SupplierID = s.SupplierID, 
                              ApproximateInHouseDate = s.ApproximateInhouseDate,
                              LoanFromJobID = s.LoanFromJobID,
                              Status = s.Status
                          }).SingleOrDefault();

            return result;
        }

        public List<DropDownListViewModel> GetPIDropDown()
        {
            var result = (from s in unitOfWork.PIRepository.Get()
                          where s.PINo != null && s.PINo != ""
                          select new DropDownListViewModel
                          {
                              Value = s.PIID,
                              Text = s.PINo
                          }).ToList();

            return result;
        }

        public List<DropDownListViewModel> GetPIDropDownByJob(int jobID)
        {
            var result = (from s in unitOfWork.PIRepository.Get()
                          join w in unitOfWork.BookingRepository.Get() on s.PIID equals w.PIId
                          join p in unitOfWork.PurchaseOrderRepository.Get() on w.PurchaseOrderID equals p.PoStyleId
                          where s.PINo != null && s.PINo != "" && p.JobId == jobID
                          select new DropDownListViewModel
                          {
                              Value = s.PIID,
                              Text = s.PINo
                          }).Distinct().ToList();

            var piForAdvancedCM = (from s in unitOfWork.PIRepository.Get()
                                   join a in unitOfWork.AdvancedCMRepository.Get() on s.PIID equals a.PIID
                                   where s.PINo != null && s.PINo != "" && a.JobID == jobID
                                   select new DropDownListViewModel
                                   {
                                       Value = s.PIID,
                                       Text = s.PINo
                                   }).Distinct().ToList();

            var piFromLoan = (from s in unitOfWork.PIRepository.Get()
                              where s.LoanFromJobID == jobID
                              select new DropDownListViewModel
                              {
                                  Value = s.PIID,
                                  Text = s.PINo
                              }).Distinct().ToList();

            var piFromExcessBooking = (from s in unitOfWork.ExcessBookingRepository.Get()
                                       join p in unitOfWork.PIRepository.Get() on s.ProformaInvoiceID equals p.PIID
                                       where s.JobID == jobID
                                       select new DropDownListViewModel
                                       {
                                           Value = s.ProformaInvoiceID,
                                           Text = p.PINo
                                       }).Distinct().ToList();

            result.AddRange(piForAdvancedCM);
            result.AddRange(piFromLoan);
            result.AddRange(piFromExcessBooking);

            return result;
        }

        public List<DropDownListViewModel> GetPIDropDownByBL(int blID)
        {
            var result = (from b in unitOfWork.BLRepository.Get()
                          join p in unitOfWork.PIRepository.Get()
                              on b.BackToBackLCID equals p.BackToBackLCID
                          where b.BLID == blID
                          select new DropDownListViewModel
                          {
                              Value = p.PIID,
                              Text = p.PINo
                          }).Distinct().ToList();
            return result;
        }

        public List<DropDownListViewModel> GetPIDropDownByBLUsingBooking(int blID)
        {
            var result = (from bl in unitOfWork.BLDetailsRepository.Get()
                          join bk in unitOfWork.BookingRepository.Get() on bl.BookingID equals bk.BookingID
                          join pi in unitOfWork.PIRepository.Get() on bk.PIId equals pi.PIID
                          where bl.BLID == blID
                          select new DropDownListViewModel
                          {
                              Text = pi.PINo,
                              Value = pi.PIID
                          }).Distinct().ToList();

            return result;
        }

        public List<DropDownListViewModel> GetReferenceDropDown()
        {
            var result = (from s in unitOfWork.PIRepository.Get()
                          select new DropDownListViewModel
                          {
                              Value = s.PIID,
                              Text = s.ReferenceNo
                          }).ToList();

            return result;
        }

        public List<DropDownListViewModel> GetReferenceDropDownByJob(int jobID)
        {
            var result = (from s in unitOfWork.PIRepository.Get()
                          join w in unitOfWork.BookingRepository.Get() on s.PIID equals w.PIId
                          join p in unitOfWork.PurchaseOrderRepository.Get() on w.PurchaseOrderID equals p.PoStyleId
                          where s.PINo != null && s.PINo != "" && p.JobId == jobID
                          select new DropDownListViewModel
                          {
                              Value = s.PIID,
                              Text = s.ReferenceNo
                          }).Distinct().ToList();

            var piForAdvancedCM = (from s in unitOfWork.PIRepository.Get()
                                   join a in unitOfWork.AdvancedCMRepository.Get() on s.PIID equals a.PIID
                                   where s.PINo != null && s.PINo != "" && a.JobID == jobID
                                   select new DropDownListViewModel
                                   {
                                       Value = s.PIID,
                                       Text = s.ReferenceNo
                                   }).Distinct().ToList();

            var piFromLoan = (from s in unitOfWork.PIRepository.Get()
                              where s.LoanFromJobID == jobID
                              select new DropDownListViewModel
                              {
                                  Value = s.PIID,
                                  Text = s.ReferenceNo
                              }).Distinct().ToList();

            var piFromExcessBooking = (from s in unitOfWork.ExcessBookingRepository.Get()
                                       join p in unitOfWork.PIRepository.Get() on s.ProformaInvoiceID equals p.PIID
                                       where s.JobID == jobID
                                       select new DropDownListViewModel
                                       {
                                           Value = s.ProformaInvoiceID,
                                           Text = p.ReferenceNo
                                       }).Distinct().ToList();

            result.AddRange(piForAdvancedCM);
            result.AddRange(piFromLoan);
            result.AddRange(piFromExcessBooking);

            return result;
        }

        public bool IsUniquePI(string piName, Nullable<int> piID = null)
        {
            IQueryable<int> result;

            if (piID == null)
            {
                result = from s in unitOfWork.PIRepository.Get()
                         where s.PINo == piName
                         select s.PIID;
            }
            else
            {
                result = from s in unitOfWork.PIRepository.Get()
                         where s.PINo == piName & s.PIID != piID
                         select s.PIID;
            }

            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }

        public decimal GetPIValueByID(int piID)
        {
            decimal? fromBooking = (from c in unitOfWork.BookingRepository.Get()
                                    where c.PIId == piID
                                    group c by c.PIId into g
                                    select g.Sum(c => c.TotalQuantity * c.UnitPrice)).SingleOrDefault();

            decimal? fromExcessBooking = (from c in unitOfWork.ExcessBookingRepository.Get()
                                          where c.ProformaInvoiceID == piID
                                          group c by c.ProformaInvoiceID into g
                                          select g.Sum(x => x.TotalPrice)).SingleOrDefault();

            decimal result = fromBooking + fromExcessBooking ?? 0;

            return result;
        }

        public List<DropDownListViewModel> GetPIDropDownByBackToBackLCID(int backTobackLCID)
        {
            var result = (from pi in unitOfWork.PIRepository.Get()
                          join b in unitOfWork.BackToBackLCRepository.Get() on pi.BackToBackLCID equals b.BackToBackLCID
                          where b.BackToBackLCID == backTobackLCID
                          select new DropDownListViewModel
                          {
                              Text = pi.PINo,
                              Value = pi.PIID
                          }
                          ).ToList();
            return result;
        }
    }
}
