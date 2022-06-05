using ScopoERP.Domain.Repositories;
using ScopoERP.ProductionStatus.ViewModel;
using ScopoERP.Reports.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.BLL
{
    public class ReportLogic
    {
        private UnitOfWork unitOfWork;

        public ReportLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #region WIP

        public IQueryable<WIPViewModel> GetWIP()
        {
            var orderData = (from p in unitOfWork.PurchaseOrderRepository.Get()
                             join s in unitOfWork.StyleRepository.Get() on p.StyleId equals s.StyleId
                             join a in unitOfWork.AccountRepository.Get() on s.AccountId equals a.AccountId
                             join b in unitOfWork.BuyerRepository.Get() on s.BuyerId equals b.BuyerId
                             join c in unitOfWork.CustomerRepository.Get() on s.CustomerId equals c.CustomerId
                             join dv in unitOfWork.DivisionRepository.Get() on s.DevisionId equals dv.DevisionId
                             join f in unitOfWork.FactoryRepository.Get() on p.FactoryId equals f.FactoryId
                             join sn in unitOfWork.SeasonRepository.Get() on p.SeasonId equals sn.SeasonId
                             join jb in unitOfWork.JobRepository.Get() on p.JobId equals jb.JobInfoId into jj
                             from j in jj.DefaultIfEmpty()
                             orderby p.PoStyleId descending
                             select new { b, c, dv, f, sn, s, a, p, j }).AsQueryable();

            var productionData = (from s in unitOfWork.ProductionStatusRepository.Get()
                                  group s by s.PoStyleId into d
                                  select new
                                  {
                                      PurchaseOrderID = d.Key,
                                      TotalCutting = d.Sum(x => x.Cutting),
                                      TotalSewing = d.Sum(x => x.TodaySewing),
                                      TotalFinishing = d.Sum(x => x.TodayFinish)
                                  }).AsQueryable();

            var shipmentData = (from s in unitOfWork.ShipmentRepository.Get()
                                group s by s.PurchaseOrderID into c
                                select new
                                {
                                    PurchaseOrderID = c.Key,
                                    TotalShipment = c.Sum(x => x.ChalanQuantity)
                                }).AsQueryable();

            var results = (from x in orderData
                           join dpr in productionData on x.p.PoStyleId equals dpr.PurchaseOrderID into dd
                           from p in dd.DefaultIfEmpty()
                           join shp in shipmentData on x.p.PoStyleId equals shp.PurchaseOrderID into ss
                           from s in ss.DefaultIfEmpty()
                           select new WIPViewModel
                           {
                               BuyerName = x.b.BuyerName,
                               CustomerName = x.c.CustomerName,
                               DevisionName = x.dv.DevisionName,
                               FactoryName = x.f.FactoryName,
                               SeasonName = x.sn.SeasonName,

                               AccountName = x.a.AccountName,

                               StyleNo = x.s.StyleNo,
                               StyleDescription = x.s.StyleDescription,
                               Capacity = x.s.Capacity,
                               BodyStyle = x.s.BodyStyle,
                               Item = x.s.Item,
                               Febrication = x.s.Febrication,

                               PoNo = x.p.PoNo,
                               Remarks = x.p.Remarks,
                               OrderQuantity = x.p.OrderQuantity,
                               AgreedCm = x.p.AgreedCm,
                               Fob = x.p.Fob,
                               SubContractRate = x.p.SubContractRate,
                               FactoryCM = x.p.FactoryCM,
                               TotalFob = x.p.Fob * x.p.OrderQuantity,
                               TotalAgreedCM = x.p.AgreedCm * x.p.OrderQuantity,
                               PoExitMonth = x.p.ExitDate.Month,
                               ExitDate = x.p.ExitDate,
                               CurrentStatus = x.p.CurrentStatus,

                               JobNo = x.j.JobNo,
                               ContractNo = x.j.ContractNo,

                               TotalCutting = p.TotalCutting,
                               TotalSewing = p.TotalSewing,
                               BalanceSewing = x.p.OrderQuantity - p.TotalSewing,

                               TotalShipMent = s.TotalShipment,
                               BalanceShipQty = x.p.OrderQuantity - s.TotalShipment,
                               TotalShippedValue = s.TotalShipment * x.p.Fob
                           }).AsQueryable();

            return results;
        }

        public IQueryable<WIPViewModel> GetWIPByDate(DateTime fromDate, DateTime toDate)
        {
            var orderData = (from p in unitOfWork.PurchaseOrderRepository.Get()
                             join s in unitOfWork.StyleRepository.Get() on p.StyleId equals s.StyleId
                             join a in unitOfWork.AccountRepository.Get() on s.AccountId equals a.AccountId
                             join b in unitOfWork.BuyerRepository.Get() on s.BuyerId equals b.BuyerId
                             join c in unitOfWork.CustomerRepository.Get() on s.CustomerId equals c.CustomerId
                             join dv in unitOfWork.DivisionRepository.Get() on s.DevisionId equals dv.DevisionId
                             join f in unitOfWork.FactoryRepository.Get() on p.FactoryId equals f.FactoryId
                             join sn in unitOfWork.SeasonRepository.Get() on p.SeasonId equals sn.SeasonId
                             join jb in unitOfWork.JobRepository.Get() on p.JobId equals jb.JobInfoId into jj
                             from j in jj.DefaultIfEmpty()
                             where p.ExitDate >= fromDate && p.ExitDate <=p.ExitDate 
                             orderby p.PoStyleId descending
                             select new { b, c, dv, f, sn, s, a, p, j }).AsQueryable();

            var productionData = (from s in unitOfWork.ProductionStatusRepository.Get()
                                  group s by s.PoStyleId into d
                                  select new
                                  {
                                      PurchaseOrderID = d.Key,
                                      TotalCutting = d.Sum(x => x.Cutting),
                                      TotalSewing = d.Sum(x => x.TodaySewing),
                                      TotalFinishing = d.Sum(x => x.TodayFinish)
                                  }).AsQueryable();

            var shipmentData = (from s in unitOfWork.ShipmentRepository.Get()
                                group s by s.PurchaseOrderID into c
                                select new
                                {
                                    PurchaseOrderID = c.Key,
                                    TotalShipment = c.Sum(x => x.ChalanQuantity)
                                }).AsQueryable();

            var results = (from x in orderData
                           join dpr in productionData on x.p.PoStyleId equals dpr.PurchaseOrderID into dd
                           from p in dd.DefaultIfEmpty()
                           join shp in shipmentData on x.p.PoStyleId equals shp.PurchaseOrderID into ss
                           from s in ss.DefaultIfEmpty()
                           select new WIPViewModel
                           {
                               BuyerName = x.b.BuyerName,
                               CustomerName = x.c.CustomerName,
                               DevisionName = x.dv.DevisionName,
                               FactoryName = x.f.FactoryName,
                               SeasonName = x.sn.SeasonName,

                               AccountName = x.a.AccountName,

                               StyleNo = x.s.StyleNo,
                               StyleDescription = x.s.StyleDescription,
                               Capacity = x.s.Capacity,
                               BodyStyle = x.s.BodyStyle,
                               Item = x.s.Item,
                               Febrication = x.s.Febrication,

                               PoNo = x.p.PoNo,
                               Remarks = x.p.Remarks,
                               OrderQuantity = x.p.OrderQuantity,
                               AgreedCm = x.p.AgreedCm,
                               Fob = x.p.Fob,
                               SubContractRate = x.p.SubContractRate,
                               FactoryCM = x.p.FactoryCM,
                               TotalFob = x.p.Fob * x.p.OrderQuantity,
                               TotalAgreedCM = x.p.AgreedCm * x.p.OrderQuantity,
                               PoExitMonth = x.p.ExitDate.Month,
                               ExitDate = x.p.ExitDate,
                               CurrentStatus = x.p.CurrentStatus,

                               JobNo = x.j.JobNo,
                               ContractNo = x.j.ContractNo,

                               TotalCutting = p.TotalCutting,
                               TotalSewing = p.TotalSewing,
                               BalanceSewing = x.p.OrderQuantity - p.TotalSewing,

                               TotalShipMent = s.TotalShipment,
                               BalanceShipQty = x.p.OrderQuantity - s.TotalShipment,
                               TotalShippedValue = s.TotalShipment * x.p.Fob
                           }).AsQueryable();

            return results;
        }

        public IQueryable<ShipmentDetailsReportViewModel> GetShipmentDetailsByJobAndDate(int? jobID, DateTime fromDate, DateTime toDate)
        {
            var shipmentData = (from p in unitOfWork.PurchaseOrderRepository.Get()
                                join s in unitOfWork.StyleRepository.Get() on p.StyleId equals s.StyleId
                                join b in unitOfWork.BuyerRepository.Get() on s.BuyerId equals b.BuyerId
                                join f in unitOfWork.FactoryRepository.Get() on p.FactoryId equals f.FactoryId
                                join jb in unitOfWork.JobRepository.Get() on p.JobId equals jb.JobInfoId into jj
                                from j in jj.DefaultIfEmpty()
                                join ship in unitOfWork.ShipmentRepository.Get() on p.PoStyleId equals ship.PurchaseOrderID into sg
                                from sp in sg.DefaultIfEmpty()
                                join iv in unitOfWork.ExportInvoiceRepository.Get() on sp.InvoiceID equals iv.InvoiceId into ii
                                from i in ii.DefaultIfEmpty()
                                join bfrw in unitOfWork.BankForwardingRepository.Get() on i.BankForwardingID equals bfrw.BankForwardingID into bg
                                from bf in bg.DefaultIfEmpty()
                                orderby p.PoStyleId descending
                                where sp.ChalanDate >= fromDate && sp.ChalanDate <= toDate
                                select new { s, j, p, b, f, sp, i, bf }).AsQueryable();

            var productionData = (from s in unitOfWork.ProductionStatusRepository.Get()
                                  group s by s.PoStyleId into d
                                  select new
                                  {
                                      PurchaseOrderID = d.Key,
                                      TotalSewing = d.Sum(x => x.TodaySewing),
                                      TotalFinishing = d.Sum(x => x.TodayFinish)
                                  }).AsQueryable();

            var realizationDataSummary = (from rz in unitOfWork.RealizationRepository.Get()
                                          group rz by new { rz.BankForwardingID, rz.RealizationDate } into s
                                          select new
                                          {
                                              BankForwardingID = s.Key.BankForwardingID,
                                              RealizationDate = s.Key.RealizationDate,
                                              TotalRealizationValue = s.Sum(x => x.Amount)
                                          }).AsQueryable();

            var invoiceDataSumary = (from s in unitOfWork.ShipmentRepository.Get()
                                     join i in unitOfWork.ExportInvoiceRepository.Get() on s.InvoiceID equals i.InvoiceId
                                     group s by i.BankForwardingID into c
                                     select new
                                     {
                                         BankForwardingID = c.Key.Value,
                                         TotalInvoiceFOB = c.Sum(x => x.InvoiceFOB)
                                     }).AsQueryable();

            var financeData = (from r in realizationDataSummary
                               join i in invoiceDataSumary on r.BankForwardingID equals i.BankForwardingID
                               select new
                               {
                                   BankForwardingID = r.BankForwardingID,
                                   RealizationDate = r.RealizationDate,
                                   Ratio = r.TotalRealizationValue / i.TotalInvoiceFOB
                               }).AsQueryable();

            var results = (from c in shipmentData
                           join dpr in productionData on c.p.PoStyleId equals dpr.PurchaseOrderID into dg
                           from p in dg.DefaultIfEmpty()
                           join rz in financeData on c.bf.BankForwardingID equals rz.BankForwardingID into rg
                           from r in rg.DefaultIfEmpty()
                           select new ShipmentDetailsReportViewModel
                           {
                               BuyerName = c.b.BuyerName,
                               FactoryName = c.f.FactoryName,

                               StyleNo = c.s.StyleNo,
                               StyleDescription = c.s.StyleDescription,
                               PoNo = c.p.PoNo,
                               OrderQuantity = c.p.OrderQuantity,
                               Remarks = c.p.Remarks.Trim(),
                               TotalFob = c.p.Fob * c.p.OrderQuantity,
                               TotalAgreedCM = c.p.AgreedCm * c.p.OrderQuantity,

                               TotalShipMent = c.sp.ChalanQuantity,
                               BalanceShipQty = c.p.OrderQuantity - c.sp.ChalanQuantity,
                               TotalShippedValue = c.sp.ChalanQuantity * c.p.Fob,
                               TotalShippedAgreedCM = c.sp.ChalanQuantity * c.p.AgreedCm,
                               ShippedDate = c.sp.ChalanDate,

                               JobNo = c.j.JobNo,
                               ContractNo = c.j.ContractNo,

                               UDNo = c.j.UDNo,
                               UDDate = c.j.UDDate,

                               InvoiceNo = c.i.InvoiceNo,
                               InvoiceDate = c.i.InvoiceDate,
                               EXP = c.i.EXP,
                               EXPDate = c.i.EXPDate,
                               ICDate = c.i.ICDate,
                               ShippingBill = c.i.ShippingBill,
                               ShippingBillDate = c.i.ShippingBillDate,
                               OnBoardDate = c.i.OnBoardDate,
                               BL = c.i.BL,
                               BLRealeaseDate = c.i.BLRealeaseDate,
                               CODate = c.i.CODate,
                               FCR = c.i.FCR,
                               FCRDate = c.i.FCRDate,
                               FDBP_No = string.IsNullOrEmpty(c.i.FDBP_No) ? c.bf.FDBPNo : c.i.FDBP_No,
                               BankForwardingNo = c.bf.BankForwardingNo,
                               BankForwardingDate = c.bf.BankForwardingDate,

                               RealizationDate = r.RealizationDate,
                               TotalRealizationValue = r.Ratio * c.sp.InvoiceFOB,

                               TradeCardInPutDate = c.i.TradeCardInPutDate,
                               DocDespatchDate = c.i.DocDespatchDate == null ? c.bf.BankForwardingDate : c.i.DocDespatchDate,
                               Courier = c.i.Courier,
                               BankNegoDate = c.i.BankNegoDate,
                               PaymentReceiveDate = c.i.PaymentReceiveDate,
                               InvoiceFOB = c.sp.InvoiceFOB,
                               PortOfLoading = c.i.PortOfLoading,
                               FinalDestination = c.i.FinalDestination,
                               CountryName = c.i.CountryName,

                               TotalSewing = p.TotalSewing,
                               TotalFinishing = p.TotalFinishing
                           }).AsQueryable();

            return results;
        }

        #endregion

        #region Shipment

        public IQueryable<ShipmentDetailsReportViewModel> GetShipmentDetails()
        {
            var shipmentData = (from p in unitOfWork.PurchaseOrderRepository.Get()
                                join s in unitOfWork.StyleRepository.Get() on p.StyleId equals s.StyleId
                                join b in unitOfWork.BuyerRepository.Get() on s.BuyerId equals b.BuyerId
                                join f in unitOfWork.FactoryRepository.Get() on p.FactoryId equals f.FactoryId
                                join jb in unitOfWork.JobRepository.Get() on p.JobId equals jb.JobInfoId into jj
                                from j in jj.DefaultIfEmpty()
                                join ship in unitOfWork.ShipmentRepository.Get() on p.PoStyleId equals ship.PurchaseOrderID into sg
                                from sp in sg.DefaultIfEmpty()
                                join iv in unitOfWork.ExportInvoiceRepository.Get() on sp.InvoiceID equals iv.InvoiceId into ii
                                from i in ii.DefaultIfEmpty()
                                join bfrw in unitOfWork.BankForwardingRepository.Get() on i.BankForwardingID equals bfrw.BankForwardingID into bg
                                from bf in bg.DefaultIfEmpty()
                                orderby p.PoStyleId descending
                                select new { s, j, p, b, f, sp, i, bf }).AsQueryable();

            var productionData = (from s in unitOfWork.ProductionStatusRepository.Get()
                                  group s by s.PoStyleId into d
                                  select new
                                  {
                                      PurchaseOrderID = d.Key,
                                      TotalSewing = d.Sum(x => x.TodaySewing),
                                      TotalFinishing = d.Sum(x => x.TodayFinish)
                                  }).AsQueryable();

            var realizationDataSummary = (from rz in unitOfWork.RealizationRepository.Get()
                                          group rz by new { rz.BankForwardingID, rz.RealizationDate } into s
                                          select new
                                          {
                                              BankForwardingID = s.Key.BankForwardingID,
                                              RealizationDate = s.Key.RealizationDate,
                                              TotalRealizationValue = s.Sum(x => x.Amount)
                                          }).AsQueryable();

            var invoiceDataSumary = (from s in unitOfWork.ShipmentRepository.Get()
                                     join i in unitOfWork.ExportInvoiceRepository.Get() on s.InvoiceID equals i.InvoiceId
                                     group s by i.BankForwardingID into c
                                     select new
                                     {
                                         BankForwardingID = c.Key.Value,
                                         TotalInvoiceFOB = c.Sum(x => x.InvoiceFOB)
                                     }).AsQueryable();

            var financeData = (from r in realizationDataSummary
                               join i in invoiceDataSumary on r.BankForwardingID equals i.BankForwardingID
                               select new
                               {
                                   BankForwardingID = r.BankForwardingID,
                                   RealizationDate = r.RealizationDate,
                                   Ratio = r.TotalRealizationValue / i.TotalInvoiceFOB
                               }).AsQueryable();

            var results = (from c in shipmentData
                           join dpr in productionData on c.p.PoStyleId equals dpr.PurchaseOrderID into dg
                           from p in dg.DefaultIfEmpty()
                           join rz in financeData on c.bf.BankForwardingID equals rz.BankForwardingID into rg
                           from r in rg.DefaultIfEmpty()
                           select new ShipmentDetailsReportViewModel
                           {
                               BuyerName = c.b.BuyerName,
                               FactoryName = c.f.FactoryName,

                               StyleNo = c.s.StyleNo,
                               StyleDescription = c.s.StyleDescription,
                               PoNo = c.p.PoNo,
                               OrderQuantity = c.p.OrderQuantity,
                               Remarks = c.p.Remarks.Trim(),
                               TotalFob = c.p.Fob * c.p.OrderQuantity,
                               TotalAgreedCM = c.p.AgreedCm * c.p.OrderQuantity,

                               TotalShipMent = c.sp.ChalanQuantity,
                               BalanceShipQty = c.p.OrderQuantity - c.sp.ChalanQuantity,
                               TotalShippedValue = c.sp.ChalanQuantity * c.p.Fob,
                               TotalShippedAgreedCM = c.sp.ChalanQuantity * c.p.AgreedCm,
                               ShippedDate = c.sp.ChalanDate,

                               JobNo = c.j.JobNo,
                               ContractNo = c.j.ContractNo,

                               UDNo = c.j.UDNo,
                               UDDate = c.j.UDDate,

                               InvoiceNo = c.i.InvoiceNo,
                               InvoiceDate = c.i.InvoiceDate,
                               EXP = c.i.EXP,
                               EXPDate = c.i.EXPDate,
                               ICDate = c.i.ICDate,
                               ShippingBill = c.i.ShippingBill,
                               ShippingBillDate = c.i.ShippingBillDate,
                               OnBoardDate = c.i.OnBoardDate,
                               BL = c.i.BL,
                               BLRealeaseDate = c.i.BLRealeaseDate,
                               CODate = c.i.CODate,
                               FCR = c.i.FCR,
                               FCRDate = c.i.FCRDate,
                               FDBP_No = string.IsNullOrEmpty(c.i.FDBP_No) ? c.bf.FDBPNo : c.i.FDBP_No,
                               BankForwardingNo = c.bf.BankForwardingNo,
                               BankForwardingDate = c.bf.BankForwardingDate,

                               RealizationDate = r.RealizationDate,
                               TotalRealizationValue = r.Ratio * c.sp.InvoiceFOB,

                               TradeCardInPutDate = c.i.TradeCardInPutDate,
                               DocDespatchDate = c.i.DocDespatchDate == null ? c.bf.BankForwardingDate : c.i.DocDespatchDate,
                               Courier = c.i.Courier,
                               BankNegoDate = c.i.BankNegoDate,
                               PaymentReceiveDate = c.i.PaymentReceiveDate,
                               InvoiceFOB = c.sp.InvoiceFOB,
                               PortOfLoading = c.i.PortOfLoading,
                               FinalDestination = c.i.FinalDestination,
                               CountryName = c.i.CountryName,

                               TotalSewing = p.TotalSewing,
                               TotalFinishing = p.TotalFinishing
                           }).AsQueryable();

            return results;
        }

        public List<ShipmentCrisisViewModel> GetShipmentCrisis(int accountID)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT b.BuyerName, s.StyleNO, P.PONo, P.OrderQuantity, P.ExitDate ");
            query.Append("FROM PoStyle p ");
            query.Append("INNER JOIN StyleInfo s ON p.StyleID = s.StyleID ");
            query.Append("INNER JOIN BuyerInfo b ON S.BuyerID = b.BuyerID ");
            query.Append("WHERE s.AccountID = " + accountID + " AND P.ExitDate < GETDATE() AND p.PoStyleID NOT IN ");
            query.Append("(SELECT PurchaseOrderID ");
            query.Append("FROM Shipment sp ");
            query.Append("INNER JOIN PoStyle p ON sp.PurchaseOrderID = p.PoStyleID ");
            query.Append("INNER JOIN StyleInfo s ON s.StyleID = p.StyleID ");
            query.Append("WHERE s.AccountID = " + accountID + " ) ");

            List<ShipmentCrisisViewModel> results = unitOfWork.PurchaseOrderRepository
                                                        .SelectQuery<ShipmentCrisisViewModel>(query.ToString());

            return results;
        }

        public List<ShipmentSummaryViewModel> GetShipmentSummary(DateTime fromDate, DateTime toDate)
        {
            StringBuilder query = new StringBuilder();

            query.Append("SELECT f.FactoryName, ");
            query.Append("SUM(p.OrderQuantity) AS TotalOrderQuantity, ");
            query.Append("SUM(s.Chalanquantity) AS TotalShipmentQuantity, ");
            query.Append("SUM(p.AgreedCM * p.OrderQuantity) AS TotalAgreedCM, ");
            query.Append("SUM(p.AgreedCM * s.Chalanquantity) AS TotalShippedAgreedCM, ");
            query.Append("SUM(p.FOB * p.OrderQuantity) AS TotalFOB, ");
            query.Append("SUM(s.InvoiceFOB) TotalInvoiceFOB ");
            query.Append("FROM PoStyle p ");
            query.Append("INNER JOIN Shipment s ON P.PoStyleID = s.PurchaseOrderID ");
            query.Append("INNER JOIN Factory f ON p.FactoryID = f.FactoryID ");
            query.Append("WHERE s.ChalanDate BETWEEN '" + Convert.ToDateTime(fromDate).ToString("yyy-MM-dd") + "' AND '" + Convert.ToDateTime(toDate).ToString("yyy-MM-dd") + "' ");
            query.Append("GROUP BY f.FactoryName");

            List<ShipmentSummaryViewModel> results = unitOfWork.ShipmentRepository.SelectQuery<ShipmentSummaryViewModel>(query.ToString());

            return results;
        }

        public List<ShipmentDetailsViewModel> GetShipmentDetails(DateTime fromDate, DateTime toDate)
        {
            var realizationDataSummary = (from rz in unitOfWork.RealizationRepository.Get()
                                          group rz by new { rz.BankForwardingID, rz.RealizationDate } into s
                                          select new
                                          {
                                              BankForwardingID = s.Key.BankForwardingID,
                                              RealizationDate = s.Key.RealizationDate,
                                              TotalRealizationValue = s.Sum(x => x.Amount)
                                          }).AsQueryable();

            var invoiceDataSumary = (from s in unitOfWork.ShipmentRepository.Get()
                                     join i in unitOfWork.ExportInvoiceRepository.Get() on s.InvoiceID equals i.InvoiceId
                                     group s by i.BankForwardingID into c
                                     select new
                                     {
                                         BankForwardingID = c.Key.Value,
                                         TotalInvoiceFOB = c.Sum(x => x.InvoiceFOB)
                                     }).AsQueryable();

            var financeData = (from r in realizationDataSummary
                               join i in invoiceDataSumary on r.BankForwardingID equals i.BankForwardingID
                               select new
                               {
                                   BankForwardingID = r.BankForwardingID,
                                   RealizationDate = r.RealizationDate,
                                   Ratio = r.TotalRealizationValue / i.TotalInvoiceFOB
                               }).AsQueryable();

            var results = (from sp in unitOfWork.ShipmentRepository.Get()
                           join p in unitOfWork.PurchaseOrderRepository.Get() on sp.PurchaseOrderID equals p.PoStyleId
                           join s in unitOfWork.StyleRepository.Get() on p.StyleId equals s.StyleId
                           join b in unitOfWork.BuyerRepository.Get() on s.BuyerId equals b.BuyerId
                           join f in unitOfWork.FactoryRepository.Get() on p.FactoryId equals f.FactoryId
                           join cl in unitOfWork.ChalanExportRepository.Get() on sp.ChalanID equals cl.ChalanID into cg
                           from c in cg.DefaultIfEmpty()
                           join iv in unitOfWork.ExportInvoiceRepository.Get() on sp.InvoiceID equals iv.InvoiceId into ig
                           from i in ig.DefaultIfEmpty()
                           join bfrw in unitOfWork.BankForwardingRepository.Get() on i.BankForwardingID equals bfrw.BankForwardingID into bg
                           from bf in bg.DefaultIfEmpty()
                           join rz in financeData on bf.BankForwardingID equals rz.BankForwardingID into rg
                           from r in rg.DefaultIfEmpty()
                           where sp.ChalanDate >= fromDate && sp.ChalanDate <= toDate
                           select new ShipmentDetailsViewModel
                           {
                               FactoryName = f.FactoryName,
                               BuyerName = b.BuyerName,
                               PONo = p.PoNo,
                               OrderQuantity = p.OrderQuantity,
                               ExitDate = p.ExitDate,
                               ChalanNo = c.ChalanNo,
                               ChalanDate = sp.ChalanDate,
                               ChalanQuantity = sp.ChalanQuantity,

                               TotalAgreedCM = p.AgreedCm * p.OrderQuantity,
                               ShippedAgreedCM = p.AgreedCm * sp.ChalanQuantity,
                               TotalFOB = p.Fob * p.OrderQuantity,
                               ShippedFOB = sp.ChalanQuantity * p.Fob,
                               InvoiceFOB = sp.InvoiceFOB,

                               InvoiceNo = i.InvoiceNo,
                               InvoiceDate = i.InvoiceDate,
                               OnBoardDate = i.OnBoardDate,
                               BL = i.BL,
                               BLRealeaseDate = i.BLRealeaseDate,
                               EXP = i.EXP,
                               EXPDate = i.EXPDate,
                               FCR = i.FCR,
                               FCRDate = i.FCRDate,
                               ShippingBill = i.ShippingBill,
                               ShippingBillDate = i.ShippingBillDate,
                               DocDespatchDate = i.DocDespatchDate == null ? bf.BankForwardingDate : i.DocDespatchDate,
                               FDBPNo = string.IsNullOrEmpty(i.FDBP_No) ? bf.FDBPNo : i.FDBP_No,

                               BankForwardingNo = bf.BankForwardingNo,
                               BankForwardingDate = bf.BankForwardingDate,

                               RealizationDate = r.RealizationDate,
                               RealizationValue = r.Ratio * sp.InvoiceFOB
                           }).ToList();

            return results;
        }

        #endregion

        #region Job Report

        public List<JobStatusReportViewModel> GetJobStatus(int jobID)
        {
            string query = "SELECT S.StyleNo, SUM(((C.Consumption / C.ConversionQuantity) + ((C.Consumption / C.ConversionQuantity) * (C.Wastage / 100))) * C.ActualPrice) ActualRMCost "
                        + " FROM initialcostsheet C "
                        + " INNER JOIN (SELECT DISTINCT ST.StyleId, ST.StyleNo FROM styleinfo ST INNER JOIN PoStyle P ON ST.StyleId = P.StyleId WHERE P.JobId= " + jobID + ") S ON C.StyleId = S.StyleId "
                        + " GROUP BY S.StyleNo";

            var rmvalue = unitOfWork.PurchaseOrderRepository.SelectQuery<JobStatusReportViewModel>(query);

            query = "SELECT j.JobNo, j.ContractNo, s.StyleNo StyleNo, p.PoNo, P.OrderQuantity, p.ExitDate, p.FOB, p.AgreedCM, "
                    + " p.FOB *p.OrderQuantity TotalFOB, p.AgreedCM *p.OrderQuantity TotalAgreedCM, "
                    + " p.FOB - p.AgreedCM RMCost, "
                    + " SUM(CASE WHEN i.ItemCategoryId = 9 THEN TotalQuantity * UnitPrice ELSE 0 END) FabricsValue, "
                    + " SUM(CASE WHEN i.ItemCategoryId = 33 THEN TotalQuantity * UnitPrice ELSE 0 END) WashValue, "
                    + " SUM(CASE WHEN i.ItemCategoryId != 33 AND i.ItemCategoryId != 9 THEN TotalQuantity * UnitPrice ELSE 0 END) TrimsValue "
                    + " FROM postyle p "
                    + " INNER JOIN styleinfo s ON s.StyleId = p.StyleId "
                    + " INNER JOIN jobinfo j ON j.JobInfoId = p.JobId "
                    + " LEFT JOIN Booking w on p.PoStyleId = w.PurchaseOrderID AND w.PIID IS NOT NULL"
                    + " LEFT JOIN Item i ON w.ItemID = i.ItemID "
                    + " WHERE j.JobInfoID = " + jobID + " "
                    + " GROUP BY j.JobNo, j.ContractNo, s.StyleNo, p.PoStyleId, p.PoNo, P.OrderQuantity, p.ExitDate, p.FOB, p.AgreedCM ";

            var poValue = unitOfWork.PurchaseOrderRepository.SelectQuery<JobStatusReportViewModel>(query);

            var result = (from c in poValue.AsEnumerable()
                          join rm in rmvalue.AsEnumerable() on c.StyleNo equals rm.StyleNo into rg
                          from s in rg.DefaultIfEmpty()
                          select new JobStatusReportViewModel
                          {
                              JobNo = c.JobNo,
                              StyleNo = c.StyleNo,
                              PONo = c.PONo,
                              ContractNo = c.ContractNo,
                              OrderQuantity = c.OrderQuantity,
                              ExitDate = c.ExitDate,
                              FOB = c.FOB,
                              RMCost = c.RMCost,
                              AgreedCM = c.AgreedCM,
                              TotalFOB = c.TotalFOB,
                              TotalAgreedCM = c.TotalAgreedCM,
                              FabricsValue = c.FabricsValue,
                              WashValue = c.WashValue,
                              TrimsValue = c.TrimsValue,
                              ActualRMCost = s == null ? 0 : s.ActualRMCost
                          }).ToList();

            return result;
        }


        public List<JobStatusReportViewModel> GetJobStatusSummary(int year)
        {
            string query = "SELECT j.JobNo, j.ContractNo, "
                        + " SUM(CASE WHEN i.ItemCategoryId = 9 THEN TotalQuantity * UnitPrice ELSE 0 END) FabricsValue, "
                        + " SUM(CASE WHEN i.ItemCategoryId = 33 THEN TotalQuantity * UnitPrice ELSE 0 END) WashValue, "
                        + " SUM(CASE WHEN i.ItemCategoryId != 33 AND i.ItemCategoryId != 9 THEN TotalQuantity * UnitPrice ELSE 0 END) TrimsValue "
                        + " FROM postyle p "
                        + " INNER JOIN jobinfo j ON j.JobInfoId = p.JobId "
                        + " LEFT JOIN Booking w on p.PoStyleId = w.PurchaseOrderID "
                        + " LEFT JOIN Item i ON w.ItemID = i.ItemID "
                        + " WHERE w.PIID IS NOT NULL AND j.JobNo LIKE '" + year + "%' "
                        + " GROUP BY j.JobNo, j.ContractNo";

            var rmvalue = unitOfWork.PurchaseOrderRepository.SelectQuery<JobStatusReportViewModel>(query);

            query = "SELECT j.JobNo, SUM(P.OrderQuantity) AS OrderQuantity, MAX(p.ExitDate) ExitDate, "
                        + " SUM(p.FOB * p.OrderQuantity) AS TotalFOB, SUM(p.AgreedCM * p.OrderQuantity) AS TotalAgreedCM "
                        + " FROM postyle p "
                        + " INNER JOIN styleinfo s ON s.StyleId = p.StyleId "
                        + " INNER JOIN jobinfo j ON j.JobInfoId = p.JobId "
                        + " WHERE j.JobNo LIKE '" + year + "%' "
                        + " GROUP BY j.JobNo ";

            var poValue = unitOfWork.PurchaseOrderRepository.SelectQuery<JobStatusReportViewModel>(query);

            var result = (from s in rmvalue.AsEnumerable()
                          join c in poValue.AsEnumerable() on s.JobNo equals c.JobNo
                          select new JobStatusReportViewModel
                          {
                              JobNo = s.JobNo,
                              ContractNo = s.ContractNo,
                              OrderQuantity = c.OrderQuantity,
                              ExitDate = c.ExitDate,
                              TotalFOB = c.TotalFOB,
                              TotalAgreedCM = c.TotalAgreedCM,
                              FabricsValue = s.FabricsValue,
                              WashValue = s.WashValue,
                              TrimsValue = s.TrimsValue
                          }).ToList();

            return result;
        }


        public List<JobSummaryReportViewModel> GetJobSummary(int jobID)
        {
            StringBuilder query = new StringBuilder();

            query.Append("SELECT P.CurrentStatus, J.JobNo, S.StyleNo, ");
            query.Append("P.Remarks, P.PONo, P.OrderQuantity, P.ExitDate, P.FOB, P.AgreedCM, ");
            query.Append("SUM(SP.ChalanQuantity) AS ChalanQuantity, ");
            query.Append("P.FOB * P.OrderQuantity AS TotalFOB, ");
            query.Append("P.AgreedCM * P.OrderQuantity AS TotalAgreedCM, ");
            query.Append("SUM(SP.InvoiceFOB) TotalInvoiceFOB, ");
            query.Append("C.ChalanNo AS ChalanNo, ");
            query.Append("C.ChalanDate AS ChalanDate, ");
            query.Append("I.InvoiceNo AS InvoiceNo ");
            query.Append("FROM PoStyle P ");
            query.Append("INNER JOIN StyleInfo S ON P.StyleID = S.StyleID ");
            query.Append("LEFT JOIN JobInfo j ON P.JobID = J.JobInfoID ");
            query.Append("LEFT JOIN Shipment SP ON P.PoStyleID = SP.PurchaseOrderID ");
            query.Append("LEFT JOIN ChalanExport C ON SP.ChalanID = C.ChalanID ");
            query.Append("LEFT JOIN Invoice I ON SP.InvoiceID = I.InvoiceID ");
            query.Append("WHERE P.JobID = " + jobID);
            query.Append(" GROUP BY P.CurrentStatus, J.JobNo, S.StyleNo, P.Remarks, P.PONo, P.OrderQuantity, P.ExitDate, P.FOB, P.AgreedCM, C.ChalanNo, C.ChalanDate, I.InvoiceNo");

            List<JobSummaryReportViewModel> results = unitOfWork.PurchaseOrderRepository.SelectQuery<JobSummaryReportViewModel>(query.ToString());



            return results;
        }


        public List<JobItemReportViewModel> GetJobItem(int jobID)
        {
            List<JobItemReportViewModel> poInfo = (from p in unitOfWork.PurchaseOrderRepository.Get()
                                                   join s in unitOfWork.StyleRepository.Get() on p.StyleId equals s.StyleId
                                                   join pc in unitOfWork.POCostsheetRepository.Get() on p.PoStyleId equals pc.PoStyleId
                                                   join cs in unitOfWork.CostsheetRepository.Get() on s.StyleId equals cs.StyleId into cg
                                                   from c in cg.DefaultIfEmpty()
                                                   join i in unitOfWork.ItemRepository.Get() on c.ItemId equals i.ItemId
                                                   join it in unitOfWork.ItemCategoryRepository.Get() on i.ItemCategoryId equals it.ItemCategoryId
                                                   join u in unitOfWork.ConsumptionUnitRepository.Get() on c.ConversionUnit equals u.ConsumptionUnitId
                                                   where p.JobId == jobID
                                                   select new JobItemReportViewModel
                                                   {
                                                       StyleNo = s.StyleNo,
                                                       PurchaseOrderID = p.PoStyleId,
                                                       PONo = p.PoNo,
                                                       OrderQuantity = p.OrderQuantity,
                                                       TotalFOB = p.OrderQuantity * p.Fob,
                                                       TotalAgreedCM = p.OrderQuantity * p.AgreedCm,
                                                       TotalRM = p.OrderQuantity * p.Fob - p.OrderQuantity * p.AgreedCm,
                                                       ItemID = c.ItemId,
                                                       ItemDescription = i.ItemDescription,
                                                       ItemCategory = it.Name,
                                                       CostsheetConsumption = c.Consumption,
                                                       CostsheetUnitPrice = c.ActualPrice,
                                                       CostsheetUnitName = u.UnitName,
                                                       CostsheetWastage = c.Wastage,
                                                       CostsheetTotalConsumption = ((c.Consumption / c.ConversionQuantity) + (c.Wastage / 100) * (c.Consumption / c.ConversionQuantity)) * p.OrderQuantity,
                                                       CostsheetTotalPrice = ((c.Consumption / c.ConversionQuantity) + (c.Wastage / 100) * (c.Consumption / c.ConversionQuantity)) * p.OrderQuantity * c.ActualPrice
                                                   }).ToList();


            List<JobItemReportViewModel> bookingInfo = (from b in unitOfWork.BookingRepository.Get()
                                                        join p in unitOfWork.PurchaseOrderRepository.Get() on b.PurchaseOrderID equals p.PoStyleId
                                                        join u in unitOfWork.ConsumptionUnitRepository.Get() on b.ConsumptionUnitID equals u.ConsumptionUnitId
                                                        join pi in unitOfWork.PIRepository.Get() on b.PIId equals pi.PIID
                                                        join s in unitOfWork.SupplierRepository.Get() on pi.SupplierID equals s.SupplierId
                                                        where p.JobId == jobID
                                                        group b by new { p.PoStyleId, b.ItemID, b.UnitPrice, u.UnitName, pi.PINo, pi.ReferenceNo, s.SupplierName } into s
                                                        select new JobItemReportViewModel
                                                        {
                                                            PurchaseOrderID = s.Key.PoStyleId,
                                                            ItemID = s.Key.ItemID,
                                                            //BookingConsumption = 
                                                            BookingUnitPrice = s.Key.UnitPrice,
                                                            BookingUnitName = s.Key.UnitName,
                                                            //BookingWastage = c.Wastage,
                                                            BookingTotalConsumption = s.Sum(x => x.TotalQuantity),
                                                            BookingTotalPrice = s.Sum(x => x.TotalQuantity) * s.Key.UnitPrice,
                                                            PINo = s.Key.PINo,
                                                            ReferenceNo = s.Key.ReferenceNo,
                                                            SupplierName = s.Key.SupplierName
                                                        }).ToList();

            var result = (from p in poInfo
                          join bk in bookingInfo on new { p.PurchaseOrderID, p.ItemID } equals new { bk.PurchaseOrderID, bk.ItemID } into bg
                          from b in bg.DefaultIfEmpty()
                          select new JobItemReportViewModel
                          {
                              StyleNo = p.StyleNo,
                              PurchaseOrderID = p.PurchaseOrderID,
                              PONo = p.PONo,
                              OrderQuantity = p.OrderQuantity,
                              TotalFOB = p.TotalFOB,
                              TotalAgreedCM = p.TotalAgreedCM,
                              TotalRM = p.TotalRM,

                              ItemDescription = p.ItemDescription,
                              ItemCategory = p.ItemCategory,
                              CostsheetConsumption = p.CostsheetConsumption,
                              CostsheetUnitPrice = p.CostsheetUnitPrice,
                              CostsheetUnitName = p.CostsheetUnitName,
                              CostsheetWastage = p.CostsheetWastage,
                              CostsheetTotalConsumption = p.CostsheetTotalConsumption,
                              CostsheetTotalPrice = p.CostsheetTotalPrice,

                              BookingConsumption = b == null ? null : b.BookingConsumption,
                              BookingUnitPrice = b == null ? null : b.BookingUnitPrice,
                              BookingUnitName = b == null ? "" : b.BookingUnitName,
                              BookingWastage = b == null ? null : b.BookingWastage,
                              BookingTotalConsumption = b == null ? null : b.BookingTotalConsumption,
                              BookingTotalPrice = b == null ? null : b.BookingTotalPrice,

                              PINo = b == null ? null : b.PINo,
                              ReferenceNo = b == null ? null : b.ReferenceNo,
                              SupplierName = b == null ? null : b.SupplierName
                          }).ToList();

            return result;
        }

        #endregion

        #region Proforma Invoice

        public List<ProformaInvoiceViewModel> GetProformaInvoiceSummary(int piID)
        {
            var result = (from w in unitOfWork.BookingRepository.Get()
                          join p in unitOfWork.PIRepository.Get() on w.PIId equals p.PIID
                          join i in unitOfWork.ItemRepository.Get() on w.ItemID equals i.ItemId
                          join it in unitOfWork.ItemCategoryRepository.Get() on i.ItemCategoryId equals it.ItemCategoryId
                          join c in unitOfWork.ConsumptionUnitRepository.Get() on w.ConsumptionUnitID equals c.ConsumptionUnitId
                          where w.PIId == piID
                          group w by new { w.PIId, it.Name, i.ItemDescription, w.UnitPrice, c.UnitName } into s
                          select new ProformaInvoiceViewModel
                          {
                              ItemCategory = s.Key.Name,
                              ItemDescription = s.Key.ItemDescription,
                              ToTalQuantity = s.Sum(x => x.TotalQuantity),
                              ConsumptionUnit = s.Key.UnitName,
                              UnitPrice = s.Key.UnitPrice,
                              TotalPrice = s.Sum(x => x.TotalQuantity) * s.Key.UnitPrice
                          }).ToList();

            return result;
        }

        public List<ProformaInvoiceViewModel> GetProformaInvoiceDetails(int piID)
        {
            var result = (from w in unitOfWork.BookingRepository.Get()
                          join ps in unitOfWork.PurchaseOrderRepository.Get() on w.PurchaseOrderID equals ps.PoStyleId
                          join p in unitOfWork.PIRepository.Get() on w.PIId equals p.PIID
                          join i in unitOfWork.ItemRepository.Get() on w.ItemID equals i.ItemId
                          join it in unitOfWork.ItemCategoryRepository.Get() on i.ItemCategoryId equals it.ItemCategoryId
                          join c in unitOfWork.ConsumptionUnitRepository.Get() on w.ConsumptionUnitID equals c.ConsumptionUnitId
                          where w.PIId == piID
                          group w by new { w.PIId, ps.PoNo, it.Name, i.ItemDescription, w.ItemColor, w.ItemSize, w.UnitPrice, c.UnitName } into s
                          select new ProformaInvoiceViewModel
                          {
                              PONo = s.Key.PoNo,
                              ItemCategory = s.Key.Name,
                              ItemDescription = s.Key.ItemDescription,
                              ItemColor = s.Key.ItemColor,
                              ItemSize = s.Key.ItemSize,
                              ToTalQuantity = s.Sum(x => x.TotalQuantity),
                              ConsumptionUnit = s.Key.UnitName,
                              UnitPrice = s.Key.UnitPrice,
                              TotalPrice = s.Sum(x => x.TotalQuantity) * s.Key.UnitPrice
                          }).ToList();

            return result;
        }

        #endregion

        #region Requisition Report

        public RequisitionReportViewModel GetRequisitionSummary(int requisitionID)
        {
            RequisitionReportViewModel requisitionVM = new RequisitionReportViewModel();

            var jobInfo = (from r in unitOfWork.RequisitionRepository.Get()
                           join j in unitOfWork.JobRepository.Get() on r.JobID equals j.JobInfoId
                           where r.RequisitionID == requisitionID
                           select new
                           {
                               JobID = j.JobInfoId,
                               JobNo = j.JobNo,
                               ContractNo = j.ContractNo,
                               RequisitionNo = r.RequisitionNo,
                               RequisitionDate = r.RequisitionDate,
                               RequisitionSLNo = r.RequisitionSerial
                           }).SingleOrDefault();

            requisitionVM.JobID = jobInfo.JobID;
            requisitionVM.RequisitionNo = jobInfo.RequisitionNo;
            requisitionVM.RequisitionDate = jobInfo.RequisitionDate;
            requisitionVM.RequisitionSLNo = jobInfo.RequisitionSLNo;

            requisitionVM.JobNo = jobInfo.JobNo;
            requisitionVM.ContractNo = jobInfo.ContractNo;

            var poInfo = (from p in unitOfWork.PurchaseOrderRepository.Get()
                          where p.JobId == requisitionVM.JobID
                          group p by p.JobId into s
                          select new
                          {
                              OrderQuantity = s.Sum(x => x.OrderQuantity),
                              ContractValue = s.Sum(x => x.OrderQuantity * x.Fob),
                              AgreedCM = s.Sum(x => x.OrderQuantity * x.AgreedCm),
                              BudgetValue = s.Sum(x => x.OrderQuantity * x.Fob) - s.Sum(x => x.OrderQuantity * x.AgreedCm),
                              FirstShipmentDate = s.Min(x => x.ExitDate),
                              LastShipmentDate = s.Max(x => x.ExitDate)
                          }).SingleOrDefault();

            requisitionVM.ContractValue = poInfo.ContractValue;
            requisitionVM.BudgetValue = poInfo.BudgetValue;
            requisitionVM.AgreedCM = poInfo.AgreedCM;
            requisitionVM.OrderQuantity = poInfo.OrderQuantity;
            requisitionVM.FirstShipmentDate = poInfo.FirstShipmentDate;
            requisitionVM.LastShipmentDate = poInfo.LastShipmentDate;

            return requisitionVM;
        }

        public List<RequisitionPIViewModel> GetRequisitionAppliedForB2B(int requisitionID)
        {
            var result = (from w in unitOfWork.BookingRepository.Get()
                          join p in unitOfWork.PIRepository.Get() on w.PIId equals p.PIID
                          join sp in unitOfWork.SupplierRepository.Get() on p.SupplierID equals sp.SupplierId
                          join r in unitOfWork.RequisitionRepository.Get() on p.RequisitionID equals r.RequisitionID
                          where r.RequisitionID == requisitionID
                          group w by new { p.PINo, sp.SupplierName } into s
                          select new RequisitionPIViewModel
                          {
                              PINo = s.Key.PINo,
                              PIValue = s.Sum(x => x.TotalQuantity * x.UnitPrice),
                              SupplierName = s.Key.SupplierName,
                              BookingType = "Regular"
                          }).ToList();

            var fromExcessBooking = (from e in unitOfWork.ExcessBookingRepository.Get()
                                     join p in unitOfWork.PIRepository.Get() on e.ProformaInvoiceID equals p.PIID
                                     join s in unitOfWork.SupplierRepository.Get() on p.SupplierID equals s.SupplierId
                                     where p.RequisitionID == requisitionID
                                     select new RequisitionPIViewModel
                                     {
                                         PINo = p.PINo,
                                         PIValue = e.TotalPrice,
                                         SupplierName = s.SupplierName,
                                         BookingType = "Excess"
                                     }).ToList();

            result.AddRange(fromExcessBooking);

            return result;
        }

        public List<CouponViewModel> GetCouponReport(ReportFilteringViewModel reportFilteringVM)
        {
            var purchaseInfo = unitOfWork.PurchaseOrderRepository.Get().Where(x => x.PoStyleId == reportFilteringVM.PurchaseOrderID).SingleOrDefault();
            var cuttingInfo = unitOfWork.cuttingPlanRepository.Get().Where(x => x.CuttingPlanID == reportFilteringVM.CuttingPlanID).SingleOrDefault();

            var res = (from c in unitOfWork.CouponRepository.Get()
                           //join s in unitOfWork.StyleOperationRepository.Get() on c.StyleOperationID equals s.StyleOperationID
                       join st in unitOfWork.StandardOperationRepository.Get() on c.OperationID equals st.OperationID
                       join spec in unitOfWork.SpecRepository.Get() on st.OperationID equals spec.OperationID
                       join j in unitOfWork.JobClassRepository.Get() on st.JobClassID equals j.JobClassID
                       join b in unitOfWork.BundleRepository.Get() on c.BundleID equals b.BundleID
                       where c.CuttingPlanID == reportFilteringVM.CuttingPlanID && st.OperationCategoryID == reportFilteringVM.OperationCategoryID
                       select new CouponViewModel
                       {
                           OperationID = st.OperationID,
                           Value = c.Value,
                           BundleID = c.BundleID,
                           BaseRate = j.BaseRate,
                           SpecNo = spec.SpecNo,
                           SpecName = spec.SpecName,
                           OperationName = st.OperationName,
                           PurchaseOrderNo = purchaseInfo.PoNo,
                           BundleNo = b.BundleNo,
                           Size = b.Size,
                           Quantity = b.Quantity,
                           JobClassName = j.JobClassName,
                           Time = c.Time,
                           CutNo = cuttingInfo.CuttingNo,
                           CuttingPlanID = cuttingInfo.CuttingPlanID,
                           type = "item",
                           SectionNo = c.SectionNo,
                           SupervisorID = c.SupervisorID
                       }
            ).ToList();

            return res;
        }

        public List<ExpenseBudgetReportViewModel> GetExpenseReportByRefNo(string referenceNo)
        {
            var data = (from a in unitOfWork.ChartOfAccountRepository.Get()
                        join e in unitOfWork.ExpenseRepository.Get() on a.ChartOfAccountID equals e.ChartOfAccountID
                        where e.ReferenceNo == referenceNo
                        select new ExpenseBudgetReportViewModel
                        {
                            AccountNo = a.AccountNo,
                            AccountName = a.AccountName,
                            ExpenseAmount = e.ExpenseAmount,
                            ExpenseDate = e.ExpenseDate,
                            ExpenseBy = e.ExpenseBy,
                            ReferenceNo=e.ReferenceNo
                        }).ToList();

            return data;
        }

        public List<RequisitionPIViewModel> GetRequisitionPendingForB2B(int requisitionID, int jobID)
        {
            var result = (from w in unitOfWork.BookingRepository.Get()
                          join po in unitOfWork.PurchaseOrderRepository.Get() on w.PurchaseOrderID equals po.PoStyleId
                          join p in unitOfWork.PIRepository.Get() on w.PIId equals p.PIID
                          join sp in unitOfWork.SupplierRepository.Get() on p.SupplierID equals sp.SupplierId
                          join r in unitOfWork.RequisitionRepository.Get() on p.RequisitionID equals r.RequisitionID
                          where p.BackToBackLCID == null && r.RequisitionID != requisitionID
                          && ((po.JobId == jobID && p.LoanFromJobID == null) || p.LoanFromJobID == jobID)
                          group w by new { p.PINo, sp.SupplierName, r.RequisitionDate } into s
                          select new RequisitionPIViewModel
                          {
                              PINo = s.Key.PINo,
                              PIValue = s.Sum(x => x.TotalQuantity * x.UnitPrice),
                              RequisitionDate = s.Key.RequisitionDate,
                              SupplierName = s.Key.SupplierName,
                              BookingType = "Regular"
                          }).ToList();

            var fromExcessBooking = (from e in unitOfWork.ExcessBookingRepository.Get()
                                     join p in unitOfWork.PIRepository.Get() on e.ProformaInvoiceID equals p.PIID
                                     join s in unitOfWork.SupplierRepository.Get() on p.SupplierID equals s.SupplierId
                                     where p.BackToBackLCID == null && p.RequisitionID != requisitionID
                                     && ((e.JobID == jobID && p.LoanFromJobID == null) || p.LoanFromJobID == jobID)
                                     select new RequisitionPIViewModel
                                     {
                                         PINo = p.PINo,
                                         PIValue = e.TotalPrice,
                                         SupplierName = s.SupplierName,
                                         BookingType = "Excess"
                                     }).ToList();

            result.AddRange(fromExcessBooking);

            return result;
        }

        public List<PostSheetViewModel> GetPostSheetData(ReportFilteringViewModel reportFilteringVM)
        {
            List<PostSheetViewModel> postSheetList = unitOfWork.GumSheetOffStandardRepository.SelectQuery<PostSheetViewModel>("EXEC GetPostSheetData '" + reportFilteringVM.FromDate + "','" + reportFilteringVM.ToDate + "'");
            return postSheetList;
        }

        public List<RequisitionPIViewModel> GetRequisitionAlreadyOpenedB2B(int jobID)
        {
            var result = (from w in unitOfWork.BookingRepository.Get()
                          join po in unitOfWork.PurchaseOrderRepository.Get() on w.PurchaseOrderID equals po.PoStyleId
                          join p in unitOfWork.PIRepository.Get() on w.PIId equals p.PIID
                          join sp in unitOfWork.SupplierRepository.Get() on p.SupplierID equals sp.SupplierId
                          join b in unitOfWork.BackToBackLCRepository.Get() on p.BackToBackLCID equals b.BackToBackLCID
                          join l in unitOfWork.LCTypeRepository.Get() on b.LCTypeID equals l.PaymentTypeID
                          where p.BackToBackLCID != null
                          && ((b.JobID == jobID && p.LoanFromJobID == null) || p.LoanFromJobID == jobID)
                          group w by new
                          {
                              p.PINo,
                              sp.SupplierName,
                              b.BackToBackLC1,
                              b.BackToBackLCValue,
                              b.BackToBackLCDate,
                              l.PaymentTitle,
                              p.LoanFromJobID
                          } into s
                          select new RequisitionPIViewModel
                          {
                              PINo = s.Key.PINo,
                              PIValue = s.Sum(x => x.TotalQuantity * x.UnitPrice),
                              SupplierName = s.Key.SupplierName,
                              LCType = s.Key.PaymentTitle,
                              BackToBackLC = s.Key.BackToBackLC1,
                              BackToBackLCDate = s.Key.BackToBackLCDate,
                              BackToBackLCValue = s.Key.BackToBackLCValue,
                              BookingType = s.Key.LoanFromJobID == null ? "Regular" : "Loan"
                          }).ToList();

            var fromExcessBooking = (from e in unitOfWork.ExcessBookingRepository.Get()
                                     join p in unitOfWork.PIRepository.Get() on e.ProformaInvoiceID equals p.PIID
                                     join s in unitOfWork.SupplierRepository.Get() on p.SupplierID equals s.SupplierId
                                     join b in unitOfWork.BackToBackLCRepository.Get() on p.BackToBackLCID equals b.BackToBackLCID
                                     join l in unitOfWork.LCTypeRepository.Get() on b.LCTypeID equals l.PaymentTypeID
                                     where p.BackToBackLCID != null && ((e.JobID == jobID && p.LoanFromJobID == null) || p.LoanFromJobID == jobID)
                                     select new RequisitionPIViewModel
                                     {
                                         PINo = p.PINo,
                                         PIValue = e.TotalPrice,
                                         SupplierName = s.SupplierName,
                                         LCType = l.PaymentTitle,
                                         BackToBackLC = b.BackToBackLC1,
                                         BackToBackLCDate = b.BackToBackLCDate,
                                         BackToBackLCValue = b.BackToBackLCValue,
                                         BookingType = "Excess"
                                     }).ToList();

            result.AddRange(fromExcessBooking);

            return result;
        }

        public List<InventoryViewModel> GetInventoryData()
        {


            var res = (from a in unitOfWork.BLRepository.Get()
                       join b in unitOfWork.BLDetailsRepository.Get() on a.BLID equals b.BLID
                       join c in unitOfWork.BookingRepository.Get() on b.BookingID equals c.BookingID
                       join p in unitOfWork.PurchaseOrderRepository.Get() on c.PurchaseOrderID equals p.PoStyleId
                       join st in unitOfWork.StyleRepository.Get() on p.StyleId equals st.StyleId
                       join buyer in unitOfWork.BuyerRepository.Get() on st.BuyerId equals buyer.BuyerId
                       join job in unitOfWork.JobRepository.Get() on p.JobId equals job.JobInfoId
                       join pir in unitOfWork.PIRepository.Get() on c.PIId equals pir.PIID

                       select new InventoryViewModel
                       {
                           BLNo = a.BLNo,
                           BLDate = a.BLDate,
                           TotalBookingQty = c.TotalQuantity,
                           TotalIssueQty = b.InvoiceQuantity,
                           TotalReceivedQty = b.ReceivedQuantity
                       }

                       ).ToList();

            return res;
        }

        public List<RequisitionPO> GetRequisitionPO(int jobID)
        {
            var poInfo = (from ps in unitOfWork.PurchaseOrderRepository.Get()
                          where ps.JobId == jobID
                          group ps by ps.ExitDate into p
                          select new RequisitionPO
                          {
                              ExitDate = p.Key,
                              TotalFOB = p.Sum(x => x.Fob * x.OrderQuantity),
                              TotalBudget = p.Sum(x => x.Fob * x.OrderQuantity) - p.Sum(x => x.AgreedCm * x.OrderQuantity)
                          }).ToList();

            var b2bInfo = (from w in unitOfWork.BookingRepository.Get()
                           join ps in unitOfWork.PurchaseOrderRepository.Get() on w.PurchaseOrderID equals ps.PoStyleId
                           join pi in unitOfWork.PIRepository.Get() on w.PIId equals pi.PIID
                           //join b in unitOfWork.BackToBackLCRepository.Get() on pi.BackToBackLCID equals b.BackToBackLCID
                           where pi.BackToBackLCID != null && ((ps.JobId == jobID && pi.LoanFromJobID == null) || pi.LoanFromJobID == jobID)
                           group w by ps.ExitDate into s
                           select new RequisitionPO
                           {
                               ExitDate = s.Key,
                               TotalB2BLCValue = s.Sum(x => x.TotalQuantity * x.UnitPrice)
                           }).ToList();

            var result = (from s in poInfo
                          join c in b2bInfo on s.ExitDate equals c.ExitDate into g
                          from b in g.DefaultIfEmpty()
                          select new RequisitionPO
                          {
                              ExitDate = s.ExitDate,
                              TotalFOB = s.TotalFOB,
                              TotalBudget = s.TotalBudget,
                              TotalB2BLCValue = b == null ? 0 : b.TotalB2BLCValue
                          }).ToList();

            return result;
        }

        public List<ComparativeStatementViewModel> GetComparativeStatementReport(ReportFilteringViewModel reportFilteringVM)
        {
            List<ComparativeStatementViewModel> list = unitOfWork.JobRepository.SelectQuery<ComparativeStatementViewModel>("EXEC GetComparativeStatementData '" + reportFilteringVM.FromDate + "','"+ reportFilteringVM.ToDate +"','"+reportFilteringVM.JobID+"'");
            return list;
        }

        public List<BGMEUDSystemAuditReportVM> GetBGMEUDSystemAuditReport(ReportFilteringViewModel rVM)
        {
            var res = (from j in unitOfWork.JobRepository.Get()
                       where j.UDDate >= rVM.FromDate && j.UDDate <= rVM.ToDate && j.UDNo.ToString().Contains("3323")
                       select new BGMEUDSystemAuditReportVM
                       {
                           UDNO=j.UDNo,
                           UDDate=j.UDDate
                       }).ToList();
            return res;
        }

        public List<BillOfEntryReportViewModel> GetBillOfEntryReport(int jobID)
        {
            List<BillOfEntryReportViewModel> list = unitOfWork.JobRepository.SelectQuery<BillOfEntryReportViewModel>("EXEC GetBillOfEntryData '" + jobID + "'");
            return list;
        }

        #endregion

        #region Order Financial Summary

        public List<OrderFinancialSummaryViewModel> GetOrderFinancialSummary()
        {
            var result = (from p in unitOfWork.PurchaseOrderRepository.Get()
                          join j in unitOfWork.JobRepository.Get() on p.JobId equals j.JobInfoId
                          join sh in unitOfWork.ShipmentRepository.Get() on p.PoStyleId equals sh.PurchaseOrderID into g
                          from sp in g.DefaultIfEmpty()
                          group new { p, sp } by new { j.JobNo, j.ContractNo } into s
                          select new OrderFinancialSummaryViewModel
                          {
                              JobNo = s.Key.JobNo,
                              ContractNo = s.Key.ContractNo,
                              OrderQuantity = s.Sum(x => x.p.OrderQuantity),
                              TotalShipment = s.Sum(x => x.sp != null ? x.sp.ChalanQuantity : 0),
                              TotalFOB = s.Sum(x => x.p.Fob * x.p.OrderQuantity),
                              TotalInvoiceFOB = s.Sum(x => x.sp != null ? x.sp.InvoiceFOB : 0) ?? 0
                          }).ToList();

            return result;
        }

        #endregion

        #region Doc Dispatch Invoice

        public List<DocDispatchInvoiceReportViewModel> GetDocDispatchInvoice(int? buyerID, DateTime fromDate, DateTime toDate)
        {
            var temp = (from p in unitOfWork.PurchaseOrderRepository.Get()
                        join s in unitOfWork.StyleRepository.Get() on p.StyleId equals s.StyleId
                        join j in unitOfWork.JobRepository.Get() on p.JobId equals j.JobInfoId
                        join sp in unitOfWork.ShipmentRepository.Get() on p.PoStyleId equals sp.PurchaseOrderID
                        join i in unitOfWork.ExportInvoiceRepository.Get() on sp.InvoiceID equals i.InvoiceId
                        join bfr in unitOfWork.BankForwardingRepository.Get() on i.BankForwardingID equals bfr.BankForwardingID into bfg
                        from bf in bfg.DefaultIfEmpty()
                        where i.InvoiceDate != null && i.InvoiceDate >= fromDate && i.InvoiceDate <= toDate
                        select new { sp, s, j, i, bf }).AsQueryable();

            if (buyerID != null || buyerID != 0)
                temp.Where(x => x.s.BuyerId == buyerID);

            var result = (from c in temp
                          select new DocDispatchInvoiceReportViewModel
                          {
                              JobNo = c.j.JobNo,
                              InvoiceNo = c.i.InvoiceNo,
                              InvoiceDate = c.i.InvoiceDate,
                              BL = c.i.BL,
                              OnBoardDate = c.i.OnBoardDate,
                              BLReleaseDate = c.i.BLRealeaseDate,
                              EXP = c.i.EXP,
                              EXPDate = c.i.EXPDate,
                              FDBPNo = string.IsNullOrEmpty(c.i.FDBP_No) ? c.bf.FDBPNo : c.i.FDBP_No,
                              TotalShipmentQuantity = c.sp.ChalanQuantity,
                              InvoiceFOB = c.sp.InvoiceFOB
                          }).ToList();

            return result;
        }

        public List<DocDispatchInvoiceSummaryReportViewModel> GetDocDispatchInvoiceSummary(int? buyerID, DateTime fromDate, DateTime toDate)
        {
            var temp = (from p in unitOfWork.PurchaseOrderRepository.Get()
                        join s in unitOfWork.StyleRepository.Get() on p.StyleId equals s.StyleId
                        join b in unitOfWork.BuyerRepository.Get() on s.BuyerId equals b.BuyerId
                        join j in unitOfWork.JobRepository.Get() on p.JobId equals j.JobInfoId
                        join sp in unitOfWork.ShipmentRepository.Get() on p.PoStyleId equals sp.PurchaseOrderID
                        join i in unitOfWork.ExportInvoiceRepository.Get() on sp.InvoiceID equals i.InvoiceId
                        join bfr in unitOfWork.BankForwardingRepository.Get() on i.BankForwardingID equals bfr.BankForwardingID into bfg
                        from bf in bfg.DefaultIfEmpty()
                        where i.InvoiceDate != null && i.InvoiceDate >= fromDate && i.InvoiceDate <= toDate
                        select new { sp, b, j, i, bf }).AsQueryable();

            if (buyerID != null || buyerID != 0)
                temp.Where(x => x.b.BuyerId == buyerID);

            var result = (from c in temp
                          select new DocDispatchInvoiceSummaryReportViewModel
                          {
                              BuyerName = c.b.BuyerName,
                              InvoiceNo = c.i.InvoiceNo,
                              InvoiceFOB = c.sp.InvoiceFOB,
                              Year = c.i.InvoiceDate.Value.Year,
                              Month = c.i.InvoiceDate.Value.Month,
                              DispatchStatus = (c.i.DocDespatchDate == null && string.IsNullOrEmpty(c.i.FDBP_No) && c.bf.BankForwardingDate == null && string.IsNullOrEmpty(c.bf.FDBPNo)) ? "In Process" :
                                               (c.i.DocDespatchDate == null && !string.IsNullOrEmpty(c.i.FDBP_No) && c.bf.BankForwardingDate == null && !string.IsNullOrEmpty(c.bf.FDBPNo)) ? "Doc Submitted to Bank" :
                                               "Doc Destapched"
                          }).ToList();

            return result;
        }

        #endregion

        #region VAT Details

        public List<VATReportViewModel> GetVATDetails(DateTime fromDate, DateTime toDate, string factory)
        {
            StringBuilder query = new StringBuilder();

            query.Append("SELECT J.JobNo, I.InvoiceNo, I.ShippingBillDate, I.ShippingBill AS ShippingBill, ");
            query.Append("SUM(S.ChalanQuantity) AS ShippedQuantity, SUM(S.InvoiceFOB) AS InvoiceFOB ");
            query.Append("FROM PoStyle P ");
            query.Append("INNER JOIN JobInfo J On P.JobID = J.JobInfoID ");
            query.Append("INNER JOIN Shipment S ON P.PoStyleID = S.PurchaseOrderID ");
            query.Append("INNER JOIN Invoice I ON S.InvoiceID = I.InvoiceID ");
            query.Append("WHERE S.ChalanDate BETWEEN '" + Convert.ToDateTime(fromDate).ToString("yyy-MM-dd") + "' AND '" + Convert.ToDateTime(toDate).ToString("yyy-MM-dd") + "' ");
            query.Append("AND I.InvoiceNo LIKE '%" + factory + "%' ");
            query.Append("GROUP BY J.JobNo, I.InvoiceNo, I.ShippingBillDate, I.ShippingBill ");

            List<VATReportViewModel> results = unitOfWork.PurchaseOrderRepository.SelectQuery<VATReportViewModel>(query.ToString());

            return results;
        }

        #endregion

        #region Incentive Details

        public List<IncentiveReportViewModel> GetIncentiveDetails(int jobID)
        {
            StringBuilder query = new StringBuilder();

            query.Append("SELECT I.InvoiceNo, I.InvoiceDate, I.OnBoardDate, I.BL, I.BLRealeaseDate, I.EXP, I.EXPDate, ");
            query.Append("B.FDBPNo AS FDBPNo, ");
            query.Append("I.PortOfLoading, I.FinalDestination, I.CountryName, ");
            query.Append("SUM(S.ChalanQuantity) AS ShipmentQuantity, SUM(S.InvoiceFOB) InvoiceFOB ");
            query.Append("FROM Invoice I ");
            query.Append("INNER JOIN Shipment S ON I.InvoiceID = S.InvoiceID ");
            query.Append("INNER JOIN PoStyle P ON S.PurchaseOrderID = P.PoStyleID ");
            query.Append("LEFT JOIN BankForwarding B ON I.BankForwardingID = B.BankForwardingID ");
            query.Append("WHERE P.JobID = " + jobID + " ");
            query.Append("GROUP BY I.InvoiceNo, I.InvoiceDate, I.OnBoardDate, I.BL, I.BLRealeaseDate, ");
            query.Append("I.EXP, I.EXPDate, B.FDBPNo, I.PortOfLoading, I.FinalDestination, I.CountryName");

            List<IncentiveReportViewModel> results = unitOfWork.PurchaseOrderRepository.SelectQuery<IncentiveReportViewModel>(query.ToString());

            return results;
        }

        #endregion

        #region Production Status Report

        public List<ProductionStatusReportViewModel> GetProductionStatusReport(DateTime fromDate, DateTime todate)
        {
            var result = (from p in unitOfWork.PurchaseOrderRepository.Get()
                          join s in unitOfWork.StyleRepository.Get() on p.StyleId equals s.StyleId
                          join d in unitOfWork.ProductionStatusRepository.Get() on p.PoStyleId equals d.PoStyleId
                          where d.Date >= fromDate && d.Date <= todate
                          group d by new { s.StyleNo, p.PoStyleId, p.PoNo, p.OrderQuantity, p.AgreedCm, p.Fob, d.Floor, d.Line }
                              into s
                          select new ProductionStatusReportViewModel
                          {
                              PurchaseOrderID = s.Key.PoStyleId,
                              StyleNo = s.Key.StyleNo,
                              PONo = s.Key.PoNo,
                              OrderQuantity = s.Key.OrderQuantity,

                              CuttingQuantity = s.Sum(x => x.Cutting),
                              SewingQuantity = s.Sum(x => x.TodaySewing),
                              WashSendQuantity = s.Sum(x => x.SentWash),
                              WashReceivedQuantity = s.Sum(x => x.ReceivedWash),
                              PrintEmbSentQuantity = s.Sum(x => x.SentPrintEmb),
                              PrintEmbReceivedQuantity = s.Sum(x => x.ReceivedPrintEmb),
                              FinishedQuantity = s.Sum(x => x.TodayFinish),

                              TotalFOB = s.Key.Fob * s.Key.OrderQuantity,
                              SewingFOB = s.Key.Fob * s.Sum(x => x.TodaySewing),

                              TotalAgreedCM = s.Key.AgreedCm * s.Key.OrderQuantity,
                              SewingCM = s.Key.AgreedCm * s.Sum(x => x.TodaySewing),

                              Floor = s.Key.Floor,
                              Line = s.Key.Line,
                          }).ToList();

            //var shipmentList = (from sp in unitOfWork.ShipmentRepository.Get()
            //                    join p in sewingList on sp.PurchaseOrderID equals p.PurchaseOrderID
            //                    group sp by p.PurchaseOrderID into s
            //                    select new ProductionStatusReportViewModel
            //                    {
            //                        PurchaseOrderID = s.Key,
            //                        ShipmentQuantity = s.Sum(x => x.ChalanQuantity),
            //                        InvoiceFOB = s.Sum(x => x.InvoiceFOB),
            //                    }).ToList();

            //var result = (from sp in unitOfWork.ShipmentRepository.Get()
            //              join dpr in dprList on sp.PurchaseOrderID equals dpr.PurchaseOrderID
            //              group sp by dpr.PurchaseOrderID into s
            //              select new ProductionStatusReportViewModel
            //              {
            //                  PurchaseOrderID = s.Single(x=>x.dpr.).PurchaseOrderID,
            //                  StyleNo = s.StyleNo,
            //                  PONo = s.PONo,
            //                  OrderQuantity = s.OrderQuantity,

            //                  CuttingQuantity = s.CuttingQuantity,
            //                  SewingQuantity = s.SewingQuantity,
            //                  WashSendQuantity = s.WashSendQuantity,
            //                  WashReceivedQuantity = s.WashReceivedQuantity,
            //                  PrintEmbSentQuantity = s.PrintEmbSentQuantity,
            //                  PrintEmbReceivedQuantity = s.PrintEmbReceivedQuantity,
            //                  FinishedQuantity = s.FinishedQuantity,

            //                  ShipmentQuantity = sp.ShipmentQuantity,

            //                  TotalFOB = s.TotalFOB,
            //                  SewingFOB = s.SewingFOB,
            //                  InvoiceFOB = sp.InvoiceFOB,

            //                  TotalAgreedCM = s.TotalAgreedCM,
            //                  SewingCM = s.SewingCM,
            //                  ShipmentCM = s.TotalAgreedCM / s.OrderQuantity * sp.ShipmentQuantity,

            //                  TotalRMCost = s.TotalFOB - s.TotalAgreedCM,

            //                  Floor = s.Floor,
            //                  Line = s.Line,
            //              }).ToList();

            return result;
        }

        public List<ProductionStatusFromat2ViewModel> GetProductionStatusReportFormat2(int buyerID, DateTime fromDate, DateTime todate)
        {
            IEnumerable<ProductionStatusFromat2ViewModel> poInfo = (from p in unitOfWork.PurchaseOrderRepository.Get()
                                                                    join s in unitOfWork.StyleRepository.Get() on p.StyleId equals s.StyleId
                                                                    where p.ExitDate >= fromDate && p.ExitDate <= todate & s.BuyerId == buyerID
                                                                    select new ProductionStatusFromat2ViewModel
                                                                    {
                                                                        ReceivedCRD = p.ExitDate,
                                                                        StyleNo = s.StyleNo,
                                                                        StyleDescription = s.StyleDescription,
                                                                        PurchaseOrderID = p.PoStyleId,
                                                                        PONo = p.PoNo,
                                                                        OrderQuantity = p.OrderQuantity,
                                                                        ShipMode =
                                                                        (
                                                                          p.ShipMode == 1 ? "Sea" :
                                                                          p.ShipMode == 2 ? "Air" : ""
                                                                        ),
                                                                        DCCode = p.DCCode
                                                                    }).AsEnumerable();

            var productionInfo = (from p in unitOfWork.PurchaseOrderRepository.Get()
                                  join s in unitOfWork.StyleRepository.Get() on p.StyleId equals s.StyleId
                                  join d in unitOfWork.ProductionStatusRepository.Get() on p.PoStyleId equals d.PoStyleId
                                  where p.ExitDate >= fromDate && p.ExitDate <= todate & s.BuyerId == buyerID
                                  group d by new { s.StyleNo, s.StyleDescription, p.PoStyleId, p.PoNo, p.OrderQuantity, p.ExitDate, p.ShipMode, p.DCCode }
                                      into s
                                  select new ProductionStatusFromat2ViewModel
                                  {
                                      PurchaseOrderID = s.Key.PoStyleId,

                                      CuttingQuantity = s.Sum(x => x.Cutting) ?? 0,
                                      CuttingBalanceQuantity = s.Sum(x => x.Cutting) - (int)(s.Key.OrderQuantity),

                                      ProductionCompletedQuantity = s.Sum(x => x.TodaySewing) ?? 0,
                                      ProductionBalanceQuantity = s.Sum(x => x.TodaySewing) - (int)(s.Key.OrderQuantity),

                                      WashCompletedQuantity = s.Sum(x => x.ReceivedWash) ?? 0,
                                      WashBalanceQuantity = s.Sum(x => x.ReceivedWash) - (int)(s.Key.OrderQuantity),

                                      FinishingCompletedQuantity = s.Sum(x => x.TodayFinish) ?? 0,
                                      FinishingBalanceQuantity = s.Sum(x => x.TodayFinish) - (int)(s.Key.OrderQuantity)
                                  }).AsEnumerable();

            var shipmentInfo = (from p in unitOfWork.PurchaseOrderRepository.Get()
                                join s in unitOfWork.StyleRepository.Get() on p.StyleId equals s.StyleId
                                join sp in unitOfWork.ShipmentRepository.Get() on p.PoStyleId equals sp.PurchaseOrderID
                                where p.ExitDate >= fromDate && p.ExitDate <= todate & s.BuyerId == buyerID
                                group sp by sp.PurchaseOrderID into c
                                select new
                                {
                                    PurchaseOrderID = c.Key,
                                    ShippedQuantity = c.Sum(x => x.ChalanQuantity)
                                }).AsEnumerable();

            List<ProductionStatusFromat2ViewModel> result = (from p in poInfo
                                                             join dp in productionInfo on p.PurchaseOrderID equals dp.PurchaseOrderID into dg
                                                             from d in dg.DefaultIfEmpty()
                                                             join sp in shipmentInfo on p.PurchaseOrderID equals sp.PurchaseOrderID into sg
                                                             from s in sg.DefaultIfEmpty()
                                                             select new ProductionStatusFromat2ViewModel
                                                             {
                                                                 Year = p.ReceivedCRD.Year,
                                                                 Month = p.ReceivedCRD.Month,
                                                                 ReceivedCRD = p.ReceivedCRD,
                                                                 StyleNo = p.StyleNo,
                                                                 StyleDescription = p.StyleDescription,
                                                                 PurchaseOrderID = p.PurchaseOrderID,
                                                                 PONo = p.PONo,
                                                                 OrderQuantity = p.OrderQuantity,
                                                                 ShipMode = p.ShipMode,
                                                                 DCCode = p.DCCode,

                                                                 CuttingQuantity = d == null ? 0 : d.CuttingQuantity,
                                                                 CuttingBalanceQuantity = d == null ? 0 : d.CuttingBalanceQuantity,

                                                                 ProductionCompletedQuantity = d == null ? 0 : d.ProductionCompletedQuantity,
                                                                 ProductionBalanceQuantity = d == null ? 0 : d.ProductionBalanceQuantity,

                                                                 WashCompletedQuantity = d == null ? 0 : d.WashCompletedQuantity,
                                                                 WashBalanceQuantity = d == null ? 0 : d.WashBalanceQuantity,

                                                                 FinishingCompletedQuantity = d == null ? 0 : d.FinishingCompletedQuantity,
                                                                 FinishingBalanceQuantity = d == null ? 0 : d.FinishingBalanceQuantity,

                                                                 ShippedQuanity = s == null ? 0 : s.ShippedQuantity,
                                                                 ShippedBalanceQuanity = s == null ? p.OrderQuantity : p.OrderQuantity - s.ShippedQuantity
                                                             }).ToList();

            return result;
        }

        public List<ProductionStatusFromat3ViewModel> GetProductionStatusReportFormat3(int buyerID, DateTime todate)
        {
            var productionInfo = (from p in unitOfWork.PurchaseOrderRepository.Get()
                                  join s in unitOfWork.StyleRepository.Get() on p.StyleId equals s.StyleId
                                  join d in unitOfWork.ProductionStatusRepository.Get() on p.PoStyleId equals d.PoStyleId
                                  where d.Date == todate & s.BuyerId == buyerID
                                  group d by new { s.StyleNo, s.StyleDescription, p.PoStyleId, p.PoNo, p.OrderQuantity, p.ExitDate, d.Floor, d.Line, d.Date }
                                      into s
                                  select new ProductionStatusFromat3ViewModel
                                  {
                                      PurchaseOrderID = s.Key.PoStyleId,
                                      Factory = s.Key.Floor,
                                      Line = s.Key.Line,
                                      ReceivedCRD = s.Key.ExitDate,
                                      StyleNo = s.Key.StyleNo,
                                      StyleDescription = s.Key.StyleDescription,
                                      PONo = s.Key.PoNo,
                                      OrderQuantity = s.Key.OrderQuantity,
                                      CuttingQuantity = s.Sum(x => x.Cutting),
                                      SewingQuantity = s.Sum(x => x.TodaySewing),
                                      SentForWashQuantity = s.Sum(x => x.SentWash),
                                      WashQuantity = s.Sum(x => x.ReceivedWash)
                                  }).AsEnumerable();


            var totalProductionInfo = (from p in unitOfWork.PurchaseOrderRepository.Get()
                                       join s in unitOfWork.StyleRepository.Get() on p.StyleId equals s.StyleId
                                       join d in unitOfWork.ProductionStatusRepository.Get() on p.PoStyleId equals d.PoStyleId
                                       where s.BuyerId == buyerID
                                       group d by new { p.PoStyleId, p.PoNo, p.OrderQuantity, p.ExitDate, d.Floor, d.Line }
                                           into s
                                       select new ProductionStatusFromat3ViewModel
                                       {
                                           PurchaseOrderID = s.Key.PoStyleId,
                                           Factory = s.Key.Floor,
                                           Line = s.Key.Line,
                                           TotalCuttingQuantity = s.Sum(x => x.Cutting),
                                           TotalSewingInputQuantity = s.Sum(x => x.SewingInput),
                                           TotalSewingInputBalanceQuantity = s.Key.OrderQuantity - s.Sum(x => x.SewingInput),
                                           TotalSewingQuantity = s.Sum(x => x.TodaySewing),
                                           TotalSewingBalanceQuantity = s.Key.OrderQuantity - s.Sum(x => x.TodaySewing),
                                           TotalSentForWashQuantity = s.Sum(x => x.SentWash),
                                           TotalWashQuantity = s.Sum(x => x.ReceivedWash),
                                           TotalWashBalanceQuantity = s.Sum(x => x.SentWash) - s.Sum(x => x.ReceivedWash)
                                       }).AsEnumerable();

            var shipmentInfo = (from sp in unitOfWork.ShipmentRepository.Get()
                                join p in unitOfWork.PurchaseOrderRepository.Get() on sp.PurchaseOrderID equals p.PoStyleId
                                join s in unitOfWork.StyleRepository.Get() on p.StyleId equals s.StyleId
                                where s.BuyerId == buyerID
                                group sp by new { sp.PurchaseOrderID, p.OrderQuantity } into c
                                select new ProductionStatusFromat3ViewModel
                                {
                                    PurchaseOrderID = c.Key.PurchaseOrderID,
                                    ShippedQuantity = c.Sum(x => x.ChalanQuantity),
                                    ShortOrOverQuantity = c.Sum(x => x.ChalanQuantity) - c.Key.OrderQuantity
                                }).AsEnumerable();

            var result = (from p in productionInfo
                          join tl in totalProductionInfo on new { p.PurchaseOrderID, p.Factory, p.Line } equals new { tl.PurchaseOrderID, tl.Factory, tl.Line } into tg
                          from t in tg.DefaultIfEmpty()
                          join sp in shipmentInfo on p.PurchaseOrderID equals sp.PurchaseOrderID into sg
                          from s in sg.DefaultIfEmpty()
                          select new ProductionStatusFromat3ViewModel
                          {
                              PurchaseOrderID = p.PurchaseOrderID,
                              Factory = p.Factory,
                              Line = p.Line,
                              PONo = p.PONo,
                              ReceivedCRD = p.ReceivedCRD,
                              //OriginalCRD = p.OriginalCRD,
                              StyleNo = p.StyleNo,
                              StyleDescription = p.StyleDescription,
                              OrderQuantity = p.OrderQuantity,
                              //ColorName = p.ColorName,
                              CuttingQuantity = p.CuttingQuantity,
                              TotalCuttingQuantity = t.TotalCuttingQuantity,
                              TotalSewingInputQuantity = t.TotalSewingInputQuantity,
                              TotalSewingInputBalanceQuantity = t.TotalSewingInputBalanceQuantity,
                              SewingQuantity = p.SewingQuantity,
                              TotalSewingQuantity = t.TotalSewingQuantity,
                              TotalSewingBalanceQuantity = t.TotalSewingBalanceQuantity,
                              SentForWashQuantity = p.SentForWashQuantity,
                              TotalSentForWashQuantity = t.TotalSentForWashQuantity,
                              WashQuantity = p.WashQuantity,
                              TotalWashQuantity = t.TotalWashQuantity,
                              TotalWashBalanceQuantity = t.TotalWashBalanceQuantity,

                              ShippedQuantity = s == null ? null : s.ShippedQuantity,
                              ShortOrOverQuantity = s == null ? null : s.ShortOrOverQuantity
                          }).ToList();

            return result;
        }

        public List<ProductionCrisisViewModel> GetProducttionCrisiReport()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT B.BuyerName, S.StyleNo, P.PONo, P.OrderQuantity, P.ExitDate, ");
            query.Append("SP.ChalanQuantity AS ShipmentQuantity, SP.ChalanDate AS ShipmentDate FROM postyle P ");
            query.Append("INNER JOIN styleinfo S ON P.StyleID = S.StyleID ");
            query.Append("INNER JOIN buyerinfo B ON S.BuyerID = B.BuyerID ");
            query.Append("INNER JOIN shipment SP ON P.PoStyleID = SP.PurchaseOrderID ");
            query.Append("WHERE SP.ChalanQuantity != 0 AND P.PoStyleID NOT IN(SELECT PoStyleId FROM productiondailyreport)");

            var data = unitOfWork.ProductionStatusRepository.SelectQuery<ProductionCrisisViewModel>(query.ToString());

            return data;
        }

        public List<SewingShipmentSummaryReportViewModel> GetSewingShipmentSummary(int? buyerID, string floor, DateTime fromDate, DateTime todate)
        {
            var productionQuery
                = (from p in unitOfWork.PurchaseOrderRepository.Get()
                   join j in unitOfWork.JobRepository.Get() on p.JobId equals j.JobInfoId
                   join s in unitOfWork.StyleRepository.Get() on p.StyleId equals s.StyleId
                   join b in unitOfWork.BuyerRepository.Get() on s.BuyerId equals b.BuyerId
                   join d in unitOfWork.ProductionStatusRepository.Get() on p.PoStyleId equals d.PoStyleId
                   where p.ExitDate >= fromDate && p.ExitDate <= todate
                   group d by new { d.Floor, j.JobNo, b.BuyerId, b.BuyerName, s.StyleNo, p.PoStyleId, p.PoNo, p.OrderQuantity, p.ExitDate, p.Fob, p.AgreedCm }
                       into s
                   select new SewingShipmentSummaryReportViewModel
                   {
                       Floor = s.Key.Floor,
                       JobNo = s.Key.JobNo,
                       Buyer = s.Key.BuyerName,
                       BuyerID = s.Key.BuyerId,
                       StyleNo = s.Key.StyleNo,
                       PurchaseOrderID = s.Key.PoStyleId,
                       PONo = s.Key.PoNo,
                       AgreedCM = s.Key.AgreedCm,
                       FOB = s.Key.Fob,
                       ExitDate = s.Key.ExitDate,

                       OrderQuantity = s.Key.OrderQuantity,
                       OrderCM = s.Key.OrderQuantity * s.Key.AgreedCm,
                       OrderFOB = s.Key.OrderQuantity * s.Key.Fob,

                       SewingQuantity = s.Sum(x => x.TodaySewing) ?? 0,
                       SewingCM = s.Sum(x => x.TodaySewing) * s.Key.AgreedCm,
                       SewingFOB = s.Sum(x => x.TodaySewing) * s.Key.Fob
                   }).AsQueryable();

            if (!string.IsNullOrEmpty(floor))
                productionQuery = productionQuery.Where(x => x.Floor == floor);

            if (buyerID != null)
                productionQuery = productionQuery.Where(x => x.BuyerID == buyerID);

            var productionInfo = productionQuery.AsEnumerable();

            var shipmentQuery = (from p in unitOfWork.PurchaseOrderRepository.Get()
                                 join s in unitOfWork.StyleRepository.Get() on p.StyleId equals s.StyleId
                                 join sp in unitOfWork.ShipmentRepository.Get() on p.PoStyleId equals sp.PurchaseOrderID
                                 where p.ExitDate >= fromDate && p.ExitDate <= todate
                                 group sp by new { s.BuyerId, sp.PurchaseOrderID } into c
                                 select new
                                 {
                                     BuyerID = c.Key.BuyerId,
                                     PurchaseOrderID = c.Key.PurchaseOrderID,
                                     ShippedQuantity = c.Sum(x => x.ChalanQuantity)
                                 }).AsQueryable();

            if (buyerID != null)
                shipmentQuery = shipmentQuery.Where(x => x.BuyerID == buyerID);

            var shipmentInfo = shipmentQuery.AsEnumerable();

            List<SewingShipmentSummaryReportViewModel> result
                = (from p in productionInfo
                   join s in shipmentInfo on p.PurchaseOrderID equals s.PurchaseOrderID
                   select new SewingShipmentSummaryReportViewModel
                   {
                       Floor = p.Floor,
                       JobNo = p.JobNo,
                       Buyer = p.Buyer,
                       StyleNo = p.StyleNo,
                       PurchaseOrderID = p.PurchaseOrderID,
                       PONo = p.PONo,
                       ExitDate = p.ExitDate,

                       OrderQuantity = p.OrderQuantity,
                       OrderCM = p.OrderCM,
                       OrderFOB = p.OrderFOB,

                       SewingQuantity = p.SewingQuantity,
                       SewingCM = p.SewingCM,
                       SewingFOB = p.SewingFOB,

                       ShippedQuantity = s.ShippedQuantity,
                       ShippedCM = s.ShippedQuantity * p.AgreedCM,
                       ShippedFOB = s.ShippedQuantity * p.FOB
                   }).ToList();

            return result;
        }

        #endregion

        #region Contract Paper

        public List<ContractPaperViewModel> GetContractPaper(int jobID)
        {
            var result = (from p in unitOfWork.PurchaseOrderRepository.Get()
                          join s in unitOfWork.StyleRepository.Get() on p.StyleId equals s.StyleId
                          where p.JobId == jobID
                          select new ContractPaperViewModel
                          {
                              StyleNo = s.StyleNo,
                              PONo = p.PoNo,
                              OrderQuantity = p.OrderQuantity,
                              FOB = p.Fob,
                              ExitDate = p.ExitDate,
                              Item = s.Item,
                              FactoryCM = p.FactoryCM
                          }).ToList();

            return result;
        }

        #endregion

        #region Monthly Financial Report

        public List<MonthlyFinancialReportViewModel> GetMonthlyFinancialReport(DateTime fromDate, DateTime toDate)
        {
            var result = (from p in unitOfWork.PurchaseOrderRepository.Get()
                          join s in unitOfWork.StyleRepository.Get() on p.StyleId equals s.StyleId
                          where p.ExitDate >= fromDate && p.ExitDate <= toDate
                          select new MonthlyFinancialReportViewModel
                          {
                              StyleNo = s.StyleNo,
                              PONo = p.PoNo,
                              OrderQuantity = p.OrderQuantity
                          }).ToList();

            return result;
        }

        #endregion

        #region BackToBackLC Crisis Report

        public List<BackToBackLCCrisisReportViewModel> GetBackToBackLCCrisisReport()
        {
            var piList = (from pi in unitOfWork.PIRepository.Get()
                          join b in unitOfWork.BookingRepository.Get() on pi.PIID equals b.PIId
                          join p in unitOfWork.PurchaseOrderRepository.Get() on b.PurchaseOrderID equals p.PoStyleId
                          join j in unitOfWork.JobRepository.Get() on p.JobId equals j.JobInfoId
                          join sp in unitOfWork.SupplierRepository.Get() on pi.SupplierID equals sp.SupplierId into sg
                          from s in sg.DefaultIfEmpty()
                          orderby pi.PIID descending
                          where pi.BackToBackLCID == null
                          select new BackToBackLCCrisisReportViewModel
                          {
                              JobNo = j.JobNo,
                              SupplierName = s.SupplierName,
                              PINo = pi.PINo,
                              PIDate = pi.PIDate
                          }).Distinct().ToList();

            var piListAdvancedCM = (from a in unitOfWork.AdvancedCMRepository.Get()
                                    join pi in unitOfWork.PIRepository.Get() on a.PIID equals pi.PIID
                                    join j in unitOfWork.JobRepository.Get() on a.JobID equals j.JobInfoId
                                    join sp in unitOfWork.SupplierRepository.Get() on pi.SupplierID equals sp.SupplierId into sg
                                    from s in sg.DefaultIfEmpty()
                                    orderby pi.PIID descending
                                    where pi.BackToBackLCID == null
                                    select new BackToBackLCCrisisReportViewModel
                                    {
                                        JobNo = j.JobNo,
                                        SupplierName = s.SupplierName,
                                        PINo = pi.PINo,
                                        PIDate = pi.PIDate
                                    }).ToList();

            piList.AddRange(piListAdvancedCM);

            return piList;
        }

        #endregion

        #region Bank Forwarding

        public List<BankForwardingReportViewModel> GetBankForwardingInvoiceInfo(int bankForwardingID)
        {
            var data = (from i in unitOfWork.ExportInvoiceRepository.Get()
                        join sp in unitOfWork.ShipmentRepository.Get() on i.InvoiceId equals sp.InvoiceID
                        join p in unitOfWork.PurchaseOrderRepository.Get() on sp.PurchaseOrderID equals p.PoStyleId
                        join j in unitOfWork.JobRepository.Get() on p.JobId equals j.JobInfoId
                        where i.BankForwardingID == bankForwardingID
                        group sp by new { j.ContractNo, i.InvoiceNo, i.EXP, i.BLToBeEndorsedTo } into c
                        select new BankForwardingReportViewModel
                        {
                            ContractNo = c.Key.ContractNo,
                            InvoiceNo = c.Key.InvoiceNo,
                            InvoiceFOB = c.Sum(x => x.InvoiceFOB.Value),
                            InvoiceQuantity = c.Sum(x => x.ChalanQuantity),
                            EXPNo = c.Key.EXP,
                            BLToBeEndorsedTo = c.Key.BLToBeEndorsedTo
                        }).ToList();

            return data;
        }

        #endregion

        public IQueryable<PurchaseOrderCrisisViewModel> GetPurchaseOrderCrisis(int accountID)
        {
            IQueryable<PurchaseOrderCrisisViewModel> result =
                (from p in unitOfWork.PurchaseOrderRepository.Get()
                 join s in unitOfWork.StyleRepository.Get() on p.StyleId equals s.StyleId
                 join j in unitOfWork.JobRepository.Get() on p.JobId equals j.JobInfoId
                 where
                 s.AccountId == accountID &&
                 !p.sizecolor.Select(x => x.PoStyleId).Contains(p.PoStyleId)
                 select new PurchaseOrderCrisisViewModel
                 {
                     JobNo = j.JobNo,
                     StyleNo = s.StyleNo,
                     PurchaseOrderID = p.PoStyleId,
                     PONo = p.PoNo,
                     ExitDate = p.ExitDate,
                     OrderQuantity = p.OrderQuantity,
                     IsSizeColorExists = false
                 }).AsQueryable();

            return result;
        }

        public List<AdvancedCMReportViewModel> GetAdvancedCmReport(int year)
        {
            var data = (from p in unitOfWork.PurchaseOrderRepository.Get()
                        join j in unitOfWork.JobRepository.Get() on p.JobId equals j.JobInfoId
                        where j.JobNo.StartsWith(year.ToString())
                        select new
                        {
                            JobNo = j.JobNo,
                            PONo = p.PoNo,
                            ExitDate = p.ExitDate,
                            TotalFOB = p.OrderQuantity * p.Fob,
                            AdvancedCMPercentage = j.AdvancedCMPercentage,
                            SightDays = j.SightDays
                        }).AsEnumerable();

            var result = (from s in data
                          select new AdvancedCMReportViewModel
                          {
                              JobNo = s.JobNo,
                              PONo = s.PONo,
                              Month = s.ExitDate.AddDays((double)(s.SightDays ?? 0) * -1).Month,
                              Period = s.ExitDate.AddDays((double)(s.SightDays ?? 0) * -1).Day <= 15 ? "First" : "Second",
                              TotalFOB = s.TotalFOB,
                              AvailableCM = s.TotalFOB * (s.AdvancedCMPercentage ?? 0) / 100
                          }).ToList();

            return result;
        }

        public List<RealizationReportViewModel> GetRelizationReport(int accountType, DateTime fromDate, DateTime toDate, Boolean inBDT)
        {
            var result = (from r in unitOfWork.RealizationRepository.Get()
                          join b in unitOfWork.BankForwardingRepository.Get() on r.BankForwardingID equals b.BankForwardingID
                          join a in unitOfWork.RealizationAccountRepository.Get() on r.AccountID equals a.RealizationAccountID
                          where a.RealizationAccountType == accountType & r.RealizationDate >= fromDate & r.RealizationDate <= toDate
                          select new RealizationReportViewModel
                          {
                              BankForwardingID = b.BankForwardingID,
                              FDBPNo = b.FDBPNo,
                              RealizationDate = r.RealizationDate,
                              AccountName = a.RealizationAccountName,
                              Amount = inBDT == true ? r.Amount * (r.CurrencyRate ?? 1) : r.Amount,
                              CurrencyRate = r.CurrencyRate
                          }).ToList();

            decimal? _currencyRate = result.Where(x => x.CurrencyRate != null).Select(x => x.CurrencyRate).FirstOrDefault();

            foreach (var item in result)
            {
                item.RealizationValue = result.Where(x => x.FDBPNo == item.FDBPNo).Sum(x => x.Amount);

                item.InvoiceValue = (from s in unitOfWork.ExportInvoiceRepository.Get()
                                     join sp in unitOfWork.ShipmentRepository.Get() on s.InvoiceId equals sp.InvoiceID
                                     where s.BankForwardingID == item.BankForwardingID
                                     select sp.InvoiceFOB).Sum();

                if (inBDT)
                {
                    item.InvoiceValue = item.InvoiceValue * (_currencyRate ?? 1);
                }

                item.Difference = item.InvoiceValue - item.RealizationValue;
            }

            return result;
        }

        public List<SubContractReportViewModel> GetSubContractReport(DateTime fromDate, DateTime toDate)
        {
            var result = (from p in unitOfWork.PurchaseOrderRepository.Get()
                          join st in unitOfWork.StyleRepository.Get() on p.StyleId equals st.StyleId
                          join b in unitOfWork.BuyerRepository.Get() on st.BuyerId equals b.BuyerId
                          join s in unitOfWork.SubContractRepository.Get() on p.PoStyleId equals s.PurchaseOrderID
                          join f in unitOfWork.FactoryRepository.Get() on p.FactoryId equals f.FactoryId
                          join sf in unitOfWork.FactoryRepository.Get() on s.FactoryID equals sf.FactoryId
                          join sh in unitOfWork.ShipmentRepository.Get() on p.PoStyleId equals sh.PurchaseOrderID into sg
                          from sp in sg.DefaultIfEmpty()
                          join iv in unitOfWork.ExportInvoiceRepository.Get() on sp.InvoiceID equals iv.InvoiceId into ig
                          from i in ig.DefaultIfEmpty()
                          where p.ExitDate >= fromDate && p.ExitDate <= toDate
                          select new SubContractReportViewModel
                          {
                              FactoryName = f.FactoryName,
                              SubContractFactory = sf.FactoryName,
                              BuyerName = b.BuyerName,
                              PONo = p.PoNo,
                              OrderQuantity = p.OrderQuantity,
                              FOB = p.Fob,
                              AgreedCM = p.AgreedCm,
                              SubContractRate = s.SubContractRate,
                              CommercialPercentage = s.CommercialPercentage,
                              ShippedDate = sp.ChalanDate,
                              TotalShipment = sp.ChalanQuantity,
                              InvoiceFOB = sp.InvoiceFOB,
                              InvoiceNo = i.InvoiceNo
                          }).ToList();

            return result;
        }

        public List<PurchaseRequisitionReportViewModel> GetPuchaseRequisition(int PurchaseRequisitionID)
        {
            List<PurchaseRequisitionReportViewModel> result = (from s in unitOfWork.PurchaseRequisitionRepository.Get()
                                                               join p in unitOfWork.PurchaseRequisitionDetailsRepository.Get() on s.PurchaseRequisitionID equals p.PurchaseRequisitionID
                                                               join d in unitOfWork.DepartmentRepository.Get() on s.DepartmentID equals d.DepartmentID
                                                               join u in unitOfWork.ConsumptionUnitRepository.Get() on p.UnitID equals u.ConsumptionUnitId
                                                               where s.PurchaseRequisitionID == PurchaseRequisitionID
                                                               select new PurchaseRequisitionReportViewModel
                                                               {
                                                                   RequisitionNo = s.RequisitionNo,
                                                                   RequisitionDate = s.RequisitionDate,
                                                                   DepartmentName = d.DepartmentName,

                                                                   ProductDescription = p.ProductDescription,
                                                                   Quantity = p.Quantity,
                                                                   UnitName = u.UnitName,
                                                                   UnitPrice = p.UnitPrice,
                                                                   Remarks = s.Remarks,
                                                                   Sector = s.Sector
                                                               }).ToList();

            return result;
        }

        #region Realization Crisis

        public List<RealizationCrisisReportViewModel> GetRealizationCrisis(int jobID)
        {
            List<RealizationCrisisReportViewModel> data = (from s in unitOfWork.BankForwardingRepository.Get()
                                                           join rz in unitOfWork.RealizationRepository.Get() on s.BankForwardingID equals rz.BankForwardingID into rg
                                                           from c in rg.DefaultIfEmpty()
                                                           where s.JobID == jobID && c.BankForwardingID == 0
                                                           select new RealizationCrisisReportViewModel
                                                           {
                                                               BankForwardingID = s.BankForwardingID,
                                                               BankForwardingNo = s.BankForwardingNo,
                                                               BankForwardingDate = s.BankForwardingDate,
                                                               FDBPNo = s.FDBPNo
                                                           }).ToList();

            return data;
        }

        #endregion

        #region Import Status

        public List<ImportStatusViewModel> GetImportStatus(int jobID)
        {
            List<ImportStatusViewModel> result = (from j in unitOfWork.JobRepository.Get()
                                                  join b in unitOfWork.BackToBackLCRepository.Get() on j.JobInfoId equals b.JobID
                                                  join p in unitOfWork.PIRepository.Get() on b.BackToBackLCID equals p.BackToBackLCID
                                                  join s in unitOfWork.SupplierRepository.Get() on p.SupplierID equals s.SupplierId
                                                  join bl in unitOfWork.BLRepository.Get() on b.BackToBackLCID equals bl.BackToBackLCID
                                                  where j.JobInfoId == jobID
                                                  select new ImportStatusViewModel
                                                  {
                                                      BackToBackLCNo = b.BackToBackLC1,
                                                      ExportLCNo = j.ContractNo,
                                                      JobNo = j.JobNo,
                                                      Buyer = "",
                                                      Benificiary = s.SupplierName,
                                                      InvoiceNo = bl.InvoiceNo,
                                                      InvoiceDate = bl.InvoiceDate,
                                                      InvoiceValue = null,
                                                      InvoiceQuantity = null,
                                                      Unit = null,
                                                      BLNo = bl.BLNo,
                                                      BLDate = bl.BLDate,
                                                      PortName = "",
                                                      CopyDocuSentToCnf = null,
                                                      NegoOriginal = null,
                                                      ETA = null,
                                                      PositionOfVessel = "",
                                                      DeliveryByCnf = null,
                                                      Stuffing = "",
                                                      Container = "",
                                                      Remarks = "",
                                                  }).ToList();


            return result;
        }

        #endregion

        #region Inventory
        public List<InventoryViewModel> GetInventory(ReportFilteringViewModel rptVM)
        {
            List<InventoryViewModel> inventoryList = unitOfWork.BLRepository.SelectQuery<InventoryViewModel>("EXEC InventoryReport '" + rptVM.FromDate + "','" + rptVM.ToDate + "','" + rptVM.ShipmentMode + "','"+rptVM.JobID+"','"+rptVM.PurchaseOrderID+"'");
            return inventoryList;
        }
        #endregion

        #region Accounts

        public List<ExpenseBudgetReportViewModel> GetExpenseBudgetReport(DateTime fromDate, DateTime toDate, int factory)
        {

            var data = (from a in unitOfWork.ChartOfAccountRepository.Get()
                        join b in unitOfWork.BudgetRepository.Get() on a.ChartOfAccountID equals b.ChartOfAccountID
                        join e in unitOfWork.ExpenseRepository.Get() on a.ChartOfAccountID equals e.ChartOfAccountID
                        where e.ExpenseDate >= fromDate & e.ExpenseDate <= toDate
                        select new ExpenseBudgetReportViewModel
                        {
                            AccountNo = a.AccountNo,
                            AccountName = a.AccountName,
                            YearlyBudget = b.BudgetAmount,
                            MonthlyBudget = b.BudgetAmount / 12,
                            ExpenseAmount = e.ExpenseAmount,
                            ExpenseDate = e.ExpenseDate,
                            ExpenseBy = e.ExpenseBy
                        }).OrderBy(x => x.ExpenseDate).ToList();

            if (factory == 1)
            {
                data = data.Where(x => x.AccountNo.StartsWith("20")).ToList();
            }
            else
            {
                data = data.Where(x => x.AccountNo.StartsWith("10")).ToList();
            }
            return data;
        }

        public List<ExpenseBudgetReportViewModel> GetExpenseBudgetSummaryReport(DateTime fromDate, DateTime toDate, int factory)
        {
            List<ExpenseBudgetReportViewModel> data 
                = unitOfWork.BLRepository
                .SelectQuery<ExpenseBudgetReportViewModel>("EXEC RPT_GetExpenseBudgetSummaryRport '" + fromDate + "','" + toDate + "','" + factory + "'");

            return data;
        }

        #endregion
    }
}

