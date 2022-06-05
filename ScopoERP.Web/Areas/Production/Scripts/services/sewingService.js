scopoAppServices.service('poEmployeeMappingService', function ($http) {

    this.getEmployeeList = function (floor) {
        return $http.get("/Production/SewingPlan/GetEmployeeList/", { params: {floor:floor}});
    }

    this.getProductionPlanningData = function (id) {
        return $http.get("/Production/SewingPlan/GetProductionPlanningData/", { params: { id: id } });
    }

    this.getPoEmployeeMappingData = function (id) {
        return $http.get("/Production/SewingPlan/GetPoEmployeeMappingData/", { params: {id:id}});
    }

    this.save = function (data) {
        return $http.post("/Production/SewingPlan/Save/", data);
    }

});