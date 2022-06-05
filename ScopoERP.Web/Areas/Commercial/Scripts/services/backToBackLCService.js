scopoAppServices.service('backToBackLCService', function ($http) {
    this.getJobDropDown = function () {
        return $http.get("/Commercial/BackToBackLC/GetJobDropDown/");
    }

    this.getBackToBackLCByJob = function(jobID){
        return $http.get("/Commercial/BackToBackLC/GetBackToBackLCByJob/",{params: {jobID: jobID}});
    }

    this.getLCTypeDropDown = function () {
        return $http.get("/Commercial/BackToBackLC/GetLCTypeDropDown/");
    }

    this.saveBackToBackLC = function (backToBackLC) {
        return $http.post("/Commercial/BackToBackLC/SaveBackToBackLC/", backToBackLC);
    }

    this.getPISummaryByJobID = function (jobID) {
        return $http.get("/Commercial/BackToBackLC/GetPISummaryByJobID/", { params: { jobID: jobID } });
    }

    this.updatePIByLCID = function (currentLC) {        
        return $http.put("/Commercial/BackToBackLC/UpdatePIByLCID/",currentLC);
    }

    this.deleteBackToBackLC = function (lcID) {
        return $http.delete("/Commercial/BackToBackLC/DeleteBackToBackLC/", { params: {lcID: lcID}});
    }

    this.updateBackToBackLC = function (b2bLCObj) {
        return $http.put("/Commercial/BackToBackLC/updateBackToBackLC/", b2bLCObj);
    }

    this.createBackToBackLC = function (b2bLCObj) {
        return $http.post("/Commercial/BackToBackLC/createBackToBackLC/", b2bLCObj);
    }
});