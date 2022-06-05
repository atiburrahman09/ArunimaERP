scopoAppServices.service('dashboardService', function ($http) {

    this.getDashboardData = function () {
        return $http.get("/Home/GetDashboardData");
    }

    this.getShipmentPerDayDataSet = function () {
        return $http.get('/Home/GetShipmentPerDayDataSet');
    }
});