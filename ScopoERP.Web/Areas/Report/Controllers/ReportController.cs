using Newtonsoft.Json;
using ScopoERP.Accounts.BLL;
using ScopoERP.Commercial.BankForwardingL;
using ScopoERP.Commercial.BLL;
using ScopoERP.Commercial.ViewModel;
using ScopoERP.Common.ViewModel;
using ScopoERP.LC.BLL;
using ScopoERP.MaterialManagement.BLL;
using ScopoERP.OrderManagement.BLL;
using ScopoERP.Production.BLL;
using ScopoERP.ProductionStatus.BLL;
using ScopoERP.ProductionStatus.ViewModel;
using ScopoERP.Reports.BLL;
using ScopoERP.Reports.ViewModel;
using ScopoERP.Stackholder.BLL;
using ScopoERP.Web.Helper;
using ScopoERP.Web.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.Extensions;

namespace ScopoERP.Web.Areas.Report.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {

        private ReportLogic reportLogic;
        private BuyerLogic buyerLogic;
        private PILogic piLogic;
        private JobLogic jobLogic;
        private RequisitionLogic requisitionLogic;
        private ProductionFloorLogic productionFloorLogic;
        private BankForwardingLogic bankForwardingLogic;
        private PurchaseRequisitionLogic purchaseRequisitionLogic;
        private CouponLogic couponLogic;
        private PurchaseOrderLogic poLogic;
        private ExportInvoiceLogic invoiceLogic;
        private StyleLogic styleLogic;
        private CuttingPlanLogic cuttingPlanLogic;
        private StandardOperationLogic stdOperationLogic;

        #region Contructor

        public ReportController(ReportLogic reportLogic, BuyerLogic buyerLogic,
                                PILogic piLogic, JobLogic jobLogic, RequisitionLogic requisitionLogic,
                                ProductionFloorLogic productionFloorLogic, BankForwardingLogic bankForwardingLogic,
                                PurchaseRequisitionLogic purchaseRequisitionLogic, CouponLogic couponLogic, PurchaseOrderLogic poLogic,
                                ExportInvoiceLogic invoiceLogic, StyleLogic styleLogic, CuttingPlanLogic cuttingPlanLogic, StandardOperationLogic stdOperationLogic)
        {
            this.reportLogic = reportLogic;
            this.buyerLogic = buyerLogic;
            this.piLogic = piLogic;
            this.jobLogic = jobLogic;
            this.requisitionLogic = requisitionLogic;
            this.productionFloorLogic = productionFloorLogic;
            this.bankForwardingLogic = bankForwardingLogic;
            this.purchaseRequisitionLogic = purchaseRequisitionLogic;
            this.couponLogic = couponLogic;
            this.poLogic = poLogic;
            this.invoiceLogic = invoiceLogic;
            this.styleLogic = styleLogic;
            this.cuttingPlanLogic = cuttingPlanLogic;
            this.stdOperationLogic = stdOperationLogic;
        }


        #endregion

        #region WIP
        public ActionResult WIP()
        {
            return View(new List<WIPViewModel>());
        }

        public ActionResult WIPExport(DateTime fromDate, DateTime toDate)
        {
            List<WIPViewModel> data = reportLogic.GetWIPByDate(fromDate, toDate).ToList();

            return View("WIP", data);
        }

        [GridAction]
        public ActionResult GetWIP(string orderBy, string filter, int page, int pageSize = 0)
        {
            var results = reportLogic.GetWIP();

            var wip = results.ToGridModel(page, pageSize, orderBy, string.Empty, filter).Data.Cast<WIPViewModel>().ToList();

            return View(new GridModel(wip));
        }

        #endregion

        #region Shipment

        public ActionResult ShipmentDetails()
        {
            return View(new List<ShipmentDetailsReportViewModel>());
        }

        [HttpPost]
        public ActionResult ShipmentDetails(int? jobID, DateTime fromDate, DateTime toDate)
        {
            List<ShipmentDetailsReportViewModel> data = reportLogic.GetShipmentDetailsByJobAndDate(jobID, fromDate, toDate).ToList();
            return View(data);
        }

        [GridAction]
        public ActionResult GetShipmentDetails(string orderBy, string filter, int page, int pageSize = 0)
        {
            var results = reportLogic.GetShipmentDetails();

            var shipmentDetails = results.ToGridModel(page, pageSize, orderBy, string.Empty, filter).Data.Cast<ShipmentDetailsReportViewModel>().ToList();

            return View(new GridModel(shipmentDetails));
        }

        #endregion


        #region ShipmentCrisis

        public ActionResult ShipmentCrisis()
        {
            return View();
        }

        [GridAction]
        public ActionResult GetShipmentCrisis()
        {
            var results = reportLogic.GetShipmentCrisis(CurrentUser.AccountID);

            return View(new GridModel(results));
        }

        #endregion


        #region ShipmentReport

        public ActionResult ShipmentReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ShipmentReport(ReportFilteringViewModel reportFilteringVM)
        {
            if (reportFilteringVM.Decision == 2)
            {
                List<ShipmentSummaryViewModel> data = reportLogic.GetShipmentSummary(reportFilteringVM.FromDate, reportFilteringVM.ToDate);

                DataSet ds = new DataSet();
                ds.Tables.Add(data.ConvertToDataTable());

                ReportHelper.SetData("ShipmentSummaryReport", ds, reportFilteringVM);
            }
            else if (reportFilteringVM.Decision == 1)
            {
                List<ShipmentDetailsViewModel> data = reportLogic.GetShipmentDetails(reportFilteringVM.FromDate, reportFilteringVM.ToDate);

                DataSet ds = new DataSet();
                ds.Tables.Add(data.ConvertToDataTable());

                ReportHelper.SetData("ShipmentDetailsReport", ds, reportFilteringVM);
            }

            return Redirect("/Reports/ReportViewer.aspx");
        }

        #endregion


        #region OrderFinancialSummary

        public ActionResult OrderFinancialSummary()
        {
            ReportFilteringViewModel reportFilteringVM = new ReportFilteringViewModel();

            List<OrderFinancialSummaryViewModel> data = reportLogic.GetOrderFinancialSummary();

            DataSet ds = new DataSet();
            ds.Tables.Add(data.ConvertToDataTable());

            ReportHelper.SetData("OrderFinancialSummaryReport", ds, reportFilteringVM);

            return Redirect("/Reports/ReportViewer.aspx");
        }

        #endregion


        #region ProformaInvoiceReport

        public ActionResult ProformaInvoiceReport()
        {
            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text");

            return View();
        }

        [HttpPost]
        public ActionResult ProformaInvoiceReport(ReportFilteringViewModel reportFilteringVM)
        {
            if (ModelState.IsValid)
            {
                List<ProformaInvoiceViewModel> summaryData = reportLogic.GetProformaInvoiceSummary(reportFilteringVM.PIID);
                List<ProformaInvoiceViewModel> detailsData = reportLogic.GetProformaInvoiceDetails(reportFilteringVM.PIID);

                DataSet ds = new DataSet();
                ds.Tables.Add(summaryData.ConvertToDataTable());
                ds.Tables.Add(detailsData.ConvertToDataTable());

                // Get Proforma Invoice information
                var piInfo = piLogic.GetPIByID(reportFilteringVM.PIID);
                reportFilteringVM.PINo = piInfo.PINo;
                reportFilteringVM.PIDate = piInfo.PIDate;

                // Set values
                ReportHelper.SetData("ProformaInvoiceReport", ds, reportFilteringVM);

                return Redirect("/Reports/ReportViewer.aspx");
            }

            ViewBag.PI = new SelectList(piLogic.GetPIDropDown(), "Value", "Text", reportFilteringVM.PIID);

            return View();
        }

        public JsonResult GetPIByJob(int id)
        {
            var result = piLogic.GetPIDropDownByJob(id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion



        //#region CouponReport

        //public bool Coupon(int styleID, int purchaseOrderID, int cuttingPlanID)
        //{
        //        List<CouponViewModel> couponData = couponLogic.GetCouponSearchResult(styleID, purchaseOrderID, cuttingPlanID);

        //        DataSet ds = new DataSet();
        //        ds.Tables.Add(couponData.ConvertToDataTable());

        //        ReportFilteringViewModel reportVM = new ReportFilteringViewModel();

        //        // Set values
        //        ReportHelper.SetData("Coupon", ds, reportVM);

        //        return true;

        //}

        //#endregion






        #region Job Status

        public ActionResult JobStatusReport()
        {
            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text");

            IDictionary<int, int> yearList = new Dictionary<int, int>();
            yearList.Add(2012, 2012);
            yearList.Add(2013, 2013);
            yearList.Add(2014, 2014);
            yearList.Add(2015, 2015);
            yearList.Add(2016, 2016);
            yearList.Add(2017, 2017);
            yearList.Add(2018, 2018);
            yearList.Add(2019, 2019);
            yearList.Add(2020, 2020);

            ViewBag.Years = new SelectList(yearList, "Key", "Value");

            return View();
        }

        [HttpPost]
        public ActionResult JobStatusReport(ReportFilteringViewModel reportFilteringVM)
        {
            if (reportFilteringVM.Decision == 1)
            {
                List<JobStatusReportViewModel> data = reportLogic.GetJobStatus(reportFilteringVM.JobID);

                DataSet ds = new DataSet();
                ds.Tables.Add(data.ConvertToDataTable());

                ReportHelper.SetData("JobStatusReport", ds, reportFilteringVM);
            }
            else
            {
                List<JobStatusReportViewModel> data = reportLogic.GetJobStatusSummary(reportFilteringVM.Year);

                DataSet ds = new DataSet();
                ds.Tables.Add(data.ConvertToDataTable());

                ReportHelper.SetData("JobStatusSummaryReport", ds, reportFilteringVM);
            }

            return Redirect("/Reports/ReportViewer.aspx");
        }

        #endregion


        #region Job Summary

        public ActionResult JobSummaryReport()
        {
            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text");

            return View();
        }

        [HttpPost]
        public ActionResult JobSummaryReport(ReportFilteringViewModel reportFilteringVM)
        {
            if (ModelState.IsValid)
            {
                List<JobSummaryReportViewModel> data = reportLogic.GetJobSummary(reportFilteringVM.JobID);

                DataSet ds = new DataSet();
                ds.Tables.Add(data.ConvertToDataTable());

                // Set values
                ReportHelper.SetData("JobSummaryReport", ds, reportFilteringVM);

                return Redirect("/Reports/ReportViewer.aspx");
            }

            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text", reportFilteringVM.JobID);

            return View();
        }

        #endregion


        #region Job Item Report

        public ActionResult JobItemReport()
        {
            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text");

            return View();
        }

        [HttpPost]
        public ActionResult JobItemReport(ReportFilteringViewModel reportFilteringVM)
        {
            if (ModelState.IsValid)
            {
                List<JobItemReportViewModel> data = reportLogic.GetJobItem(reportFilteringVM.JobID);

                DataSet ds = new DataSet();
                ds.Tables.Add(data.ConvertToDataTable());

                // Set values
                ReportHelper.SetData("JobItemStatusReport", ds, reportFilteringVM);

                return Redirect("/Reports/ReportViewer.aspx");
            }

            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text", reportFilteringVM.JobID);

            return View();
        }

        #endregion


        #region DocDispatchInvoiceReport

        public ActionResult DocDispatchInvoiceReport()
        {
            List<DropDownListViewModel> buyerList = buyerLogic.GetBuyerDropDown();
            buyerList.Add(new DropDownListViewModel { Text = "All", Value = 0 });

            ViewBag.Buyer = new SelectList(buyerList, "Value", "Text", 0);

            return View();
        }

        [HttpPost]
        public ActionResult DocDispatchInvoiceReport(ReportFilteringViewModel reportFilteringVM)
        {
            if (ModelState.IsValid)
            {
                if (reportFilteringVM.Decision == 2)
                {
                    List<DocDispatchInvoiceSummaryReportViewModel> data = reportLogic.GetDocDispatchInvoiceSummary(reportFilteringVM.BuyerID, reportFilteringVM.FromDate, reportFilteringVM.ToDate);

                    DataSet ds = new DataSet();
                    ds.Tables.Add(data.ConvertToDataTable());

                    ReportHelper.SetData("DocDispatchInvoiceSummaryReport", ds, reportFilteringVM);
                }
                else if (reportFilteringVM.Decision == 1)
                {
                    List<DocDispatchInvoiceReportViewModel> data = reportLogic.GetDocDispatchInvoice(reportFilteringVM.BuyerID, reportFilteringVM.FromDate, reportFilteringVM.ToDate);

                    DataSet ds = new DataSet();
                    ds.Tables.Add(data.ConvertToDataTable());

                    ReportHelper.SetData("DocDispatchInvoiceReport", ds, reportFilteringVM);
                }

                return Redirect("/Reports/ReportViewer.aspx");
            }

            List<DropDownListViewModel> buyerList = buyerLogic.GetBuyerDropDown();
            buyerList.Add(new DropDownListViewModel { Text = "All", Value = 0 });
            ViewBag.Buyer = new SelectList(buyerList, "Value", "Text", reportFilteringVM.BuyerID);

            return View();
        }

        #endregion


        #region VATReport

        public ActionResult VATReport()
        {
            IDictionary<string, string> factoryList = new Dictionary<string, string>();
            factoryList.Add("ASWL", "ASWL");
            factoryList.Add("DMC", "DMC");

            ViewBag.FactoryList = new SelectList(factoryList, "Key", "Value");

            return View();
        }

        [HttpPost]
        public ActionResult VATReport(ReportFilteringViewModel reportFilteringVM)
        {
            List<VATReportViewModel> data = reportLogic.GetVATDetails(reportFilteringVM.FromDate, reportFilteringVM.ToDate, reportFilteringVM.Factory);

            DataSet ds = new DataSet();
            ds.Tables.Add(data.ConvertToDataTable());

            ReportHelper.SetData("VATReport", ds, reportFilteringVM);

            return Redirect("/Reports/ReportViewer.aspx");
        }

        #endregion


        #region Incentive Report

        public ActionResult IncentiveReport()
        {
            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text");

            return View();
        }

        [HttpPost]
        public ActionResult IncentiveReport(ReportFilteringViewModel reportFilteringVM)
        {
            if (ModelState.IsValid)
            {
                List<IncentiveReportViewModel> data = reportLogic.GetIncentiveDetails(reportFilteringVM.JobID);

                DataSet ds = new DataSet();
                ds.Tables.Add(data.ConvertToDataTable());

                // Set values
                ReportHelper.SetData("IncentiveReport", ds, reportFilteringVM);

                return Redirect("/Reports/ReportViewer.aspx");
            }

            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text", reportFilteringVM.JobID);

            return View();
        }

        #endregion


        #region Requisition Report

        public ActionResult RequisitionReport()
        {
            ViewBag.JobList = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text");
            return View();
        }

        [HttpPost]
        public ActionResult RequisitionReport(ReportFilteringViewModel reportFilteringVM)
        {
            if (ModelState.IsValid)
            {
                RequisitionReportViewModel summaryData = reportLogic.GetRequisitionSummary(reportFilteringVM.RequisitionID);
                List<RequisitionPIViewModel> appliedForB2B = reportLogic.GetRequisitionAppliedForB2B(reportFilteringVM.RequisitionID);
                List<RequisitionPIViewModel> pendingForB2B = reportLogic.GetRequisitionPendingForB2B(reportFilteringVM.RequisitionID, summaryData.JobID);
                List<RequisitionPIViewModel> alreadyOpenedB2B = reportLogic.GetRequisitionAlreadyOpenedB2B(summaryData.JobID);

                summaryData.AppliedForB2BLCValue = appliedForB2B.Sum(x => x.PIValue);
                summaryData.PendingB2BLCValue = pendingForB2B.Sum(x => x.PIValue);
                summaryData.BackToBackLCValue = alreadyOpenedB2B.Sum(x => x.PIValue);
                summaryData.TotalValue = summaryData.AppliedForB2BLCValue + summaryData.PendingB2BLCValue
                                            + summaryData.BackToBackLCValue;
                summaryData.AvailableBudget = summaryData.BudgetValue - summaryData.TotalValue;

                List<RequisitionReportViewModel> summaryDataList = new List<RequisitionReportViewModel>();
                summaryDataList.Add(summaryData);

                List<RequisitionPO> requisitionPO = reportLogic.GetRequisitionPO(summaryData.JobID);

                DataSet ds = new DataSet();
                ds.Tables.Add(summaryDataList.ConvertToDataTable());
                ds.Tables.Add(appliedForB2B.ConvertToDataTable());
                ds.Tables.Add(alreadyOpenedB2B.ConvertToDataTable());
                ds.Tables.Add(pendingForB2B.ConvertToDataTable());
                ds.Tables.Add(requisitionPO.ConvertToDataTable());

                reportFilteringVM.ContractValue = summaryData.ContractValue;
                reportFilteringVM.OpenedB2BLCValue = summaryData.BackToBackLCValue;

                // Set values
                ReportHelper.SetData("RequisitionReport", ds, reportFilteringVM);

                return Redirect("/Reports/ReportViewer.aspx");
            }

            ViewBag.Requisition = new SelectList(requisitionLogic.GetRequisitionDropDown(), "Value", "Text", reportFilteringVM.RequisitionID);

            return View();
        }

        public JsonResult GetRequisitionByJob(int jobID)
        {
            var data = requisitionLogic.GetRequisitionDropDown(jobID);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Job Finance Report
        public ActionResult JobFinanceReport()
        {
            ViewBag.JobList = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text");

            return View();
        }

        [HttpPost]
        public ActionResult JobFinanceReport(ReportFilteringViewModel reportFilteringVM)
        {
            if (ModelState.IsValid)
            {
                RequisitionReportViewModel summaryData = reportLogic.GetRequisitionSummary(reportFilteringVM.RequisitionID);
                //List<RequisitionPIViewModel> appliedForB2B = reportLogic.GetRequisitionAppliedForB2B(reportFilteringVM.RequisitionID);
                List<RequisitionPIViewModel> pendingForB2B = reportLogic.GetRequisitionPendingForB2B(reportFilteringVM.RequisitionID, summaryData.JobID);
                List<RequisitionPIViewModel> alreadyOpenedB2B = reportLogic.GetRequisitionAlreadyOpenedB2B(summaryData.JobID);

                //summaryData.AppliedForB2BLCValue = appliedForB2B.Sum(x => x.PIValue);
                summaryData.PendingB2BLCValue = pendingForB2B.Sum(x => x.PIValue);
                summaryData.BackToBackLCValue = alreadyOpenedB2B.Sum(x => x.PIValue);
                summaryData.TotalValue = summaryData.AppliedForB2BLCValue + summaryData.PendingB2BLCValue
                                            + summaryData.BackToBackLCValue;
                summaryData.AvailableBudget = summaryData.BudgetValue - summaryData.TotalValue;

                List<RequisitionReportViewModel> summaryDataList = new List<RequisitionReportViewModel>();
                summaryDataList.Add(summaryData);

                List<RequisitionPO> requisitionPO = reportLogic.GetRequisitionPO(summaryData.JobID);

                DataSet ds = new DataSet();
                ds.Tables.Add(summaryDataList.ConvertToDataTable());
                //ds.Tables.Add(appliedForB2B.ConvertToDataTable());
                //ds.Tables.Add(alreadyOpenedB2B.ConvertToDataTable());
                //ds.Tables.Add(pendingForB2B.ConvertToDataTable());
                //ds.Tables.Add(requisitionPO.ConvertToDataTable());

                reportFilteringVM.ContractValue = summaryData.ContractValue;
                reportFilteringVM.OpenedB2BLCValue = summaryData.BackToBackLCValue;

                // Set values
                ReportHelper.SetData("JobFinanceReport", ds, reportFilteringVM);

                return Redirect("/Reports/ReportViewer.aspx");
            }

            ViewBag.Requisition = new SelectList(requisitionLogic.GetRequisitionDropDown(), "Value", "Text", reportFilteringVM.RequisitionID);

            return View();
        }
        #endregion


        #region ProductionReport

        public ActionResult ProductionReport()
        {
            ViewBag.Buyer = new SelectList(buyerLogic.GetBuyerDropDown(), "Value", "Text");

            return View();
        }

        [HttpPost]
        public ActionResult ProductionReport(ReportFilteringViewModel reportFilteringVM)
        {
            if (ModelState.IsValid)
            {
                if (reportFilteringVM.Decision == 1)
                {
                    List<ProductionStatusReportViewModel> data = reportLogic.GetProductionStatusReport(reportFilteringVM.FromDate, reportFilteringVM.ToDate);

                    DataSet ds = new DataSet();
                    ds.Tables.Add(data.ConvertToDataTable());

                    ReportHelper.SetData("ProductionStatusReport", ds, reportFilteringVM);
                }
                else if (reportFilteringVM.Decision == 2)
                {
                    List<ProductionStatusFromat2ViewModel> data = reportLogic.GetProductionStatusReportFormat2(reportFilteringVM.BuyerID ?? 0, reportFilteringVM.FromDate, reportFilteringVM.ToDate);

                    DataSet ds = new DataSet();
                    ds.Tables.Add(data.ConvertToDataTable());

                    ReportHelper.SetData("ProductionStatusReport-2", ds, reportFilteringVM);
                }
                else if (reportFilteringVM.Decision == 3)
                {
                    List<ProductionStatusFromat3ViewModel> data = reportLogic.GetProductionStatusReportFormat3(reportFilteringVM.BuyerID ?? 0, reportFilteringVM.ToDate);

                    DataSet ds = new DataSet();
                    ds.Tables.Add(data.ConvertToDataTable());

                    ReportHelper.SetData("ProductionStatusReport-3", ds, reportFilteringVM);
                }
            }

            return Redirect("/Reports/ReportViewer.aspx");
        }

        public ActionResult ProductionCrisisReport()
        {
            return View();
        }

        [GridAction]
        public ActionResult GetProductionCrisisReport()
        {
            var results = reportLogic.GetProducttionCrisiReport();

            return View(new GridModel(results));
        }

        public ActionResult ProductionCrisisReportExport(string column, string orderBy, string filter)
        {
            var results = reportLogic.GetProducttionCrisiReport();
            var output = ReportHelper.ConvertToCSV<ProductionCrisisViewModel>(column, results);

            return File(output, "text/comma-separated-values", "ProductionCrisisReport.csv");
        }

        #endregion


        #region OrderStatus

        public ActionResult OrderStatus()
        {
            ViewBag.Buyer = new SelectList(buyerLogic.GetBuyerDropDown(), "Value", "Text");

            return View();
        }

        [HttpPost]
        public ActionResult OrderStatus(ReportFilteringViewModel reportFilteringVM)
        {
            //List<SewingReportViewModel> data = reportLogic.GetSewingReport(reportFilteringVM.FromDate, reportFilteringVM.ToDate, reportFilteringVM.Floor);

            //DataSet ds = new DataSet();
            //ds.Tables.Add(data.ConvertToDataTable());

            //ReportHelper.SetData("SewingReport", ds, reportFilteringVM);

            return Redirect("/Reports/ReportViewer.aspx");
        }

        #endregion


        #region Contract Paper

        public ActionResult ContractPaper()
        {
            ViewBag.Contract = new SelectList(jobLogic.GetContractDropDown(), "Value", "Text");

            return View();
        }

        [HttpPost]
        public ActionResult ContractPaper(ReportFilteringViewModel reportFilteringVM)
        {
            if (ModelState.IsValid)
            {
                List<ContractPaperViewModel> data = reportLogic.GetContractPaper(reportFilteringVM.JobID);

                DataSet ds = new DataSet();
                ds.Tables.Add(data.ConvertToDataTable());

                reportFilteringVM.ContractDate = reportFilteringVM.ContractDate ?? DateTime.Now;

                if (reportFilteringVM.AmendmentDate == null)
                {
                    reportFilteringVM.IsShow = false;
                    reportFilteringVM.AmendmentDate = reportFilteringVM.AmendmentDate ?? DateTime.Now;
                }
                else
                {
                    reportFilteringVM.IsShow = true;
                }

                decimal totalValue = 0;
                int totalQuantity = 0;
                string reportName = string.Empty;

                if (reportFilteringVM.Decision == 1)
                {
                    totalValue = data.Sum(x => x.OrderQuantity * x.FactoryCM) ?? 0;
                    reportName = "ContractPaper(VF-DMC-ASWL)";

                    var contract = jobLogic.GetJobByID(reportFilteringVM.JobID);

                    reportFilteringVM.ContractNo =
                        contract.ExtraContractNo != string.Empty && contract.ExtraContractNo != null
                            ? contract.ExtraContractNo : contract.ContractNo;
                }
                else if (reportFilteringVM.Decision == 2)
                {
                    totalValue = data.Sum(x => x.OrderQuantity * x.FOB);
                    reportName = "ContractPaper(VF-DMC)";
                }
                else if (reportFilteringVM.Decision == 3)
                {
                    totalValue = data.Sum(x => x.OrderQuantity * x.FOB);
                    reportName = "ContractPaper(GAP-DMC)";

                    reportFilteringVM.ShipmentDate = data.Max(x => x.ExitDate);
                    reportFilteringVM.ExpiryDate = reportFilteringVM.ShipmentDate.Value.AddDays(25);

                    var contract = jobLogic.GetJobByID(reportFilteringVM.JobID);
                    reportFilteringVM.JobNo = contract.JobNo;
                }
                else if (reportFilteringVM.Decision == 4)
                {
                    totalValue = data.Sum(x => x.OrderQuantity * x.FactoryCM) ?? 0;
                    reportName = "ContractPaper(GAP-DMC-ASWL)";

                    reportFilteringVM.ShipmentDate = data.Max(x => x.ExitDate);
                    reportFilteringVM.ExpiryDate = reportFilteringVM.ShipmentDate.Value.AddDays(25);

                    var contract = jobLogic.GetJobByID(reportFilteringVM.JobID);
                    reportFilteringVM.JobNo = contract.JobNo;

                    reportFilteringVM.ContractNo =
                        contract.ExtraContractNo != string.Empty && contract.ExtraContractNo != null
                            ? contract.ExtraContractNo : contract.ContractNo;
                }
                else if (reportFilteringVM.Decision == 5)
                {
                    totalValue = data.Sum(x => x.OrderQuantity * x.FOB);
                    reportName = "ContractPaper(GAP-ASWL)";

                    reportFilteringVM.ShipmentDate = data.Max(x => x.ExitDate);
                    reportFilteringVM.ExpiryDate = reportFilteringVM.ShipmentDate.Value.AddDays(25);

                    var contract = jobLogic.GetJobByID(reportFilteringVM.JobID);
                    reportFilteringVM.JobNo = contract.JobNo;
                }
                else if (reportFilteringVM.Decision == 6)
                {
                    totalValue = data.Sum(x => x.OrderQuantity * x.FOB);
                    reportName = "ContractPaper(Matalan-ASWL)";
                }
                else if (reportFilteringVM.Decision == 7)
                {
                    totalValue = data.Sum(x => x.OrderQuantity * x.FOB);
                    reportName = "ContractPaper(Matalan-DMC)";
                    
                }
                else if (reportFilteringVM.Decision == 8)
                {
                    totalValue = data.Sum(x => x.OrderQuantity * x.FOB);
                    reportName = "ContractPaper(PGI-ASWL)";
                }
                else if (reportFilteringVM.Decision == 9)
                {
                    totalValue = data.Sum(x => x.OrderQuantity * x.FOB);
                    reportName = "ContractPaper(PGI-DMC)";
                }
                totalQuantity = data.Sum(x => x.OrderQuantity);

                int changeQuantity = totalQuantity - reportFilteringVM.PreviousQuantity ?? 0;
                decimal changeValue = totalValue - reportFilteringVM.PreviousValue ?? 0;

                string changeText = "";

                if (changeQuantity > 0)
                {
                    changeText = "Garments Qty increased by " + changeQuantity + " pcs, now total Qty should read "
                                + totalQuantity + " pcs instead of " + reportFilteringVM.PreviousQuantity + " pcs";
                }
                else if (changeQuantity < 0)
                {
                    changeText = "Garments Qty decreased by " + changeQuantity + " pcs, now total Qty should read "
                                + totalQuantity + " pcs instead of " + reportFilteringVM.PreviousQuantity + " pcs";
                }

                if (changeValue > 0)
                {
                    changeText += ", Credit value increased by $" + changeValue.ToString("#.####") + ", now total value should read $"
                                + totalValue.ToString("#.####") + " only instead of $" + reportFilteringVM.PreviousValue;
                }
                else if (changeValue < 0)
                {
                    changeText += ", Credit value decreased by $" + changeValue.ToString("#.####") + ", now total value should read $"
                                + totalValue.ToString("#.####") + " only instead of $" + reportFilteringVM.PreviousValue;
                }

                if (!String.IsNullOrEmpty(reportFilteringVM.DestinationChangedText))
                {
                    changeText += Environment.NewLine + reportFilteringVM.DestinationChangedText.Trim();
                }

                reportFilteringVM.ChangeText = changeText;

                NumberHelper numberText = new NumberHelper();
                reportFilteringVM.AmountInWords = numberText.NumberToCurrencyText(totalValue);

                ReportHelper.SetData(reportName, ds, reportFilteringVM);

                return Redirect("/Reports/ReportViewer.aspx");
            }

            ViewBag.Contract = new SelectList(jobLogic.GetContractDropDown(), "Value", "Text", reportFilteringVM.JobID);

            return View();
        }

        #endregion


        #region Monthly Financial Report

        public ActionResult MonthlyFinancialReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MonthlyFinancialReport(ReportFilteringViewModel reportFilteringVM)
        {
            if (ModelState.IsValid)
            {
                List<MonthlyFinancialReportViewModel> data = reportLogic.GetMonthlyFinancialReport(reportFilteringVM.FromDate, reportFilteringVM.ToDate);

                DataSet ds = new DataSet();
                ds.Tables.Add(data.ConvertToDataTable());

                ReportHelper.SetData("MonthlyFinancialReport", ds, reportFilteringVM);
            }

            return Redirect("/Reports/ReportViewer.aspx");
        }

        #endregion


        #region BackToBackCrisis

        public ActionResult BackToBackCrisis()
        {
            return View();
        }

        [GridAction]
        public ActionResult GetBackToBackCrisis()
        {
            var results = reportLogic.GetBackToBackLCCrisisReport();

            return View(new GridModel(results));
        }

        public ActionResult BackToBackCrisisExport(string column, string orderBy, string filter)
        {
            var results = reportLogic.GetBackToBackLCCrisisReport();
            var output = ReportHelper.ConvertToCSV<BackToBackLCCrisisReportViewModel>(column, results);

            return File(output, "text/comma-separated-values", "BackToBackCrisis.csv");
        }

        #endregion


        #region Bank Forwarding Report

        public ActionResult BankForwardingReport()
        {
            var bankForwardingList = bankForwardingLogic.GetBankForwardingDropDown();
            ViewBag.BankForwardingList = new SelectList(bankForwardingList, "Value", "Text");

            return View();
        }

        [HttpPost]
        public ActionResult BankForwardingReport(ReportFilteringViewModel reportFilteringVM)
        {
            if (ModelState.IsValid)
            {
                List<BankForwardingReportViewModel> data = reportLogic.GetBankForwardingInvoiceInfo(reportFilteringVM.BankForwardingID);

                var bankForwarding = bankForwardingLogic.GetBankForwardingByID(reportFilteringVM.BankForwardingID);
                reportFilteringVM.BankForwardingNo = bankForwarding.BankForwardingNo;
                reportFilteringVM.BankForwardingDate = bankForwarding.BankForwardingDate;

                var job = jobLogic.GetJobByID(bankForwarding.JobID);
                reportFilteringVM.BankName = job.BankName;
                reportFilteringVM.BranchName = job.BankID == 2 ? "Uttara Branch" : "Gulshan Branch";

                DataSet ds = new DataSet();
                ds.Tables.Add(data.ConvertToDataTable());

                string rdlcName = reportFilteringVM.Decision == 1 ? "BankForwardingReport(ASWL)" : "BankForwardingReport(DMC)";

                ReportHelper.SetData(rdlcName, ds, reportFilteringVM);

                return Redirect("/Reports/ReportViewer.aspx");
            }
            var bankForwardingList = bankForwardingLogic.GetBankForwardingDropDown();
            ViewBag.BankForwardingList = new SelectList(bankForwardingList, "Value", "Text", reportFilteringVM.BankForwardingID);

            return View(reportFilteringVM);
        }

        #endregion


        #region PurchaseOrder Crisis

        public ActionResult PurchaseOrderCrisis()
        {
            return View();
        }

        [GridAction]
        public ActionResult GetPurchaseOrderCrisis(string orderBy, string filter, int page, int pageSize = 0)
        {
            var results = reportLogic.GetPurchaseOrderCrisis(CurrentUser.AccountID)
                .ToGridModel(page, pageSize, orderBy, null, filter)
                    .Data
                        .Cast<PurchaseOrderCrisisViewModel>().ToList();

            return View(new GridModel(results));
        }

        public ActionResult PurchaseOrderCrisisExport(string column, string orderBy, string filter)
        {
            var results = reportLogic.GetPurchaseOrderCrisis(CurrentUser.AccountID)
                .ToGridModel(0, 0, orderBy, null, filter)
                    .Data
                        .Cast<PurchaseOrderCrisisViewModel>().ToList();

            var output = ReportHelper.ConvertToCSV<PurchaseOrderCrisisViewModel>(column, results);

            return File(output, "text/comma-separated-values", "PurchaseOrderCrisis.csv");
        }

        #endregion


        #region AdvancedCM

        public ActionResult AdvancedCMReport()
        {
            IDictionary<int, int> yearList = new Dictionary<int, int>();
            yearList.Add(2012, 2012);
            yearList.Add(2013, 2013);
            yearList.Add(2014, 2014);
            yearList.Add(2015, 2015);
            yearList.Add(2016, 2016);
            yearList.Add(2017, 2017);
            yearList.Add(2018, 2018);
            yearList.Add(2019, 2019);
            yearList.Add(2020, 2020);

            ViewBag.Years = new SelectList(yearList, "Key", "Value");

            return View();
        }

        [HttpPost]
        public ActionResult AdvancedCMReport(ReportFilteringViewModel reportFilteringVM)
        {
            if (ModelState.IsValid)
            {
                List<AdvancedCMReportViewModel> data = reportLogic.GetAdvancedCmReport(reportFilteringVM.Year);

                DataSet ds = new DataSet();
                ds.Tables.Add(data.ConvertToDataTable());

                ReportHelper.SetData("AdvancedCMReport", ds, reportFilteringVM);
            }

            return Redirect("/Reports/ReportViewer.aspx");
        }

        #endregion


        #region Realization Report

        public ActionResult RealizationReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RealizationReport(ReportFilteringViewModel reportFilteringVM)
        {
            if (ModelState.IsValid)
            {
                List<RealizationReportViewModel> data
                    = reportLogic.GetRelizationReport(reportFilteringVM.Decision, reportFilteringVM.FromDate, reportFilteringVM.ToDate, reportFilteringVM.InBDT);

                DataSet ds = new DataSet();
                ds.Tables.Add(data.ConvertToDataTable());

                ReportHelper.SetData("RealizationReport", ds, reportFilteringVM);
            }

            return Redirect("/Reports/ReportViewer.aspx");
        }

        #endregion


        #region SubContract

        public ActionResult SubContractReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SubContractReport(ReportFilteringViewModel reportFilteringVM)
        {
            if (ModelState.IsValid)
            {
                List<SubContractReportViewModel> data = reportLogic.GetSubContractReport(reportFilteringVM.FromDate, reportFilteringVM.ToDate);

                DataSet ds = new DataSet();
                ds.Tables.Add(data.ConvertToDataTable());

                ReportHelper.SetData("SubContractReport", ds, reportFilteringVM);
            }

            return Redirect("/Reports/ReportViewer.aspx");
        }

        #endregion


        #region Purchase Requisition Report

        public ActionResult PurchaseRequisitionReport()
        {
            ViewBag.purchaseRequisitionList = new SelectList(purchaseRequisitionLogic.GetPurchaseRequisitionDropDown(), "Value", "Text");

            return View();
        }

        [HttpPost]
        public ActionResult PurchaseRequisitionReport(ReportFilteringViewModel reportFilteringVM)
        {
            if (ModelState.IsValid)
            {
                List<PurchaseRequisitionReportViewModel> data
                    = reportLogic.GetPuchaseRequisition(reportFilteringVM.PurchaseRequisitionID);

                DataSet ds = new DataSet();
                ds.Tables.Add(data.ConvertToDataTable());

                ReportHelper.SetData("PurchaseRequisitionReport", ds, reportFilteringVM);
            }

            return Redirect("/Reports/ReportViewer.aspx");
        }

        #endregion


        #region Invoice Bangla Report

        public ActionResult BanglaReport()
        {
            ViewBag.jobList = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text");

            return View();
        }

        [HttpPost]
        public ActionResult BanglaReport(ReportFilteringViewModel reportFilteringVM)
        {
            if (ModelState.IsValid)
            {
                //List<PurchaseRequisitionReportViewModel> data
                //    = reportLogic.GetPuchaseRequisition(reportFilteringVM.PurchaseRequisitionID);

                List<object> data = new List<object>();

                DataSet ds = new DataSet();
                ds.Tables.Add(data.ConvertToDataTable());

                ReportHelper.SetData("BanglaReport", ds, reportFilteringVM);
            }

            return Redirect("/Reports/ReportViewer.aspx");
        }

        #endregion


        #region Realization Crisis Report

        public ActionResult RealizationCrisisReport()
        {
            ViewBag.jobList = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text");

            return View();
        }

        [HttpPost]
        public ActionResult RealizationCrisisReport(ReportFilteringViewModel reportFilteringVM)
        {
            if (ModelState.IsValid)
            {
                List<RealizationCrisisReportViewModel> data = reportLogic.GetRealizationCrisis(reportFilteringVM.JobID);

                DataSet ds = new DataSet();
                ds.Tables.Add(data.ConvertToDataTable());

                ReportHelper.SetData("RealizationCrisisReport", ds, reportFilteringVM);
            }

            return Redirect("/Reports/ReportViewer.aspx");
        }

        #endregion


        #region Sewing & Shipment Summary Report

        public ActionResult SewingShipmentSummaryReport()
        {
            ViewBag.floorList = new SelectList(productionFloorLogic.GetFloorDropDown(), "ValueString", "Text");
            ViewBag.buyerList = new SelectList(buyerLogic.GetBuyerDropDown(), "Value", "Text");

            return View();
        }

        [HttpPost]
        public ActionResult SewingShipmentSummaryReport(ReportFilteringViewModel reportFilteringVM)
        {
            if (ModelState.IsValid)
            {
                List<SewingShipmentSummaryReportViewModel> data
                    = reportLogic
                    .GetSewingShipmentSummary(reportFilteringVM.BuyerID, reportFilteringVM.Floor, reportFilteringVM.FromDate, reportFilteringVM.ToDate);

                DataSet ds = new DataSet();
                ds.Tables.Add(data.ConvertToDataTable());

                ReportHelper.SetData("SewingShipmentSummaryReport", ds, reportFilteringVM);
            }

            return Redirect("/Reports/ReportViewer.aspx");
        }

        #endregion


        #region Import Status Report

        public ActionResult ImportStatusReport()
        {
            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text");

            return View();
        }


        [HttpPost]
        public ActionResult ImportStatusReport(ReportFilteringViewModel reportFilteringVM)
        {
            if (ModelState.IsValid)
            {
                List<ImportStatusViewModel> data = reportLogic.GetImportStatus(reportFilteringVM.JobID);

                DataSet ds = new DataSet();
                ds.Tables.Add(data.ConvertToDataTable());

                ReportHelper.SetData("ImportStatusReport", ds, reportFilteringVM);
            }

            return Redirect("/Reports/ReportViewer.aspx");
        }

        #endregion


        #region Post Sheet

        public ActionResult PostSheet()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PostSheetReport(ReportFilteringViewModel reportFiltaringVM)
        {
            List<PostSheetViewModel> data = reportLogic.GetPostSheetData(reportFiltaringVM);
            //List<PostSheetViewModel> data = new List<PostSheetViewModel>();       
            DataSet ds = new DataSet();
            ds.Tables.Add(data.ConvertToDataTable());

            ReportHelper.SetData("PostSheet", ds, reportFiltaringVM);

            return Redirect("/Reports/ReportViewer.aspx");
        }

        #endregion

        #region Inventory 

        public ActionResult InventoryReport()
        {
            ViewBag.JobList = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text");
            return View();

        }

        [HttpPost]
        public ActionResult InventoryReport(ReportFilteringViewModel reportFilteringVM)
        {
            List<InventoryViewModel> data = reportLogic.GetInventory(reportFilteringVM);

            DataSet ds = new DataSet();
            ds.Tables.Add(data.ConvertToDataTable());

            ReportHelper.SetData("InventoryReport", ds, reportFilteringVM);

            return Redirect("/Reports/ReportViewer.aspx");
        }
        public JsonResult GetPurchaseOrderByJob(int jobID)
        {
            var data = poLogic.GetPurchaseOrderDropDownByJob(jobID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        #endregion


        #region InventoryInTelerik 

        public ActionResult InventoryReportInTelerik()
        {
            return View();
        }


        [GridAction]
        public ActionResult GetInventoryReport()
        {
            var results = reportLogic.GetInventoryData();

            return View(new GridModel(results));
        }
        public ActionResult InventoryReportExport(string column, string orderBy, string filter)
        {
            var results = reportLogic.GetInventoryData();
            var output = ReportHelper.ConvertToCSV<InventoryViewModel>(column, results);

            return File(output, "text/comma-separated-values", "InventoryReport.csv");
        }


        #endregion


        #region Accounts 

        public ActionResult ExpenseBudgetReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ExpenseBudgetReport(ReportFilteringViewModel reportFilteringVM)
        {


            if (reportFilteringVM.Decision == 1)
            {
                List<ExpenseBudgetReportViewModel> data = reportLogic.GetExpenseBudgetReport(reportFilteringVM.FromDate, reportFilteringVM.ToDate, reportFilteringVM.FactoryMode);

                DataSet ds = new DataSet();
                ds.Tables.Add(data.ConvertToDataTable());
                ReportHelper.SetData("ExpenseBudgetReport", ds, reportFilteringVM);

                return Redirect("/Reports/ReportViewer.aspx");
            }
            else
            {
                List<ExpenseBudgetReportViewModel> data = reportLogic.GetExpenseBudgetSummaryReport(reportFilteringVM.FromDate, reportFilteringVM.ToDate, reportFilteringVM.FactoryMode);

                DataSet ds = new DataSet();
                ds.Tables.Add(data.ConvertToDataTable());
                ReportHelper.SetData("ExpenseSummaryReport", ds, reportFilteringVM);

                return Redirect("/Reports/ReportViewer.aspx");
            }


        }


        #endregion

        #region Commercial Invoice Report 

        public ActionResult CommercialInvoiceReport()
        {
            ViewBag.JobList = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text");
            return View();
        }

        [HttpPost]
        public ActionResult GetCommercialInvoiceReport(ReportFilteringViewModel reportFilteringVM)
        {
            List<CommercialInvoiceReportVM> summaryData = invoiceLogic.GetInvoiceSummaryByInvoiceId(reportFilteringVM.InvoiceId);
            List<CommercialInvoiceDescriptionVM> detailsData = invoiceLogic.GetDetailsDataByInvoiceId(reportFilteringVM.InvoiceId);

            //summaryTestData = new CommercialInvoiceReportVM { InvoiceNo = "70258khact", InvoiceDate =Convert.ToDateTime("2018/06/06"),ExportNo="EXP-21580",ExportDate= Convert.ToDateTime("2018/06/06"), Department="075",PurchaseOrderNo="P-235810",Terms="N/A",FOBNo="23658ASD",FOBDate= Convert.ToDateTime("2018/06/06"), Vessel="N/A",Voyage="N/A",DateOfExport=null,FOBPoint="Chattogram",Destination="Los Angles"};
            //summaryData.Add(summaryTestData);

            //detailsTestData = new CommercialInvoiceDescriptionVM { QTY = 777, PONo = "125863", TargetPO = "36528", StyleNo = "00025", DPCIItem = "2158dasd", Color = "BLACK", UnitPrice = 700 };
            //detailsData.Add(detailsTestData);

            //detailsTestData = new CommercialInvoiceDescriptionVM { QTY = 250, PONo = "365820", TargetPO = "1582300", StyleNo = "00024", DPCIItem = "ASDVG33", Color = "Yellow", UnitPrice = 250 };
            //detailsData.Add(detailsTestData);

            //detailsTestData = new CommercialInvoiceDescriptionVM { QTY = 250, PONo = "365820", TargetPO = "654789", StyleNo = "00024", DPCIItem = "88551", Color = "GREEN", UnitPrice = 300 };
            //detailsData.Add(detailsTestData);

            //detailsTestData = new CommercialInvoiceDescriptionVM { QTY = 250, PONo = "852147", TargetPO = "123654", StyleNo = "000009", DPCIItem = "99933", Color = "RED", UnitPrice = 350 };
            //detailsData.Add(detailsTestData);

            //detailsTestData = new CommercialInvoiceDescriptionVM { QTY = 250, PONo = "999965", TargetPO = "258963", StyleNo = "000001", DPCIItem = "22258", Color = "SOFT", UnitPrice = 400 };
            //detailsData.Add(detailsTestData);

            //detailsTestData = new CommercialInvoiceDescriptionVM { QTY = 250, PONo = "345344", TargetPO = "1582300", StyleNo = "000005", DPCIItem = "ASDVG33", Color = "BLUE", UnitPrice = 500 };
            //detailsData.Add(detailsTestData);

            //detailsTestData = new CommercialInvoiceDescriptionVM { QTY = 250, PONo = "345344", TargetPO = "1582300", StyleNo = "000005", DPCIItem = "ASDVG33", Color = "BLUE", UnitPrice = 500 };
            //detailsData.Add(detailsTestData);

            //detailsTestData = new CommercialInvoiceDescriptionVM { QTY = 250, PONo = "345344", TargetPO = "1582300", StyleNo = "000005", DPCIItem = "ASDVG33", Color = "BLUE", UnitPrice = 500 };
            //detailsData.Add(detailsTestData);

            DataSet ds = new DataSet();
            ds.Tables.Add(summaryData.ConvertToDataTable());
            ds.Tables.Add(detailsData.ConvertToDataTable());

            ReportHelper.SetData("CommercialInvoiceReport2", ds, reportFilteringVM);
            return Redirect("/Reports/ReportViewer.aspx");

        }

        public JsonResult GetInvoiceListByJobID(int jobID)
        {
            var data = invoiceLogic.GetInvoiceDropDown(jobID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Bill Of Entry Report 

        public ActionResult BillOfEntryReport()
        {
            ViewBag.JobList = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text");
            return View();
        }



        public ActionResult GetBillOfEntryReport(ReportFilteringViewModel reportFilteringVM)
        {
            List<BillOfEntryReportViewModel> data = reportLogic.GetBillOfEntryReport(reportFilteringVM.JobID);

            DataSet ds = new DataSet();
            ds.Tables.Add(data.ConvertToDataTable());
            ReportHelper.SetData("BillOfEntryReport", ds, reportFilteringVM);

            return Redirect("/Reports/ReportViewer.aspx");
        }


        #endregion


        #region Comparative Statement Report   

        public ActionResult ComparativeStatementReport()
        {
            ViewBag.JobList = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text");
            return View();
        }

        public ActionResult GetComparativeStatementReport(ReportFilteringViewModel reportFilteringVM)
        {
            List<ComparativeStatementViewModel> data = reportLogic.GetComparativeStatementReport(reportFilteringVM);

            DataSet ds = new DataSet();
            ds.Tables.Add(data.ConvertToDataTable());
            ReportHelper.SetData("ComparativeStatementReport", ds, reportFilteringVM);

            return Redirect("/Reports/ReportViewer.aspx");
        }


        #endregion

        #region BGME UD System Audit Report 

        public ActionResult BGMEUDSystemAuditReport()
        {
            return View();
        }



        public ActionResult GetBGMEUDSystemAuditReport(ReportFilteringViewModel reportFilteringVM)
        {
            List<BGMEUDSystemAuditReportVM> data = reportLogic.GetBGMEUDSystemAuditReport(reportFilteringVM);

            DataSet ds = new DataSet();
            ds.Tables.Add(data.ConvertToDataTable());
            ReportHelper.SetData("BGMEUDSystemAuditReport", ds, reportFilteringVM);

            return Redirect("/Reports/ReportViewer.aspx");
        }


        #endregion

        #region BGME Forwarding Report 

        public ActionResult BGMEForwardingReport()
        {
            return View();
        }



        public ActionResult GetBGMEForwardingReport(ReportFilteringViewModel reportFilteringVM)
        {
            //DataSet ds = new DataSet();            

            if (reportFilteringVM.Decision == 1)
            {
                ReportHelper.SetData("BGMEForwardingDMCReport", null, reportFilteringVM);
            }
            else
            {
                ReportHelper.SetData("BGMEForwardingASWLReport", null, reportFilteringVM);
            }

            return Redirect("/Reports/ReportViewer.aspx");
        }


        #endregion


        #region Expense Report 

        public ActionResult ExpenseReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetExpenseReport(ReportFilteringViewModel reportFilteringVM)
        {
            List<ExpenseBudgetReportViewModel> data = reportLogic.GetExpenseReportByRefNo(reportFilteringVM.ReferenceNo);

            DataSet ds = new DataSet();
            ds.Tables.Add(data.ConvertToDataTable());
            ReportHelper.SetData("ExpenseReport", ds, reportFilteringVM);

            return Redirect("/Reports/ReportViewer.aspx");



        }


        #endregion

        //#region Coupon Report 

        //public ActionResult CouponReport()
        //{
        //    List<DropDownListViewModel> buyerList = buyerLogic.GetBuyerDropDown();
        //    buyerList.Add(new DropDownListViewModel { Text = "All", Value = 0 });

        //    List <DropDownListViewModel> operationCategoryList = stdOperationLogic.GetAllOperationCategoriesDropDown();
        //    ViewBag.OperationCategoryList = new SelectList(operationCategoryList, "Value", "Text");

        //    ViewBag.Buyer = new SelectList(buyerList, "Value", "Text", 0);
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult GetCouponReport(ReportFilteringViewModel reportFilteringVM)
        //{
        //    List<CouponViewModel> data = reportLogic.GetCouponReport(reportFilteringVM);

        //    DataSet ds = new DataSet();
        //    ds.Tables.Add(data.ConvertToDataTable());
        //    ReportHelper.SetData("CouponReport", ds, reportFilteringVM);

        //    return Redirect("/Reports/ReportViewer.aspx");

        //}

        //public JsonResult GetStyleListByBuyerID(int buyerID)
        //{
        //    var data = styleLogic.GetStyleDropDownByBuyerID(1,buyerID);
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetPurchaseOrderListByStyleID(int styleID)
        //{
        //    var data = poLogic.GetPurchaseOrderDropDownByStyleID(styleID);
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetCuttingPlanListByPOID(int purchaseOrderID)
        //{
        //    var data = cuttingPlanLogic.GetCuttingPlanDropDownByPOID(purchaseOrderID);
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}


        //#endregion
    }
}