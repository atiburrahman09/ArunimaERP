var app = angular.module("costsheet.controllers", [
    "ngGrid",
    "globalModule",
    "costsheet.services",
    "costsheet.filters",
    "localytics.directives",
    "ui.select2"
]);

app.directive('ngBlur', function () {
    return function (scope, elem, attrs) {
        elem.bind('blur', function () {
            scope.$apply(attrs.ngBlur);
        });
    };
});

app.controller('costsheetController', ['$scope', '$parse', '$timeout', 'costsheetSearchService', 'costsheetCrudService', 'toasterService', 'dropdownService', function ($scope, $parse, $timeout, costsheetSearchService, costsheetCrudService, toasterService, dropdownService) {

    $scope.costsheet = {};
    $scope.searchCriteria = {};
    $scope.init = function () {
        dropdownService.getSingleStyleDropDown().then(function (res) {
            $scope.searchCriteria.styleDropDown = res.data;
        });

        costsheetCrudService.getUnitDropDown().then(function (res) {
            $scope.unitDropDown = res.data;
        });

        costsheetCrudService.getItemCategoryDropDown().then(function (res) {
            $scope.itemCategoryDropDown = res.data;
        });
    };

    $scope.styleOptions = {
        minimumInputLength: 2
    };

    $scope.onCboStyleChange = function () {
        toasterService.wait();
        costsheetSearchService.getCostsheetNoDropDown($scope.searchCriteria.styleID).then(function (res) {
            $scope.costsheet.dropDown = res.data;
            $scope.searchCriteria.costsheetNo = null;
            $scope.gridData = [{
                'CostSheetID': '',
                'StyleID': '',
                'CostSheetNo': '',
                'ItemCategoryID': '',
                'ItemCategory': '',
                'ItemID': '',
                'ItemCode': '',
                'ItemDescription': '',
                'Consumption': '',
                'ConsumptionUnitID': '',
                'ConversionQuantity': '',
                'ConversionUnitID': '',
                'Wastage': '',
                'UnitPrice': '',
                getActualConsumption: function () {
                    if (this.ConversionQuantity <= 0)
                        this.ConversionQuantity = NaN;
                    return (this.Consumption / this.ConversionQuantity).toFixed(4);
                },
                getTotalRawMaterials: function () {
                    return ((this.Consumption / this.ConversionQuantity) + (this.Wastage / 100)).toFixed(4);
                },
                getTotalCost: function () {
                    return (((this.Consumption / this.ConversionQuantity) + (this.Wastage / 100)) * this.UnitPrice).toFixed(4);
                }
            }];
            toasterService.clear();
        });
    };

    $scope.onCboCosheetNoChange = function () {
        toasterService.wait();
        costsheetCrudService.getCostsheet($scope.searchCriteria.costsheetNo).then(function (res) {
            $scope.gridData = res.data;
            angular.forEach($scope.gridData, function (row) {
                row.getActualConsumption = function () {
                    if (row.ConversionQuantity <= 0)
                        row.ConversionQuantity = NaN;
                    return (row.Consumption / row.ConversionQuantity).toFixed(4);
                };

                row.getTotalRawMaterials = function () {
                    return ((row.Consumption / row.ConversionQuantity) + (row.Wastage / 100)).toFixed(4);
                };

                row.getTotalCost = function () {
                    return (((row.Consumption / row.ConversionQuantity) + (row.Wastage / 100)) * row.UnitPrice).toFixed(4);
                }
            });
            toasterService.clear();
        });
    };

    $scope.$on('ngGridEventEndCellEdit', function (data) {
        if (data.targetScope.col.field == 'ItemCode') {
            console.log(data.targetScope.row);
            costsheetCrudService.getItemByItemCode(data.targetScope.row.entity.ItemCode).then(function (res) {
                data.targetScope.row.entity.ItemDescription = res.data.ItemDescription;
                data.targetScope.row.entity.ItemCategory = res.data.ItemCategoryName;
                data.targetScope.row.entity.ItemID = res.data.ItemID;
                data.targetScope.row.entity.ItemCategoryID = res.data.ItemCategoryID;
            });
        }

        if (data.targetScope.col.field == 'ItemCategoryID') {
            costsheetCrudService.getItemDropDown(data.targetScope.row.entity.ItemCategoryID).then(function (res) {
                
                var model = $parse("itemDropDown_"+data.targetScope.row.rowIndex);

                model.assign($scope, res.data);

                $scope.$apply();
                console.log($scope.itemDropDown_0);
            });
        }
        
    });

    $scope.colDefs = [
        { displayName: 'Action', pinned: true, cellTemplate: "/Content/ng-template/costsheet/RemoveRowBtnTpl.html", width: 55, enableCellEdit: false },
        { field: "ItemCategoryID", displayName: "Item Category", width: 150, pinned: true, editableCellTemplate: "/Content/ng-template/costsheet/ItemCategoryDropDownTpl.html", cellFilter: 'mapItemCategory: itemCategoryDropDown' },
        { field: "ItemCode", displayName: "Item Code", width: 100, pinned: true },
        { field: "ItemID", displayName: "Item", width: 120, pinned: true, editableCellTemplate: "/Content/ng-template/costsheet/ItemDropDownTpl.html", cellFilter: 'mapItem: itemDropDown[$index]' },
        { field: "Consumption", displayName: "Consumption", width: 100 },
        { field: "ConsumptionUnitID", displayName: "Consumption Unit", width: 140, editableCellTemplate: "/Content/ng-template/costsheet/UnitDropDownTpl.html", cellFilter: 'mapUnit: unitDropDown' },
        { field: "ConversionQuantity", displayName: "Conversion Quantity", width: 160 },
        { field: "getActualConsumption()", displayName: "Actual Consumption", width: 160, enableCellEdit: false },
        { field: "ConversionUnitID", displayName: "Conversion Unit", width: 120, editableCellTemplate: "/Content/ng-template/costsheet/UnitDropDownTpl.html", cellFilter: 'mapUnit: unitDropDown' },
        { field: "Wastage", displayName: "Wastage", width: 80 },
        { field: "getTotalRawMaterials()", displayName: "Total Raw Materials", width: 140, enableCellEdit: false },
        { field: "UnitPrice", displayName: "Unit Cost", width: 80 },
        { field: "getTotalCost()", displayName: "Total Cost", width: 80, enableCellEdit: false }
    ];

    $scope.gridOptions = {
        data: 'gridData',
        enablePinning: true,
        enableCellSelection: true,
        enableRowSelection: false,
        enableCellEditOnFocus: true,
        columnDefs: 'colDefs'
    };

    $scope.addAnotherRow = function () {
        $scope.gridData.push({
            'CostSheetID': '',
            'StyleID': '',
            'CostSheetNo': '',
            'ItemCategoryID': '',
            'ItemCategory': '',
            'ItemID': '',
            'ItemCode': '',
            'ItemDescription': '',
            'Consumption': '',
            'ConsumptionUnitID': '',
            'ConversionQuantity': '',
            'ConversionUnitID': '',
            'Wastage': '',
            'UnitPrice': '',
            getActualConsumption: function () {
                if (this.ConversionQuantity <= 0)
                    this.ConversionQuantity = NaN;
                return (this.Consumption / this.ConversionQuantity).toFixed(4);
            },
            getTotalRawMaterials: function () {
                return ((this.Consumption / this.ConversionQuantity) + (this.Wastage / 100)).toFixed(4);
            },
            getTotalCost: function () {
                return (((this.Consumption / this.ConversionQuantity) + (this.Wastage / 100)) * this.UnitPrice).toFixed(4);
            }
        });
    };

    $scope.removeRow = function (row) {
        var index = row.rowIndex;
        $scope.gridOptions.selectItem(index, false);
        $scope.gridData.splice(index, 1);
    };

    $scope.txtItemCodeBlur = function (row) {
        console.log(row);
    }

    $scope.save = function () {
        toasterService.wait();
        for (var i in $scope.gridData) {
            $scope.gridData[i].StyleID = parseInt($scope.searchCriteria.styleID);
            $scope.gridData[i].CostSheetNo = $scope.searchCriteria.costsheetNo;
        }

        costsheetCrudService.save(JSON.stringify($scope.gridData)).then(function (res) {
            if (res.data.errorMessage) {
                toasterService.clear();
                toasterService.customError(res.data.errorMessage);
                return;
            }
            if (res.data === "true") {
                toasterService.clear();
                toasterService.success("Costsheet");
                $scope.onCboStyleChange();
            } else {
                toasterService.clear();
                toasterService.error();
            }
        })

    }
}]);