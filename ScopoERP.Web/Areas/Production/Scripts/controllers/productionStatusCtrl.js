scopoAppControllers.controller('productionStatusCtrl', ['$scope', 'productionStatusService', 'alertify', function ($scope, productionStatusService, alertify) {

    $scope.init = function () {

        $scope.accountID = 1;
        $scope.currentIndex = null;
        $scope.previousIndex = null;
        $scope.buyerID = null;
        $scope.styleList = null;
        $scope.filter = { selectedStyles: [] };
        $scope.filteredResult = [];
        $scope.createMode = false;
        $scope.createModeFilter = {};
        $scope.purchaseOrders = [];

        getAllBuyer();
        getAllStyle();
        getLineDropDown();
        $scope.getProductionFloorDropDown();
    }
    
    function getStyleDropDownByBuyerID(accountID, buyerID) {

        console.log(buyerID);
        if (buyerID != null) {
            productionStatusService.getStyleDropDownByBuyerID(accountID, buyerID).then(function (res) {
                $scope.styleList = res.data;
            });
        } else {
            $scope.styleList = [];
        }
    }


    function selectedStyle(style) {

        var styleIndex = indexOfObjectInArray($scope.styleList, 'Value', style.Value);
        $scope.styleList[styleIndex].selected = !$scope.styleList[styleIndex].selected;

        if ($scope.filter != null) {
            var tempIndex = $scope.filter.selectedStyles.indexOf($scope.styleList[styleIndex].Value);

            if (tempIndex < 0) {
                $scope.filter.selectedStyles.push($scope.styleList[styleIndex].Value);
            } else {
                $scope.filter.selectedStyles.splice(tempIndex, 1);
            }
            $scope.styleCount = $scope.filter.selectedStyles.length;
        }
    }


    function getProductionFloorDropDown() {
        productionStatusService.getProductionFloorDropDown().then(function (res) {
            $scope.productionFloorList = res.data;
        })
    }

    function getProductionLineByFloor(floor) {
        productionStatusService.getProductionLineByFloor(floor).then(function (res) {
            $scope.productionLineList = res.data;
            console.log(res.data);
        });
    }

    function getFilteredResult() {

        if($scope.filterForm.$valid)
        {
            if (!$scope.createMode) {
                //$scope.filter.ProductionDate = $('#ProductionDate').val().trim();
                if ($scope.filter.ProductionDate != null && $scope.filter.ProductionDate != "") {
                    productionStatusService.getFilteredProductionStatus($scope.filter).then(function (res) {
                        $scope.filteredResult = res.data;
                        $scope.createMode = false;
                        if ($scope.filteredResult.length > 0) {
                            for (var i in $scope.filteredResult) {
                                if ($scope.filteredResult[i].Date) {
                                    $scope.filteredResult[i].Date = moment($scope.filteredResult[i].Date).format("MM/DD/YYYY");
                                    $scope.filteredResult[i].BuyerID = $scope.filter.BuyerID;
                                    $scope.filteredResult[i].BuyerName = $scope.filter.BuyerName;
                                    $scope.filteredResult[i].Floor = $scope.filter.productionFloor;
                                    $scope.filteredResult[i].Line = $scope.filter.productionLine;
                                    $scope.filteredResult[i].StyleID = $scope.filter.StyleID;
                                }
                            }
                        }

                        $scope.filteredResult = $scope.filteredResult.filter(function (obj) {
                            return obj.ProductionDailyReportID !== 0;
                        });
                    });
                } else {
                    alertify.alert("Please insert a date!");
                }
            } else {
                getPurchaseOrdersByStyleIDs($scope.filter.selectedStyles)
            }
        }
        else {
            alertify.alert("Please fill up the informations.");
        }
    }


    function indexOfObjectInArray(array, property, value) {
        for (var i = 0; i < array.length; i++) {
            if (array[i][property] == value) {
                return i;
            }
        }
        return -1;
    }

    function resetForm(formName, objectName) {
        if (formName) {
            $scope[formName].$setPristine();
            $scope[formName].$setUntouched();
            $('#ProductionDate').val('');
            $scope.filter = {};
            $scope.removeAllRow();
        }
        if (objectName) {
            for (var i in $scope.styleList) {
                $scope.styleList[i].selected = false;
            }
            $scope.filter.selectedStyles = [];
            $scope.styleCount = 0;
        }
    }

    function getAllStyle() {
        productionStatusService.getAllStyle().then(function (res) {
            $scope.allStyle = res.data;
        })
    }

    function getAllBuyer() {
        productionStatusService.getAllBuyer().then(function (res) {
            $scope.buyerList = res.data;
        });
    }

    function getLineDropDown() {
        productionStatusService.getLineDropDown().then(function (res) {
            $scope.allLine = res.data;
        })
    }

    function getPurchaseOrdersByStyleIDs(list) {
        if (list.length > 0) {
            productionStatusService.getPurchaseOrdersByStyleIDs(list).then(function (res) {
                $scope.purchaseOrders = res.data;
                console.log(res.data);
            });
        } else {
            alertify.alert("Please Select Styles");
        }
    }

    function addRow() {

        if (!$scope.filter.BuyerID) {
            alertify.alert('Select buyer');
            return;
        }
        if (!$scope.filter.StyleID) {
            alertify.alert('Select style');
            return;
        }

        $scope.filteredResult.push({});
        if ($scope.filteredResult.length > 0) {
            for (var i in $scope.filteredResult) {
                $scope.filteredResult[i].Date = moment($scope.filter.ProductionDate).format("MM/DD/YYYY");
                    $scope.filteredResult[i].BuyerID = $scope.filter.BuyerID;
                    $scope.filteredResult[i].BuyerName = $scope.filter.BuyerName;
                    $scope.filteredResult[i].Floor = $scope.filter.productionFloor;
                    $scope.filteredResult[i].Line = $scope.filter.productionLine;
                    $scope.filteredResult[i].StyleID = $scope.filter.StyleID;
            }
        }
        console.log("in add row");
        console.log($scope.filteredResult);
        $scope.createMode = true;
        //getPurchaseOrdersByStyleID($scope.filter.Value);
    }

    function removeRow(index, poId) {

        

        if (!$scope.filteredResult[index].ProductionDailyReportID) {
            $scope.filteredResult.splice(index, 1);
            return;
        }

       
        alertify.confirm("You sure to remove the entry?", function (e) {
            if (e) {

                if (!$scope.filteredResult[index].ProductionDailyReportID) {
                    $scope.filteredResult.splice(index, 1);
                    return;
                }

                if ($scope.filteredResult[index].ProductionDailyReportID != 0) {
                    console.log("here pro report id is " + $scope.filteredResult[index].ProductionDailyReportID);
                    productionStatusService.removeProductionStatus($scope.filteredResult[index].ProductionDailyReportID).then(function (res) {
                        alertify.success(res.data);
                        $scope.createMode = false;
                    }, function (err) {
                        handleHttpError(err);
                    });
                }
                else {
                    alertify.error("Purchase order isn't mapped with production status. Remove failed.");
                }
                $scope.filteredResult.splice(index, 1);
            }
        });
    }

    $scope.removeAllRow = function () {
        $scope.filteredResult = [];
        $scope.createMode = false;
    }

    $scope.saveProductionStatus = function () {
        if ($scope.purchaseForm.$valid)
        {
            if ($scope.filteredResult.length > 0) {
                console.log("in save");
                console.log($scope.filteredResult);
               
                productionStatusService.saveProductionStatus($scope.filteredResult).then(function (res) {

                    console.log("in save res data here");
                    console.log(res.data);

                    if (res.data) {
                        alertify.success("Changes saved.");
                        $scope.filterForm.$setPristine();
                        $scope.filterForm.$setUntouched();
                        $('#ProductionDate').val('');
                        $scope.filter = {};
                        $scope.removeAllRow();
                    } else {
                        alertify.alert("Please try again !");
                    }
                })

            } else {
                alertify.alert("Nothing to save!");
            }

        }
        else {
            alertify.error("Please fill up required fields.");
        }
        
    }

    function getDateValue(index) {
        var selector = '#Date' + index;

        $scope.filteredResult[index].Date = $(selector).val().trim();
        console.log($(selector).val().trim());
    }


    //Searching, new method added

    $scope.getStyleByKeyword = function (val) {
        if (val.length < 3) {
            return;
        }
        console.log($scope.filter);
        return productionStatusService.getStyleDropDownByKeyword(val, $scope.filter.BuyerID).then(function (res) {
            return res.data;
        });
    };

    $scope.getBuyerByKeyword = function (val) {
        if (val.length < 3) {
            return;
        }

        return productionStatusService.getBuyerDropDownByKeyword(val).then(function (res) {
            console.log(res.data);
            return res.data;
        });
    };

    $scope.getPurchaseOrdersByStyleID = function (item, model, label) {
        //console.log(item);
        //$scope.filter.StyleID = item.Value;
        //productionStatusService.getPurchaseOrdersByStyleID(item.Value).then(function (res) {
        //    $scope.purchaseOrders = res.data;
        //    console.log(res.data);
        //})
    };
    $scope.getStyleID = function (item, model, label) {
        console.log(item.Value);
        $scope.filter.StyleID = item.Value;
        productionStatusService.getPurchaseOrdersByStyleID(item.Value).then(function (res) {
            $scope.purchaseOrders = res.data;
            console.log(res.data);
        })
    };

    $scope.getBuyerIDAndStyle = function (item, model, label) {
        $scope.filter.BuyerID = item.BuyerID;
        $scope.filter.BuyerName = item.BuyerName;
        getStyleDropDownByBuyerID($scope.accountID, item.BuyerID);
    };

    $scope.bindDate = function (id, model) {
        $scope[model][id] = $('#' + id).val();
    };


    $scope.ifPOexists = function (selectedPO, index) {        
        if (selectedPO && $scope.filteredResult.filter(x => x.PurchaseOrderID == selectedPO).length > 1) {
            alertify.error("Purchase order already listed once.");
            $scope.filteredResult[index].PurchaseOrderID = null;
            return;
        }
    }


    $scope.addRow = addRow;
    $scope.removeRow = removeRow;
    $scope.resetForm = resetForm;
    $scope.getFilteredResult = getFilteredResult;
    $scope.getProductionFloorDropDown = getProductionFloorDropDown;
    $scope.getProductionLineByFloor = getProductionLineByFloor;
    $scope.selectedStyle = selectedStyle;
    $scope.getStylesByBuyerID = getStyleDropDownByBuyerID;
    $scope.getDateValue = getDateValue;
}]);