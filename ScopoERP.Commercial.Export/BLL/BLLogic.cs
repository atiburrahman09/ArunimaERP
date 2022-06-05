using ScopoERP.Commercial.ViewModel;
using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Commercial.BLL
{
    public class BLLogic
    {
        private UnitOfWork unitOfWork;
        private bl bl;

        public BLLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public int CreateChalan(BLViewModel blVM)
        {
            this.bl = new bl()
            {
                BLNo = blVM.BLNo,
                BLDate = blVM.BLDate,
                InvoiceNo = blVM.InvoiceNo,
                InvoiceDate = blVM.InvoiceDate,
                CopyDocumentReceivedDate = blVM.CopyDocumentReceivedDate,
                OriginalDocumentReceivedDate = blVM.OriginalDocumentReceivedDate,
                DocumentSentToCNF = blVM.DocumentSentToCNF,
                GoodsDeliveryDateByCNF = blVM.GoodsDeliveryDateByCNF,
                GoodsInHouseDate = blVM.GoodsInHouseDate,
                AcceptanceValue = blVM.AcceptanceValue,
                AcceptanceDate = blVM.AcceptanceDate,
                BackToBackLCID = blVM.BackToBackLCID,
                MaturityDate = blVM.MaturityDate,
                Port = blVM.Port,
                BillEntryNo = blVM.BillEntryNo,
                BillEntryDate = blVM.BillEntryDate,
                IsChalan = true,
                Status = blVM.Status
            };

            unitOfWork.BLRepository.Insert(bl);
            unitOfWork.Save();

            return bl.BLID;
        }
        

        public int CreateBL(BLViewModel blVM)
        {
            this.bl = new bl()
            {
                BLNo = blVM.BLNo,
                BLDate = blVM.BLDate,
                InvoiceNo = blVM.InvoiceNo,
                InvoiceDate = blVM.InvoiceDate,
                CopyDocumentReceivedDate = blVM.CopyDocumentReceivedDate,
                OriginalDocumentReceivedDate = blVM.OriginalDocumentReceivedDate,
                DocumentSentToCNF = blVM.DocumentSentToCNF,
                GoodsDeliveryDateByCNF = blVM.GoodsDeliveryDateByCNF,
                GoodsInHouseDate = blVM.GoodsInHouseDate,
                AcceptanceValue = blVM.AcceptanceValue,
                AcceptanceDate = blVM.AcceptanceDate,
                BackToBackLCID = blVM.BackToBackLCID,
                MaturityDate = blVM.MaturityDate,
                Port = blVM.Port,
                BillEntryNo = blVM.BillEntryNo,
                BillEntryDate = blVM.BillEntryDate,
                IsChalan = false,
                Status = blVM.Status
            };

            unitOfWork.BLRepository.Insert(bl);
            unitOfWork.Save();

            return bl.BLID;
        }

        public void UpdateBL(BLViewModel blVM)
        {
            this.bl = new bl()
            {
                BLID = blVM.BLID,
                BLNo = blVM.BLNo,
                BLDate = blVM.BLDate,
                InvoiceNo = blVM.InvoiceNo,
                InvoiceDate = blVM.InvoiceDate,
                CopyDocumentReceivedDate = blVM.CopyDocumentReceivedDate,
                OriginalDocumentReceivedDate = blVM.OriginalDocumentReceivedDate,
                DocumentSentToCNF = blVM.DocumentSentToCNF,
                GoodsDeliveryDateByCNF = blVM.GoodsDeliveryDateByCNF,
                GoodsInHouseDate = blVM.GoodsInHouseDate,
                AcceptanceValue = blVM.AcceptanceValue,
                AcceptanceDate = blVM.AcceptanceDate,
                BackToBackLCID = blVM.BackToBackLCID,
                MaturityDate = blVM.MaturityDate,
                Port = blVM.Port,
                BillEntryNo = blVM.BillEntryNo,
                BillEntryDate = blVM.BillEntryDate,
                IsChalan = blVM.IsChalan,
                Status = blVM.Status
            };

            unitOfWork.BLRepository.Update(bl);
            unitOfWork.Save();
        }

        public BLViewModel GetBLByID(int id)
        {
            var result = (from c in unitOfWork.BLRepository.Get()
                          join b in unitOfWork.BackToBackLCRepository.Get() on c.BackToBackLCID equals b.BackToBackLCID
                          where c.BLID == id
                          select new BLViewModel
                          {
                              BLID = c.BLID,
                              BLNo = c.BLNo,
                              BLDate = c.BLDate,
                              InvoiceNo = c.InvoiceNo,
                              InvoiceDate = c.InvoiceDate,
                              CopyDocumentReceivedDate = c.CopyDocumentReceivedDate,
                              OriginalDocumentReceivedDate = c.OriginalDocumentReceivedDate,
                              DocumentSentToCNF = c.DocumentSentToCNF,
                              GoodsDeliveryDateByCNF = c.GoodsDeliveryDateByCNF,
                              GoodsInHouseDate = c.GoodsInHouseDate,
                              AcceptanceValue = c.AcceptanceValue,
                              AcceptanceDate = c.AcceptanceDate,

                              BackToBackLCID = c.BackToBackLCID,
                              BackToBackLCNo = b.BackToBackLC1,

                              MaturityDate = c.MaturityDate,
                              Port = c.Port,
                              BillEntryNo = c.BillEntryNo,
                              BillEntryDate = c.BillEntryDate,

                              IsChalan = c.IsChalan,

                              Status = c.Status
                          }).SingleOrDefault();

            return result;
        }

        public List<BLViewModel> GetAllBL()
        {
            var result = (from c in unitOfWork.BLRepository.Get().Where(x=> x.IsChalan == false)
                          join b in unitOfWork.BackToBackLCRepository.Get() on c.BackToBackLCID equals b.BackToBackLCID
                          orderby c.BLID descending
                          select new BLViewModel
                          {
                              BLID = c.BLID,
                              BLNo = c.BLNo,
                              BLDate = c.BLDate,
                              InvoiceNo = c.InvoiceNo,
                              InvoiceDate = c.InvoiceDate,
                              CopyDocumentReceivedDate = c.CopyDocumentReceivedDate,
                              OriginalDocumentReceivedDate = c.OriginalDocumentReceivedDate,
                              DocumentSentToCNF = c.DocumentSentToCNF,
                              GoodsDeliveryDateByCNF = c.GoodsDeliveryDateByCNF,
                              GoodsInHouseDate = c.GoodsInHouseDate,
                              AcceptanceValue = c.AcceptanceValue,
                              AcceptanceDate = c.AcceptanceDate,

                              BackToBackLCID = c.BackToBackLCID,
                              BackToBackLCNo = b.BackToBackLC1,

                              MaturityDate = c.MaturityDate,
                              Port = c.Port,
                              BillEntryNo = c.BillEntryNo,
                              BillEntryDate = c.BillEntryDate,

                              IsChalan = c.IsChalan,

                              Status = c.Status
                          }).ToList();

            return result;
        }

        public List<BLViewModel> GetAllChalan()
        {
            var result = (from c in unitOfWork.BLRepository.Get().Where(x=> x.IsChalan == true)
                          join b in unitOfWork.BackToBackLCRepository.Get() on c.BackToBackLCID equals b.BackToBackLCID into b2b 
                          from lc in b2b.DefaultIfEmpty()
                          orderby c.BLID descending
                          select new BLViewModel
                          {
                              BLID = c.BLID,
                              BLNo = c.BLNo,
                              BLDate = c.BLDate,
                              InvoiceNo = c.InvoiceNo,
                              InvoiceDate = c.InvoiceDate,
                              CopyDocumentReceivedDate = c.CopyDocumentReceivedDate,
                              OriginalDocumentReceivedDate = c.OriginalDocumentReceivedDate,
                              DocumentSentToCNF = c.DocumentSentToCNF,
                              GoodsDeliveryDateByCNF = c.GoodsDeliveryDateByCNF,
                              GoodsInHouseDate = c.GoodsInHouseDate,
                              AcceptanceValue = c.AcceptanceValue,
                              AcceptanceDate = c.AcceptanceDate,

                              BackToBackLCID = c.BackToBackLCID,
                              BackToBackLCNo = lc.BackToBackLC1,

                              MaturityDate = c.MaturityDate,
                              Port = c.Port,
                              BillEntryNo = c.BillEntryNo,
                              BillEntryDate = c.BillEntryDate,

                              IsChalan = c.IsChalan,

                              Status = c.Status
                          }).ToList();

            return result;
        }

        public bool IsUniqueBL(string blNo, Nullable<int> blID = null)
        {
            IQueryable<int> result;

            if (blID == null)
            {
                result = from s in unitOfWork.BLRepository.Get()
                         where s.BLNo == blNo
                         select s.BLID;
            }
            else
            {
                result = from s in unitOfWork.BLRepository.Get()
                         where s.BLNo == blNo & s.BLID != blID
                         select s.BLID;
            }

            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }

        public List<DropDownListViewModel> GetBLDropDown()
        {
            var results = (from c in unitOfWork.BLRepository.Get()
                           where c.IsChalan == false
                           select new DropDownListViewModel
                              {
                                  Value = c.BLID,
                                  Text = c.BLNo
                              }).ToList();
            
            return results;
        }

        public List<DropDownListViewModel> GetBLDropDownByPIID(int piID)
        {
            var results = (from b in unitOfWork.BLRepository.Get()
                           join bd in unitOfWork.BLDetailsRepository.Get() on b.BLID equals bd.BLID
                           join bk in unitOfWork.BookingRepository.Get() on bd.BookingID equals bk.BookingID
                           where bk.PIId == piID
                           select new DropDownListViewModel
                           {
                               Value = b.BLID,
                               Text = b.BLNo
                           }).ToList();

            return results;
        }


        //----------New Methods
        public List<BLViewModel> GetBLDropDownByBackToBackLCID(int backToBackLCID)
        {
            var result = (from c in unitOfWork.BLRepository.Get().Where(x => x.IsChalan == false)                          
                          where c.BackToBackLCID == backToBackLCID
                          orderby c.BLID descending
                          select new BLViewModel
                          {   
                              BLID = c.BLID,
                              BLNo = c.BLNo,
                              AcceptanceValue = c.AcceptanceValue,
                              AcceptanceDate = c.AcceptanceDate,
                                          
                          }).ToList();

            return result;
        }

        public List<BLViewModel> GetAllBL(int jobID)
        {
            var result = (from c in unitOfWork.BLRepository.Get().Where(x => x.IsChalan == false)
                          join b in unitOfWork.BackToBackLCRepository.Get() on c.BackToBackLCID equals b.BackToBackLCID
                          where b.JobID == jobID
                          orderby c.BLID descending
                          select new BLViewModel
                          {
                              BLID = c.BLID,
                              BLNo = c.BLNo,
                              BLDate = c.BLDate,
                              InvoiceNo = c.InvoiceNo,
                              InvoiceDate = c.InvoiceDate,
                              CopyDocumentReceivedDate = c.CopyDocumentReceivedDate,
                              OriginalDocumentReceivedDate = c.OriginalDocumentReceivedDate,
                              DocumentSentToCNF = c.DocumentSentToCNF,
                              GoodsDeliveryDateByCNF = c.GoodsDeliveryDateByCNF,
                              GoodsInHouseDate = c.GoodsInHouseDate,
                              AcceptanceValue = c.AcceptanceValue,
                              AcceptanceDate = c.AcceptanceDate,

                              BackToBackLCID = c.BackToBackLCID,
                              BackToBackLCNo = b.BackToBackLC1,

                              MaturityDate = c.MaturityDate,
                              Port = c.Port,
                              BillEntryNo = c.BillEntryNo,
                              BillEntryDate = c.BillEntryDate,

                              IsChalan = c.IsChalan,

                              Status = c.Status
                          }).ToList();

            return result;
        }
    }
}
