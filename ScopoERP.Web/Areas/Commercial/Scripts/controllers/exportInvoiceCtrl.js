scopoAppControllers.controller('exportInvoiceCtrl', ['$scope', 'exportInvoiceService', function ($scope, exportInvoiceService) {
    $scope.exportInvoiceList = [];
    $scope.jobList = [];
    $scope.exportInvoice = {};
    $scope.PODropDownList = [];
    $scope.reportList = [];
    
    $scope.shipmentList = [];
    $scope.forms = {};
    $scope.exportInvoiceReport = false;

    $scope.init = function () {       
        exportInvoiceService.getJobDropDown().then(function (res) {
            $scope.jobList = res.data;
        })
    }

    $scope.onJobChange = function (selectedJob) {
        if (selectedJob != null) {
            getExportInvoiceByJobID(selectedJob);
            getPODropDownByJobID(selectedJob);
            getAllExportInvoiceByJobID(selectedJob);
        }        
    }

    $scope.selectExportInvoice = function (invoice) {
        $scope.exportInvoice = JSON.parse(invoice);

        getShipmentByInvoiceID($scope.exportInvoice.InvoiceId);
    }
 
    $scope.addShipment = function (po) {
        if (!po) {
            return;
        }
        
        var purchaseOrderId = JSON.parse(po).Value;
        exportInvoiceService.getShipmentByPurchaseOrder(purchaseOrderId).then(function (res) {
            res.data.ChalanDate = new Date(parseInt(res.data.ChalanDate.substr(6)));
            res.data.SetupDate = new Date(parseInt(res.data.SetupDate.substr(6)));
            $scope.shipmentList.push(res.data);         
        });
    }

    $scope.removeShipment = function (sh, shipmentIndex) {
        $scope.shipmentList.splice(shipmentIndex, 1);        
        $scope.PODropDownList.push({ Text: sh.PONo, Value: sh.PurchaseOrderID });
    }
    
    $scope.saveExportInvoice = function (isShipment) {
        if ($scope.forms.exportInvoiceForm.$valid) {
            if ($scope.selectedJob == null) {
                alertify.error("Please select a Job first!");
                return;
            }
            
            $scope.exportInvoice.ShipmentList = $scope.shipmentList;
            exportInvoiceService.saveExportInvoice($scope.exportInvoice).then(function (res) {
                if (res.data.Error) {
                    alertify.alert(res.data.Error);
                }
                else if (res.data.Success) {
                    alertify.success(res.data.Success);

                    $scope.resetForm();
                    getPODropDownByJobID($scope.selectedJob);
                    getExportInvoiceByJobID($scope.selectedJob);
                    getAllExportInvoiceByJobID($scope.selectedJob);
                } else {
                    alertify.error("Unknown Error occured!");
                }
            });
        }
    }

    $scope.resetForm = function () {
        $scope.exportInvoice = { JobID: $scope.selectedJob };
        $scope.shipmentList = [];
        
        if ($scope.forms.exportInvoiceForm != undefined) {
            $scope.forms.exportInvoiceForm.$setPristine();
            $scope.forms.exportInvoiceForm.$setUntouched();
        }
    }

    $scope.toggleView = function () {
        $scope.exportInvoiceReport = !$scope.exportInvoiceReport;        
    }

    $scope.fixedHeaderTable = function () {
        $('.table-fixed-header').fixedHeaderTable();
    }

    function getPODropDownByJobID(jobID) {
        exportInvoiceService.getPODropDownByJobID(jobID).then(function (res) {
            $scope.PODropDownList = res.data;
        })
    }

    function getExportInvoiceByJobID(jobID) {          
        exportInvoiceService.getExportInvoiceByJobID(jobID).then(function (res) {
            $scope.exportInvoiceList = formatDate(res.data, []);
            $scope.resetForm();
        });
    }

    function getShipmentByInvoiceID(invoiceID) {
        exportInvoiceService.getShipmentByInvoiceID(invoiceID).then(function (res) {
            $scope.shipmentList = formatDate(res.data, []);
        });
    }

    function getAllExportInvoiceByJobID(jobID) {
        exportInvoiceService.getAllExportInvoiceByJobID(jobID).then(function (res) {
            $scope.reportList = formatDate(res.data, []);
        });
    }
}])