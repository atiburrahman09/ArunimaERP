using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using ScopoERP.Stackholder.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Stackholder.BLL
{
    public class CustomerLogic
    {
        private UnitOfWork unitOfWork;
        private customerinfo customer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        public CustomerLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerVM"></param>
        public void CreateCustomer(CustomerViewModel customerVM)
        {
            if(customerVM.CustomerID != 0)
            {
                UpdateCustomer(customerVM);
            }
            else
            {
                customer = new customerinfo
                {
                    CustomerName = customerVM.CustomerName
                };

                unitOfWork.CustomerRepository.Insert(customer);
                unitOfWork.Save();
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerVM"></param>
        public void UpdateCustomer(CustomerViewModel customerVM)
        {
            customer = new customerinfo
            {
                CustomerId = customerVM.CustomerID,
                CustomerName = customerVM.CustomerName
            };

            unitOfWork.CustomerRepository.Update(customer);
            unitOfWork.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<CustomerViewModel> GetAllCustomer()
        {
            var result = (from s in unitOfWork.CustomerRepository.Get()
                          orderby s.CustomerId descending
                          select new CustomerViewModel
                          {
                              CustomerID = s.CustomerId,
                              CustomerName = s.CustomerName
                          }).ToList();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CustomerViewModel GetCustomerByID(int id)
        {
            var result = (from s in unitOfWork.CustomerRepository.Get()
                          where s.CustomerId == id
                          select new CustomerViewModel
                          {
                              CustomerID = s.CustomerId,
                              CustomerName = s.CustomerName
                          }).SingleOrDefault();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<DropDownListViewModel> GetCustomerDropDown()
        {
            var result = (from s in unitOfWork.CustomerRepository.Get()
                          select new DropDownListViewModel
                          {
                              Value = s.CustomerId,
                              Text = s.CustomerName
                          }).ToList();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerName"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public bool IsUniqueCustomer(string customerName, Nullable<int> customerID = null)
        {
            IQueryable<int> result;

            if (customerID == null)
            {
                result = from s in unitOfWork.CustomerRepository.Get()
                         where s.CustomerName == customerName
                         select s.CustomerId;
            }
            else
            {
                result = from s in unitOfWork.CustomerRepository.Get()
                         where s.CustomerName == customerName & s.CustomerId != customerID
                         select s.CustomerId;
            }

            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }
    }
}
