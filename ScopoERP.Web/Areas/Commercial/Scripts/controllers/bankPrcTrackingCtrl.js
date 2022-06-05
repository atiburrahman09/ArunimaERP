scopoAppControllers.controller('bankPrcTrackingCtrl', function ($scope, bankPrcTrackingService, $http) {

    $scope.init = function () {
        $scope.allBankPRC = [];
        $scope.jobList = [];
        $scope.invoiceList = [];
        $scope.bankPRC = { InvoiceList: [] };        
        getPrcTrackings();
    }
    
    $scope.getInvoicesAsync = function(val) {
        return bankPrcTrackingService.getInvoices(val).then(function (res) {
            return formatDate(res.data, []);
        }, function (err) {
            // handle error
        });
    }
    
    function getPrcTrackings() {
        bankPrcTrackingService.getPrcTrackings().then(function (res) {

            var data = formatDate(res.data, ["CreatedOn", "UpdatedOn"]);
            $scope.allBankPRC = formatDate(data, []);
        }, function (err) {
            // handle error
        });
    }

    $scope.selectBankPRC = function (selectedBPRC) {
        if (selectedBPRC) {
            $scope.bankPRC = JSON.parse(selectedBPRC);
            bankPrcTrackingService.getInvoiceListByBankPrcTrackingID($scope.bankPRC.Id).then(function (res) {
                $scope.bankPRC.InvoiceList = formatDate(res.data, []);
            });
        } else {
            $scope.bankPRC = {};
        }
    }

    $scope.saveBankPRC = function () {

        if ($scope.bankPRCForm.$valid) {
            bankPrcTrackingService.saveBankPRC($scope.bankPRC).then(function (res) {
                if (res.data) {
                    alertify.success("BankPRCTracking " + res.data + " - Successfully Saved");
                    $scope.resetForm();
                    getPrcTrackings();
                } else {
                    alertify.error("Failed! Please try again!");
                }
            });
        }
    }

    $scope.addToGrid = function (selectedInvoice) {
        if (selectedInvoice == null || selectedInvoice == undefined) {
            alertify.error("Please select invoice.");
            return;
        }

        if (!$scope.bankPRC.InvoiceList)
            $scope.bankPRC.InvoiceList = [];

        $scope.bankPRC.InvoiceList.push(selectedInvoice);
        $scope.selectedInvoice = null;
    }

    $scope.removeFromGrid = function (index) {
        $scope.invoiceList.push($scope.bankPRC.InvoiceList[index]);
        $scope.bankPRC.InvoiceList.splice(index, 1);
    }

    $scope.resetForm = function () {
        $scope.bankPRC = {};

        $scope.selectedBPRC = '';

        $scope.bankPRCForm.$setPristine();
        $scope.bankPRCForm.$setUntouched();
    };

    $scope.bindDate = function bindDate(id, model) {
        $scope[model][id] = $('#' + id).val();
    };
});