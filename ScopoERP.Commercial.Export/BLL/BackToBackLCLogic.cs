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
    public class BackToBackLCLogic
    {
        private UnitOfWork unitOfWork;
        private backtobacklc backtobacklc;
        private piinfo piInfo;

        public BackToBackLCLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public List<BackToBackLCViewModel> GetBackToBackLCByJob(int jobID)
        {
            var result = (from c in unitOfWork.BackToBackLCRepository.Get()
                          join lc in unitOfWork.LCTypeRepository.Get() on c.LCTypeID equals lc.PaymentTypeID into lcg
                          from l in lcg.DefaultIfEmpty()
                          where c.JobID == jobID
                          orderby c.BackToBackLCID descending
                          select new BackToBackLCViewModel
                          {
                              BackToBackLCID = c.BackToBackLCID,
                              BackToBackLCNo = c.BackToBackLC1,
                              BackToBackLCDate = c.BackToBackLCDate,
                              BackToBackLCValue = c.BackToBackLCValue,
                              BackToBackShippedDate = c.BackToBackShippedDate,
                              JobID = c.JobID,
                              LCTypeID = c.LCTypeID,
                              LCTypeTitle = l.PaymentTitle,
                              SightDays = c.SightDays,
                              Status = c.Status
                          }).ToList();
            return result;
        }

        public void SaveBackToBackLC(List<BackToBackLCViewModel> lcListVM)
        {

            foreach (var item in lcListVM)
            {
                if (item.BackToBackLCID != 0)
                {
                    backtobacklc = new backtobacklc
                    {
                        BackToBackLCID = item.BackToBackLCID,
                        BackToBackLC1 = item.BackToBackLCNo,
                        BackToBackLCDate = item.BackToBackLCDate,
                        BackToBackShippedDate = item.BackToBackShippedDate,
                        SightDays = item.SightDays,
                        BackToBackLCValue = item.BackToBackLCValue,
                        LCTypeID = item.LCTypeID,
                        JobID = item.JobID
                    };
                    unitOfWork.BackToBackLCRepository.Update(backtobacklc);
                }
                else
                {
                    backtobacklc = new backtobacklc
                    {
                        BackToBackLC1 = item.BackToBackLCNo,
                        BackToBackLCDate = item.BackToBackLCDate,
                        BackToBackShippedDate = item.BackToBackShippedDate,
                        SightDays = item.SightDays,
                        BackToBackLCValue = item.BackToBackLCValue,
                        LCTypeID = item.LCTypeID,
                        JobID = item.JobID
                    };
                    unitOfWork.BackToBackLCRepository.Insert(backtobacklc);
                }
            }
            unitOfWork.Save();
        }

        public List<PISummary> GetPISummaryByLCID(int lcID)
        {
            var result = (from pi in unitOfWork.PIRepository.Get()
                          where pi.BackToBackLCID == lcID
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

        private bool ReleasePIFromBackToBackLC(int lcID)
        {
            try
            {
                var expiList = (from s in unitOfWork.PIRepository.Get()
                                where s.BackToBackLCID == lcID
                                select s).ToList();

                foreach (var item in expiList)
                {
                    piInfo = unitOfWork.PIRepository.Get().SingleOrDefault(x => x.PIID == item.PIID);
                    piInfo.BackToBackLCID = null;
                    unitOfWork.PIRepository.Update(piInfo);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void UpdatePIByLCID(int lcID, List<PISummary> piList)
        {
            if (ReleasePIFromBackToBackLC(lcID))
            {
                foreach (var item in piList)
                {
                    piInfo = unitOfWork.PIRepository.Get().SingleOrDefault(x => x.PIID == item.PIID);
                    piInfo.BackToBackLCID = lcID;

                    unitOfWork.PIRepository.Update(piInfo);
                }
                unitOfWork.Save();
            }


        }

        public bool DeleteBackToBackLC(int lcID)
        {
            if (ReleasePIFromBackToBackLC(lcID))
            {
                backtobacklc = unitOfWork.BackToBackLCRepository.GetById(lcID);
                unitOfWork.BackToBackLCRepository.Delete(backtobacklc);
                unitOfWork.Save();
                return true;
            }
            return false;
        }
        // Above methods are newly created by 'Leton'






        public int CreateBackToBackLC(BackToBackLCViewModel backToBackLCVM)
        {
            this.backtobacklc = new backtobacklc()
            {
                BackToBackLC1 = backToBackLCVM.BackToBackLCNo,
                BackToBackLCDate = backToBackLCVM.BackToBackLCDate,
                BackToBackLCValue = backToBackLCVM.BackToBackLCValue,
                BackToBackShippedDate = backToBackLCVM.BackToBackShippedDate,

                JobID = backToBackLCVM.JobID,
                LCTypeID = backToBackLCVM.LCTypeID,
                SightDays = backToBackLCVM.SightDays,
                Status = backToBackLCVM.Status
            };

            unitOfWork.BackToBackLCRepository.Insert(backtobacklc);

            if (backToBackLCVM.PIList != null)
            {
                foreach (var item in backToBackLCVM.PIList)
                {
                    piInfo = unitOfWork.PIRepository.Get().SingleOrDefault(x => x.PIID == item.PIID);
                    piInfo.BackToBackLCID = backToBackLCVM.BackToBackLCID;

                    unitOfWork.PIRepository.Update(piInfo);
                }
            }

            unitOfWork.Save();

            return backtobacklc.BackToBackLCID;
        }

        public void UpdateBackToBackLC(BackToBackLCViewModel backToBackLCVM)
        {
            this.backtobacklc = new backtobacklc()
            {
                BackToBackLCID = backToBackLCVM.BackToBackLCID,
                BackToBackLC1 = backToBackLCVM.BackToBackLCNo,
                BackToBackLCDate = backToBackLCVM.BackToBackLCDate,
                BackToBackLCValue = backToBackLCVM.BackToBackLCValue,
                BackToBackShippedDate = backToBackLCVM.BackToBackShippedDate,

                JobID = backToBackLCVM.JobID,
                LCTypeID = backToBackLCVM.LCTypeID,
                SightDays = backToBackLCVM.SightDays,
                Status = backToBackLCVM.Status
            };

            unitOfWork.BackToBackLCRepository.Update(backtobacklc);

            var piList = (from s in unitOfWork.PIRepository.Get()
                          where s.BackToBackLCID == backToBackLCVM.BackToBackLCID
                          select s).ToList();

            foreach (var item in piList)
            {
                piInfo = unitOfWork.PIRepository.Get().SingleOrDefault(x => x.PIID == item.PIID);
                piInfo.BackToBackLCID = null;
                unitOfWork.PIRepository.Update(piInfo);
            }

            if (backToBackLCVM.PIList != null)
            {
                foreach (var item in backToBackLCVM.PIList)
                {
                    piInfo = unitOfWork.PIRepository.Get().SingleOrDefault(x => x.PIID == item.PIID);
                    piInfo.BackToBackLCID = backToBackLCVM.BackToBackLCID;
                    unitOfWork.PIRepository.Update(piInfo);
                }
            }

            unitOfWork.Save();
        }

        public BackToBackLCViewModel GetBackToBackLCByID(int? id)
        {
            var result = (from c in unitOfWork.BackToBackLCRepository.Get()
                          join j in unitOfWork.JobRepository.Get() on c.JobID equals j.JobInfoId
                          join lc in unitOfWork.LCTypeRepository.Get() on c.LCTypeID equals lc.PaymentTypeID into g
                          from l in g.DefaultIfEmpty()
                          where c.BackToBackLCID == id
                          select new BackToBackLCViewModel
                          {
                              BackToBackLCID = c.BackToBackLCID,
                              BackToBackLCNo = c.BackToBackLC1,
                              BackToBackLCDate = c.BackToBackLCDate,
                              BackToBackLCValue = c.BackToBackLCValue,
                              BackToBackShippedDate = c.BackToBackShippedDate,

                              JobID = c.JobID,
                              JobNo = j.JobNo,

                              LCTypeID = c.LCTypeID,
                              LCTypeTitle = l.PaymentTitle,

                              SightDays = c.SightDays,

                              Status = c.Status
                          }).SingleOrDefault();

            var piList = (from p in unitOfWork.PIRepository.Get()
                          join bc in unitOfWork.BackToBackLCRepository.Get() on p.BackToBackLCID equals bc.BackToBackLCID
                          join b in unitOfWork.BookingRepository.Get() on p.PIID equals b.PIId
                          where bc.BackToBackLCID == id
                          group b by new { p.PIID, p.PINo } into s
                          select new PISummary
                          {
                              PIID = s.Key.PIID,
                              PINo = s.Key.PINo,
                              PIValue = s.Sum(x => x.TotalQuantity * x.UnitPrice)
                          }).ToList();

            var piListAdvancedCM = (from a in unitOfWork.AdvancedCMRepository.Get()
                                    join p in unitOfWork.PIRepository.Get() on a.PIID equals p.PIID
                                    join bc in unitOfWork.BackToBackLCRepository.Get() on p.BackToBackLCID equals bc.BackToBackLCID
                                    where p.PINo != null && p.PINo != "" && bc.BackToBackLCID == id
                                    select new PISummary
                                    {
                                        PIID = p.PIID,
                                        PINo = p.PINo,
                                        PIValue = a.PIValue
                                    }).Distinct().ToList();

            piList.AddRange(piListAdvancedCM);

            result.PIList = piList;

            return result;
        }

        public IQueryable<BackToBackLCViewModel> GetAllBackToBackLC()
        {
            var result = (from c in unitOfWork.BackToBackLCRepository.Get()
                          join j in unitOfWork.JobRepository.Get() on c.JobID equals j.JobInfoId
                          join lc in unitOfWork.LCTypeRepository.Get() on c.LCTypeID equals lc.PaymentTypeID into g
                          from l in g.DefaultIfEmpty()
                          orderby c.BackToBackLCID descending
                          select new BackToBackLCViewModel
                          {
                              BackToBackLCID = c.BackToBackLCID,
                              BackToBackLCNo = c.BackToBackLC1,
                              BackToBackLCDate = c.BackToBackLCDate,
                              BackToBackLCValue = c.BackToBackLCValue,
                              BackToBackShippedDate = c.BackToBackShippedDate,

                              JobID = c.JobID,
                              JobNo = j.JobNo,

                              LCTypeID = c.LCTypeID,
                              LCTypeTitle = l.PaymentTitle,

                              SightDays = c.SightDays,

                              Status = c.Status
                          }).AsQueryable();

            return result;
        }

        public int GetTotalBackToBackLC()
        {
            int count = unitOfWork.BackToBackLCRepository.Get().Count();

            return count;
        }

        public bool IsUniqueBackToBackLC(string backToBackLCNo, Nullable<int> blID = null)
        {
            IQueryable<int> result;

            if (blID == null)
            {
                result = from s in unitOfWork.BackToBackLCRepository.Get()
                         where s.BackToBackLC1 == backToBackLCNo
                         select s.BackToBackLCID;
            }
            else
            {
                result = from s in unitOfWork.BackToBackLCRepository.Get()
                         where s.BackToBackLC1 == backToBackLCNo & s.BackToBackLCID != blID
                         select s.BackToBackLCID;
            }

            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }

        public List<DropDownListViewModel> GetBackToBackLCDropDown(int? jobID)
        {
            var query = from c in unitOfWork.BackToBackLCRepository.Get() select c;

            if (jobID != null)
            {
                query = query.Where(x => x.JobID == jobID);
            }

            var results = (from s in query
                           select new DropDownListViewModel
                           {
                               Value = s.BackToBackLCID,
                               Text = s.BackToBackLC1
                           }).ToList();

            return results;
        }

    }
}
