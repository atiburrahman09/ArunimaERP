
scopoAppServices.service('sizeColorService', ['$http', function ($http) {
    this.getSizeColorByPurchaseOrder = function (purchaseOrderID) {
        return $http({
            url: '/Merchandising/SizeColor/GetSizeColorByPurchaseOrder',
            method: "GET",
            params: { purchaseOrderID: purchaseOrderID }
        });
    };

    this.save = function (sizeColorList) {
        return $http.post('/Merchandising/SizeColor/Create', sizeColorList);
    };

    this.copy = function (fromPurchaseOrderID, toPurchaseOrderID) {
        return $http({
            url: '/Merchandising/SizeColor/Copy',
            method: "GET",
            params: {
                fromPurchaseOrderID: fromPurchaseOrderID,
                toPurchaseOrderID: toPurchaseOrderID
            }
        });
    }

    this.getPODropDown = function () {
        return $http({
            url: '/Merchandising/SizeColor/GetPODropDown',
            method: 'GET'
        });
    };

    this.getSingleStyleDropDown = function () {
        return $http({
            url: '/Merchandising/SizeColor/GetStyleDropDown',
            method: 'GET'
        });
    };

    this.getSinglePODropDownBySingleStyle = function (styleID) {
        return $http({
            url: '/Merchandising/SizeColor/GetSinglePODropDownBySingleStyle',
            method: 'GET',
            params: { styleID: styleID }
        });
    };
}]);