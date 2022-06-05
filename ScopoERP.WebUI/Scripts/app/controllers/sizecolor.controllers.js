var myModule = angular.module("sizecolor.controllers", [
	'sizecolor.services',
	'globalModule',
    "ui.select2"
]);

myModule.controller('sizeColorController', ['$scope', '$location', 'sizeColorService', 'toasterService', 'dropdownService', function ($scope, $location, sizeColorService, toasterService, dropdownService) {
	//$scope.sizeColorList = sizeColorList;

    $scope.init = function () {
        toasterService.wait();
		sizeColorService.getSizeColorByPurchaseOrder($location.$$absUrl.split('/')[$location.$$absUrl.split('/').length - 1]).then(function (res) {
			if (res.data.length > 0)
				$scope.sizeColorList = res.data;
			else
				$scope.sizeColorList = [{
					Color: "", SizeQuantity: [{ Size: "", Quantity: "" }]
				}];
			toasterService.clear();
		});

		dropdownService.getPODropDown().then(function (res) {
		    $scope.poDropDown = res.data;
		});
    };

    $scope.dropDownOptions = {
        minimumInputLength: 2
    };

	$scope.addRow = function () {
		var temp = $scope.sizeColorList.slice(0, 1);

		var newSizeQuantity = [];
		for (var i in temp[0].SizeQuantity) {
		    newSizeQuantity.push({ Size: temp[0].SizeQuantity[i].Size, Quantity: '' });
		}
		$scope.sizeColorList.push(
				{
					Color: "", SizeQuantity: newSizeQuantity
				}
			);
	};

	$scope.addColumn = function () {
	    for (var i in $scope.sizeColorList) {
	        $scope.sizeColorList[i].SizeQuantity.push({ Size: "", Quantity: "" });
		}
	};

	$scope.removeRow = function (index) {
	    if ($scope.sizeColorList.length > 1)
	        $scope.sizeColorList.splice(index, 1);
	    else
	        toasterService.warning("Sorry you can not remove the last color");
	};

	$scope.removeColumn = function (index) {
	    var flag = false;
	    for (var i in $scope.sizeColorList) {
	        if ($scope.sizeColorList[i].SizeQuantity.length > 1) {
	            $scope.sizeColorList[i].SizeQuantity.splice(index, 1);
	            flag = false;
	        }
	        else
	            flag = true;
	    }
	    if (flag) {
	        toasterService.warning("Sorry you can not remove the last size");
	    }
	};

	$scope.saveButtonClick = function () {
	    toasterService.wait();
	    var firstSizeQuantity = $scope.sizeColorList[0].SizeQuantity;
		for (var i in $scope.sizeColorList) {
		    $scope.sizeColorList[i].PoStyleID = parseInt($location.$$absUrl.split('/')[$location.$$absUrl.split('/').length - 1]);
		    for (var j in $scope.sizeColorList[i].SizeQuantity) {
		        $scope.sizeColorList[i].SizeQuantity[j].Size = firstSizeQuantity[j].Size;
		    }
		}
		sizeColorService.save(JSON.stringify($scope.sizeColorList)).then(function (res) {
			if (res.data === "true") {
				toasterService.clear();
				toasterService.success("Size Color");
			} else {
				toasterService.clear();
				toasterService.error();
			}
		});
	};

	$scope.copyButtonClick = function () {
	    if ($scope.copyFromPO) {
	        toasterService.wait();
	        sizeColorService.copy($scope.copyFromPO, parseInt($location.$$absUrl.split('/')[$location.$$absUrl.split('/').length - 1])).then(function (res) {
	            if (res.data === "true") {

	                sizeColorService.getSizeColorByPurchaseOrder($location.$$absUrl.split('/')[$location.$$absUrl.split('/').length - 1]).then(function (res) {
	                    if (res.data.length > 0)
	                        $scope.sizeColorList = res.data;
	                    else
	                        $scope.sizeColorList = [{
	                            Color: "", SizeQuantity: [{ Size: "", Quantity: "" }]
	                        }];
	                });

	                toasterService.clear();
	                toasterService.success("Size Color");
	            } else if (res.data.errorMessage) {
	                toasterService.clear();
	                toasterService.customError(res.data.errorMessage);
	            } else {
	                toasterService.clear();
	                toasterService.error();
	            }
	        });
	    } else {
	        toasterService.customError("Please Select an Purchase Order");
	    }
	};

	$scope.getColumnTotal = function (index) {
	    var total = 0;
	    for (var i in $scope.sizeColorList) {
	        total += parseFloat($scope.sizeColorList[i].SizeQuantity[index].Quantity);
	    }
	    return total;
	};

	$scope.getRowTotal = function (index) {
	    var total = 0;
	    for (var i in $scope.sizeColorList[index].SizeQuantity) {
	        total += parseFloat($scope.sizeColorList[index].SizeQuantity[i].Quantity);
	    }
	    return total;
	};

	$scope.getGrandTotal = function () {
	    var total = 0;
	    for (var i in $scope.sizeColorList) {
	        for (var j in $scope.sizeColorList[i].SizeQuantity) {
	            total += parseFloat($scope.sizeColorList[i].SizeQuantity[j].Quantity);
	        }
	    }
	    return total;
	}
}]);