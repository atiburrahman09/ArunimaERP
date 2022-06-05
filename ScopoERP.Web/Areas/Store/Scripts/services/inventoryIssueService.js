scopoAppServices.service('inventoryIssueService', function ($http) {

    this.getPurchaseOrderDropDown = function () {
        return $http.get("/Inventory/GetPurchaseOrderDropDown/");
    }

    this.getAllItemByPOID = function (PoStyleId) {
        return $http.get("/Inventory/GetAllItemByPOID/", { params: { purchaseOrderID: PoStyleId } });
    }
    
    this.getAllFloorLine = function () {
        return $http.get("/Inventory/GetAllFloorLine/");
    }

    this.saveIssuedInventories = function (srVM) {
        return $http.post('/Inventory/SaveIssuedInventories', srVM);
    }

    this.getIssues = function () {
        return $http.get('/Inventory/GetIssues');
    }

    this.getIssueById = function (id) {
        return $http.get('/Inventory/GetIssueById?id=' + id);
    }
    this.getIssueBySR = function (id) {
        return $http.get('/Inventory/getIssueBySR?id=' + id);
    }
});