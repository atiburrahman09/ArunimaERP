var app = angular.module('realization.services', []);

app.service('realizationService', ['$http', function ($http) {
    
    this.getAllAccountType = function () {
        return $http({
            url: '/Realization/GetAllAccountType',
            method: 'GET'
        });
    }

    this.getAllFDBPNo = function () {
        return $http({
            url: '/Realization/GetAllFDBPNo',
            method: 'GET'
        });
    }

    this.getAllRealization = function (bankForwardingID, accountType) {
        return $http({
            url: '/Realization/GetAllRealization',
            method: 'GET',
            params: { bankForwardingID: bankForwardingID, accountType: accountType }
        });
    }

    this.saveRealization = function (realizationList) {
        return $http.post('/Realization/Save', realizationList);
    }
}])