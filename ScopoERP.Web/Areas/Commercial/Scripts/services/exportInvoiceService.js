scopoAppServices.service('exportInvoiceService', function ($http) {

    this.getExportInvoiceByJobID = function (jobID) {
        return $http.get("/Commercial/ExportInvoice/GetExportInvoiceByJobID/", { params: { jobID: jobID } });
    }

    this.getJobDropDown = function () {
        return $http.get("/Commercial/ExportInvoice/GetJobDropDown/");

    }

    //this.getExportInvoiceByID = function(id){
    //    return $http.get("/Commercial/ExportInvoice/GetExportInvoiceByID", { params: { id: id } });
    //}

    this.saveExportInvoice = function (exportInvoice) {
        return $http.post("/Commercial/ExportInvoice/SaveExportInvoice", exportInvoice);
    }

    this.isUniqueInvoiceNo = function (invoiceNo) {
        return $http.get("/Commercial/ExportInvoice/IsUniqueInvoiceNo", { params: { invoiceNo: invoiceNo } });
    }

    this.getPODropDownByJobID = function (jobID) {
        return $http.get("/Commercial/ExportInvoice/GetPurchaseOrderByJob", { params: { jobID: jobID } });
    }

    this.getShipmentByPurchaseOrder = function (purchaseOrderID) {
        return $http.get("/Commercial/ExportInvoice/GetShipmentByPurchaseOrder", { params: { purchaseOrderID: purchaseOrderID } });
    }

    this.getShipmentByInvoiceID = function (invoiceID) {
        return $http.get("/Commercial/ExportInvoice/GetShipmentByInvoiceID", { params: { invoiceID: invoiceID } });
    }

    //this.saveShipment = function (shipments) {
    //    return $http.post("/Commercial/ExportInvoice/SaveShipment", shipments);
    //}

    this.getAllExportInvoiceByJobID = function (jobID) {
        return $http.get("/Commercial/ExportInvoice/GetAllExportInvoiceByJobID", { params: { jobID: jobID } });
    }


})