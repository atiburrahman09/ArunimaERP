scopoAppControllers.controller('poEmployeeMappingCtrl', ['$scope', 'poEmployeeMappingService', 'alertify', '$window', '$timeout', function ($scope, poEmployeeMappingService, alertify, $window, $timeout) {

    $scope.init = function (ProductionPlanningID) {
        $scope.po = {};
        $scope.list = [];
        
        poEmployeeMappingService.getProductionPlanningData(ProductionPlanningID).then(function (res) {
            $scope.po = res.data;
            $scope.getEmployeeList($scope.po.FloorName);
            $scope.getPoEmployeeMappingData(ProductionPlanningID);            
        }, function (err) {
            console.log(err);
        });
    }

    $scope.getEmployeeList = function (floor) {
        poEmployeeMappingService.getEmployeeList(floor).then(function (res) {
            console.log(res.data);
            $scope.employeeList = res.data;
        }, function (err) {
            console.log(err);
        });
    }
    $scope.getPoEmployeeMappingData = function (id) {
        poEmployeeMappingService.getPoEmployeeMappingData(id).then(function (res) {
            $scope.po.empList = res.data;
        });
    }

    $scope.mapEmployeewithPO = function () {
        var emp = JSON.parse($scope.selectedEmployee);
        console.log(emp);

        if ($scope.po.empList == undefined) {
            $scope.po.empList = [];
        }

        for (index in $scope.po.empList) {
            if (emp.Text === $scope.po.empList[index].Text) {
                alertify.error("Employee already exists.");
                return;
            }
        }
        
        $scope.po.empList.push(emp);
        
    };

    $scope.deleteEmpClicked = function (index) {
        $scope.po.empList.splice(index, 1);
    }

    $scope.save = function () {
        $scope.po.empList.forEach(x=> {
            $scope.list.push({ ProductionPlanningID: $scope.po.ProductionPlanningID, EmployeeID: x.Value })
        });
        console.log($scope.list);
        poEmployeeMappingService.save($scope.list).then(function (res) {
            if (res) {
                alertify.success("PO succsesfully added with employees.");
                $timeout(function () { $scope.callAtTimeout(); }, 3000);
                
            }
            else {
                alertify.error("Failed to save data.");
            }
        });
    };
    $scope.callAtTimeout = function () {
        //console.log("$scope.callAtTimeout - Timeout occurred");
        $window.location.href = '/Production/SewingPlan/index';
    }

}]);
