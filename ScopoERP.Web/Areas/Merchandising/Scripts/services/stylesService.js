scopoAppServices.service('stylesService', function ($http) {

    this.getAllBuyer = function () {
        return $http.get("/styles/getAllBuyer/")
        .then(function (result) {
            return result.data;
        },
        function (err) {
            console.log(err);
        });
    }

    this.getAllCustomer = function () {
        return $http.get("/styles/getAllCustomer/")
        .then(function (result) {
            return result.data;
        },
        function (err) {
            console.log(err);
        });
    }

    this.getAllDivision = function () {
        return $http.get("/styles/getAllDivision/")
        .then(function (result) {
            return result.data;
        },
        function (err) {
            console.log(err);
        });
    }


    this.getStyles = function (buyerId) {
        return $http.get("/styles/GetAllStyle/", { params: { buyerId: buyerId } });
    }
    //this.getStyles = function (buyerId) {
    //    return $http.get("/styles/GetAllStyle/" + buyerId)
    //        .then(function (result) {
    //            return result.data;
    //        },
    //        function (err) {
    //            console.log(err);
    //        });
    //}


    this.getCostSheetList = function (styleID) {
        return $http.get("/styles/GetCosheetNoDropDown/", { params: { styleID: styleID } });
    }
    //this.getCostSheetList = function (styleID) {
    //    return $http.get("/styles/GetCosheetNoDropDown/"+ styleID)
    //        .then(function (result) {
    //            return result.data;
    //        },
    //        function (err) {
    //            console.log(err);
    //        });
    //}

    this.costSheetDetailsList = function (costsheetNo) {
        return $http.get("/styles/GetCostSheetByCostsheetNo/?costsheetNo=" + costsheetNo)
            .then(function (result) {
                return result.data;
            },
            function (err) {
                console.log(err);
            });
    }

    this.getItemCategoryDropDown = function () {
        return $http.get("/styles/GetItemCategoryDropDown/")
        .then(function (result) {
            return result.data;
        },
        function (err) {
            console.log(err);
        });
    }

    this.getConsumptionUnitDropDown = function () {
        return $http.get("/styles/GetConsumptionUnitDropDown/")
        .then(function (result) {
            return result.data;
        },
        function (err) {
            console.log(err);
        });
    }

    this.getItemDropDown = function (itemCategoryID) {
        return $http.get("/styles/GetItemDropDown/?itemCategoryID=" + itemCategoryID)
        .then(function (result) {
            return result.data;
        },
        function (err) {
            console.log(err);
        });
    }

    this.createStyle = function (styleVM) {
        return $http.post("/styles/CreateStyle/", { 'styleVM': styleVM}).then(function (res) {
            return res.data;
        });
    }

    this.updateStyle = function (styleVM) {
        return $http.post("/styles/UpdateStyle/", { 'styleVM': styleVM }).then(function (res) {
            return res.data;
        });
    }

    this.createCostSheet = function (costSheetVM) {
        return $http.post("/styles/CreateCostSheet/", { 'costSheetVM': costSheetVM }).then(function (res) {
            return res.data;
        });
    }
})
