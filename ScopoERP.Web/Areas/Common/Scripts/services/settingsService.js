scopoAppServices.service('settingsService', function ($http) {

    this.saveJobClass = function (jobClass) {
        return $http.post("/Production/JobClass/SaveJobClass/", jobClass);
    }

    this.getAllJobClasses = function () {
        return $http.get("/Production/JobClass/GetAllJobClassList/");
    }

    this.getFloor = function () {
        return $http.get("/Common/Settings/GetFloor/");
    }

    this.getLine = function (Floor) {
        return $http.get("/Common/Settings/GetLine/", { params: { Floor: Floor } });
    }

    this.saveSupervisor = function (supervisor) {
        return $http.post("/Common/Settings/SaveSupervisor/", supervisor);
    }

    this.getAllSupervisors = function () {
        return $http.get("/Common/Settings/GetAllSupervisors/");
    }

    this.saveOperation = function (operation) {
        return $http.post("/Common/Settings/SaveOperation/", operation);
    }

    this.getAllStdOperations = function () {
        return $http.get("/Common/Settings/GetAllStandardOperations/");
    }
    this.getAllOperationCategories = function () {
        return $http.get("/Common/Settings/GetAllOperationCategories/");
    }
    this.getAllJobClasses = function () {
        return $http.get("/Common/Settings/GetAllJobClassList/");
    }

    this.getAllSpecs = function () {
        return $http.get("/Common/Settings/GetAllSpecs/");
    }
    this.saveSpec = function (spec) {
        return $http.post("/Common/Settings/SaveSpec/", spec);
    }

    this.getAllOperations = function () {
        return $http.get("/Common/Settings/GetAllStandardOperations/");
    }


});