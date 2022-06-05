var app = angular.module('costSheet.controllers', ['costSheet.services'])

app.controller('costSheetCtrl', ['$scope', '$filter', 'costSheetService', 'toasterService', function ($scope, $filter, costSheetService, toasterService) {
    console.log('hhhhhh');

    $scope.init = function () {
        console.log('test');

        $scope.getCoshSheetDetails($scope.costSheetNo);
        $scope.getItemCategory();
        $scope.getConsumptionUnit();
    };


    $scope.getItemCategory = function () {
        costSheetService.getItemCategory().then(function (res) {
            $scope.itemCategoryList = res.data;
        });
    };


    $scope.getConsumptionUnit = function () {
        costSheetService.getConsumptionUnit().then(function (res) {
            $scope.consumptionUnitList = res.data;
        });
    };


    $scope.getCoshSheetDetails = function (costSheetNo) {
        costSheetService.getCoshSheetDetails(costSheetNo).then(function (res) {
            $scope.costSheetList = res.data;
        });
    };


    var newRow = {

    };


    $scope.addNew = function () {
        $scope.costSheetList.push(newRow);
    };


    $scope.save = function () {
        costSheetService.save($scope.costSheetList).then(function (res) {
            if (res.data == 'true') {
                toasterService.clear();
                toasterService.success("Successfully saved!");
            }
        });
    };
}]);