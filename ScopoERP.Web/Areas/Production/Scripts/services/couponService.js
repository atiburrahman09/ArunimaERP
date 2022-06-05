scopoAppServices.service('couponService', function ($http) {
    //this.getAllStyles = function () {
    //    return $http.get("/Production/Coupon/GetStyleDropDown/");
    //};

    this.getCouponOnSearch = function (styleID, completedDate) {
        return $http.get("/Production/Coupon/GetCouponByStyleOrDate/", { params: { styleID: styleID, completedDate: completedDate } });
    };

    this.getPOList = function (styleID) {
        return $http.get("/Production/Coupon/GetPOList/", { params: { styleID: styleID } });
    };

    this.getCutPlanList = function (poID) {
        return $http.get("/Production/Coupon/GetCuttingPlanList/", { params: { poID: poID } });
    };

    this.getStyles = function (accountId, buyerId) {
        return $http.get("/Production/Coupon/GetAllStyle/?accountId=" + accountId + "&buyerId=" + buyerId)
            .then(function (result) {
                return result.data;
            },
            function (err) {
                console.log(err);
            });
    }

    this.getAllBuyer = function () {
        return $http.get("/Production/Coupon/getAllBuyer/")
        .then(function (result) {
            return result.data;
        },
        function (err) {
            console.log(err);
        });
    }

    this.getAllCoupons = function () {
        return $http.get("/Production/Coupon/GetAllCoupons/");
    };

    this.getBundleInformation = function (CuttingPlanID) {
        console.log(CuttingPlanID);
        return $http.get("/Production/Coupon/GetBundleInformation/", { params: { CuttingPlanID: CuttingPlanID } });
    };

    this.getStyleInformation = function (StyleID) {
        console.log(StyleID);
        return $http.get("/Production/Coupon/GetStyleInformation/", { params: { StyleID: StyleID } });
    };

    this.save = function (couponList) {
        return $http.post("/Production/Coupon/Save/", couponList);
    };

    this.generateCouponInformation = function (CuttingPlanID, StyleID, PurchaseOrderID, CutPlanNo, OperationCategoryID) {
        console.log(PurchaseOrderID);
        return $http.get("/Production/Coupon/GenerateCouponInformation/", { params: { CuttingPlanID: CuttingPlanID, StyleID: StyleID, PurchaseOrderID: PurchaseOrderID, CutPlanNo: CutPlanNo, operationCategoryID: OperationCategoryID } });
    };

    this.deleteCoupon = function (cuttingPlanID, operationCategoryID) {
        return $http.get("/Production/Coupon/DeleteCoupon/", { params: { cuttingPlanID: cuttingPlanID, operationCategoryID: operationCategoryID } });
    };

    this.getBundleList = function (specNo) {
        return $http.get("/Production/Coupon/GetAllBundles/", { params: { specNo: specNo } });
    }

    this.searchCoupon = function (coupon) {
        console.log(coupon);
        return $http.get("/Production/Coupon/SearchCoupon/", { params: { cuttingPlanID: coupon.CuttingPlanID, PurchaseOrderID: coupon.PurchaseOrderID, CutPlanNo: coupon.CuttingNo, OperationCategoryID : coupon.OperationCategoryID } });
    };

    this.getAllOperationCategories = function () {
        return $http.get("/Production/Coupon/GetAllOperationCategories/");
    }
});