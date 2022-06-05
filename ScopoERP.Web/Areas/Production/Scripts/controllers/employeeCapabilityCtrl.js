scopoAppControllers.controller('employeeCapabilityCtrl', ['$scope', 'employeeCapabilityService', 'alertify', function ($scope, employeeCapabilityService, alertify) {

    $scope.employee = {};
    var prevIndex = null;
    var currentIndex = null;
    $scope.employeeList = [];
    $scope.specs = [];

    $scope.init = function () {
        $scope.getRecentEmployees();
        $scope.getAllSpecs();
    }

    $scope.getAllSpecs = function () {
        employeeCapabilityService.getAllSpecs().then(function (res) {
            $scope.specs = res.data;
            console.log($scope.specs);
        }, function (err) { handleHttpError(err); })
    };

    $scope.saveInformation = function () {
        $scope.employee.EmployeeID = $scope.employeeID;
        $scope.employee.EmployeeCardNo = $scope.cardNo;
        console.log($scope.employee);
        if ($scope.employee.EmployeeID != null) {
            employeeCapabilityService.saveInformation($scope.employee).then(function (res) {
                alertify.success(res.data);
                $scope.employee = {};
                $scope.employeeList[currentIndex].selected = false;
                $scope.employeeCapabilityForm.$setPristine();
                $scope.employeeCapabilityForm.$setUntouched();
            }, function (err) {
                handleHttpError(err);
            });
        }
        else {
            alertify.error("Please select employee");
        }
    }

    // searching
    $scope.getEmployeeByKeyword = function (val) {
        console.log(val);
        if (val.length < 3) {
            if (val.length == 0)
                $scope.getRecentEmployees();
            return;
        }
        employeeCapabilityService.getEmployeeDropDownByKeyword(val).then(function (res) {
            $scope.employeeList = res.data;
        });
    };

    $scope.getRecentEmployees = function () {
        employeeCapabilityService.getRecentEmployees().then(function (res) {
            $scope.employeeList = res.data;
        }, function (err) {
            handleHttpError(err);
        })
    }
    // emplyee selection
    $scope.employeeSelected = function (e) {
        console.log(e);
        var index = indexOfObjectInArray($scope.employeeList, 'EmployeeName', e.EmployeeName);
        currentIndex = index;
        $scope.employeeList[index].selected = true;
        if (prevIndex != null && prevIndex != index) {
            $scope.employeeList[prevIndex].selected = false;
        }
        prevIndex = index;
        $scope.employeeID = $scope.employeeList[index].EmployeeID;
        $scope.cardNo = $scope.employeeList[index].EmployeeCardNo;
        $scope.getEmployeeeCapabilityDetailsById($scope.employeeList[index].EmployeeCardNo);
    };

    $scope.getEmployeeeCapabilityDetailsById = function (id) {
        console.log(id);
        employeeCapabilityService.getEmployeeeCapabilityDetailsById(id).then(function (res) {
            $scope.employee.Specs = res.data;
        }, function (err) { handleHttpError(err); });
    }

    //getting index of selected employee
    function indexOfObjectInArray(array, property, value) {
        for (var i = 0; i < array.length; i++) {
            if (array[i][property] == value) {
                return i;
            }
        }
        return -1;
    }

}]);
