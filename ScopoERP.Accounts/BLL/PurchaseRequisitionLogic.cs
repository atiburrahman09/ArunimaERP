using ScopoERP.Accounts.ViewModel;
using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Accounts.BLL
{
    public class PurchaseRequisitionLogic
    {
        private UnitOfWork unitOfWork;
        private purchaserequisition purchaseRequisition;
        private purchaserequisitiondetails purchaseRequisitionDetails;


        public PurchaseRequisitionLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public List<PurchaseRequisitionViewModel> GetAllPurchaseRequisition(string userId)
        {
            var requisitionList = (from s in unitOfWork.PurchaseRequisitionRepository.Get()
                                   join d in unitOfWork.PurchaseRequisitionDetailsRepository.Get() on s.PurchaseRequisitionID equals d.PurchaseRequisitionID
                                   group d by s into c
                                   where c.Key.UserID == 0
                                   select new PurchaseRequisitionViewModel
                                   {
                                       PurchaseRequisitionID = c.Key.PurchaseRequisitionID,
                                       RequisitionNo = c.Key.RequisitionNo,
                                       RequisitionDate = c.Key.RequisitionDate,
                                       DepartmentID = c.Key.DepartmentID,
                                       Remarks = c.Key.Remarks,
                                       Sector = c.Key.Sector,
                                       UserID = 0,
                                       SetDate = c.Key.SetDate,
                                       RequisitionAmount = c.Sum(x => x.Quantity * x.UnitPrice)
                                   }).OrderByDescending(x => x.RequisitionNo).ToList();

            var installmentList = unitOfWork.PurchaseRequisitionInstallmentRepository.Get().AsEnumerable();

            var result = (from s in requisitionList
                          join i in installmentList on s.PurchaseRequisitionID equals i.PurchaseRequisitionID into ig
                          from c in ig.DefaultIfEmpty()
                          select new PurchaseRequisitionViewModel
                            {
                                PurchaseRequisitionID = s.PurchaseRequisitionID,
                                RequisitionNo = s.RequisitionNo,
                                RequisitionDate = s.RequisitionDate,
                                DepartmentID = s.DepartmentID,
                                Remarks = s.Remarks,
                                Sector = s.Sector,
                                UserID = s.UserID,
                                SetDate = s.SetDate,

                                RequisitionAmount = s.RequisitionAmount,
                                InstallmentAmount = c == null ? null : (decimal?)c.Amount,
                                InstallmentDate = c == null ? null : (DateTime?)c.InstallmentDate,
                                PayableAmount = c == null ? null : (decimal?)c.PayableAmount,
                                PayableDate = c == null ? null : (DateTime?)c.PayableDate
                            }).ToList();

            return result;
        }


        public PurchaseRequisitionViewModel GetPurchaseRequisition(int purchaseRequisitionID)
        {
            var requisition = (from s in unitOfWork.PurchaseRequisitionRepository.Get()
                               where s.PurchaseRequisitionID == purchaseRequisitionID
                               select new PurchaseRequisitionViewModel
                               {
                                   PurchaseRequisitionID = s.PurchaseRequisitionID,
                                   RequisitionNo = s.RequisitionNo,
                                   RequisitionDate = s.RequisitionDate,
                                   DepartmentID = s.DepartmentID,
                                   Remarks = s.Remarks,
                                   Sector = s.Sector,
                                   UserID = 0,
                                   SetDate = s.SetDate
                               }).SingleOrDefault();

            if (requisition != null)
            {
                IEnumerable<PurchaseRequisitionDetailsViewModel> requisitionDetails
                    = (from s in unitOfWork.PurchaseRequisitionDetailsRepository.Get()
                       where s.PurchaseRequisitionID == purchaseRequisitionID
                       select new PurchaseRequisitionDetailsViewModel
                       {
                           PurchaseRequisitionDetailsID = s.PurchaseRequisitionDetailsID,
                           ProductDescription = s.ProductDescription,
                           Quantity = s.Quantity,
                           UnitID = s.UnitID,
                           UnitPrice = s.UnitPrice
                       }).AsEnumerable();

                requisition.RequisitionDetails = requisitionDetails;
            }

            return requisition;
        }


        public string SavePurchaseRequisition(PurchaseRequisitionViewModel purchaseRequisitionVM)
        {
            var existingRequisition
                = unitOfWork.PurchaseRequisitionRepository
                .Get()
                .Where(x => x.RequisitionNo == purchaseRequisitionVM.RequisitionNo)
                .SingleOrDefault();

            if (existingRequisition != null)
            {
                unitOfWork.PurchaseRequisitionRepository.Delete(existingRequisition);
                unitOfWork.Save();
            }

            purchaseRequisition = new purchaserequisition();
            purchaseRequisition.RequisitionNo = String.IsNullOrEmpty(purchaseRequisitionVM.RequisitionNo)
                ? GetNewRequisitionNo() : purchaseRequisitionVM.RequisitionNo;

            purchaseRequisition.RequisitionDate = purchaseRequisitionVM.RequisitionDate;
            purchaseRequisition.DepartmentID = purchaseRequisitionVM.DepartmentID;
            purchaseRequisition.Sector = purchaseRequisitionVM.Sector;

            purchaseRequisition.Remarks = purchaseRequisitionVM.Remarks;
            purchaseRequisition.UserID = 0;
            purchaseRequisition.SetDate = DateTime.Now;

            foreach (var item in purchaseRequisitionVM.RequisitionDetails)
            {
                purchaseRequisitionDetails = new purchaserequisitiondetails
                {
                    ProductDescription = item.ProductDescription,
                    Quantity = item.Quantity,
                    UnitID = item.UnitID,
                    UnitPrice = item.UnitPrice
                };
                purchaseRequisition.purchaserequisitiondetails.Add(purchaseRequisitionDetails);
            }

            unitOfWork.PurchaseRequisitionRepository.Insert(purchaseRequisition);
            unitOfWork.Save();

            return purchaseRequisition.RequisitionNo;
        }


        public string GetNewRequisitionNo()
        {
            string newRequisitionNo = string.Empty;

            var result = (from c in unitOfWork.PurchaseRequisitionRepository.Get()
                          orderby c.RequisitionNo descending
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


        public List<DropDownListViewModel> GetPurchaseRequisitionDropDown()
        {
            List<DropDownListViewModel> result = (from s in unitOfWork.PurchaseRequisitionRepository.Get()
                                                  select new DropDownListViewModel
                                                  {
                                                      Text = s.RequisitionNo,
                                                      Value = s.PurchaseRequisitionID
                                                  }).Distinct().ToList();

            return result;
        }


        public decimal GetTotalAmount(int purchaseRequisitionID)
        {
            decimal totalAmount = (from s in unitOfWork.PurchaseRequisitionDetailsRepository.Get()
                                   where s.PurchaseRequisitionID == purchaseRequisitionID
                                   select s.Quantity * s.UnitPrice).Sum();

            return totalAmount;
        }
    }
}
