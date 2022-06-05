var app = angular.module('job.controllers', ['job.services'])

app.controller('jobCtrl', ['$scope', '$filter', 'jobService', 'toasterService', function ($scope, $filter, jobService, toasterService) {

    $scope.init = function () {
    }

    $scope.getAllJob = function () {
        jobService.getAllJob($scope.Year).then(function (res) {
            $scope.jobList = res.data;
        });
    }

    $scope.updateAdvancedCM = function () {
        jobService.updateAdvancedCM($scope.jobList).then(function (res) {
            toasterService.clear();
            toasterService.success("Successfully saved!");
        });
    }

    $scope.calculatePercentage = function(index) {
        $scope.jobList[index].AdvancedCMPercentage = $scope.jobList[index].Limit - $scope.jobList[index].BudgetPercentage;
    }
}]);