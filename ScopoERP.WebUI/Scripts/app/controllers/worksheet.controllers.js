var app = angular.module("worksheet.controllers", [
    "ngGrid",
    "ui.bootstrap",
    "globalModule",
    "worksheet.services",
    "ui.select2"
]);

app.controller('worksheetController', ['$scope', '$modal', 'toasterService', 'dropdownService', 'worksheetSearchService', 'worksheetCrudService', function ($scope, $modal, toasterService, dropdownService, worksheetSearchService, worksheetCrudService) {
    $scope.searchCriteria = {};
    $scope.referenceNo = "";
	$scope.init = function () {
		dropdownService.getSingleStyleDropDown().then(function (res) {
			$scope.searchCriteria.styleDropDown = res.data;
		});
	};

	$scope.styleDropDownChange = function () {
		if ($scope.searchCriteria.styleDropDownVal) {
			dropdownService.getSingleCostsheetDropDownBySingleStyle($scope.searchCriteria.styleDropDownVal).then(function (res) {
			    $scope.searchCriteria.costsheetDropDown = res.data;
			});

			dropdownService.getSinglePODropDownBySingleStyle($scope.searchCriteria.styleDropDownVal).then(function (res) {
				$scope.searchCriteria.poDropDown = res.data;
			});
			$scope.worksheetGridData = null;

			$scope.searchCriteria.costsheetDropDownVal = '';
			$scope.searchCriteria.poDropDownVal = '';
			$scope.searchCriteria.itemDropDownVal = '';
		}
	};

	$scope.styleOptions = {
		minimumInputLength: 2
	};

	$scope.costsheetDropDownChange = function () {
		if ($scope.searchCriteria.costsheetDropDownVal) {
			dropdownService.getItemBySingleCostsheet($scope.searchCriteria.costsheetDropDownVal).then(function (res) {
				$scope.searchCriteria.itemDropDown = res.data;
			});
			$scope.worksheetGridData = null;

			$scope.searchCriteria.itemDropDownVal = '';
		}
	};

	$scope.searchWorksheetButtonClick = function () {

	    if ($scope.searchCriteria.poDropDownVal == '' || $scope.searchCriteria.poDropDownVal == undefined) {
	        toasterService.customError('Please select a Purchase Order');
	        return;
	    } else if ($scope.searchCriteria.itemDropDownVal == '' || $scope.searchCriteria.itemDropDownVal == undefined) {
	        toasterService.customError('Please select an Item');
	        return;
	    } else {
	        toasterService.wait();
	        worksheetSearchService.getWorksheet($scope.searchCriteria.poDropDownVal, $scope.searchCriteria.itemDropDownVal).then(function (res) {
                //if data exists
	            if (res.data.length > 0) {
	                toasterService.clear();
	                $scope.worksheetGridData = res.data;

	                worksheetSearchService.getReferenceNo($scope.searchCriteria.poDropDownVal, $scope.searchCriteria.itemDropDownVal).then(function (res) {
	                    var refernceNo = res.data.substring(1, res.data.length - 1);
	                    if (refernceNo) {
	                        $scope.referenceNo = refernceNo;
	                        $('.ngRow.odd').css({
	                            'background-color': '#e3fff9'
	                        });
	                        $('.ngRow.even').css({
	                            'background-color': '#d1fff5'
	                        });
	                    } else {
	                        $scope.referenceNo = "";
	                        $('.ngRow.odd').css({
	                            'background-color': '#fdfdfd'
	                        });
	                        $('.ngRow.even').css({
	                            'background-color': '#f3f3f3'
	                        });
	                    }
	                });
	            }
                //if data not exists
	            else {
	                $scope.referenceNo = "";
	                $scope.worksheetGridData = null;
	                
	                worksheetSearchService.isSizeColorExists($scope.searchCriteria.poDropDownVal).then(function (res) {
	                    toasterService.clear();
                        //if Size color not exists
	                    if (res.data == "false") {
	                        toasterService.customError('Please Create Size Color First');
	                    }
                        //if Size Color exists
	                    else {
	                        toasterService.clear();
	                        dropdownService.getFormulaDropDown().then(function (res) {
	                            $scope.formulaDropDown = res.data;

	                            var modalInstance = $modal.open({
	                                templateUrl: 'FormulaModal.html',
	                                controller: 'formulaModalController',
	                                size: 'sm',
	                                resolve: {
	                                    formulaDropDown: function () {
	                                        return $scope.formulaDropDown;
	                                    }
	                                }
	                            });

	                            modalInstance.result.then(function (formulaDropDownVal) {
	                                $scope.formulaDropDownVal = formulaDropDownVal;

	                                toasterService.wait();
	                                worksheetCrudService.createWorksheet($scope.searchCriteria.costsheetDropDownVal, $scope.searchCriteria.poDropDownVal, $scope.searchCriteria.itemDropDownVal, $scope.formulaDropDownVal).then(function (res) {

	                                    if (res.data === "false") {
	                                        toasterService.clear();
	                                        toasterService.error();
	                                    }
	                                    else if (res.data == "1") {
	                                        toasterService.clear();
	                                        toasterService.customError("Any item of this PO is already generated from another costsheet");
	                                    }
	                                    else {
	                                        toasterService.clear();
	                                        toasterService.success("Worksheet");
	                                        $scope.worksheetGridData = res.data;

	                                        angular.forEach($scope.worksheetGridData, function (row) {
	                                            row.getTotalPrice = function () {
	                                                row.TotalPrice = (row.TotalQuantity * row.UnitPrice).toFixed(6);
	                                                return row.TotalPrice;
	                                            };
	                                        });
	                                    }
	                                });
	                            }, function () {
	                                console.log('Modal dismissed at: ' + new Date());
	                            });

	                        });
	                    }
	                });
	                
	            }

	            angular.forEach($scope.worksheetGridData, function (row) {
	                row.getTotalPrice = function () {
	                    row.TotalPrice = (row.TotalQuantity * row.UnitPrice).toFixed(6);
	                    return row.TotalPrice;
	                };
	            });
	        });
	    }
	};

	$scope.colDefs = [
        { field: "ItemCategoryName", displayName: "Item", width: 150, pinned: true, enableCellEdit: false },
        { field: "ItemCode", displayName: "Item Code", width: 100, pinned: true, enableCellEdit: false },
        { field: "ItemName", displayName: "Item Description", width: 120, pinned: true, enableCellEdit: false },

        { field: "Size", displayName: "Size", width: 120, pinned: false, enableCellEdit: false },
        { field: "Color", displayName: "Color", width: 120, pinned: false, enableCellEdit: false },
        { field: "ItemSize", displayName: "Item Size", width: 120, pinned: false, enableCellEdit: true },
        { field: "ItemColor", displayName: "Item Color", width: 120, pinned: false, enableCellEdit: true },

        { field: "Consumption", displayName: "Consumption", width: 100, enableCellEdit: true },
        { field: "ConsumptionUnitName", displayName: "Consumption Unit", width: 100, enableCellEdit: false },

        { field: "Wastage", displayName: "Wastage", width: 80 },
        { field: "UnitPrice", displayName: "Unit Cost", width: 80 },
        { field: "ItemQuantity", displayName: "Item Quantity", width: 80, enableCellEdit: true, visible: false },
        { field: "TotalQuantity", displayName: "Total Quantity", width: 120 },
	    { field: "getTotalPrice()", displayName: "Total Price", width: 120, enableCellEdit: false },
	    { field: "FormulaText", displayName: "Formula", width: 120 },
	];

	$scope.$on('ngGridEventEndCellEdit', function (event) {
	    if (event.targetScope.col.field == 'Consumption' || event.targetScope.col.field == 'Wastage') {
	        var consumption = Number(event.targetScope.row.entity['Consumption']);
	        var wastage = Number(event.targetScope.row.entity['Wastage']);
	        var itemQuantity = Number(event.targetScope.row.entity['ItemQuantity']);

	        var totalRawMaterials = consumption + ((consumption * wastage) / 100);

	        event.targetScope.row.entity['TotalQuantity'] = (itemQuantity * totalRawMaterials).toFixed(4);
	    }
        
	    if (event.targetScope.col.field == 'TotalQuantity') {
	        var consumption = Number(event.targetScope.row.entity['Consumption']);
	        var itemQuantity = Number(event.targetScope.row.entity['ItemQuantity']);
	        var totalQuantity = Number(event.targetScope.row.entity['TotalQuantity']);

	        event.targetScope.row.entity['Wastage'] = (((totalQuantity / itemQuantity) - consumption) * 100) / consumption;
	    }
	});

	$scope.worksheetGrid = {
	    data: 'worksheetGridData',
	    enablePinning: true,
	    enableCellSelection: true,
	    enableRowSelection: false,
	    enableCellEditOnFocus: true,
	    columnDefs: 'colDefs'
	};

	$scope.updateWorksheetButtonClick = function () {
	    toasterService.wait();

	    worksheetCrudService.updateWorksheet(JSON.stringify($scope.worksheetGridData)).then(function (res) {
	        if (res.data === "true") {
	            toasterService.clear();
	            toasterService.success("Worksheet");
	        } else {
	            toasterService.clear();
	            toasterService.error();
	        }
	    })
	};

	$scope.deleteWorksheetButtonClick = function () {
	    if ($scope.searchCriteria.poDropDownVal == '' || $scope.searchCriteria.poDropDownVal == undefined) {
	        toasterService.customError('Please select a Purchase Order');
	        return;
	    } else if ($scope.searchCriteria.itemDropDownVal == '' || $scope.searchCriteria.itemDropDownVal == undefined) {
	        toasterService.customError('Please select an Item');
	        return;
	    } else {
	        alertify.confirm('Do you want to delete the worksheet', function (e) {
	            if (e) {
	                toasterService.wait();
	                worksheetCrudService.deleteWorksheet($scope.searchCriteria.poDropDownVal, $scope.searchCriteria.itemDropDownVal).then(function (res) {
	                    if (res.data === "true") {
	                        toasterService.clear();
	                        $scope.worksheetGridData = null
	                        toasterService.customSuccess("Worksheet is deleted successfully");
	                    } else {
	                        toasterService.clear();
	                        toasterService.error();
	                    }
	                });
	            }
	        });
	    }
	    
	};
}]);

app.controller('formulaModalController', ['$scope', '$modalInstance', 'formulaDropDown', function ($scope, $modalInstance, formulaDropDown) {
    $scope.formulaDropDown = formulaDropDown;

    $scope.formulaModalOkButton = function () {
        $modalInstance.close($scope.formulaDropDownVal);
    };

    $scope.formulaModalCancelButton = function () {
        $modalInstance.dismiss('cancel');
    };
}]);