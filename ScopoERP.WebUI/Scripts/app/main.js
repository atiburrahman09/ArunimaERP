var globalModule = angular.module("globalModule", ["ngAnimate", "toaster"]);
globalModule.service("toasterService", ['toaster', function (toaster) {
    this.wait = function () {
        toaster.pop('wait', "Please Wait...");
    };

    this.success = function (item) {
        toaster.pop('success', item+" is saved successfully", null, 2000);
    };

    this.customSuccess = function (msg) {
        toaster.pop('success', msg, null, 50000);
    }

    this.error = function () {
        toaster.pop('error', "Sorry, Some error has been occured", null, 2000);
    };

    this.customError = function (msg) {
        toaster.pop('error', msg, null, 2000);
    };

    this.warning = function (message) {
        toaster.pop('warning', message, null, 3000);
    }

    this.clear = function () {
        toaster.clear();
    };
}]);

globalModule.service('dropdownService', ['$http', function ($http) {
    this.getStyleDropDown = function () {
        return $http({
            url: '/MaterialManagement/Booking/GetMultipleStyleDropDown',
            method: 'GET'
        }); 
    };

    this.getSingleStyleDropDown = function () {
        return $http({
            url: '/MaterialManagement/Worksheet/GetStyleDropDown',
            method: 'GET'
        });
    };

    this.getPIDropDown = function () {
        return $http({
            url: '/MaterialManagement/Booking/GetPIDropDown',
            method: 'GET'
        });
    };

    this.getMultipleCostsheetDropDownByMultileStyles = function (styleIDs) {
        return $http({
            url: '/MaterialManagement/Costsheet/GetMultipleCostsheetByMultileStyle',
            method: 'GET',
            params: { styleIDs: styleIDs }
        });
    };

    this.getSingleCostsheetDropDownBySingleStyle = function (styleID) {
        return $http({
            url: '/MaterialManagement/Worksheet/GetSingleCostsheetDropDownBySingleStyle',
            method: 'GET',
            params: { styleID: styleID }
        });
    };

    this.getMultiplePODropDownByMultipleStyles = function (styleIDs) {
        return $http({
            url: '/MaterialManagement/Booking/GetMultiplePODropDownByMultipleStyles',
            method: 'GET',
            params: { styleIDs: styleIDs }
        });
    };

    this.getSinglePODropDownBySingleStyle = function (styleID) {
        return $http({
            url: '/MaterialManagement/Worksheet/GetSinglePODropDownBySingleStyle',
            method: 'GET',
            params: { styleID: styleID }
        });
    };

    this.getItemByMultipleCostsheets = function (costsheetNos) {
        return $http({
            url: '/MaterialManagement/Booking/GetItemByMultipleCostsheets',
            method: 'GET',
            params: { costsheetNos: costsheetNos }
        });
    };

    this.getItemByMultiplePO = function (poIDs) {
        return $http({
            url: '/MaterialManagement/Booking/GetItemByMultiplePO',
            method: 'GET',
            params: { poIDs: poIDs }
        });
    };

    this.getItemBySingleCostsheet = function (costsheetNo) {
        return $http({
            url: '/MaterialManagement/Worksheet/GetItemBySingleCostsheet',
            method: 'GET',
            params: { costsheetNo: costsheetNo }
        });
    };

    this.getFormulaDropDown = function () {
        return $http({
            url: '/MaterialManagement/Worksheet/getFormulaDropDown',
            method: 'GET'
        });
    };

    this.getSupplierDropDown = function () {
        return $http({
            url: '/MaterialManagement/Booking/GetSupplierDropDown',
            method: 'GET'
        });
    };

    this.getReferenceNoDropDown = function () {
        return $http({
            url: '/MaterialManagement/Booking/GetReferenceNoDropDown',
            method: 'GET'
        });
    };

    this.getPODropDown = function () {
        return $http({
            url: '/OrderManagement/SizeColor/GetPODropDown',
            method: 'GET'
        });
    };

}]);