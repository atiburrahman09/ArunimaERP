scopoAppControllers.controller('employeeRateCtrl', ['$scope', 'employeeRateService', 'alertify', function ($scope, employeeRateService, alertify) {

    $scope.employee = {};
    $scope.operationList = [];
    var prevIndex = null;
    var currentIndex = null;
    $scope.employeeList = [];
    $scope.specs = [];

    $scope.init = function () {
        $scope.getRecentEmployees();
        $scope.getAllOperations();
    }

    $scope.getAllOperations = function () {
        employeeRateService.getAllOperations().then(function (res) {
            $scope.operationList = res.data;
            console.log($scope.operationList)

        });
    }

    $scope.saveInformation = function () {
        $scope.employee.EmployeeID = $scope.employeeID;
        $scope.employee.EmployeeCardNo = $scope.cardNo;
        console.log($scope.employee);
        if ($scope.employeeRateForm.$valid && $scope.employee.EmployeeID!=null) {
            employeeRateService.saveInformation($scope.employee).then(function (res) {
                alertify.success(res.data);
                $scope.employee = {};
                $scope.employeeList[currentIndex].selected = false;
                $scope.employeeRateForm.$setPristine();
                $scope.employeeRateForm.$setUntouched();
            }, function (err) {
                handleHttpError(err);
            });
        }
        else {
            alertify.error("Please select employee or fill required information's");
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
        employeeRateService.getEmployeeDropDownByKeyword(val).then(function (res) {
            $scope.employeeList = res.data;
        });
    };

    $scope.getRecentEmployees = function () {
        employeeRateService.getRecentEmployees().then(function (res) {
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
        $scope.getEmployeeeRateDetailsById($scope.employeeList[index].EmployeeCardNo);
    };

    $scope.getEmployeeeRateDetailsById = function (id) {
        console.log(id);
        employeeRateService.getEmployeeeRateDetailsById(id).then(function (res) {
            $scope.employee = res.data;
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
