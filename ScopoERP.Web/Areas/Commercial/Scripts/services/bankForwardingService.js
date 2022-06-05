scopoAppServices.service('bankForwardingService', function ($http) {
    //this.getAllBankFW = function () {
    //    return $http.get("/Commercial/BankForwarding/GetAllBankForwarding/");
    //}

    this.getInvoiceDropDownByJob = function (jobID) {
        return $http.get("/Commercial/BankForwarding/GetInvoiceDropDownByJob/", { params: { jobID: jobID } });
    }

    this.getJobDropDown = function () {
        return $http.get("/Commercial/BankForwarding/GetJobDropDown/");
    }

    this.saveBankForwarding = function (bankFW) {
        return $http.post("/Commercial/BankForwarding/SaveBankForwarding", bankFW);
    }

    this.getInvoiceListByBankForwardingID = function (bankForwardingID) {
        return $http.get("/Commercial/BankForwarding/GetInvoiceListByBankForwardingID", { params: { bankForwardingID: bankForwardingID } });
    }

    this.getAllBankForwardingByJobID = function (jobID) {
        return $http.get("/Commercial/BankForwarding/GetAllBankForwardingByJobID", { params: { jobID: jobID } });
    }
})