using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScopoERP.ProductionStatus.ViewModel;

namespace ScopoERP.ProductionStatus.BLL
{
    
    public class SupervisorLogic
    {
        private UnitOfWork unitOfWork;
        private supervisor supervisor;

        public SupervisorLogic(UnitOfWork unitOfWork, supervisor supervisor)
        {
            this.supervisor = supervisor;
            this.unitOfWork = unitOfWork;
        }

        public bool IsUnique(SupervisorViewModel supervisorViewModel)
        {
            IQueryable<int> result;

            if (supervisorViewModel.SupervisorID == 0)
            {
                result = (from s in unitOfWork.SupervisorRepository.Get()
                          where s.SupervisorName.ToLower() == supervisorViewModel.SupervisorName.ToLower()
                          select s.SupervisorID);
            }
            else
            {
                result = (from s in unitOfWork.SupervisorRepository.Get()
                          where s.SupervisorName.ToLower() == supervisorViewModel.SupervisorName.ToLower() && s.SupervisorID != supervisorViewModel.SupervisorID
                          select s.SupervisorID);
            }

            if (result.Count() > 0)
            {
                return true;
            }
            return false;
        }

        public void CreateSupervisor(SupervisorViewModel supervisorViewModel)
        {
            supervisor = new supervisor
            {
                SupervisorName=supervisorViewModel.SupervisorName,
                Floor=supervisorViewModel.Floor,
                Line=supervisorViewModel.Line,
                CardNo=supervisorViewModel.CardNo
            };
            unitOfWork.SupervisorRepository.Insert(supervisor);
            unitOfWork.Save();
        }

        public void UpdateSupervisor(SupervisorViewModel supervisorViewModel)
        {
            supervisor = new supervisor
            {
                SupervisorID=supervisorViewModel.SupervisorID,
                SupervisorName = supervisorViewModel.SupervisorName,
                Floor = supervisorViewModel.Floor,
                Line = supervisorViewModel.Line,
                CardNo = supervisorViewModel.CardNo
            };
            unitOfWork.SupervisorRepository.Update(supervisor);
            unitOfWork.Save();
        }

        public object GetAllSupervisors()
        {
            var res = (from s in unitOfWork.SupervisorRepository.Get()
                       select new SupervisorViewModel
                       {
                           SupervisorID = s.SupervisorID,
                           SupervisorName = s.SupervisorName,
                           Floor = s.Floor,
                           Line = s.Line,
                           CardNo = s.CardNo

                       }).ToList();

            return res;
        }
    }
}
