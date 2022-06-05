var app = angular.module("booking.controllers.new", [
    "ngGrid",
    "ui.bootstrap",
    "globalModule",
    "booking.services",
    "ui.select2",
    "angular.filter"
]);

app.controller('bookingControllerNew', ['$scope', '$modal', 'bookingSearchService', 'bookingCrudService', 'toasterService', 'dropdownService', function ($scope, $modal, bookingSearchService, bookingCrudService, toasterService, dropdownService) {
    $scope.searchCriteria = {};
    $scope.init = function () {
        dropdownService.getStyleDropDown().then(function (res) {
            $scope.searchCriteria.multipleStyleDropDown = res.data;
            dropdownService.getReferenceNoDropDown().then(function (res) {
                $scope.searchCriteria.piDropDown = res.data;
            });
        });
        
    };

    $scope.refernceNoOptions = {
        minimumInputLength: 8
    };

    $scope.styleOptions = {
        minimumInputLength: 2
    };

    $scope.refernceNoDropDownChange = function () {
        $scope.bookingGridData = null;
        $scope.isWorksheetShow = true;
        $scope.isBookingShow = false;
        $scope.grandTotalQuantityForRevise = 0;

        if ($scope.searchBooking.piID) {
            toasterService.wait();
            bookingSearchService.getBookingByPIID($scope.searchBooking.piID).then(function (res) {
                if (res.data.length) {
                    $scope.worksheetGridData = res.data;
                    toasterService.clear();

                    angular.forEach($scope.worksheetGridData, function (row) {
                        $scope.RevisionNo = row.RevisionNo;

                        $scope.grandTotalQuantityForRevise += row.TotalQuantity;

                        row.getTotalPrice = function () {
                            row.TotalPrice = (row.TotalQuantity * row.UnitPrice).toFixed(6);
                            return row.TotalPrice;
                        };
                    });
                } else {
                    $scope.worksheetGridData = res.data;
                    toasterService.clear();
                    toasterService.warning("There is no worksheet of this Reference No");
                }
            });
        } else {
            $scope.worksheetGridData = null;
        }

    };

    $scope.colDefs = [
        { field: "PONo", displayName: "Purchase Order No", width: 150, pinned: true, enableCellEdit: false },
        { field: "ItemCode", displayName: "Item Code", width: 100, pinned: true, enableCellEdit: false },
        { field: "ItemDescription", displayName: "Item", width: 200, pinned: true, enableCellEdit: false },

        { field: "ItemSize", displayName: "Item Size", width: 100, pinned: false, enableCellEdit: false },
        { field: "ItemColor", displayName: "Item Color", width: 100, pinned: false, enableCellEdit: false },

        { field: "TotalQuantity", displayName: "Total Quantity", width: 130 },
        { field: "ConsumptionUnitName", displayName: "Consumption Unit", width: 150, enableCellEdit: false },
        { field: "UnitPrice", displayName: "Unit Cost", width: 80, enableCellEdit: false },

        { field: "getTotalPrice()", displayName: "Total Price", width: 130, enableCellEdit: false },

        { field: "ReferenceNo", displayName: "Reference No", width: 130, enableCellEdit: false }
    ];

    $scope.worksheetGridOptions = {
        data: 'worksheetGridData',
        enablePinning: true,
        enableCellSelection: true,
        enableRowSelection: false,
        enableCellEditOnFocus: true,
        columnDefs: 'colDefs'
    };

    $scope.reviseBooking = function () {
        $scope.isWorksheetShow = true;
        $scope.isBookingShow = false;
        $scope.grandTotalQuantityForRevise = 0;

        toasterService.wait();
        bookingCrudService.reviseBooking(JSON.stringify($scope.worksheetGridData)).then(function (res) {
            if (res.data === "false") {
                toasterService.clear();
                toasterService.error();
            } else {
                toasterService.clear();
                toasterService.success("Booking");
                $scope.worksheetGridData = res.data;

                angular.forEach($scope.worksheetGridData, function (row) {
                    $scope.RevisionNo = row.RevisionNo;
                    $scope.grandTotalQuantityForRevise += row.TotalQuantity;
                    row.getTotalPrice = function () {
                        row.TotalPrice = (row.TotalQuantity * row.UnitPrice).toFixed(6);
                        return row.TotalPrice;
                    };
                });
            }
        });
    };

    $scope.styleDropDownChange = function () {
        if ($scope.searchCriteria.multipleStyle.length > 0) {
            dropdownService.getMultiplePODropDownByMultipleStyles($scope.searchCriteria.multipleStyle.join(",")).then(function (res) {
                $scope.searchCriteria.multiplePODropDown = res.data;
            });
        } else {
            $scope.searchCriteria.multiplePODropDown = null;
        }
    };

    $scope.poDropDownChange = function () {
        if ($scope.searchCriteria.multiplePO.length > 0) {
            dropdownService.getItemByMultiplePO($scope.searchCriteria.multiplePO.join(",")).then(function (res) {
                $scope.searchCriteria.itemDropDown = res.data;
            });
        } else {
            $scope.searchCriteria.itemDropDown = null;
        }
    };

    $scope.searchBookingButtonClick = function () {
        $scope.worksheetGridData = null;
        $scope.isWorksheetShow = false;
        $scope.isBookingShow = true;
        $scope.grandTotalQuantity = 0;

        toasterService.wait();
        
        bookingSearchService.getBookingFromWorksheet($scope.searchCriteria.multiplePO.join(','), $scope.searchCriteria.item).then(function (res) {
            if (res.data.length) {
                $scope.bookingGridData = res.data;

                dropdownService.getReferenceNoDropDown().then(function (res) {
                    $scope.referenceNoDropDown = res.data;
                });

                toasterService.clear();

                angular.forEach($scope.bookingGridData, function (row) {
                    $scope.grandTotalQuantity += row.TotalQuantity;

                    row.getTotalPrice = function () {
                        row.TotalPrice = (row.TotalQuantity * row.UnitPrice).toFixed(6);
                        return row.TotalPrice;
                    };
                });

            } else {
                $scope.bookingGridData = res.data;
                toasterService.clear();
                toasterService.warning("There is no booking item available for these po. Please create worksheet first");
            }
        });
    };

    $scope.bookingGridOptions = {
        data: 'bookingGridData',
        enablePinning: true,
        enableCellSelection: true,
        enableRowSelection: false,
        enableCellEditOnFocus: true,
        columnDefs: 'bookingColDefs'
    };

    $scope.bookingColDefs = [
        { field: "PONo", displayName: "Purchase Order No", width: 150, pinned: true, enableCellEdit: false },
        { field: "ItemCode", displayName: "Item Code", width: 100, pinned: true, enableCellEdit: false },
        { field: "ItemDescription", displayName: "Item", width: 200, pinned: true, enableCellEdit: false },

        { field: "ItemSize", displayName: "Item Size", width: 100, pinned: false, enableCellEdit: false },
        { field: "ItemColor", displayName: "Item Color", width: 100, pinned: false, enableCellEdit: false },

        { field: "TotalQuantity", displayName: "Total Quantity", width: 130 },
        { field: "ConsumptionUnitName", displayName: "Consumption Unit", width: 150, enableCellEdit: false },
        { field: "UnitPrice", displayName: "Unit Cost", width: 80, enableCellEdit: false },

        { field: "getTotalPrice()", displayName: "Total Price", width: 130, enableCellEdit: false }
    ];

    $scope.$on('ngGridEventEndCellEdit', function (event) {
        $scope.grandTotalQuantity = 0;
        for (var i in $scope.bookingGridData) {
            $scope.grandTotalQuantity += Number($scope.bookingGridData[i].TotalQuantity);
        }
    });

    $scope.changeAllRowTotalQuantity = function () {
        if (isNaN($scope.grandTotalQuantity)) {
            $scope.grandTotalQuantity = '';
        } else {
            for (var i in $scope.bookingGridData) {
                $scope.bookingGridData[i].TotalQuantity = ($scope.bookingGridData[i].Ratio * $scope.grandTotalQuantity).toFixed(2);
            }
        }
    };

    $scope.changeAllRowTotalQuantityForRevise = function () {
        if (isNaN($scope.grandTotalQuantityForRevise)) {
            $scope.grandTotalQuantityForRevise = '';
        } else {
            for (var i in $scope.worksheetGridData) {
                $scope.worksheetGridData[i].TotalQuantity = ($scope.worksheetGridData[i].Ratio * $scope.grandTotalQuantityForRevise).toFixed(2);
            }
        }
    };

    $scope.createBooking = function () {
        toasterService.wait();
        $scope.bookingGridData[0].PIID = $scope.referenceNoDropDownVal;

        bookingCrudService.createBooking(JSON.stringify($scope.bookingGridData)).then(function (res) {

            if (res.data === "false") {
                toasterService.clear();
                toasterService.error();
            } else {
                toasterService.clear();
                toasterService.customSuccess(res.data.successMsg);
                $scope.bookingGridData = null;

                dropdownService.getReferenceNoDropDown().then(function (res) {
                    $scope.referenceNoDropDown = res.data;
                    $scope.referenceNoDropDownVal = '';
                });
            }
        });
    };

}]);