var app = angular.module('sizecolor.services', []);

app.service('sizeColorService', ['$http', function ($http) {
    this.getSizeColorByPurchaseOrder = function (purchaseOrderID) {
        return $http({
            url: '/OrderManagement/SizeColor/GetSizeColorByPurchaseOrder',
            method: "GET",
            params: { purchaseOrderID: purchaseOrderID }
        });
    };

    this.save = function (sizeColorList) {
        return $http.post('/OrderManagement/SizeColor/Create', sizeColorList);
    };

    this.copy = function (fromPurchaseOrderID, toPurchaseOrderID) {
        return $http({
            url: '/OrderManagement/SizeColor/Copy',
            method: "GET",
            params: {
                fromPurchaseOrderID: fromPurchaseOrderID,
                toPurchaseOrderID: toPurchaseOrderID
            }
        });
    }
}]);