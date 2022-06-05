scopoAppControllers.controller('blCtrl', ['$scope', 'blService', function ($scope, blService) {

    $scope.totalInvoiceQty = 0;
    $scope.isBLDetailsView = false;
    $scope.blSummary = false;

    $scope.dt = {
        blDate: false,
        invoiceDate: false,
        originDocDate: false,
        copyDocDate: false,
        goodsDelDate: false,
        cnfDate: false,
        inhouseDate: false,
        acceptanceDate: false,
        billDate: false,
    }

    $scope.pickDate = function (val) {
        $scope.dt[val] = true;
    }


    $scope.init = function () {        
        $scope.backToBackLCList = [];
        $scope.blSummaryList = [];
        $scope.piDropDownList = [];
        $scope.blList = [];
        $scope.itemDropDownList = [];
        $scope.blDetailsList = [];
        $scope.blInstance = {};
        
        blService.getJobDropDown().then(function (res) {
            $scope.jobList = res.data;
        });
    }
    
    $scope.onJobChange = function (selectedJob) {
        getBackToBackLCByJob(selectedJob);
        getBLByJob(selectedJob);
    }

    function getBackToBackLCByJob(jobId) {
        blService.getBackToBackLCDropDownByJobID(jobId).then(function (res) {
            $scope.backToBackLCList = res.data;
        });
    }

    function getBLByJob(jobId) {
        blService.getBLByJobID(jobId).then(function (res) {                 
            $scope.blSummaryList = formatDate(res.data, []);
            $scope.blSummaryList = formatDate(res.data, ['DocumentSentToCNF']);
            
        });
    }

    $scope.onBackToBackChange = function (selectedB2B) {
        if (!selectedB2B) {
            return;
        }
            
        $scope.resetForm();

        blService.getBLAndPIDropDownByBackToBackLCID(selectedB2B).then(function (res) {
            $scope.blList = res.data.BL;
            $scope.piDropDownList = res.data.PI;
        });
    }

    $scope.onBLChange = function (selectedBL) {
        if (!selectedBL) {
            return;
        }
        blService.getBLByID(selectedBL).then(function (res) {            
            var data = formatDate([res.data], ['DocumentSentToCNF']);
            data = formatDate(data, []);
            $scope.blInstance = data[0];
        });
    }   

    $scope.toggleView = function () {
        $scope.blSummary = !$scope.blSummary;
    }
    
    $scope.saveBL = function () {      
        if (!$scope.selectedB2B) {
            alertify.alert('Select Back To Back LC');
            return;
        }

        if (!$scope.blInstance.BackToBackLCID) {
            $scope.blInstance.BackToBackLCID   = $scope.selectedB2B;
        }

        if (!$scope.isBLDetailsView) {
            if ($scope.blForm.$valid) {                
                blService.saveBL($scope.blInstance).then(function (res) {
                    if (res.data.Success) {
                        //$scope.getBLAndPIDropDownByBackToBackLCID();
                        
                        alertify.success(res.data.Success);
                    }
                    else {
                        alertify.error(res.data.Error);
                    }
                });
            }
        } else {
            if ($scope.blDetailsList.length > 0) {
                blService.saveBLDetails($scope.blDetailsList).then(function (res) {
                    if (res.data.Success) {
                        alertify.success(res.data.Success);
                    } else {
                        alertify.error(res.data.Error);
                    }
                });
            }
        }
        $scope.resetForm();
    }

    $scope.resetForm = function () {
        $scope.blForm.$setPristine();
        $scope.blForm.$setUntouched();
        $scope.blInstance = {};
    }

    $scope.getItemDropDownByPIID = function (piID) {
        blService.getItemDropDownByPIID(piID).then(function (res) {
            $scope.itemDropDownList = res.data;
        });
    }

    $scope.onItemChange = function (blID, piID, itemID) {
        if (!itemID) {
            $scope.blDetailsList = [];
            return;
        }
        blService.getBLDetails(blID, piID, itemID).then(function (res) {
            $scope.blDetailsList = res.data;
            $scope.totalInvoiceQty = 0;
            $scope.getTotalInvoiceQty();
        });
    }
    
    $scope.detectFormToSave = function (isBLDetailsView) {
        $scope.isBLDetailsView = isBLDetailsView;
    }

    $scope.getTotalInvoiceQty = function () {
        var x = 0;
        for (var i in $scope.blDetailsList) {
            x += parseFloat($scope.blDetailsList[i].InvoiceQuantity || 0);
        }

        $scope.totalInvoiceQty = x;
    }
    
    $scope.exportExcel = function () {
        alasql('SELECT BackToBackLCNo, BLNo, BLDate, MaturityDate,InvoiceNo,InvoiceDate,DocumentSentToCNF,GoodsDeliveryDateByCNF,GoodsInHouseDate,AcceptanceValue,AcceptanceDate INTO XLSX("' + new Date().toLocaleString() + ' BLReport.xlsx",{sheetid:"Data", headers:true}) FROM ?', [$scope.blSummaryList]);
    };
}]);