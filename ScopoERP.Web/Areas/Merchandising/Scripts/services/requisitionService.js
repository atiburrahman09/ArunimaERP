scopoAppServices.service('requisitionService', function ($http) {
    this.getJobDropDown = function () {
        return $http.get("/Merchandising/Requisition/GetJobDropDown/");
    }
    this.getSupplierDropDown = function () {
        return $http.get("/Merchandising/Requisition/GetSupplierDropDown/");
    }

    this.getReqByJob = function (jobID) {
        return $http.get("/Merchandising/Requisition/GetReqByJob/", { params: { jobID: jobID } });
    }

    this.getLCTypeDropDown = function () {
        return $http.get("/Merchandising/Requisition/GetLCTypeDropDown/");
    }

    this.saveBackToBackLC = function (backToBackLC) {
        console.log("in service: " +JSON.stringify(backToBackLC));
       // return $http.post("/Commercial/BackToBackLC/SaveBackToBackLC/", backToBackLC);
    }

    this.getPISummaryByJobID = function (jobID) {
        return $http.get("/Merchandising/Requisition/GetPISummaryByJobID/", { params: { jobID: jobID } });
    }

    this.updatePIByLCID = function (currentLC) {        
        return $http.put("/Merchandising/Requisition/UpdatePIByLCID/", currentLC);
    }

    this.deleteBackToBackLC = function (lcID) {
        return $http.delete("/Merchandising/Requisition/DeleteBackToBackLC/", { params: { lcID: lcID } });
    }

    this.updateRequisition = function (req) {
        return $http.post("/Merchandising/Requisition/Edit/", req);
    }

    this.createRequisition = function (req) {
        return $http.post("/Merchandising/Requisition/Create/", req);
    }

    //this.getPIDropDownByJob = function (jobID) {
    //    return $http.get("/Commercial/BackToBackLC/GetPIDropDownByJob/", { params: { jobID: jobID } });
    //}
});//end of service