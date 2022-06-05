scopoAppServices.service('purchaseOrderService', function ($http) {

    this.getJobList = function () {
        return $http.get("/PurchaseOrder/GetJobDropDown/")
        .then(function (result) {
            return result.data;
        },
        function (err) {
            console.log(err);
        });
    }

    this.getStyleList = function () {
        return $http.get("/PurchaseOrder/GetStyleDropDown/")
        .then(function (result) {
            return result.data;
        },
        function (err) {
            console.log(err);
        });
    }

    this.getFactoryList = function () {
        return $http.get("/PurchaseOrder/GetFactoryDropDown/")
        .then(function (result) {
            return result.data;
        },
        function (err) {
            console.log(err);
        });
    }

    this.getSeasonList = function () {
        return $http.get("/PurchaseOrder/GetSeasonDropDown/")
        .then(function (result) {
            return result.data;
        },
        function (err) {
            console.log(err);
        });
    }

    this.getCurrentStatusList = function () {
        return $http.get("/PurchaseOrder/GetCurrentStatusDropDown/")
        .then(function (result) {
            return result.data;
        },
        function (err) {
            console.log(err);
        });
    }

    this.getPurchaseOrderListByStyle = function (styleID) {
        return $http.get("/PurchaseOrder/GetPurchaseOrderListByStyle/?styleID=" + styleID);
    }

    this.createPurchaseOrder = function (purchaseOrderVM) {
        return $http.post("/PurchaseOrder/CreatePurchaseOrder/", { 'purchaseOrderVM': purchaseOrderVM }).then(function (res) {
            return res.data;
        });
    }

    this.updatePurchaseOrder = function (purchaseOrderVM) {
        return $http.post("/PurchaseOrder/UpdatePurchaseOrder/", { 'purchaseOrderVM': purchaseOrderVM }).then(function (res) {
            return res.data;
        });
    }

    this.getCSByStyleID = function (styleID) {
        return $http.get("/PurchaseOrder/GetCSByStyleID/", { params: { styleID: styleID } });
    }

    this.getOrCreateWorkSheet = function (csNO, poID) {
        return $http.get("/PurchaseOrder/GetOrCreateWorkSheet/", {
            params: {
                costSheetNo: csNO,
                purchaseOrderID: poID
            }
        });
    }

    this.updateWorkSheets = function (workSheetList) {
        return $http.put("/PurchaseOrder/UpdateWorkSheets/", workSheetList);
    }

    this.getPurchaseOrdersForBulkEdit = function (jobId) {
        return $http.get('/PurchaseOrder/GetPurchaseOrdersForBulkEdit/', { params: { jobId: jobId } });
    }

    this.saveAmendment = function (bulkPurchaseOrder) {
        return $http.post('/PurchaseOrder/SaveAmendment', bulkPurchaseOrder);
    }

    this.getWorkSheetsByCostSheetAndPurchaseOrderId = function (costSheetNo, purchaseOrderId) {
        return $http.get('/PurchaseOrder/GetWorkSheetsByCostSheetAndPurchaseOrderId/', { params: { costSheetNo: costSheetNo, purchaseOrderId: purchaseOrderId } });
    }

    this.getCostSheetByCostsheetNo = function (costsheetNo) {
        return $http.get('/PurchaseOrder/GetCostSheetByCostsheetNo/', { params: { costsheetNo: costsheetNo } });
    }

    this.getCostSheet = function () {
        return $http.get("/PurchaseOrder/GetCostSheetDropDown/")
        .then(function (result) {
            return result.data;
        },
        function (err) {
            console.log(err);
        });
    }

    this.getCostSheetOrWorkSheet = function (costSheetNo, purchaseOrderId) {
        return $http.get('/PurchaseOrder/GetOrCreateWorkSheet/', { params: { costSheetNo: costSheetNo, purchaseOrderId: purchaseOrderId } });
    }


});