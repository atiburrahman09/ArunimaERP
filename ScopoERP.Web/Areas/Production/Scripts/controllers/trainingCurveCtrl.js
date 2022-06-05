scopoAppControllers.controller('trainingCurveCtrl', ['$scope', 'trainingCurveService', 'alertify', function ($scope, trainingCurveService, alertify) {
    var currentIndex = null;
    var previousIndex = null;
    $scope.trainingCurveList = [];

    $scope.init = function () {
        $scope.getAllTrainingCurve();
    }


    $scope.getAllTrainingCurve = function () {
        trainingCurveService.getAllTrainingCurve().then(function (res) {
            $scope.trainingCurveList = res.data;
        });
    }
   

    function saveTrainingCurve() {
        if ($scope.trainingCurveForm.$valid) {
            trainingCurveService.saveTrainingCurve($scope.curve).then(function (res) {
                console.log(res);
                if (res.data.ErrorCode == false) {
                    alertify.error(res.data.Message);
                } else {
                    alertify.success(res.data.Message);
                    $scope.getAllTrainingCurve();
                    resetForm();
                }

            });
        }
    }

    function selectedTrainingCurve(curve) {
        var index = indexOfObjectInArray($scope.trainingCurveList, 'TrainingCurveID', curve.TrainingCurveID);

        if (index == currentIndex) {
            $scope.trainingCurveList[currentIndex].selected = true;
        } else {
            previousIndex = currentIndex;
            currentIndex = index;
            $scope.trainingCurveList[currentIndex].selected = true;
            $scope.curve = angular.copy($scope.trainingCurveList[currentIndex]);

            if (previousIndex != null) {
                $scope.trainingCurveList[previousIndex].selected = false;
            }
            //getAllJobClass();
        }
    }



    function resetForm() {
        if (currentIndex != null) {
            currentIndex = null;
            previousIndex = null;
        }
        $scope.curve = {};
        $scope.trainingCurveForm.$setPristine();
        $scope.trainingCurveForm.$setUntouched();
    }

    function indexOfObjectInArray(array, property, value) {
        for (var i = 0; i < array.length; i++) {
            if (array[i][property] == value) {
                return i;
            }
        }
        return -1;
    }


    $scope.saveTrainingCurve = saveTrainingCurve;
    $scope.selectedTrainingCurve = selectedTrainingCurve;
    //$scope.getAllTrainingCurve = getAllTrainingCurve;

    $scope.resetForm = resetForm;
}])