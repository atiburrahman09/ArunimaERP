using ScopoERP.Stackholder.ViewModel;
using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Stackholder.BLL
{
    public class SupplierLogic
    {
        private UnitOfWork unitOfWork;
        private supplier supplier;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        public SupplierLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierVM"></param>
        public void CreateSupplier(SupplierViewModel supplierVM)
        {
            if(supplierVM.SupplierID != 0)
            {
                UpdateSupplier(supplierVM);
            }
            else
            {
                supplier = new supplier
                {
                    SupplierName = supplierVM.SupplierName,
                    Address = supplierVM.Address,
                    Email = supplierVM.Email,
                    ContactNumber = supplierVM.ContactNumber
                };

                unitOfWork.SupplierRepository.Insert(supplier);
                unitOfWork.Save();
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierVM"></param>
        public void UpdateSupplier(SupplierViewModel supplierVM)
        {
            supplier = new supplier
            {
                SupplierId = supplierVM.SupplierID,
                SupplierName = supplierVM.SupplierName,
                Address = supplierVM.Address,
                Email = supplierVM.Email,
                ContactNumber = supplierVM.ContactNumber
            };

            unitOfWork.SupplierRepository.Update(supplier);
            unitOfWork.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<SupplierViewModel> GetAllSupplier()
        {
            var result = (from s in unitOfWork.SupplierRepository.Get()
                          orderby s.SupplierId descending
                          select new SupplierViewModel
                          {
                              SupplierID = s.SupplierId,
                              SupplierName = s.SupplierName,
                              Address = s.Address,
                              Email = s.Email,
                              ContactNumber = s.ContactNumber
                          }).ToList();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SupplierViewModel GetSupplierByID(int id)
        {
            var result = (from s in unitOfWork.SupplierRepository.Get()
                          where s.SupplierId == id
                          select new SupplierViewModel
                          {
                              SupplierID = s.SupplierId,
                              SupplierName = s.SupplierName,
                              Address = s.Address,
                              Email = s.Email,
                              ContactNumber = s.ContactNumber
                          }).SingleOrDefault();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<DropDownListViewModel> GetSupplierDropDown()
        {
            var result = (from s in unitOfWork.SupplierRepository.Get()
                          select new DropDownListViewModel
                          {
                              Value = s.SupplierId,
                              Text = s.SupplierName
                          }).ToList();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierName"></param>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        public bool IsUniqueSupplier(string supplierName, Nullable<int> supplierID = null)
        {
            IQueryable<int> result;

            if (supplierID == null)
            {
                result = from s in unitOfWork.SupplierRepository.Get()
                         where s.SupplierName == supplierName
                         select s.SupplierId;
            }
            else
            {
                result = from s in unitOfWork.SupplierRepository.Get()
                         where s.SupplierName == supplierName & s.SupplierId != supplierID
                         select s.SupplierId;
            }

            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }

       
    }
}
