<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViewer.aspx.cs" Inherits="ScopoERP.WebUI.Reports.ReportViewer" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="ThemeBucket">
    <link rel="shortcut icon" href="images/favicon.png">
    <title>Welcome To ArunimaERP</title>

    <link href="../Content/Tenplate/bs3/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/Tenplate/css/bootstrap-reset.css" rel="stylesheet">
    <link href="../Content/Tenplate/font-awesome/css/font-awesome.css" rel="stylesheet" />
    <link href="../Content/Tenplate/css/style.css" rel="stylesheet">
    <link href="../Content/Tenplate/css/style-responsive.css" rel="stylesheet" />


</head>
<body>
    <section id="container">
        <!--header start-->
        <header class="header fixed-top clearfix">
            <!--logo start-->
            <div class="brand">

                <a href="/Home/Index" class="logo">
                    <img src="../Content/Tenplate/images/logo.png" alt="">
                </a>
                <div class="sidebar-toggle-box">
                    <div class="fa fa-bars"></div>
                </div>
            </div>
            <!--logo end-->

        </header>
        <!--header end-->

        <!--sidebar start-->
        
        <!--sidebar end-->

        <!--main content start-->
        <section id="main-content">
            <section class="wrapper">
                <!-- page start-->

                <div class="row">

                    <form id="form1" runat="server">
                        <asp:ScriptManager ID="ScopoScriptManager" runat="server"></asp:ScriptManager>
                        <div>
                            <rsweb:ReportViewer ID="ScopoReportViewer" runat="server" Height="768px" Width="100%"></rsweb:ReportViewer>
                        </div>
                    </form>
                </div>

                <!-- page end-->
            </section>
        </section>
        <!--main content end-->

    </section>
</body>
</html>
