scopoAppControllers.controller('purchaseRequisitionCtrl', ['$scope', 'purchaseRequisitionService', 'alertify', '$window', function ($scope, purchaseRequisitionService, alertify, $window) {

    $scope.init = function (PurchaseRequisitionID) {

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
        if (PurchaseRequisitionID) {
            $scope.getPurchaseRequisition(PurchaseRequisitionID);
        }
        angular.element(document).ready(function () {
            if (PurchaseRequisitionID) {
                $scope.getPurchaseRequisition(PurchaseRequisitionID);
            }
        });
    }


    $scope.getPurchaseRequisition = function (purchaseRequisitionID) {
        purchaseRequisitionService.getPurchaseRequisition(purchaseRequisitionID).then(function (res) {            
            $scope.purchaseRequisition = formatDate([res.data], ['RequisitionDate', 'SetDate'])[0];
            console.log($scope.purchaseRequisition);
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
        if ($scope.purchaseRequisition.RequisitionDetails === 0) { alertify.error("Please add item details."); return; }
        if ($scope.requisitionForm.$valid) {
            purchaseRequisitionService.savePurchaseRequisition($scope.purchaseRequisition).then(function (res) {
                if (res.data) {
                    //alertify.success("Requisition : " + res.data + " ");
                    $window.location.href = '/MISC/PurchaseRequisition/Index';                    
                }
                $scope.requisitionForm.$setPristine();
                $scope.requisitionForm.$setUntouched();
                $scope.purchaseRequisition = {};

            });
        }
    }

    $scope.bindDate = function (id, model) {
        $scope[model][id] = $('#' + id).val();
    };

}]);