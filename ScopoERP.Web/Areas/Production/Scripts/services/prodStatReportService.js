
scopoAppServices.service('prodStatReportService', function ($http) {
    const BASE_URL = '/Production/ProductionReport/'
    this.getDailySewingStatus = function(){
        url = BASE_URL+'GetDailySewingStatus';
        return $http.get(url);
    }
});