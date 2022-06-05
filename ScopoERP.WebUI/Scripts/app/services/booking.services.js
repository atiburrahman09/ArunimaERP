var app = angular.module('booking.services', []);

app.service("bookingSearchService", ['$http', function ($http) {

    this.getBookingByPIID = function (piID) {
        return $http({
            url: '/MaterialManagement/Booking/GetBookingByPIID',
            method: "GET",
            params: { piID: piID }
        });
    };

    this.getBookingFromWorksheet = function (poStyleIDs, itemID) {
        return $http({
            url: '/MaterialManagement/Booking/GetBookingFromWorksheet',
            method: "GET",
            params: { poStyleIDs: poStyleIDs, itemID: itemID }
        });
    };
}]);

app.service("bookingCrudService", ['$http', function ($http) {

    this.reviseBooking = function (bookingVM) {
        return $http.post('/MaterialManagement/Booking/ReviseBooking', bookingVM);
    };

    this.createBooking = function (bookingVMs) {
        
        return $http.post('/MaterialManagement/Booking/CreateBooking', bookingVMs);
    };

    this.unmapBooking = function (bookingID) {
        return $http({
            url: '/MaterialManagement/Booking/UnmapBooking',
            method: "GET",
            params: { bookingID: bookingID }
        });
    };
}]);