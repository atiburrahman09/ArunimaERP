scopoAppServices.service('rawMaterialService', function ($http) {

    this.getPurchaseOrderDropDown = function () {
        return $http.get("/RawMaterialReq/GetPurchaseOrderDropDown/");
    }

    this.getAllItemByPOID = function (PoStyleId) {
        return $http.get("/RawMaterialReq/GetAllItemByPOID/", { params: { purchaseOrderID: PoStyleId } });
    }
    
    this.getAllFloorLine = function () {
        return $http.get("/RawMaterialReq/GetAllFloorLine/");
    }

    this.saveIssuedInventories = function (srVM) {
        return $http.post('/RawMaterialReq/SaveIssuedInventories', srVM);
    }

    this.getIssues = function () {
        return $http.get('/RawMaterialReq/GetIssues');
    }

    this.getIssueById = function (id) {
        return $http.get('/RawMaterialReq/GetIssueById?id=' + id);
    }
    
});