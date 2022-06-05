scopoAppServices.service('sampleApproveService', function ($http) {

    this.getAllStyle = function () {
        return $http.get("/Merchandising/Sample/GetAllStyle");
    }

    this.getAllSample = function () {
        return $http.get("/Merchandising/Sample/GetAllSample");
    }
    this.saveApprove = function (sampleApproveVM) {
        console.log(sampleApproveVM);
        return $http.post("/Merchandising/Sample/SaveApprove", sampleApproveVM);
    }

    this.getAllSampleApprove = function (StyleID) {
        return $http.get("/Merchandising/Sample/GetAllSampleApprove", { params: { StyleID: StyleID } });
    }

    this.removeSampleApprove = function (sampleApprovalID) {
        return $http.get("/Merchandising/Sample/RemoveSampleApprove", { params: { sampleApprovalID: sampleApprovalID } });
    }
});