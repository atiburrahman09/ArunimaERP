using ScopoERP.MaterialManagement.BLL;
using ScopoERP.MaterialManagement.ViewModel;
using ScopoERP.LC.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace ScopoERP.Web.Areas.Merchandising.Controllers
{
    [Authorize(Roles = "Admin, ItemEntry")]
    public class ItemCategoryController : Controller
    {
        private ItemCategoryLogic itemCategoryLogic;

        public ItemCategoryController(ItemCategoryLogic itemCategoryLogic)
        {
            this.itemCategoryLogic = itemCategoryLogic;
        }

        public ActionResult Index()
        {
            List<ItemCategoryViewModel> data = itemCategoryLogic.GetAllItemCategory();
            return View(data);
        }

        //[GridAction]
        //public ActionResult GetAllItemCategory()
        //{
            

        //    return View(new GridModel(data));
        //}

        public ActionResult Create()
        {
            ViewBag.ParentCategory = new SelectList(itemCategoryLogic.GetItemCategoryDropDown(), "Value", "Text");
            return View();
        }

        [HttpPost]
        public ActionResult Create(ItemCategoryViewModel itemCategoryVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    itemCategoryLogic.CreateItemCategory(itemCategoryVM);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                        the problem persists, Contact with Entitas Technologia.");
                }
            }
            ViewBag.ParentCategory = new SelectList(itemCategoryLogic.GetItemCategoryDropDown(), "Value", "Text", itemCategoryVM.ParentCategoryID);
            return View(itemCategoryVM);
        }

        public ActionResult Edit(int id)
        {
            ItemCategoryViewModel itemCategoryVM = itemCategoryLogic.GetItemCategoryByID(id);

            if (itemCategoryVM == null)
            {
                //it will actually return to 404 page
                return RedirectToAction("NotFound404", "Error");
            }
            ViewBag.ParentCategory = new SelectList(itemCategoryLogic.GetItemCategoryDropDown(), "Value", "Text", itemCategoryVM.ParentCategoryID);
            return View(itemCategoryVM);
        }

        [HttpPost]
        public ActionResult Edit(ItemCategoryViewModel itemCategoryVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    itemCategoryLogic.UpdateItemCategory(itemCategoryVM);

                    return RedirectToAction("Index");
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                        the problem persists, Contact with Entitas Technologia.");
                }
            }
            ViewBag.ParentCategory = new SelectList(itemCategoryLogic.GetItemCategoryDropDown(), "Value", "Text", itemCategoryVM.ParentCategoryID);
            return View(itemCategoryVM);
        }
    }
}