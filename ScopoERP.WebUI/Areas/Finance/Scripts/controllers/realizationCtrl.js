var app = angular.module('realization.controllers', ['realization.services'])

app.controller('realizationCtrl', ['$scope', '$filter', 'realizationService', 'toasterService', function ($scope, $filter, realizationService, toasterService) {

    $scope.init = function () {
        realizationService.getAllAccountType().then(function (res) {
            $scope.AccountTypeList = res.data;
        });

        realizationService.getAllFDBPNo().then(function (res) {
            $scope.FDBPNoList = res.data;
        });
    }

    $scope.getAllRealization = function () {
        realizationService.getAllRealization($scope.BankForwardingID, $scope.AccountType).then(function (res) {
            $scope.realizationList = res.data;
            $scope.InvoiceValue = res.data[0].InvoiceValue;
            $scope.CurrencyRate = res.data[0].CurrencyRate;

            if ($scope.realizationList[0].RealizationDate) {
                for (i in $scope.realizationList) {
                    if ($scope.realizationList[i].RealizationDate && $scope.realizationList[i].RealizationDate != null) {
                        $scope.realizationList[i].RealizationDate = $filter('date')($scope.realizationList[i].RealizationDate.slice(6, -2), 'MM/dd/yyyy');
                    }
                }
            }

            $scope.RealizationDate = res.data[0].RealizationDate;
        });
    }

    $scope.getTotalAmount = function () {
        var sum = 0;
        for (var item in $scope.realizationList) {
            sum += parseFloat($scope.realizationList[item].Amount) || 0;
        }
        return sum;
    }

    $scope.getBalanceAmount = function () {
        return $scope.InvoiceValue - $scope.getTotalAmount();
    }

    $scope.saveRealization = function () {

        $scope.realizationList[0].RealizationDate = $scope.RealizationDate;
        $scope.realizationList[0].CurrencyRate = $scope.CurrencyRate;

        console.log($scope.realizationList[0]);

        realizationService.saveRealization($scope.realizationList).then(function (res) {
            toasterService.clear();
            toasterService.success("Successfully saved!");
        });
    }
}]);