scopoAppServices.service('styleConfigureService', function ($http) {

    this.getStyles = function (accountId, buyerId) {
        return $http.get("/StyleConfigure/GetAllStyle/?accountId=" + accountId + "&buyerId=" + buyerId)
            .then(function (result) {
                return result.data;
            },
            function (err) {
                console.log(err);
            });
    }
    
    this.getAllBuyer = function () {
        return $http.get("/StyleConfigure/getAllBuyer/")
        .then(function (result) {
            return result.data;
        },
        function (err) {
            console.log(err);
        });
    }

    this.getMachineDropDown = function () {
        return $http.get("/StyleConfigure/GetMachineDropDown/");
    }

    this.GetStyleOperationListByStyleID = function (styleId) {
        return $http.get("/Production/StyleConfigure/GetStyleOperationListByStyleID/?styleId=" + styleId);
    }
    
    this.saveStyleOperation = function (operationList) {
        return $http.post("/Production/StyleConfigure/SaveStyleOperation/", operationList);
    }

    this.saveOperation = function (stdOperation) {
        return $http.post("/Production/StyleConfigure/SaveOperation/", stdOperation);
    }

    this.getAllStandardOperations = function () {
        return $http.get("/Production/StyleConfigure/GetAllStandardOperations/");
    }

    this.getStandardOperationDropDown = function () {
        return $http.get("/Production/StyleConfigure/GetStandardOperationDropDown/");
    }

    this.saveCutting = function (cutting) {
        return $http.post("/Production/Cutting/SaveCuttingClass/", cutting);
    }

    this.getAllCuttings = function () {
        return $http.get("/Production/Cutting/GetAllCuttingClassList/");
    }

    this.getAllSupervisors = function () {
        return $http.get("/Production/Cutting/GetAllSupervisors/");
    }
    this.getAllOperationCategories = function () {
        return $http.get("/Production/StyleConfigure/GetAllOperationCategories/");
    }

    this.getAllJobClasses = function () {
        return $http.get("/Production/JobClass/GetAllJobClassList/");
    }

    this.getSpec = function () {
        return $http.get("/Production/StyleConfigure/GetSpec/");
    }




})