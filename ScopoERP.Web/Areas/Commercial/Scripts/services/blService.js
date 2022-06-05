scopoAppServices.service('blService', function ($http) {
    this.getJobDropDown = function () {
        return $http.get("/Commercial/BL/GetJobDropDown/");
    }

    this.getBackToBackLCDropDownByJobID = function (jobID) {
        return $http.get("/Commercial/BL/GetBackToBackLCDropDownByJobID/", { params: { jobID: jobID } });
    }

    this.getBLAndPIDropDownByBackToBackLCID = function (backToBackLCID) {
        return $http.get("/Commercial/BL/GetBLAndPIDropDownByBackToBackLCID/", { params: { backToBackLCID: backToBackLCID } })
    }

    this.getBLByJobID = function (jobID) {
        return $http.get("/Commercial/BL/GetBLByJobID", { params: { jobID: jobID } });
    }

    this.saveBL = function (blInstance) {
        return $http.post("/Commercial/BL/SaveBL", blInstance);
    }

    this.getBLByID = function (id) {
        return $http.get("/Commercial/BL/GetBLByID", { params: { id: id } });
    }

    this.getItemDropDownByPIID = function (id) {
        return $http.get("/Commercial/BL/GetItemDropDownByPIID", { params: { id: id } });
    }

    this.getBLDetails = function (blID, piID, itemID) {
        return $http.get("/Commercial/BL/GetBLDetails", { params: { blID: blID, piID: piID, itemID: itemID } });
    }

    this.saveBLDetails = function (blDetails) {
        return $http.post("/Commercial/BL/SaveBLDetails", blDetails);
    }
});