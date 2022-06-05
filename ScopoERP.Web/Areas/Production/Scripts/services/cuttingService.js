scopoAppServices.service('cuttingService', function ($http) {

    this.getStyles = function (accountId, buyerId) {
        return $http.get("/Production/Cutting/GetAllStyle/?accountId=" + accountId + "&buyerId=" + buyerId)
            .then(function (result) {
                return result.data;
            },
            function (err) {
                console.log(err);
            });
    }

    this.getAllBuyer = function () {
        return $http.get("/Production/Cutting/getAllBuyer/")
        .then(function (result) {
            return result.data;
        },
        function (err) {
            console.log(err);
        });
    }
    this.saveCutting = function (cuttingListVM) {
        return $http.post("/Production/Cutting/SaveCuttingClass/", cuttingListVM);
    }

    this.getAllCuttings = function () {
        return $http.get("/Production/Cutting/GetAllCuttingClassList/");
    }

    this.getCuttingList = function (POID) {
        return $http.get("/Production/Cutting/GetCuttingListByPOID/", { params: { POID: POID } });
    }

    this.getPurchaseOrders = function (StyleID) {
        return $http.get("/Production/Cutting/GetPurchaseOrders/?StyleID=" + StyleID)
            .then(function (result) {
                return result.data;
            },
            function (err) {
                console.log(err);
            });
    }

    
    //this.getCuttingDetails = function (POId) {
    //    return $http.get("/Production/Cutting/GetCuttingDetails/?POId=" + POId)
    //        .then(function (result) {
    //            return result.data;
    //        },
    //        function (err) {
    //            console.log(err);
    //        });
    //}

    this.getBundleInfoByCuttingID = function (CuttingPlanID) {
        return $http.get("/Production/Cutting/GetBundleInfoByCuttingID/?CuttingPlanID=" + CuttingPlanID)
            .then(function (result) {
                return result.data;
            },
            function (err) {
                console.log(err);
            });
    }

    this.saveBundleInfo = function (bundleInformation) {
        return $http.post("/Production/Cutting/SaveBundleInformation/", bundleInformation);
    }

    this.getLastBundleNo = function () {
        return $http.get("/Production/Cutting/GetLastBundleNo/");
    }

    this.updateBundleInfo = function (bundleInformation) {
        console.log("Test");
        return $http.post("/Production/Cutting/UpdateBundleInformation/", bundleInformation);
    }

    this.updateCutting = function (cuttingListVM) {
        return $http.post("/Production/Cutting/UpdateCuttingClass/", cuttingListVM);
    }
})