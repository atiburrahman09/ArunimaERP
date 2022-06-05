using Microsoft.Reporting.WebForms;
using ScopoERP.Reports.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;

namespace ScopoERP.WebUI.Reports
{
    public partial class ReportViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["RdlcName"] != null)
                {
                    string rdlcName = Session["RdlcName"].ToString();

                    ScopoReportViewer.LocalReport.ReportPath = Server.MapPath("../Reports/" + rdlcName + ".rdlc");

                    SetReportDataSource();
                    SetReportParameter();
                }
            }
        }

        private void SetReportDataSource()
        {
            DataSet ds = (DataSet)Session["DataSet"];

            int totalDataSources = ScopoReportViewer.LocalReport.GetDataSourceNames().Count;

            for (int i = 0; i < totalDataSources; i++)
            {
                ScopoReportViewer.LocalReport.DataSources.Add(
                    new ReportDataSource(ScopoReportViewer.LocalReport.GetDataSourceNames()[i], ds.Tables[i]));
            }
        }

        private void SetReportParameter()
        {
            ReportFilteringViewModel reportFilteringVM = (ReportFilteringViewModel)Session["ReportFilteringViewModel"];

            int totalParameter = ScopoReportViewer.LocalReport.GetParameters().Count;
            string parameterName = string.Empty;

            List<ReportParameter> paramList = new List<ReportParameter>();

            for (int i = 0; i < totalParameter; i++)
            {
                parameterName = ScopoReportViewer.LocalReport.GetParameters()[i].Name;

                paramList.Add(new ReportParameter(parameterName, reportFilteringVM.GetPropertyValue(parameterName)));
            }

            ScopoReportViewer.LocalReport.SetParameters(paramList);
        }
    }
}