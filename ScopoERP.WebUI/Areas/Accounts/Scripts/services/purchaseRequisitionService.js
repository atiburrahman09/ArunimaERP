var app = angular.module('purchaseRequisition.services', []);

app.service('purchaseRequisitionService', ['$http', function ($http) {
    
    this.getAllDepartment = function () {
        return $http({

            url: '/PurchaseRequisition/GetAllDepartment',
            method: 'GET'
        });
    }

    this.getAllSupplier = function () {
        return $http({
            url: '/PurchaseRequisition/GetAllSupplier',
            method: 'GET'
        });
    }

    this.getAllUnit = function () {
        return $http({
            url: '/PurchaseRequisition/GetAllUnit',
            method: 'GET'
        });
    }


    this.getPurchaseRequisition = function (purchaseRequisitionID) {
        return $http({
            url: '/PurchaseRequisition/GetPurchaseRequisition',
            method: 'GET',
            params: { purchaseRequisitionID: purchaseRequisitionID }
        });
    }


    this.savePurchaseRequisition = function (purchaseRequisition) {
        return $http.post('/PurchaseRequisition/Save', purchaseRequisition);
    }
}])