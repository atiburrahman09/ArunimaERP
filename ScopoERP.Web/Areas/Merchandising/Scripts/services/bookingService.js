scopoAppServices.service('bookingService', function ($http) {
    this.getJobDropDown = function () {
        return $http.get('/Booking/GetJobDropDown');
    }

    this.getPIDropDownByJob = function (jobId) {
        return $http.get('/Booking/GetPIDropDownByJob/', { params: { jobId: jobId } });
    }

    this.getReferenceDropDownByJob = function (jobId) {
        return $http.get('/Booking/GetReferenceDropDownByJob/', { params: { jobId: jobId } });
    }


    this.getPIById = function (piId) {
        return $http.get('/Booking/GetPIById/', { params: { piId: piId } });
    }

    this.getBackToBackLCByJobId = function (jobId) {
        return $http.get('/Booking/GetBackToBackLCByJobId/', { params: { jobId: jobId } });
    }

    this.getSupplierDropDown = function () {
        return $http.get('/Booking/GetSupplierDropDown/');
    }

    this.updatePI = function(pi){
        return $http.post('/Booking/UpdatePI', pi);
    }

    this.getBookingByPIId = function (piId) {
        return $http.get('/Booking/GetBookingByPIId/', { params: { piId: piId } });
    }

    this.saveBookings = function (bookingList) {
        return $http.post('/Booking/SaveBookings/', bookingList);
    }
    this.reviseBookings = function (bookingList) {
        return $http.post('/Booking/ReviseBookings/', bookingList);
    }

    this.createPI = function (pi) {
        return $http.post('/Booking/CreatePI', pi);
    }


    this.getPurchaseOrderListByJob = function (jobId) {
        return $http.get('/Booking/GetPurchaseOrderListByJob/', { params: { jobId: jobId } });
    }

    this.getItemListByPO = function (poIdList) {
        return $http.get('/Booking/GetItemList/', { params: { poIdList: poIdList } });
    }

    this.getBookingList = function (poIdList, itemId) {
        return $http.get('/Booking/GetBookingList/', { params: { poIdList: poIdList, itemId: itemId } });
    }

    this.getFormulaDropDown = function () {
        return $http.get('/Booking/GetFormulaDropDown/');
    }

    this.IsSizeColorByPurchaseOrder = function (poId) {
        return $http.get('/Booking/IsSizeColorByPurchaseOrder/', { params: { poId: poId } });
    }




    //new
    //'/MaterialManagement/Booking/GetMultiplePODropDownByMultipleStyles'
    this.getItemByMultiplePO = function (poIDs) {
        return $http.get('/Booking/GetItemByMultiplePO', { params: { poIDs: poIDs } });
    }

    this.getItemByAllPO = function (poList) {
        return $http.post('/Booking/GetItemByAllPO', poList);
    }

    this.generateBooking = function (generateBookingViewModel) {
        return $http.post('/Booking/GetBookingList', generateBookingViewModel);
    }

    this.deletePI = function (PIID) {
        return $http.get('/Booking/DeletePI', { params: { PIID: PIID} });
    }
    
});