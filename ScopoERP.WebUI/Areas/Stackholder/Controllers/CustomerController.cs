using ScopoERP.Domain.Repositories;
using ScopoERP.LC.BLL;
using ScopoERP.LC.ViewModel;
using ScopoERP.Stackholder.BLL;
using ScopoERP.Stackholder.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace ScopoERP.WebUI.Areas.Stackholder.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CustomerController : Controller
    {
        private CustomerLogic customerLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerLogic"></param>
        public CustomerController(CustomerLogic customerLogic)
        {
            this.customerLogic = customerLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [GridAction]
        public ViewResult GetAllCustomer()
        {
            List<CustomerViewModel> customerVM = customerLogic.GetAllCustomer();

            return View(new GridModel(customerVM));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerVM"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(CustomerViewModel customerVM)
        {
            if (ModelState.IsValid)
            {
                if (!customerLogic.IsUniqueCustomer(customerVM.CustomerName.Trim()))
                {
                    ModelState.AddModelError("", customerVM.CustomerName + " already exists");
                }
                else
                {
                    try
                    {
                        customerLogic.CreateCustomer(customerVM);

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }

            return View(customerVM);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            CustomerViewModel customerVM = customerLogic.GetCustomerByID(id);

            if (customerVM == null)
            {
                //it will actually return to 404 page
                return RedirectToAction("NotFound404", "Error");
            }
            return View(customerVM);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerVM"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(CustomerViewModel customerVM)
        {
            if (ModelState.IsValid)
            {
                if (!customerLogic.IsUniqueCustomer(customerVM.CustomerName.Trim(), customerVM.CustomerID))
                {
                    ModelState.AddModelError("", @"This Customer No is already exists");
                }
                else
                {
                    try
                    {
                        customerLogic.UpdateCustomer(customerVM);

                        return RedirectToAction("Index");
                    }
                    catch (DataException)
                    {
                        ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                        the problem persists, Contact with Entitas Technologia.");
                    }
                }
            }
            return View(customerVM);
        }
    }
}