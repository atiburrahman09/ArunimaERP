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

namespace ScopoERP.WebUI.Areas.MaterialManagement.Controllers
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
            return View();
        }

        [GridAction]
        public ActionResult GetAllItemCategory()
        {
            List<ItemCategoryViewModel> data = itemCategoryLogic.GetAllItemCategory();

            return View(new GridModel(data));
        }

        public ActionResult Create()
        {
            ViewBag.ParentCategory = new SelectList(itemCategoryLogic.GetItemCategoryDropDown(), "Value", "Text");
            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(ItemCategoryViewModel itemCategoryVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    itemCategoryLogic.CreateItemCategory(itemCategoryVM);

                    return Json(true);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                        the problem persists, Contact with Entitas Technologia.");
                }
            }
            ViewBag.ParentCategory = new SelectList(itemCategoryLogic.GetItemCategoryDropDown(), "Value", "Text", itemCategoryVM.ParentCategoryID);
            return PartialView(itemCategoryVM);
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
            return PartialView(itemCategoryVM);
        }

        [HttpPost]
        public ActionResult Edit(ItemCategoryViewModel itemCategoryVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    itemCategoryLogic.UpdateItemCategory(itemCategoryVM);

                    return Json(true);
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                        the problem persists, Contact with Entitas Technologia.");
                }
            }
            ViewBag.ParentCategory = new SelectList(itemCategoryLogic.GetItemCategoryDropDown(), "Value", "Text", itemCategoryVM.ParentCategoryID);
            return PartialView(itemCategoryVM);
        }
    }
}