using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using ScopoERP.Production.ViewModel;
using ScopoERP.ProductionStatus.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Production.BLL
{
    public class ProductionStatusLogic
    {
        private UnitOfWork unitOfWork;
        private productiondailyreport productionDailyReport;

        public ProductionStatusLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void CreateProductionStatus(ProductionStatusViewModel productionStatusVM)
        {
            productionDailyReport = new productiondailyreport
            {
                Date = productionStatusVM.Date,
                Hour = productionStatusVM.Hour,
                PoStyleId = productionStatusVM.PurchaseOrderID,
                Floor = productionStatusVM.Floor,
                Line = productionStatusVM.Line,
                Color = productionStatusVM.Color,
                Cutting = productionStatusVM.Cutting,
                SentPrintEmb = productionStatusVM.SentPrintEmb,
                ReceivedPrintEmb = productionStatusVM.ReceivedPrintEmb,
                SewingInput = productionStatusVM.SewingInput,
                TodaySewing = productionStatusVM.TodaySewing,
                SentWash = productionStatusVM.SentWash,
                ReceivedWash = productionStatusVM.ReceivedWash,
                TodayFinish = productionStatusVM.TodayFinish
            };
            unitOfWork.ProductionStatusRepository.Insert(productionDailyReport);

            unitOfWork.Save();
        }

        public void UpdateProductionStatus(ProductionStatusViewModel productionStatusVM)
        {
            productionDailyReport = new productiondailyreport
            {
                ProductionDailyReportId = productionStatusVM.ProductionDailyReportID,
                Date = productionStatusVM.Date,
                Hour = productionStatusVM.Hour,
                PoStyleId = productionStatusVM.PurchaseOrderID,
                Floor = productionStatusVM.Floor,
                Line = productionStatusVM.Line,
                Color = productionStatusVM.Color,
                Cutting = productionStatusVM.Cutting,
                SentPrintEmb = productionStatusVM.SentPrintEmb,
                ReceivedPrintEmb = productionStatusVM.ReceivedPrintEmb,
                SewingInput = productionStatusVM.SewingInput,
                TodaySewing = productionStatusVM.TodaySewing,
                SentWash = productionStatusVM.SentWash,
                ReceivedWash = productionStatusVM.ReceivedWash,
                TodayFinish = productionStatusVM.TodayFinish
            };
            unitOfWork.ProductionStatusRepository.Update(productionDailyReport);

            unitOfWork.Save();
        }

        public void DeleteProductionStatus(int productionDailyReportID)
        {
            unitOfWork.ProductionStatusRepository.Delete(new productiondailyreport { ProductionDailyReportId = productionDailyReportID });

            unitOfWork.Save();
        }

        public IQueryable<ProductionStatusViewModel> GetAllProductionStatus()
        {
            var result = (from dpr in unitOfWork.ProductionStatusRepository.Get()
                          join p in unitOfWork.PurchaseOrderRepository.Get() on dpr.PoStyleId equals p.PoStyleId
                          join s in unitOfWork.StyleRepository.Get() on p.StyleId equals s.StyleId
                          join b in unitOfWork.BuyerRepository.Get() on s.BuyerId equals b.BuyerId
                          select new ProductionStatusViewModel
                          {
                              ProductionDailyReportID = dpr.ProductionDailyReportId,
                              Date = dpr.Date,
                              Hour = dpr.Hour,

                              BuyerName = b.BuyerName,
                              StyleNo = s.StyleNo,
                              
                              PurchaseOrderID = dpr.PoStyleId,
                              PONo = p.PoNo,

                              Floor = dpr.Floor,
                              Line = dpr.Line,
                              Color = dpr.Color,

                              Cutting = dpr.Cutting,
                              SentPrintEmb = dpr.SentPrintEmb,
                              ReceivedPrintEmb = dpr.ReceivedPrintEmb,
                              SewingInput = dpr.SewingInput,
                              TodaySewing = dpr.TodaySewing,
                              SentWash = dpr.SentWash,
                              ReceivedWash = dpr.ReceivedWash,
                              TodayFinish = dpr.TodayFinish
                          }).AsQueryable<ProductionStatusViewModel>();

            return result;
        }

        public ProductionStatusViewModel GetProductionStatusByID(int id)
        {
            var result = (from dpr in unitOfWork.ProductionStatusRepository.Get()
                          join p in unitOfWork.PurchaseOrderRepository.Get() on dpr.PoStyleId equals p.PoStyleId
                          join s in unitOfWork.StyleRepository.Get() on p.StyleId equals s.StyleId
                          where dpr.ProductionDailyReportId == id
                          select new ProductionStatusViewModel
                          {
                              ProductionDailyReportID = dpr.ProductionDailyReportId,
                              
                              Date = dpr.Date,
                              Hour = dpr.Hour,
                              
                              PurchaseOrderID = dpr.PoStyleId,
                              StyleID = s.StyleId,

                              Floor = dpr.Floor,
                              Line = dpr.Line,
                              Color = dpr.Color,
                              
                              Cutting = dpr.Cutting,
                              SentPrintEmb = dpr.SentPrintEmb,
                              ReceivedPrintEmb = dpr.ReceivedPrintEmb,
                              SewingInput = dpr.SewingInput,
                              TodaySewing = dpr.TodaySewing,
                              SentWash = dpr.SentWash,
                              ReceivedWash = dpr.ReceivedWash,
                              TodayFinish = dpr.TodayFinish
                          }).SingleOrDefault();

            return result;
        }

        public void RemoveProductionStatus(int ProductionDailyReportId)
        {
            unitOfWork.ProductionStatusRepository.RawQuery("DELETE FROM productiondailyreport WHERE ProductionDailyReportId = '" + ProductionDailyReportId + "'");
            unitOfWork.Save();

        }

        public ProductionStatusViewModel GetTotalProductionStatusByPurchaseOrder(int purchaseOrderID)
        {
            var dpr = (from d in unitOfWork.ProductionStatusRepository.Get()
                       where d.PoStyleId == purchaseOrderID
                       group d by d.PoStyleId into s
                       select new ProductionStatusViewModel
                       {
                           Cutting = s.Sum(x => x.Cutting),
                           TodaySewing = s.Sum(x => x.TodaySewing),
                           SewingInput = s.Sum(x => x.SewingInput),
                           SentPrintEmb = s.Sum(x => x.SentPrintEmb),
                           ReceivedPrintEmb = s.Sum(x => x.ReceivedPrintEmb),
                           SentWash = s.Sum(x => x.SentWash),
                           ReceivedWash = s.Sum(x => x.ReceivedWash),
                           TodayFinish = s.Sum(x => x.TodayFinish)
                       }).SingleOrDefault();

            return dpr;
        }

        public List<ProductionStatusViewModel> GetFilteredProductionStatus(ProductionStatusFilterViewModel filterVM)
        {
            var data = (from p in unitOfWork.PurchaseOrderRepository.Get()
                        join s in unitOfWork.StyleRepository.Get() on p.StyleId equals s.StyleId
                        join dpr in unitOfWork.ProductionStatusRepository.Get()
                                   .Where(x => x.Date == filterVM.ProductionDate && x.Floor == filterVM.ProductionFloor && x.Line == filterVM.ProductionLine)
                           on p.PoStyleId equals dpr.PoStyleId into dg
                        from d in dg.DefaultIfEmpty()
                        where s.BuyerId == filterVM.BuyerID && s.StyleId == filterVM.StyleID
                        select new ProductionStatusViewModel
                        {
                            ProductionDailyReportID = d != null ? d.ProductionDailyReportId : 0,
                            Date = d != null ? d.Date : filterVM.ProductionDate,
                            //Hour = d != null ? d.Hour : 0,

                            PurchaseOrderID = p.PoStyleId,
                            PONo = p.PoNo,

                            Color = d != null ? d.Color : "N/A",

                            Cutting = d != null ? d.Cutting : 0,
                            SentPrintEmb = d != null ? d.SentPrintEmb : 0,
                            ReceivedPrintEmb = d != null ? d.ReceivedPrintEmb :0,
                            SewingInput = d != null ? d.SewingInput : 0,
                            TodaySewing = d != null ? d.TodaySewing : 0,
                            SentWash = d != null ? d.SentWash : 0,
                            ReceivedWash = d != null ? d.ReceivedWash : 0,
                            TodayFinish = d != null ? d.TodayFinish : 0
                        }).ToList();

            return data;
        }

        public void SaveProductionStatus(List<ProductionStatusViewModel> statusList)
        {   
            if(statusList[0].ProductionDailyReportID != 0)
            {
                foreach (var item in statusList)
                {
                    if (item.ProductionDailyReportID != 0)
                    {
                        productionDailyReport = new productiondailyreport
                        {
                            ProductionDailyReportId = item.ProductionDailyReportID,
                            Date = item.Date,
                            Hour = item.Hour,
                            PoStyleId = item.PurchaseOrderID,
                            Floor = item.Floor,
                            Line = item.Line,
                            Color = item.Color,
                            Cutting = item.Cutting,
                            SentPrintEmb = item.SentPrintEmb,
                            ReceivedPrintEmb = item.ReceivedPrintEmb,
                            SewingInput = item.SewingInput,
                            TodaySewing = item.TodaySewing,
                            SentWash = item.SentWash,
                            ReceivedWash = item.ReceivedWash,
                            TodayFinish = item.TodayFinish
                        };
                        unitOfWork.ProductionStatusRepository.Update(productionDailyReport);
                    }
                    else
                    {
                        productionDailyReport = new productiondailyreport
                        {
                            Date = item.Date,
                            Hour = item.Hour,
                            PoStyleId = item.PurchaseOrderID,
                            Floor = item.Floor,
                            Line = item.Line,
                            Color = item.Color,
                            Cutting = item.Cutting,
                            SentPrintEmb = item.SentPrintEmb,
                            ReceivedPrintEmb = item.ReceivedPrintEmb,
                            SewingInput = item.SewingInput,
                            TodaySewing = item.TodaySewing,
                            SentWash = item.SentWash,
                            ReceivedWash = item.ReceivedWash,
                            TodayFinish = item.TodayFinish
                        };
                        unitOfWork.ProductionStatusRepository.Insert(productionDailyReport);
                    }
                   
                }
                unitOfWork.Save();
            }
            else
            {

                foreach (var item in statusList)
                {
                    productionDailyReport = new productiondailyreport
                    {
                        Date = item.Date,
                        Hour = item.Hour,
                        PoStyleId = item.PurchaseOrderID,
                        Floor = item.Floor,
                        Line = item.Line,
                        Color = item.Color,
                        Cutting = item.Cutting,
                        SentPrintEmb = item.SentPrintEmb,
                        ReceivedPrintEmb = item.ReceivedPrintEmb,
                        SewingInput = item.SewingInput,
                        TodaySewing = item.TodaySewing,
                        SentWash = item.SentWash,
                        ReceivedWash = item.ReceivedWash,
                        TodayFinish = item.TodayFinish
                    };
                    unitOfWork.ProductionStatusRepository.Insert(productionDailyReport);
                }
                unitOfWork.Save();
            }

        }
    }
}
