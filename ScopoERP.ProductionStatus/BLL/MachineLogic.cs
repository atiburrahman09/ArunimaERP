using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using ScopoERP.Production.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Production.BLL
{
    public class MachineLogic
    {
        private UnitOfWork unitOfWork;
        private machine machine;

        public MachineLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public void CreateMachine(MachineViewModel machineVM)
        {
            machine = new machine
            {
                MachineCode = machineVM.MachineCode,
                MachineCategoryID = machineVM.MachineCategoryID,
                Unit=machineVM.Unit ?? 0,
                AG=machineVM.AG ?? 0,
                BrandName=machineVM.BrandName,
                MobileNo=machineVM.MobileNo,
                MachineCondition=machineVM.MachineCondition,
                LoanTo=machineVM.LoanTo,
                LoanFrom=machineVM.LoanFrom,
                MCValue=machineVM.MCValue,
                BookValue=machineVM.BookValue,
                Remarks=machineVM.Remarks,
                SetupDate=DateTime.Now

            };

            unitOfWork.MachineRepository.Insert(machine);
            unitOfWork.Save();
        }


        public void UpdateMachine(MachineViewModel machineVM)
        {
            machine = new machine
            {
                MachineID = machineVM.MachineID,
                MachineCode = machineVM.MachineCode,
                MachineCategoryID = machineVM.MachineCategoryID,
                Unit = machineVM.Unit ??0,
                AG = machineVM.AG ??0,
                BrandName = machineVM.BrandName,
                MobileNo = machineVM.MobileNo,
                MachineCondition = machineVM.MachineCondition,
                LoanTo = machineVM.LoanTo,
                LoanFrom = machineVM.LoanFrom,
                MCValue = machineVM.MCValue,
                BookValue = machineVM.BookValue,
                Remarks = machineVM.Remarks,
                SetupDate = DateTime.Now
            };

            unitOfWork.MachineRepository.Update(machine);
            unitOfWork.Save();
        }


        public List<MachineViewModel> GetAllMachine()
        {
            var result = (from s in unitOfWork.MachineRepository.Get()
                          join it in unitOfWork.MachineCategoryRepository.Get()
                          on s.MachineCategoryID equals it.MachineCategoryID
                          orderby s.MachineID descending
                          select new MachineViewModel
                          {
                              MachineID = s.MachineID,
                              MachineCode = s.MachineCode,
                              MachineCategoryID = s.MachineCategoryID,
                              Unit = s.Unit,
                              AG = s.AG,
                              BrandName = s.BrandName,
                              MobileNo = s.MobileNo,
                              MachineCondition = s.MachineCondition ?? 0,
                              LoanTo = s.LoanTo,
                              LoanFrom = s.LoanFrom,
                              MCValue = s.MCValue,
                              BookValue = s.BookValue,
                              Remarks = s.Remarks,
                              SetupDate = DateTime.Now,
                              MachineCategoryName = it.Name
                          }).ToList();

            return result;
        }


        public int GetTotalMachine()
        {
            int count = unitOfWork.MachineRepository.Get().Count();
            return count;
        }


        public MachineViewModel GetMachineByID(int id)
        {
            var result = (from s in unitOfWork.MachineRepository.Get()
                          join it in unitOfWork.MachineCategoryRepository.Get()
                          on s.MachineCategoryID equals it.MachineCategoryID
                          where s.MachineID == id
                          select new MachineViewModel
                          {
                              MachineID = s.MachineID,
                              MachineCode = s.MachineCode,
                              MachineCategoryID = s.MachineCategoryID,
                              Unit = s.Unit,
                              AG = s.AG,
                              BrandName = s.BookValue,
                              MobileNo = s.MobileNo,
                              MachineCondition = s.MachineCondition ?? 0,
                              LoanTo = s.LoanTo,
                              LoanFrom = s.LoanFrom,
                              MCValue = s.MCValue,
                              BookValue = s.BookValue,
                              Remarks = s.Remarks,
                              SetupDate = DateTime.Now,
                              MachineCategoryName = it.Name
                          }).SingleOrDefault();

            return result;
        }


        public MachineViewModel GetMachineByCode(string machineCode)
        {
            var result = (from s in unitOfWork.MachineRepository.Get()
                          join it in unitOfWork.MachineCategoryRepository.Get()
                          on s.MachineCategoryID equals it.MachineCategoryID
                          where s.MachineCode == machineCode
                          select new MachineViewModel
                          {
                              MachineID = s.MachineID,
                              MachineCode = s.MachineCode,
                              MachineCategoryID = s.MachineCategoryID,
                              Unit = s.Unit,
                              AG = s.AG,
                              BrandName = s.BookValue,
                              MobileNo = s.MobileNo,
                              MachineCondition = s.MachineCondition ?? 0,
                              LoanTo = s.LoanTo,
                              LoanFrom = s.LoanFrom,
                              MCValue = s.MCValue,
                              BookValue = s.BookValue,
                              Remarks = s.Remarks,
                              SetupDate = DateTime.Now,
                              MachineCategoryName = it.Name
                          }).FirstOrDefault();

            return result;
        }


        public List<DropDownListViewModel> GetMachineDropDown()
        {
            var result = (from s in unitOfWork.MachineRepository.Get()
                          select new DropDownListViewModel
                          {
                              Value = s.MachineID,
                              Text =  s.MachineCode
                          }).ToList();

            return result;
        }


        public bool IsUniqueMachine(string machineDescription, Nullable<int> machineID = null)
        {
            IQueryable<int> result;

            if (machineID == null)
            {
                result = from s in unitOfWork.MachineRepository.Get()
                         //where s.MachineDescription == machineDescription
                         select s.MachineID;
            }
            else
            {
                result = from s in unitOfWork.MachineRepository.Get()
                         //where s.MachineDescription == machineDescription & s.MachineID != machineID
                         select s.MachineID;
            }

            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }
    }
}
