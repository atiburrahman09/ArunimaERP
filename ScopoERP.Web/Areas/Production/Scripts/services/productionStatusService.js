scopoAppServices.service('productionStatusService', function ($http) {
    this.getAllBuyer = function () {
        return $http.get("/Production/ProductionStatus/GetAllBuyer/");
    }

    this.getAllStyle = function () {
        return $http.get("/Production/ProductionStatus/GetAllStyle");
    }

    this.getPurchaseOrdersByStyleIDs = function (list) {
        return $http.get("/Production/ProductionStatus/GetPurchaseOrdersByStyleIDs", { params: { list: list } });
    }
    this.getPurchaseOrdersByStyleID = function (styleValue) {
        return $http.get("/Production/ProductionStatus/GetPurchaseOrdersByStyleID", { params: { Value: styleValue } });
    }

    this.getLineDropDown = function () {
        return $http.get("/Production/ProductionStatus/GetAllProductionLine");
    }

    this.getStyleDropDownByBuyerID = function (accountID, buyerID) {
        return $http.get("/Production/ProductionStatus/GetStylesByBuyerID", { params: { accountID: accountID, buyerID: buyerID } });
    }

    this.getProductionFloorDropDown = function () {
        return $http.get("/Production/ProductionStatus/GetProductionFloorDropDown/");
    }

    this.getProductionLineByFloor = function (floor) {
        return $http.get("/Production/ProductionStatus/GetProductionLineByFloor/", { params: { floor: floor } });
    }

    this.getFilteredProductionStatus = function (filter) {
        return $http.post("/Production/ProductionStatus/GetFilteredProductionStatus/", filter);
    }


    this.saveProductionStatus = function (statusList) {
        return $http.post("/Production/ProductionStatus/SaveProductionStatus", statusList);
    }

    this.getStyleDropDownByKeyword = function (inputString , buyerId) {
        return $http.get("/Production/ProductionStatus/GetStyleDropDownByKeyword", { params: { inputString: inputString, buyerId: buyerId } });
    };

    this.getBuyerDropDownByKeyword = function (inputString) {
        return $http.get("/Production/ProductionStatus/GetBuyerDropDownByKeyword", { params: { inputString: inputString} });
    };
    this.removeProductionStatus = function (ProductionDailyReportId) {
        return $http.get("/Production/ProductionStatus/RemoveProductionStatus", { params: { ProductionDailyReportId: ProductionDailyReportId } });
    }
});