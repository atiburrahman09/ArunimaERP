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
    public class ItemController : Controller
    {
        private ItemLogic itemLogic;
        private ItemCategoryLogic itemCategoryLogic;

        public ItemController(ItemLogic itemLogic, ItemCategoryLogic itemCategoryLogic)
        {
            this.itemLogic = itemLogic;
            this.itemCategoryLogic = itemCategoryLogic;
        }

        public ActionResult Index()
        {
            return View();
        }

        [GridAction]
        public ActionResult GetAllItem()
        {
            List<ItemViewModel> data = itemLogic.GetAllItem();

            return View(new GridModel(data));
        }

        public ActionResult Create()
        {
            ViewBag.ItemCategory = new SelectList(itemCategoryLogic.GetItemCategoryDropDown(), "Value", "Text");

            return View();
        }

        [HttpPost]
        public ActionResult Create(ItemViewModel itemVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    itemLogic.CreateItem(itemVM);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            ViewBag.ItemCategory = new SelectList(itemCategoryLogic.GetItemCategoryDropDown(), "Value", "Text", itemVM.ItemCategoryID);

            return View(itemVM);
        }

        public ActionResult Edit(int id)
        {
            ItemViewModel itemVM = itemLogic.GetItemByID(id);

            if (itemVM == null)
            {
                //it will actually return to 404 page
                return RedirectToAction("NotFound404", "Error");
            }

            ViewBag.ItemCategory = new SelectList(itemCategoryLogic.GetItemCategoryDropDown(), "Value", "Text", itemVM.ItemCategoryID);

            return View(itemVM);
        }

        [HttpPost]
        public ActionResult Edit(ItemViewModel itemVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    itemLogic.UpdateItem(itemVM);

                    return RedirectToAction("Index");
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                        the problem persists, Contact with Entitas Technologia.");
                }
            }

            ViewBag.ItemCategory = new SelectList(itemCategoryLogic.GetItemCategoryDropDown(), "Value", "Text", itemVM.ItemCategoryID);

            return View(itemVM);
        }
    }
}