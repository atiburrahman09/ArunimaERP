var app = angular.module('styleImage.services', []);

app.service('styleImageService', ['$http', function ($http) {

    this.getGallery = function (purchaseOrderID) {
        return $http({
            url: '/StyleImage/GetGallery',
            method: 'GET',
            params: { styleID: styleID }
        });
    }

    this.create = function (subContractVM) {
        return $http.post('/SubContract/Create', subContractVM);
    }

    this.delete = function (styleImageID) {
        return $http.post('/SubContract/Delete', styleImageID);
    }
}])