using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using ScopoERP.OrderManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.OrderManagement.BLL
{
    public class SubContractLogic
    {
        private UnitOfWork unitOfWork;
        private subcontract subContract;

        public SubContractLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public List<SubContractViewModel> GetAllSubContract(int purchaseOrderID)
        {
            List<SubContractViewModel> result =
                (from s in unitOfWork.SubContractRepository.Get()
                 where s.PurchaseOrderID == purchaseOrderID
                 select new SubContractViewModel
                 {
                     SubContractID = s.SubContractID,
                     SubContractNo = s.SubContractNo,
                     SubFactoryID = s.FactoryID,
                     FactoryID = s.FactoryID,
                     PurchaseOrderID = s.PurchaseOrderID,
                     SubContractQuantity = s.SubContractQuantity,
                     SubContractExitDate = s.SubContractExitDate,
                     SubContractRate = s.SubContractRate,
                     CommercialPercentage = s.CommercialPercentage,
                     Remarks = s.Remarks,
                     UserID = s.UserID,
                     SetupDate = s.SetupDate
                 }).ToList();
            
            return result;
        }


        public void SaveSubContract(int purchaseOrderID, List<SubContractViewModel> subContractVMList)
        {
            IEnumerable<int> sunContractIDs = new List<int>();

            if (subContractVMList != null)
            {
                sunContractIDs = subContractVMList.Select(x => x.SubContractID);
            }

            var deletedSubContractList = (from s in unitOfWork.SubContractRepository.Get()
                                          where s.PurchaseOrderID == purchaseOrderID
                                          && !sunContractIDs.Contains(s.SubContractID)
                                          select s).AsEnumerable();

            foreach(var item in deletedSubContractList)
            {
                unitOfWork.SubContractRepository.Delete(item);
            }

            string subContractNo = null;

            if (subContractVMList != null)
            {
                foreach (SubContractViewModel item in subContractVMList)
                {
                    subContract = new subcontract
                    {
                        SubContractID = item.SubContractID,
                        FactoryID = item.FactoryID,
                        PurchaseOrderID = item.PurchaseOrderID,
                        SubContractQuantity = item.SubContractQuantity,
                        SubContractExitDate = item.SubContractExitDate,
                        SubContractRate = item.SubContractRate,
                        CommercialPercentage = item.CommercialPercentage,
                        Remarks = item.Remarks,
                        UserID = item.UserID,
                        SetupDate = DateTime.Now
                    };

                    if (item.SubContractID == 0)
                    {
                        subContractNo = subContractNo == null ? this.GetNewOrderNo() : this.GetNewOrderNo(subContractNo);
                        subContract.SubContractNo = subContractNo;

                        unitOfWork.SubContractRepository.Insert(subContract);
                    }
                    else
                    {
                        subContract.SubContractNo = item.SubContractNo;
                        unitOfWork.SubContractRepository.Update(subContract);
                    }
                }
            }
            unitOfWork.Save();
        }

        public string GetNewOrderNo(string oldOrderNo = null)
        {
            string newOrdernNo = string.Empty;

            string result = string.Empty;

            if (oldOrderNo == null)
            {
                result = (from c in unitOfWork.SubContractRepository.Get()
                          orderby c.SubContractID descending
                          select c.SubContractNo).FirstOrDefault();
            }
            else
            {
                result = oldOrderNo;
            }

            if (result == null)
            {
                newOrdernNo = "SUB-" + DateTime.Now.Year.ToString() + "-00001";
            }
            else
            {
                string newOrderInDigit = (Convert.ToInt32(result.Split('-').Last()) + 1).ToString().PadLeft(5, '0');

                newOrdernNo = "SUB-" + DateTime.Now.Year.ToString() + "-" + newOrderInDigit;
            }
            return newOrdernNo;
        }
    }
}
