var app = angular.module('purchaseRequisition.controllers', ['purchaseRequisition.services'])

app.controller('purchaseRequisitionCtrl', ['$scope', '$filter', 'purchaseRequisitionService', 'toasterService', function ($scope, $filter, purchaseRequisitionService, toasterService) {

    $scope.init = function () {

        $scope.purchaseRequisition = {};
        $scope.purchaseRequisition.RequisitionDetails = [];

        purchaseRequisitionService.getAllUnit().then(function (res) {
            $scope.unitList = res.data;
        });

        purchaseRequisitionService.getAllDepartment().then(function (res) {
            $scope.departmentList = res.data;
        });

        purchaseRequisitionService.getAllSupplier().then(function (res) {
            $scope.supplierList = res.data;
        });

        angular.element(document).ready(function () {
            if ($scope.PurchaseRequisitionID) {
                $scope.getPurchaseRequisition($scope.PurchaseRequisitionID);
            }
        });
    }


    $scope.getPurchaseRequisition = function(purchaseRequisitionID) {
        purchaseRequisitionService.getPurchaseRequisition(purchaseRequisitionID).then(function (res) {
            $scope.purchaseRequisition = res.data;

            $scope.purchaseRequisition.RequisitionDate = $filter('date')($scope.purchaseRequisition.RequisitionDate.slice(6, -2), 'MM/dd/yyyy');
            $scope.purchaseRequisition.SetDate = $filter('date')($scope.purchaseRequisition.SetDate.slice(6, -2), 'MM/dd/yyyy');
        });
    }

    $scope.addItem = function () {

        var item = {
            PurchaseOrderRequsitionDetailsID: 0,
            ItemID: '',
            Quantity: '',
            UnitID: '',
            UnitPrice: ''
        };

        $scope.purchaseRequisition.RequisitionDetails.push(item);
    }

    $scope.deleteRow = function (index) {
        $scope.purchaseRequisition.RequisitionDetails.splice(index, 1);
    }

    $scope.getTotalAmount = function () {
        var sum = 0;
        for (var item in $scope.purchaseRequisition.RequisitionDetails) {
            sum += (parseFloat($scope.purchaseRequisition.RequisitionDetails[item].Quantity) || 0)
                    * (parseFloat($scope.purchaseRequisition.RequisitionDetails[item].UnitPrice) || 0);
        }
        return sum;
    }

    $scope.savePurchaseRequisition = function () {
        purchaseRequisitionService.savePurchaseRequisition($scope.purchaseRequisition).then(function (res) {
            toasterService.clear();

            if (res.data) {
                toasterService.success("Requisition : " + res.data + " ");
            } else {
                toasterService.error();
            }
            
        });
    }
}]);