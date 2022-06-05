var app = angular.module('worksheet.services', []);

app.service("worksheetSearchService", ['$http', function ($http) {
    this.getWorksheet = function (purchaseOrderID, itemID) {
        return $http({
            url: '/MaterialManagement/Worksheet/GetWorksheet',
            method: "GET",
            params: {
                purchaseOrderID: purchaseOrderID,
                itemID: itemID
            }
        });
    };

    this.getReferenceNo = function (purchaseOrderID, itemID) {
        return $http({
            url: '/MaterialManagement/Worksheet/GetReferenceNo',
            method: "GET",
            params: {
                purchaseOrderID: purchaseOrderID,
                itemID: itemID
            }
        });
    };

    this.isSizeColorExists = function (purchaseOrderID) {
        return $http({
            url: '/MaterialManagement/Worksheet/IsSizeColorExists',
            method: "GET",
            params: {
                purchaseOrderID: purchaseOrderID
            }
        });
    };
    
}]);

app.service("worksheetCrudService", ['$http', function ($http) {
    this.updateWorksheet = function (worksheetVM) {
        return $http.post('/MaterialManagement/Worksheet/UpdateWorksheet', worksheetVM);
    };

    this.createWorksheet = function (costsheetNo, purchaseOrderID, itemID, formula) {
        return $http.post('/MaterialManagement/Worksheet/CreateWorksheet', {
            costsheetNo: costsheetNo,
            purchaseOrderID: purchaseOrderID,
            itemID: itemID,
            formula: formula
        });
    };

    this.deleteWorksheet = function (purchaseOrderID, itemID) {
        return $http.post('/MaterialManagement/Worksheet/DeleteWorksheet', {
            purchaseOrderID: purchaseOrderID,
            itemID: itemID
        });
    }
}]);