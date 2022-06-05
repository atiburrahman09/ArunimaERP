scopoAppControllers.controller('bankForwardingCtrl', ['$scope', 'bankForwardingService', function ($scope, bankForwardingService, CommonFunc) {
    
    $scope.init = function () {
        $scope.allBankFW = [];
        $scope.jobList = [];
        $scope.invoiceList = [];
        $scope.bankFW = { InvoiceList: [] };

        bankForwardingService.getJobDropDown().then(function (res) {
            $scope.jobList = res.data;
        });     
    }
    
    $scope.onJobChange = function (jobID) {
        bankForwardingService.getAllBankForwardingByJobID(jobID).then(function (res) {
            $scope.allBankFW = data = formatDate(res.data, []);
        });

        bankForwardingService.getInvoiceDropDownByJob(jobID).then(function (res) {
            $scope.invoiceList = formatDate(res.data, []);
        });
    }

    $scope.selectBankFW = function (selectedBF) {
        if (selectedBF) {
            $scope.bankFW = JSON.parse(selectedBF);

            bankForwardingService.getInvoiceListByBankForwardingID($scope.bankFW.BankForwardingID).then(function (res) {
                $scope.bankFW.InvoiceList = res.data;
            });
        } else {
            $scope.bankFW = {};
        }
    }
    
    $scope.saveBankForwarding = function () {
        if (!$scope.selectedJob) {
            alertify.error("Please select a job first.");
            return;
        } else {
            $scope.bankFW.JobID = $scope.selectedJob;
        }
        if ($scope.bankForwardingForm.$valid) {
            bankForwardingService.saveBankForwarding($scope.bankFW).then(function (res) {
                if (res.data) {
                    alertify.success("BankForwarding "+ res.data + " - Successfully Saved");
                    $scope.resetForm();
                    $scope.onJobChange($scope.selectedJob);
                } else {
                    alertify.error("Failed! Please try again!");
                }
            });
        }
    }

    $scope.addToGrid = function (selectedInvoice) {
        if (selectedInvoice == null || selectedInvoice == undefined)
        {
            alertify.error("Please select invoice.");
            return;
        }

        if (!$scope.bankFW.InvoiceList)
            $scope.bankFW.InvoiceList = [];
        
        $scope.bankFW.InvoiceList.push(selectedInvoice);
    }

    $scope.removeFromGrid = function (index) {
        $scope.invoiceList.push($scope.bankFW.InvoiceList[index]);
        $scope.bankFW.InvoiceList.splice(index, 1);
    }

    $scope.resetForm = function () {
        $scope.bankFW = {};

        $scope.selectedBF = '';

        $scope.bankForwardingForm.$setPristine();
        $scope.bankForwardingForm.$setUntouched();
    };
    
    $scope.bindDate = function bindDate(id, model) {
        $scope[model][id] = $('#' + id).val();
    };
}]);