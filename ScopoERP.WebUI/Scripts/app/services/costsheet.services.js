var app = angular.module('costsheet.services', []);

app.service("costsheetSearchService", ['$http', function ($http) {
    this.getCostsheetNoDropDown = function (styleID) {
        return $http({
            url: '/MaterialManagement/Costsheet/GetCosheetNoDropDown',
            method: "GET",
            params: { styleID: styleID }
        });
    };
}]);

app.service('costsheetCrudService', ['$http', function ($http) {
    this.getUnitDropDown = function () {
        return $http({
            url: '/MaterialManagement/Costsheet/GetConsumptionUnitDropDown',
            method: "GET"
        });
    };

    this.getItemCategoryDropDown = function () {
        return $http({
            url: '/MaterialManagement/Costsheet/GetItemCategoryDropDown',
            method: "GET"
        });
    };

    this.getItemDropDown = function (itemCategoryID) {
        return $http({
            url: '/MaterialManagement/Costsheet/GetItemDropDown',
            params: { itemCategoryID: itemCategoryID },
            method: "GET"
        });
    };

    this.getCostsheet = function (costsheetNo) {
        return $http({
            url: '/MaterialManagement/Costsheet/GetCostsheet',
            method: "GET",
            params: { costsheetNo: costsheetNo }
        });
    };

    this.getItemByItemCode = function (itemCode) {
        return $http({
            url: '/MaterialManagement/Costsheet/GetItemInfo',
            method: "GET",
            params: { itemCode: itemCode }
        });
    };

    this.save = function (costsheetVM) {
        return $http.post('/MaterialManagement/Costsheet/Create', costsheetVM);
    };
}]);