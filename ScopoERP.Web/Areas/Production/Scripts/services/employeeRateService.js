scopoAppServices.service('employeeRateService', function ($http) {

    let serviceBase = "/Production/TrainingCurve/";


    this.saveInformation = function (info) {
        console.log(info);
        return $http.post(serviceBase + "SaveInformation", info);
    };

    this.getEmployeeeRateDetailsById = function (CardNo) {
        return $http.get(serviceBase + "GetEmployeeeRateDetailsById", { params: { CardNo: CardNo } });
    }

    this.getEmployeeDropDownByKeyword = function (inputString) {
        return $http.get(serviceBase + "GetEmployeeDropDownByKeyword", { params: { inputString: inputString } });
    };


    this.getRecentEmployees = function () {
        return $http.get(serviceBase + "GetRecentEmployees");
    }
    this.getAllOperations = function () {
        return $http.get(serviceBase + "GetAllOperations");
    }
});