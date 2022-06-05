using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using ScopoERP.LC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.LC.BLL
{
    public class JobLogic
    {
        private UnitOfWork unitOfWork;
        private jobinfo jobinfo;

        public JobLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void CreateJob(JobViewModel jobVM)
        {
            jobinfo = new jobinfo
            {
                JobNo = jobVM.JobNo,
                ContractNo = jobVM.ContractNo,
                ExtraContractNo = jobVM.ExtraContractNo,
                BankID = jobVM.BankID,
                ShippedTo=jobVM.ShippedTo,
                ContractDate=jobVM.ContractDate,
                NotifyParty=jobVM.NotifyParty,
                AlsoNotifyParty=jobVM.AlsoNotifyParty,
                CATNO=jobVM.CATNO,
                IsClosed=false
            };

            unitOfWork.JobRepository.Insert(jobinfo);
            unitOfWork.Save();
        }

        public void Closejob(int id)
        {
            jobinfo = unitOfWork.JobRepository.GetById(id);

            jobinfo.IsClosed = true;
            unitOfWork.JobRepository.Update(jobinfo);
            unitOfWork.Save();

        }

        public void UpdateJob(JobViewModel jobVM)
        {
            jobinfo = unitOfWork.JobRepository.GetById(jobVM.JobId);

            jobinfo.JobInfoId = jobVM.JobId;
            jobinfo.JobNo = jobVM.JobNo;
            jobinfo.ContractNo = jobVM.ContractNo;
            jobinfo.ExtraContractNo = jobVM.ExtraContractNo;
            jobinfo.BankID = jobVM.BankID;

            jobinfo.UDNo = jobVM.UDNo;
            jobinfo.UDDate = jobVM.UDDate;
            jobinfo.ContractDate = jobVM.ContractDate;
            jobinfo.NotifyParty = jobVM.NotifyParty;
            jobinfo.AlsoNotifyParty = jobVM.AlsoNotifyParty;
            jobinfo.CATNO = jobVM.CATNO;
            jobinfo.ShippedTo = jobVM.ShippedTo;
            jobinfo.IsClosed = jobVM.IsClosed;

            unitOfWork.JobRepository.Update(jobinfo);
            unitOfWork.Save();
        }

        public List<JobViewModel> GetAllJob()
        {
            var result = (from s in unitOfWork.JobRepository.Get()
                          join bk in unitOfWork.BankRepository.Get() on s.BankID equals bk.BankID into bg
                          from b in bg.DefaultIfEmpty()
                          //where s.IsClosed == false || s.IsClosed == null
                          orderby s.JobInfoId descending
                          select new JobViewModel
                          {
                              JobId = s.JobInfoId,
                              JobNo = s.JobNo,
                              ContractNo = s.ContractNo,
                              ExtraContractNo = s.ExtraContractNo,
                              BankID = b.BankID,
                              BankName = b.BankName,

                              UDNo = s.UDNo,
                              UDDate = s.UDDate,
                              ContractDate = s.ContractDate,
                              NotifyParty = s.NotifyParty,
                              AlsoNotifyParty = s.AlsoNotifyParty,
                              CATNO = s.CATNO,
                              ShippedTo=s.ShippedTo,
                              IsClosed=s.IsClosed
                          }).ToList();

            return result;
        }

        public List<JobViewModel> GetAllJobForAdvancedCM(int year)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT J.JobInfoId AS JobID, J.JobNo, J.AdvancedCMPercentage, J.SightDays, ");
            query.Append("SUM(P.FOB * P.OrderQuantity - P.AgreedCM * P.OrderQuantity) / SUM(P.FOB * P.OrderQuantity) * 100  AS BudgetPercentage ");
            query.Append("FROM PoStyle P ");
            query.Append("INNER JOIN JobInfo J ON P.JobID = J.JobInfoID ");
            query.Append("WHERE J.JobNo LIKE '" + year + "%' ");
            query.Append("GROUP BY J.JobInfoId");

            var result = unitOfWork.JobRepository.SelectQuery<JobViewModel>(query.ToString()).ToList();

            result.ForEach(x => x.Limit = x.BudgetPercentage + x.AdvancedCMPercentage);

            return result;
        }

        public void UpdateAdvancedCM(List<JobViewModel> jobList)
        {
            foreach(var item in jobList)
            {
                if(item.AdvancedCMPercentage != null)
                {
                    jobinfo = unitOfWork.JobRepository.GetById(item.JobId);
                    jobinfo.AdvancedCMPercentage = item.AdvancedCMPercentage;
                    jobinfo.SightDays = item.SightDays;

                    unitOfWork.JobRepository.Update(jobinfo);
                }
            }

            unitOfWork.Save();
        }

        public JobViewModel GetJobByID(int id)
        {
            var result = (from s in unitOfWork.JobRepository.Get()
                          join bk in unitOfWork.BankRepository.Get() on s.BankID equals bk.BankID into bg
                          from b in bg.DefaultIfEmpty()
                          where s.JobInfoId == id
                          select new JobViewModel
                          {
                              JobId = s.JobInfoId,
                              JobNo = s.JobNo,
                              ContractNo = s.ContractNo,
                              ExtraContractNo = s.ExtraContractNo,
                              BankID = b.BankID,
                              BankName = b.BankName,
                              UDNo = s.UDNo,
                              UDDate = s.UDDate,
                              ContractDate=s.ContractDate,
                              ShippedTo=s.ShippedTo,
                              NotifyParty=s.NotifyParty,
                              AlsoNotifyParty=s.AlsoNotifyParty,
                              CATNO=s.CATNO,
                              IsClosed=s.IsClosed
                          }).SingleOrDefault();

            return result;
        }

        public List<DropDownListViewModel> GetJobDropDown(bool isClosed = true)
        {
            var result = (from s in unitOfWork.JobRepository.Get()
                          where s.IsClosed == false || s.IsClosed == isClosed
                          select new DropDownListViewModel
                          {
                              Value = s.JobInfoId,
                              Text = s.JobNo
                          }).AsEnumerable().OrderByDescending(x => x.Text).ToList();

            return result;
        }

        public List<DropDownListViewModel> GetContractDropDown(bool isClosed = true)
        {
            var result = (from s in unitOfWork.JobRepository.Get()
                          where s.IsClosed == false || s.IsClosed == isClosed
                          select new DropDownListViewModel
                          {
                              Value = s.JobInfoId,
                              Text = s.ContractNo
                          }).ToList();

            return result;
        }

        public bool IsUniqueJob(string jobNo, Nullable<int> jobID = null)
        {
            IQueryable<int> result;

            if (jobID == null)
            {
                result = from s in unitOfWork.JobRepository.Get()
                         where s.JobNo == jobNo
                         select s.JobInfoId;
            }
            else
            {
                result = from s in unitOfWork.JobRepository.Get()
                         where s.JobNo == jobNo & s.JobInfoId != jobID
                         select s.JobInfoId;
            }

            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }
    }
}
