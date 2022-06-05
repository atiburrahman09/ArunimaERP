var app = angular.module('subContract.services', []);

app.service('subContractService', ['$http', function ($http) {

    this.getAllSubContract = function (purchaseOrderID) {
        return $http({
            url: '/SubContract/GetAllSubContract',
            method: 'GET',
            params: { purchaseOrderID: purchaseOrderID }
        });
    }

    this.getPurchaseOrderDetails = function (purchaseOrderID) {
        return $http({
            url: '/SubContract/GetPurchaseOrderDetails',
            method: 'GET',
            params: { purchaseOrderID: purchaseOrderID }
        });
    }

    this.getFactoryList = function () {
        return $http({
            url: '/SubContract/GetFactoryList',
            method: 'GET'
        });
    }

    this.save = function (purchaseOrderID, subContractList) {
        return $http.post('/SubContract/Save', { purchaseOrderID: purchaseOrderID, subContractVMList: subContractList });
    }
}])