using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using ScopoERP.Store.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Store.BLL
{
    public class InventoryIssueLogic
    {
        private inventoryissue inventoryIssue;
        private UnitOfWork unitOfWork;
        private sr sr;

        public InventoryIssueLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void SaveInventoryIssue(SrViewModel srVM, int srId)
        {
            unitOfWork.InventoryissueRepository.RawQuery("DELETE FROM inventoryissue WHERE SRID = '" + srVM.SRID + "'");
            foreach (var issue in srVM.Inventories)
            {
                inventoryIssue = new inventoryissue
                {
                    SRID = srId,
                    IssuedQuantity = issue.IssuedQuantity,
                    ItemID = issue.ItemID,
                    PoStyleId = issue.PoStyleId,
                    Status = issue.Status,
                    RequestedQuantity = issue.RequestedQuantity
                };
                unitOfWork.InventoryissueRepository.Insert(inventoryIssue);

            }
            unitOfWork.Save();
        }

        public int SaveSr(SrViewModel srVM)
        {
            if (srVM.SRID == 0)
            {
                sr = new sr
                {
                    SRNo = srVM.SRNo,
                    IssuedDate = srVM.IssuedDate,
                    ReceiverName = srVM.ReceiverName,
                    CreatedBy = srVM.CreatedBy,
                    CreatedDate = srVM.CreatedDate,
                    FloorLineID = srVM.FloorLineID,
                    Remarks = srVM.Remarks,
                    Status = 1
                };
                unitOfWork.SrRepository.Insert(sr);
            }
            else
            {
                sr = new sr()
                {
                    SRNo = srVM.SRNo,
                    IssuedDate = srVM.IssuedDate,
                    ReceiverName = srVM.ReceiverName,
                    CreatedBy = srVM.CreatedBy,
                    CreatedDate = srVM.CreatedDate,
                    FloorLineID = srVM.FloorLineID,
                    Remarks = srVM.Remarks,
                    Status = 1,
                    SRID = srVM.SRID
                };
                unitOfWork.SrRepository.Update(sr);
            }

            unitOfWork.Save();
            return sr.SRID;
        }

        public string GetAutoSRNo()
        {
            var lastRef = unitOfWork.SrRepository.Get()
                .OrderByDescending(x => x.SRID)
                .Select(x => x.SRNo).FirstOrDefault();
            string newRef = "SR-" + DateTime.Now.Year + "-";
            if (lastRef == null)
            {
                newRef += "00001";
            }
            else
            {
                int num = int.Parse(lastRef.Substring(8, 5));
                num += 1;
                newRef += num.ToString().PadLeft(5, '0');
            }
            return newRef;
        }

        public List<DropDownListViewModel> GetIssues()
        {
            var result = unitOfWork.SrRepository.Get()
                .Select(x => new DropDownListViewModel
                {
                    Text = x.SRNo,
                    Value = x.SRID
                }).ToList();
            return result;
        }

        public SrViewModel GetIssueById(int id)
        {
            var result = unitOfWork.SrRepository.Get()
                .Where(sr => sr.SRID == id)
                .Select(sr => new SrViewModel()
                {
                    SRNo = sr.SRNo,
                    IssuedDate = sr.IssuedDate,
                    ReceiverName = sr.ReceiverName,
                    CreatedBy = sr.CreatedBy,
                    CreatedDate = sr.CreatedDate,
                    FloorLineID = sr.FloorLineID,
                    Remarks = sr.Remarks,
                    Status = sr.Status,
                    IssuedBy = sr.IssuedBy,
                    SRID = sr.SRID
                }).SingleOrDefault();


            var inventories = unitOfWork.InventoryissueRepository.Get()
                .Where(issue => issue.SRID == id)
                .Select(issue => new InventoryIssueViewModel
                {
                    SRID = id,
                    InventoryIssueID = issue.InventoryIssueID,
                    IssuedQuantity = issue.IssuedQuantity,
                    ItemID = issue.ItemID,
                    PoStyleId = issue.PoStyleId,
                    Status = issue.Status,
                    RequestedQuantity = issue.RequestedQuantity
                }).ToList();

            result.Inventories = inventories;

            return result;
        }


        public bool IsUnique(SrViewModel srVM)
        {
            IQueryable<int> result;

            if (srVM.SRID == 0)
            {
                result = (from s in unitOfWork.SrRepository.Get()
                          where s.SRNo.ToLower() == srVM.SRNo.ToLower()
                          select s.SRID);
            }
            else
            {
                result = (from s in unitOfWork.SrRepository.Get()
                          where s.SRNo.ToLower().Trim() == srVM.SRNo.ToLower().Trim() && s.SRID != srVM.SRID
                          select s.SRID);
            }

            if (result.Count() > 0)
            {
                return false;
            }
            return true;

        }

        public object GetIssueBySR(string id)
        {
            var srID = (from s in unitOfWork.SrRepository.Get()
                        where s.SRNo == id
                        select s.SRID).SingleOrDefault();

            var result = unitOfWork.SrRepository.Get()
               .Where(sr => sr.SRID == srID)
               .Select(sr => new SrViewModel()
               {
                   SRNo = sr.SRNo,
                   IssuedDate = sr.IssuedDate,
                   ReceiverName = sr.ReceiverName,
                   CreatedBy = sr.CreatedBy,
                   CreatedDate = sr.CreatedDate,
                   FloorLineID = sr.FloorLineID,
                   Remarks = sr.Remarks,
                   Status = sr.Status,
                   IssuedBy = sr.IssuedBy,
                   SRID = sr.SRID
               }).SingleOrDefault();


            var inventories = unitOfWork.InventoryissueRepository.Get()
                .Where(issue => issue.SRID == srID)
                .Select(issue => new InventoryIssueViewModel
                {
                    SRID = srID,
                    InventoryIssueID = issue.InventoryIssueID,
                    IssuedQuantity = issue.IssuedQuantity,
                    ItemID = issue.ItemID,
                    PoStyleId = issue.PoStyleId,
                    Status = issue.Status,
                    RequestedQuantity = issue.RequestedQuantity
                }).ToList();

            result.Inventories = inventories;

            return result;
        }
    }
}
