var app = angular.module("sizewisefob.controllers", [
    "ui.bootstrap",
    "globalModule",
    "sizecolor.services",
    "ui.select2"
]);

app.controller('sizeWiseFOBController', ['$scope', 'toasterService', 'dropdownService', 'sizeColorService', function ($scope, toasterService, dropdownService, sizeColorService) {
	$scope.searchCriteria = {};

	$scope.init = function () {
		dropdownService.getSingleStyleDropDown().then(function (res) {
			$scope.searchCriteria.styleDropDown = res.data;
		});
	};

	$scope.styleDropDownChange = function () {
		if ($scope.searchCriteria.styleDropDownVal) {
			dropdownService.getSinglePODropDownBySingleStyle($scope.searchCriteria.styleDropDownVal).then(function (res) {
				$scope.searchCriteria.poDropDown = res.data;
			});
			$scope.sizeColorList = '';
			$scope.searchCriteria.poDropDownVal = '';
		}
	};

	$scope.styleOptions = {
		minimumInputLength: 2
	};

	$scope.purchaseOrderDropDownChange = function () {
		if ($scope.searchCriteria.poDropDownVal) {
			toasterService.wait();
			sizeColorService.getSizeColorByPurchaseOrder($scope.searchCriteria.poDropDownVal).then(function (res) {
				if (res.data.length > 0) {
					toasterService.clear();
					$scope.sizeColorList = res.data;
				}
				else {
					$scope.sizeColorList = '';
					toasterService.clear();
					toasterService.customError('This Purchase Order has no size color');
				}
			});
		}
	};

	$scope.saveButtonClick = function () {
		toasterService.wait();
		sizeColorService.save(JSON.stringify($scope.sizeColorList)).then(function (res) {
			if (res.data === "true") {
				toasterService.clear();
				toasterService.success("Size Color");
			} else {
				toasterService.clear();
				toasterService.error();
			}
		});
	}
}]);