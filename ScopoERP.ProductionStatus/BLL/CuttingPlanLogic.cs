using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using ScopoERP.OrderManagement.BLL;
using ScopoERP.ProductionStatus.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.ProductionStatus.BLL
{
    public class CuttingPlanLogic
    {
        private UnitOfWork unitOfWork;

        private StyleLogic styleLogic;
        private cuttingPlan cuttingPlan;
        private Bundle bundle;

        public CuttingPlanLogic(UnitOfWork unitOfWork, StyleLogic styleLogic)
        {
            this.unitOfWork = unitOfWork;
            this.styleLogic = styleLogic;
        }

        public int CreatecuttingPlan(CuttingPlanViewModel cutting)
        {
            //unitOfWork.cuttingPlanRepository.RawQuery("DELETE FROM cuttingPlans WHERE PurchaseOrderID = " + cuttingPlanList[0].PurchaseOrderID);

            if (cutting != null)
            {
                cuttingPlan = new cuttingPlan
                {
                    CuttingQuantity = cutting.CuttingQuantity,
                    BundlePerQuantity = cutting.BundlePerQuantity,
                    CuttingDate = cutting.CuttingDate,
                    CuttingNo = cutting.CuttingNo,
                    PurchaseOrderID = cutting.PurchaseOrderID,
                    NoOfBundle = cutting.NoOfBundle,
                    LoopPattern = cutting.LoopPattern,
                    IsPrepack = cutting.IsPrepack,
                    Shade = cutting.Shade
                };
                unitOfWork.cuttingPlanRepository.Insert(cuttingPlan);

            }
            //unitOfWork.cuttingPlanRepository.Insert(cuttingPlan);
            unitOfWork.Save();
            return cuttingPlan.CuttingPlanID;
        }

        public void UpdatecuttingPlan(CuttingPlanViewModel cutting)
        {
            if (cutting != null)
            {
                cuttingPlan = new cuttingPlan
                {
                    CuttingPlanID = cutting.CuttingPlanID,
                    CuttingQuantity = cutting.CuttingQuantity,
                    BundlePerQuantity = cutting.BundlePerQuantity,
                    CuttingDate = cutting.CuttingDate,
                    CuttingNo = cutting.CuttingNo,
                    PurchaseOrderID = cutting.PurchaseOrderID,
                    NoOfBundle = cutting.NoOfBundle,
                    LoopPattern = cutting.LoopPattern,
                    IsPrepack = cutting.IsPrepack,
                    Shade = cutting.Shade
                };
                unitOfWork.cuttingPlanRepository.Update(cuttingPlan);
            }

            unitOfWork.Save();
        }

        public List<CuttingPlanViewModel> GetCuttingPlanByPOID(int pOID)
        {
            var result = (from c in unitOfWork.cuttingPlanRepository.Get()
                          where c.PurchaseOrderID == pOID
                          orderby c.CuttingNo descending
                          select new { c.CuttingNo, c.CuttingPlanID, c.CuttingQuantity, c.NoOfBundle, c.PurchaseOrderID, c.Shade, c.LoopPattern, c.IsPrepack, c.BundlePerQuantity, c.CuttingDate }).AsEnumerable()
                          .Select(x => new CuttingPlanViewModel
                          {
                              CuttingNo = x.CuttingNo,
                              CuttingQuantity = x.CuttingQuantity,
                              CuttingPlanID = x.CuttingPlanID,
                              NoOfBundle = x.NoOfBundle,
                              BundlePerQuantity = x.BundlePerQuantity,
                              LoopPattern = x.LoopPattern,
                              Shade = x.Shade,
                              IsPrepack = x.IsPrepack,
                              CuttingDate = x.CuttingDate
                          })
                          .ToList();

            return result;
        }


        public List<CuttingPlanViewModel> GetAllCuttingPlan()
        {
            var cuttingPlanList = (from cp in unitOfWork.cuttingPlanRepository.Get()
                                   select new CuttingPlanViewModel
                                   {
                                       CuttingPlanID = cp.CuttingPlanID,
                                       PurchaseOrderID = cp.PurchaseOrderID,
                                       CuttingQuantity = cp.CuttingQuantity,
                                       BundlePerQuantity = cp.BundlePerQuantity,
                                       CuttingDate = cp.CuttingDate,
                                       CuttingNo = cp.CuttingNo,
                                       NoOfBundle = cp.NoOfBundle,
                                       LoopPattern = cp.LoopPattern,
                                       IsPrepack = cp.IsPrepack,
                                       Shade = cp.Shade
                                   }).ToList();
            return cuttingPlanList;
        }



        public bool IsUnique(CuttingPlanViewModel cuttingPlanViewModel)
        {
            IQueryable<int> result;

            if (cuttingPlanViewModel.CuttingPlanID == 0)
            {
                result = (from c in unitOfWork.cuttingPlanRepository.Get()
                          where c.CuttingNo == cuttingPlanViewModel.CuttingNo && c.PurchaseOrderID == cuttingPlanViewModel.PurchaseOrderID
                          select c.CuttingPlanID);
            }
            else
            {
                result = (from c in unitOfWork.cuttingPlanRepository.Get()
                          where c.CuttingNo == cuttingPlanViewModel.CuttingNo && c.CuttingPlanID != cuttingPlanViewModel.CuttingPlanID && c.PurchaseOrderID == cuttingPlanViewModel.PurchaseOrderID
                          select c.CuttingPlanID);
            }

            if (result.Count() > 0)
            {
                return true;
            }
            return false;
        }

        public int GetLastBundleNo()
        {

            var lastRow = (from b in unitOfWork.BundleRepository.Get()
                           orderby b.BundleID descending
                           select b).FirstOrDefault();
            if (lastRow != null)
            {
                return Convert.ToInt32(lastRow.BundleNo);
            }
            else
            {
                return 0;
            }

        }

        //public object GetCuttingListByPOID(int pOID)
        //{
        //    var cuttingPlanList = (from cp in unitOfWork.cuttingPlanRepository.Get()
        //                           where cp.PurchaseOrderID == pOID
        //                           select new CuttingPlanViewModel
        //                           {
        //                               CuttingPlanID = cp.CuttingPlanID,
        //                               PurchaseOrderID = cp.PurchaseOrderID,
        //                               CuttingQuantity = cp.CuttingQuantity,
        //                               BundlePerQuantity = cp.BundlePerQuantity,
        //                               CuttingDate = cp.CuttingDate,
        //                               CuttingNo = cp.CuttingNo,
        //                               NoOfBundle = cp.NoOfBundle,
        //                               LoopPattern = cp.LoopPattern,
        //                               IsPrepack = cp.IsPrepack,
        //                               Shade=cp.Shade
        //                           }).ToList();
        //    return cuttingPlanList;
        //}

        public List<BundleViewModel> GetBundleInfoByCuttingID(int cuttingPLanID)
        {
            var bundleList = (from b in unitOfWork.BundleRepository.Get()
                              where b.CuttingPlanID == cuttingPLanID
                              select new BundleViewModel
                              {
                                  BundleID = b.BundleID,
                                  CuttingPlanID = b.CuttingPlanID,
                                  BundleNo = b.BundleNo,
                                  Quantity = b.Quantity,
                                  Size = b.Size
                              }).ToList();
            return bundleList;
        }



        public void CreateBundle(List<BundleViewModel> bundleVM)
        {
            foreach (var bundleInfo in bundleVM)
            {
                bundle = new Bundle
                {
                    CuttingPlanID = bundleVM[0].CuttingPlanID,
                    BundleNo = bundleInfo.BundleNo,
                    Quantity = bundleInfo.Quantity,
                    Size = bundleInfo.Size
                };
                unitOfWork.BundleRepository.Insert(bundle);
            }
            unitOfWork.Save();
        }

        public void UpdateBundle(List<BundleViewModel> bundleVM)
        {
            foreach (var bundleInfo in bundleVM)
            {
                bundle = new Bundle
                {
                    BundleID = bundleInfo.BundleID,
                    CuttingPlanID = bundleInfo.CuttingPlanID,
                    BundleNo = bundleInfo.BundleNo,
                    Quantity = bundleInfo.Quantity,
                    Size = bundleInfo.Size
                };
                unitOfWork.BundleRepository.Update(bundle);
            }
            unitOfWork.Save();
        }

        public List<CuttingPlanViewModel> GetCuttingDetails(int pOId)
        {
            var cuttingPlan = (from cp in unitOfWork.cuttingPlanRepository.Get()
                               where cp.PurchaseOrderID == pOId
                               select new CuttingPlanViewModel
                               {
                                   CuttingPlanID = cp.CuttingPlanID,
                                   PurchaseOrderID = cp.PurchaseOrderID,
                                   CuttingQuantity = cp.CuttingQuantity,
                                   BundlePerQuantity = cp.BundlePerQuantity,
                                   CuttingDate = cp.CuttingDate,
                                   CuttingNo = cp.CuttingNo,
                                   NoOfBundle = cp.NoOfBundle,
                                   LoopPattern = cp.LoopPattern,
                                   IsPrepack = cp.IsPrepack,
                                   Shade = cp.Shade
                               }).ToList();
            return cuttingPlan;
        }

        public List<DropDownListViewModel> GetCuttingPlanDropDown(int poID)
        {
            var result = (from c in unitOfWork.cuttingPlanRepository.Get()
                          where c.PurchaseOrderID == poID
                          select new { c.CuttingNo, c.CuttingPlanID }).AsEnumerable()
                          .Select(x => new DropDownListViewModel { Text = x.CuttingNo.ToString(), Value = x.CuttingPlanID })
                          .ToList();

            return result;
        }

        public List<DropDownListViewModel> GetCuttingPlanDropDownByPOID(int purchaseOrderID)
        {
            List<DropDownListViewModel> cuttingPlan = (from cp in unitOfWork.cuttingPlanRepository.Get()
                                                       where cp.PurchaseOrderID == purchaseOrderID
                                                       select new DropDownListViewModel
                                                       {
                                                           Value = cp.CuttingPlanID,
                                                           Text = cp.CuttingNo.ToString()
                                                       }).ToList();
            return cuttingPlan;
        }
    }
}
