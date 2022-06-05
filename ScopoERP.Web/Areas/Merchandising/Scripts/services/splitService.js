scopoAppServices.service('splitService', function ($http) {
    this.getStyleDropdown = function () {
        return $http.get("/PurchaseOrder/GetStyleDropDown/");
    }

    this.getPurchaseOrderByStyleId = function (styleId) {
        return $http.get("/PurchaseOrder/GetPurchaseOrderDropDown?styleId="+styleId);
    }

    this.getPurchaseOrderForSplit = function (styleId, poId) {
        return $http.get("/PurchaseOrder/GetPurchaseOrderForSplit/", {
            params: {
                styleId: styleId,
                purchaseOrderId: poId
            }
        });
    }

    this.getPurchaseOrderById = function (poId) {
        return $http.get("/PurchaseOrder/GetPurchaseOrderById?purchaseOrderId=" + poId);
    }

    this.saveSplittedPurchaseOrders = function (masterPOID,poID) {
        return $http.get("/PurchaseOrder/SplitPurchaseOrder", { params: { masterPOID: masterPOID, poID: poID } });
    }

})
