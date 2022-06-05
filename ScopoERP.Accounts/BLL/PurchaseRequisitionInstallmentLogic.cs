using ScopoERP.Accounts.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Accounts.BLL
{
    public class PurchaseRequisitionInstallmentLogic
    {
        private UnitOfWork unitOfWork;
        private purchaserequisitioninstallment purchaseRequisitionInstallment;

        public PurchaseRequisitionInstallmentLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public void Create(PurchaseRequisitionInstallmentViewModel purchaseRequisitionInstallmentVM)
        {
            purchaseRequisitionInstallment = new purchaserequisitioninstallment()
            {
                PurchaseRequisitionID = purchaseRequisitionInstallmentVM.PurchaseRequisitionID,
                InstallmentDate       = purchaseRequisitionInstallmentVM.InstallmentDate,
                Amount                = purchaseRequisitionInstallmentVM.Amount,
                PayableAmount         = purchaseRequisitionInstallmentVM.PayableAmount,
                PayableDate           = purchaseRequisitionInstallmentVM.PayableDate,
                SetDate               = DateTime.Now,
                UserID                = purchaseRequisitionInstallmentVM.UserID
            };
            unitOfWork.PurchaseRequisitionInstallmentRepository.Insert(purchaseRequisitionInstallment);
            unitOfWork.Save();
        }


        public void Update(PurchaseRequisitionInstallmentViewModel purchaseRequisitionInstallmentVM)
        {
            purchaseRequisitionInstallment = new purchaserequisitioninstallment()
            {
                PurchaseRequisitionInstallmentID = purchaseRequisitionInstallmentVM.PurchaseRequisitionInstallmentID,
                PurchaseRequisitionID            = purchaseRequisitionInstallmentVM.PurchaseRequisitionID,
                InstallmentDate                  = purchaseRequisitionInstallmentVM.InstallmentDate,
                Amount                           = purchaseRequisitionInstallmentVM.Amount,
                PayableAmount                    = purchaseRequisitionInstallmentVM.PayableAmount,
                PayableDate                      = purchaseRequisitionInstallmentVM.PayableDate,
                SetDate                          = DateTime.Now,
                UserID                           = purchaseRequisitionInstallmentVM.UserID
            };
            unitOfWork.PurchaseRequisitionInstallmentRepository.Update(purchaseRequisitionInstallment);
            unitOfWork.Save();
        }


        public PurchaseRequisitionInstallmentViewModel GetByID(int id)
        {
            PurchaseRequisitionInstallmentViewModel data = (from s in unitOfWork.PurchaseRequisitionInstallmentRepository.Get()
                                                            join p in unitOfWork.PurchaseRequisitionRepository.Get() on s.PurchaseRequisitionID equals p.PurchaseRequisitionID
                                                            where s.PurchaseRequisitionInstallmentID == id
                                                            select new PurchaseRequisitionInstallmentViewModel
                                                            {
                                                                PurchaseRequisitionInstallmentID = s.PurchaseRequisitionInstallmentID,
                                                                PurchaseRequisitionID = s.PurchaseRequisitionID,
                                                                RequisitionNo = p.RequisitionNo,
                                                                InstallmentDate = s.InstallmentDate,
                                                                PayableAmount = s.PayableAmount,
                                                                PayableDate = s.PayableDate,
                                                                Amount = s.Amount,
                                                                SetDate = s.SetDate,
                                                                UserID = s.UserID
                                                            }).SingleOrDefault();
            return data;
        }


        public IEnumerable<PurchaseRequisitionInstallmentViewModel> GetAll()
        {
           IEnumerable<PurchaseRequisitionInstallmentViewModel> data = (from s in unitOfWork.PurchaseRequisitionInstallmentRepository.Get()
                                                            join p in unitOfWork.PurchaseRequisitionRepository.Get() on s.PurchaseRequisitionID equals p.PurchaseRequisitionID
                                                            select new PurchaseRequisitionInstallmentViewModel
                                                            {
                                                                PurchaseRequisitionInstallmentID = s.PurchaseRequisitionInstallmentID,
                                                                PurchaseRequisitionID = s.PurchaseRequisitionID,
                                                                RequisitionNo = p.RequisitionNo,
                                                                InstallmentDate = s.InstallmentDate,
                                                                Amount = s.Amount,
                                                                PayableAmount = s.PayableAmount,
                                                                PayableDate = s.PayableDate,
                                                                SetDate = s.SetDate,
                                                                UserID = s.UserID
                                                            }).AsEnumerable();
            return data;
        }
    }
}
