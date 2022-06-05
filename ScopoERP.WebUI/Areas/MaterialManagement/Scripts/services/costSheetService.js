var app = angular.module('costSheet.services', []);

app.service('costSheetService', ['$http', function ($http) {

    this.getConsumptionUnit = function () {
        return $http({
            url: '/CostSheets/GetConsumptionUnit',
            method: 'GET'
        });
    }


    this.getItemCategory = function () {
        return $http({
            url: '/CostSheets/GetItemCategory',
            method: 'GET'
        });
    }


    this.getCoshSheetDetails = function (costSheetNo) {
        return $http({
            url: '/CostSheets/GetCostSheetDetails',
            method: 'GET',
            params: { costSheetNo: costSheetNo }
        });
    }


    this.save = function (costSheetList) {
        return $http.post('/CostSheets/Save', costSheetList);
    }
}])