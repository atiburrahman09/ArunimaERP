var app = angular.module('subContract.controllers', ['subContract.services'])

app.controller('subContractCtrl', ['$scope', '$filter', 'subContractService', 'toasterService', function ($scope, $filter, subContractService, toasterService) {

    $scope.init = function () {
        angular.element(document).ready(function () {
            $scope.getPurchaseOrderDetails($scope.PurchaseOrderID);
            $scope.getAllsubContract($scope.PurchaseOrderID);
            $scope.getFactoryList();
        });
    }

    $scope.getAllsubContract = function (purchaseOrderID) {
        subContractService.getAllSubContract(purchaseOrderID).then(function (res) {
            $scope.subContractList = res.data;

            if ($scope.subContractList) {
                for (i in $scope.subContractList) {
                    $scope.subContractList[i].SubContractExitDate = $filter('date')($scope.subContractList[i].SubContractExitDate.slice(6, -2), 'MM/dd/yyyy');
                    
                }
            }
        });
    }

    $scope.getPurchaseOrderDetails = function (purchaseOrderID) {
        subContractService.getPurchaseOrderDetails(purchaseOrderID).then(function (res) {
            $scope.puchaseOrderDetails = res.data;
            $scope.puchaseOrderDetails.ExitDate = $filter('date')($scope.puchaseOrderDetails.ExitDate.slice(6, -2), 'MM/dd/yyyy');
        });
    }

    $scope.getFactoryList = function () {
        subContractService.getFactoryList().then(function (res) {
            $scope.factoryList = res.data;
        });
    }


    $scope.deleteRow = function (index) {
        //var item = $scope.subContractList[index];

        $scope.subContractList.splice(index, 1);
    }


    $scope.addNew = function () {
        var row = {
            SubContractID: 0,
            SubContractNo: '',
            FactoryID: '',
            PurchaseOrderID: $scope.PurchaseOrderID,
            SubContractQuantity: '',
            SubContractExitDate: '',
            SubContractRate: '',
            CommecialPercentage: '',
            Remarks: ''
        }

        $scope.subContractList.push(row);
    }

    $scope.save = function () {
        subContractService.save($scope.PurchaseOrderID, $scope.subContractList).then(function (res) {
            if (res.data ==  'true') {
                toasterService.clear();
                toasterService.success("Successfully saved!");
            }
        });
    }
}]);