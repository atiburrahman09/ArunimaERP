using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using ScopoERP.MaterialManagement.ViewModel;
using ScopoERP.OrderManagement.BLL;
using ScopoERP.OrderManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScopoERP.MaterialManagement.BLL
{
    public class BookingLogic
    {
        private UnitOfWork unitOfWork;

        private PILogic piLogic;
        private CostsheetLogic costSheetLogic;
        private PurchaseOrderLogic purchaseOrderlogic;
        private SizeColorLogic sizeColorLogic;
        private ItemLogic itemLogic;
        private booking booking;
        private bookinghistory bookingHistory;
        private piinfo piInfo;

        public BookingLogic(UnitOfWork unitOfWork, PILogic piLogic,CostsheetLogic costSheetLogic,PurchaseOrderLogic purchaseOrderLogic,SizeColorLogic sizeColorLogic,ItemLogic itemLogic)
        {
            this.unitOfWork = unitOfWork;
            this.piLogic = piLogic;
            this.costSheetLogic = costSheetLogic;
            this.purchaseOrderlogic = purchaseOrderLogic;
            this.sizeColorLogic = sizeColorLogic;
            this.itemLogic=itemLogic;
        }

        public List<BookingViewModel> GetBookingFromWorksheet(int[] poStyleID, int itemId)
        {
            var tempResult = (from w in unitOfWork.WorksheetRepository.Get()
                              join p in unitOfWork.PurchaseOrderRepository.Get() on w.PoStyleId equals p.PoStyleId
                              join i in unitOfWork.ItemRepository.Get() on w.ItemId equals i.ItemId
                              join c in unitOfWork.ConsumptionUnitRepository.Get() on w.ConsumptionUnitId equals c.ConsumptionUnitId
                              where poStyleID.Contains(w.PoStyleId) && w.ItemId == itemId && w.Status != 0
                              group w by new
                              {
                                  w.ItemId,
                                  i.ItemCode,
                                  i.ItemDescription,
                                  w.ItemColor,
                                  w.ItemSize,
                                  w.ConsumptionUnitId,
                                  c.UnitName,
                                  w.UnitPrice,
                                  w.PoStyleId,
                                  p.PoNo
                              } into g
                              select g).ToList();


            var results = (from s in tempResult
                           select new BookingViewModel
                           {
                               PurchaseOrderID = s.Key.PoStyleId,
                               PONo = s.Key.PoNo,

                               ItemID = s.Key.ItemId,
                               ItemCode = s.Key.ItemCode,
                               ItemDescription = s.Key.ItemDescription,

                               ItemColor = s.Key.ItemColor,
                               ItemSize = s.Key.ItemSize,

                               ConsumptionUnitID = s.Key.ConsumptionUnitId,
                               ConsumptionUnitName = s.Key.UnitName,

                               UnitPrice = s.Key.UnitPrice,
                               TotalQuantity = s.Sum(x => x.TotalQuantity),
                               TotalPrice = s.Key.UnitPrice * s.Sum(x => x.TotalQuantity),
                           }).ToList();

            decimal totalQuantity = results.Sum(x=>x.TotalQuantity);
            results.ForEach(x => x.Ratio = x.TotalQuantity / totalQuantity);

            return results;
        }

        public List<BookingViewModel> GetBookingByPIID(int piID)
        {
            var results = (from s in unitOfWork.BookingRepository.Get()
                          join p in unitOfWork.PIRepository.Get() on s.PIId equals p.PIID
                          join i in unitOfWork.ItemRepository.Get() on s.ItemID equals i.ItemId
                          join c in unitOfWork.ConsumptionUnitRepository.Get() on s.ConsumptionUnitID equals c.ConsumptionUnitId
                          join po in unitOfWork.PurchaseOrderRepository.Get() on s.PurchaseOrderID equals po.PoStyleId  
                          where p.PIID == piID
                          select new BookingViewModel
                          {
                              BookingID = s.BookingID,
                              PurchaseOrderID = s.PurchaseOrderID,
                              PONo = po.PoNo,

                              ItemID = s.ItemID,
                              ItemCode = i.ItemCode,
                              ItemDescription = i.ItemDescription,

                              ItemSize = s.ItemSize,
                              ItemColor = s.ItemColor,

                              ConsumptionUnitID = s.ConsumptionUnitID,
                              ConsumptionUnitName = c.UnitName,

                              TotalQuantity = s.TotalQuantity,
                              UnitPrice = s.UnitPrice,
                              TotalPrice = s.TotalQuantity * s.UnitPrice,

                              PIID = s.PIId,
                              PINo = p.PINo,
                              ReferenceNo = p.ReferenceNo,

                              RevisionNo = s.RevisionNo,

                              UserID = s.UserID,
                              SetDate = s.SetDate
                          }).ToList();

            decimal totalQuantity = results.Sum(x => x.TotalQuantity);
            if (Convert.ToBoolean(totalQuantity)) { results.ForEach(x => x.Ratio = x.TotalQuantity / totalQuantity); }

            return results;
                         
        }

        public string CreateBooking(int? piID, List<BookingViewModel> bookingVM)
        {
            string referenceNo = "";
            if (piID == null || piID == 0)
            {
                referenceNo = this.GetNewReferenceNo();
                piInfo = new piinfo
                {
                    ReferenceNo = referenceNo,
                    Status = 1
                };

                foreach (var item in bookingVM)
                {
                    booking = new booking
                    {
                        PurchaseOrderID = item.PurchaseOrderID,
                        ItemID = item.ItemID,
                        ItemSize = item.ItemSize,
                        ItemColor = item.ItemColor,
                        TotalQuantity = item.TotalQuantity,
                        ConsumptionUnitID = item.ConsumptionUnitID,
                        UnitPrice = item.UnitPrice,
                        RevisionNo = 0,
                        UserID = item.UserID,
                        SetDate = item.SetDate,
                    };

                    piInfo.booking.Add(booking);
                }

                unitOfWork.PIRepository.Insert(piInfo);
            }
            else
            {
                piInfo = unitOfWork.PIRepository.GetById(piID);
                referenceNo = piInfo.ReferenceNo;

                foreach (var item in bookingVM)
                {
                    booking = new booking
                    {
                        PurchaseOrderID = item.PurchaseOrderID,
                        ItemID = item.ItemID,
                        ItemSize = item.ItemSize,
                        ItemColor = item.ItemColor,
                        TotalQuantity = item.TotalQuantity,
                        ConsumptionUnitID = item.ConsumptionUnitID,
                        UnitPrice = item.UnitPrice,
                        RevisionNo = 0,
                        UserID = item.UserID,
                        SetDate = item.SetDate,
                        PIId = piInfo.PIID
                    };

                    unitOfWork.BookingRepository.Insert(booking);
                }
            }

            unitOfWork.Save();

            return referenceNo;
        }

        public void ReviseBooking(List<BookingViewModel> bookingVM)
        {
            int piID = (int)bookingVM[0].PIID;

            var result = GetBookingByPIID(piID);

            foreach (var i in result)
            {
                bookingHistory = new bookinghistory
                {
                    PurchaseOrderID = i.PurchaseOrderID,
                    PIId = i.PIID ?? 0,
                    ItemID = i.ItemID,
                    ItemSize = i.ItemSize,
                    ItemColor = i.ItemColor,
                    TotalQuantity = i.TotalQuantity,
                    ConsumptionUnitID = i.ConsumptionUnitID,
                    UnitPrice = i.UnitPrice,
                    RevisionNo = i.RevisionNo,
                    UserID = i.UserID,
                    SetDate = i.SetDate
                };

                unitOfWork.BookingHistoryRepository.Insert(bookingHistory);
            }

            foreach (var item in bookingVM)
            {
                booking = new booking
                {
                    BookingID = item.BookingID,
                    PurchaseOrderID = item.PurchaseOrderID,
                    PIId = item.PIID,
                    ItemID = item.ItemID,
                    ItemSize = item.ItemSize,
                    ItemColor = item.ItemColor,
                    TotalQuantity = item.TotalQuantity,
                    ConsumptionUnitID = item.ConsumptionUnitID,
                    UnitPrice = item.UnitPrice,
                    RevisionNo = item.RevisionNo + 1,
                    UserID = item.UserID,
                    SetDate = item.SetDate,
                };

                unitOfWork.BookingRepository.Update(booking);
            }

            unitOfWork.Save();
        }

        public string GetNewReferenceNo()
        {
            string newReferenceNo = string.Empty;

            var result = (from c in unitOfWork.PIRepository.Get()
                          orderby c.PIID descending
                          select c.ReferenceNo).FirstOrDefault();

            if (result == null)
            {
                newReferenceNo = "REF-" + DateTime.Now.Year.ToString() + "-00001";
            }
            else
            {
                string newReferenceNoInDigit = (Convert.ToInt32(result.Split('-').Last()) + 1).ToString().PadLeft(5, '0');

                newReferenceNo = "REF-" + DateTime.Now.Year.ToString() + "-" + newReferenceNoInDigit;
            }

            return newReferenceNo;
        }

        public void UnmapBooking(int BookingID)
        {
            unitOfWork.BookingRepository.RawQuery("DELETE FROM booking WHERE bookingID = " + BookingID);
        }

        public List<DropDownListViewModel> GetFormulaDropDown()
        {
            List<DropDownListViewModel> FormulaList = new List<DropDownListViewModel>();
            FormulaList.Add(new DropDownListViewModel { Value = 1, Text = "Color" });
            FormulaList.Add(new DropDownListViewModel { Value = 2, Text = "Size" });
            FormulaList.Add(new DropDownListViewModel { Value = 3, Text = "Color & Size" });
            FormulaList.Add(new DropDownListViewModel { Value = 4, Text = "N/A" });

            return FormulaList;
        }

        public void DeletePI(int pIID)
        {
            unitOfWork.BookingRepository.RawQuery("DELETE FROM booking WHERE PIID = '" + pIID + "'");
            unitOfWork.BookingRepository.RawQuery("DELETE FROM piinfo WHERE PIID = '"+ pIID +"'");
        }

        #region Required for new systems

        public List<BookingSelectionViewModel> GetBillOfMaterialForBooking(int jobID, int itemCategoryID)
        {
            var result = (from w in unitOfWork.WorksheetRepository.Get()
                          join p in unitOfWork.PurchaseOrderRepository.Get() on w.PoStyleId equals p.PoStyleId
                          join s in unitOfWork.StyleRepository.Get() on p.StyleId equals s.StyleId
                          join i in unitOfWork.ItemRepository.Get() on w.ItemId equals i.ItemId
                          where p.JobId == jobID && i.ItemCategoryId == itemCategoryID
                          select new BookingSelectionViewModel
                          {
                              StyleNo         = s.StyleNo,
                              PurchaseOrderID = p.PoStyleId,
                              PONo            = p.PoNo,
                              ItemID          = i.ItemId,
                              ItemDescription = i.ItemDescription,
                              IsChecked       = false
                          }).OrderBy(x        => x.ItemDescription).ToList();

            return result;
        }


        public List<DropDownListViewModel> GetItemFromCostSheet(int[] poIDs)
        {
            var results = (from c in unitOfWork.CostsheetRepository.Get()
                           join d in unitOfWork.ItemRepository.Get()
                               on c.ItemId equals d.ItemId
                               join p in unitOfWork.PurchaseOrderRepository.Get()
                               on c.CostSheetNo equals p.CostSheetNo
                           where poIDs.Contains(p.PoStyleId)
                           //where p.PoStyleId == 4064
                           select new DropDownListViewModel
                           {
                               Value = d.ItemId,
                               Text = d.ItemDescription
                           }).Distinct().OrderBy(x => x.Text);

            return results.ToList();
        }

        public List<BookingViewModel> GetBookingByItemFormula(int[] poIDs,List<ItemFomulaViewModel> itemFormulaList)
        {
            List<BookingViewModel> BookingLists = new List<BookingViewModel>();
            foreach (var poId in poIDs)
            {
                var purchaseOrder = purchaseOrderlogic.GetPurchaseOrderByID(poId);
                var costSheetNoList = costSheetLogic.GetCostsheetNoByStyle(purchaseOrder.StyleID);
                foreach (var costSheetNo in costSheetNoList) {
                    var costSheet = costSheetLogic.GetCostSheetByCostsheetNo(costSheetNo);
                    foreach (ItemFomulaViewModel itemFormulaVM in itemFormulaList)
                    {
                        foreach (CostsheetViewModel csVM in costSheet)
                        {
                            if(Convert.ToString(itemFormulaVM.Item)=="all")
                            {
                                List<SizeColorDetailsViewModel> sizeColorVMList = sizeColorLogic.GetSizeColorByFormula(poId, itemFormulaVM.Formula);

                                foreach (SizeColorDetailsViewModel scVM in sizeColorVMList)
                                {
                                    BookingViewModel bookingVM = new BookingViewModel();
                                    bookingVM.SetDate = DateTime.Now;
                                    bookingVM.PurchaseOrderID = poId;
                                    bookingVM.ConsumptionUnitID = csVM.ConsumptionUnitID;
                                    bookingVM.PONo = purchaseOrder.PurchaseOrderNo;
                                    bookingVM.ItemID = csVM.ItemID; // Not sure about this.
                                    bookingVM.ItemDescription = itemLogic.GetItemByID(csVM.ItemID).ItemDescription;
                                    bookingVM.ItemSize = scVM.Size;
                                    bookingVM.ItemColor = scVM.Color;
                                    bookingVM.TotalPrice = csVM.TotalRawMaterials.GetValueOrDefault(1) * scVM.Quantity;
                                    bookingVM.ConsumptionUnitName = csVM.ConsumptionUnit;
                                    bookingVM.TotalQuantity = scVM.Quantity;
                                    bookingVM.UnitPrice = csVM.UnitPrice;
                                    BookingLists.Add(bookingVM);
                                }

                            }
                            else if(itemFormulaVM.Item == csVM.ItemID)
                            {
                                List<SizeColorDetailsViewModel> sizeColorVMList = sizeColorLogic.GetSizeColorByFormula(poId, itemFormulaVM.Formula);

                                foreach (SizeColorDetailsViewModel scVM in sizeColorVMList)
                                {
                                    BookingViewModel bookingVM = new BookingViewModel();
                                    bookingVM.SetDate = DateTime.Now;
                                    bookingVM.PurchaseOrderID = poId;
                                    bookingVM.ConsumptionUnitID = csVM.ConsumptionUnitID;
                                    bookingVM.PONo= purchaseOrder.PurchaseOrderNo;
                                    bookingVM.ItemID = itemFormulaVM.Item;
                                    bookingVM.ItemDescription = itemLogic.GetItemByID(itemFormulaVM.Item).ItemDescription;
                                    bookingVM.ItemSize = scVM.Size;
                                    bookingVM.ItemColor = scVM.Color;
                                    bookingVM.TotalPrice = csVM.TotalRawMaterials.GetValueOrDefault(1) * scVM.Quantity;
                                    bookingVM.ConsumptionUnitName = csVM.ConsumptionUnit;
                                    bookingVM.TotalQuantity = scVM.Quantity;
                                    bookingVM.UnitPrice = csVM.UnitPrice;
                                    BookingLists.Add(bookingVM);
                                }

                            }
                        }
                    }
                }  
            }

            return BookingLists;
        }






        #endregion
    }
}
