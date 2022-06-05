scopoAppServices.service('realizationService', function ($http) {
    
    this.getAllAccountType = function () {
        return $http({
            url: '/Finance/Realization/GetAllAccountType',
            method: 'GET'
        });
    }

    this.getAllFDBPNo = function () {
        return $http({
            url: '/Finance/Realization/GetAllFDBPNo',
            method: 'GET'
        });
    }

    this.getAllRealization = function (bankForwardingID, accountType) {
        return $http({
            url: '/Finance/Realization/GetAllRealization',
            method: 'GET',
            params: { bankForwardingID: bankForwardingID, accountType: accountType }
        });
    }

    this.saveRealization = function (realizationList) {
        return $http.post('/Finance/Realization/Save', realizationList);
    }
})