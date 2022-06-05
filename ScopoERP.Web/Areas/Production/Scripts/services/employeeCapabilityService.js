scopoAppServices.service('employeeCapabilityService', function ($http) {

    let serviceBase = "/Production/EmployeeCapability/";


    this.saveInformation = function (info) {
        console.log(info);
        return $http.get(serviceBase + "SaveInformation", { params: { cardNo: info.EmployeeCardNo, specs: info.Specs } });
    };

    this.getEmployeeeCapabilityDetailsById = function (CardNo) {
        return $http.get(serviceBase + "GetEmployeeeCapabilityDetailsById", { params: { CardNo: CardNo } });
    }

    this.getEmployeeDropDownByKeyword = function (inputString) {
        return $http.get(serviceBase + "GetEmployeeDropDownByKeyword", { params: { inputString: inputString } });
    };


    this.getRecentEmployees = function () {
        return $http.get(serviceBase + "GetRecentEmployees");
    }
    this.getAllSpecs = function () {
        return $http.get(serviceBase + "GetAllSpecs");
    }
});