using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using ScopoERP.ProductionStatus.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.ProductionStatus.BLL
{
    public class EmployeeCapabilityService
    {
        private UnitOfWork unitOfWork;
        private EmployeeCapability employeeCapability;

        public EmployeeCapabilityService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public object GetEmployeeDropDownByKeyword(string inputString)
        {
            List<EmployeeCapabilityViewModel> empList = unitOfWork.EmployeeCapabilityRepository.SelectQuery<EmployeeCapabilityViewModel>(@"SELECT EmployeeID, CardNo as EmployeeCardNo, EmployeeName FROM ScopoHR.dbo.Employees WHERE (EmployeeName LIKE '%"+ inputString +"%') OR (CardNo LIKE '%" + inputString + "%') AND IsActive=1 ORDER BY EmployeeID desc");
            return empList;
        }

        public List<string> GetEmployeeeCapabilityDetailsById(string CardNo)
        {
            List<string> res = (from c in unitOfWork.EmployeeCapabilityRepository.Get()
                       where c.EmployeeCardNo == CardNo
                       select c.SpecNo).ToList();
            return res;
        }

        public List<EmployeeCapabilityViewModel> GetRecentEmployees()
        {
            List<EmployeeCapabilityViewModel> empList =unitOfWork.EmployeeCapabilityRepository.SelectQuery<EmployeeCapabilityViewModel>("SELECT top 20 EmployeeID, CardNo as EmployeeCardNo, EmployeeName FROM ScopoHR.dbo.Employees ORDER BY EmployeeID desc");
            return empList;
        }

        public void SaveEmployeeCapabilityInfo(string cardNo, List<string> specs)
        {
            var info = unitOfWork.EmployeeCapabilityRepository.Get().Where(x => x.EmployeeCardNo == cardNo).ToList();
            if(info.Count > 0)
            {
                unitOfWork.EmployeeCapabilityRepository.RawQuery("DELETE FROM EmployeeCapabilities WHERE EmployeeCardNo = '"+cardNo+"'");

                foreach (var s in specs)
                {
                    employeeCapability = new EmployeeCapability
                    {
                        EmployeeCardNo = cardNo,
                        SpecNo = s
                    };
                    unitOfWork.EmployeeCapabilityRepository.Insert(employeeCapability);
                }
                unitOfWork.Save();
            }
            else
            {
                foreach (var s in specs)
                {
                    employeeCapability = new EmployeeCapability
                    {
                        EmployeeCardNo = cardNo,
                        SpecNo = s
                    };
                    unitOfWork.EmployeeCapabilityRepository.Insert(employeeCapability);
                }
                unitOfWork.Save();
            }
        }
    }
}
