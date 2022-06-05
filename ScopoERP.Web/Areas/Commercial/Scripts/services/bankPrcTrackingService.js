scopoAppServices.service('bankPrcTrackingService', function ($http) {
    var route = '/Commercial/BankPrcTracking/';

    this.getInvoices = function (exp) {
        return $http.get(route + 'getinvoices?exp=' + exp);
    }

    this.getPrcTrackings = function () {
        return $http.get(route+'GetAllBankPrcTrackingByJobID');
    }
    this.saveBankPRC = function (bankPRC) {
        return $http.post(route+"SaveBankPRC", bankPRC);
    }

    this.getInvoiceListByBankPrcTrackingID = function (bankPrcTrackingID) {
        return $http.get(route+"GetInvoiceListByBankPrcTrackingID",
            { params: { bankPrcTrackingID: bankPrcTrackingID } });
    }

});