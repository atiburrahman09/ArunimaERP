var app = angular.module('styleImage.controllers', ['styleImage.services'])

app.controller('styleImageCtrl', ['$scope', 'styleImageService', 'toasterService', function ($scope, styleImageService, toasterService) {

    $scope.init = function () {
        $scope.subContractVM = {};
        console.log('Hi');
    }

    $scope.save = function () {
        styleImageService.save($scope.subContractVM).then(function (res) {
            if (res.data ==  'true') {
                toasterService.clear();
                toasterService.success("Successfully saved!");
            }
        });
    }
}]);