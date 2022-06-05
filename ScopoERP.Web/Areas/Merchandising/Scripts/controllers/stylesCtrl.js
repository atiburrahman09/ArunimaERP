scopoAppControllers.controller('stylesCtrl', ['$scope', 'stylesService', 'alertify', function ($scope, stylesService, alertify) {
    var currentIndex = null;
    var previousIndex = null;
    $scope.style = {};
    
    $scope.init = function () {
        $scope.browseFile = true;
        $scope.hideSaveOnUpdate = false;
        $scope.costSheetList = "";
        $scope.customerList = "";
        $scope.divisionList = "";
        $scope.styleList = [];
        $scope.costSheetDetailsList = [];
        $scope.itemCategoryList = [];
        $scope.itemList = [];
        //$scope.accountId = 1;
        $scope.consumptionUnitList = [];
        //$scope.imageAddress = "C:\FTP\files\";


        stylesService.getAllBuyer().then(function (data) {
            $scope.buyerList = data;
        });

        stylesService.getAllCustomer().then(function (data) {
            $scope.customerList = data;
        });

        stylesService.getAllDivision().then(function (data) {
            $scope.divisionList = data;
        });

        stylesService.getItemCategoryDropDown().then(function (data) {
            $scope.itemCategoryList = data;
        });

        stylesService.getConsumptionUnitDropDown().then(function (data) {
            $scope.consumptionUnitList = data;
        });
    }; // end init()


    function onBuyerChange(buyerId) {
        $scope.buyerId = buyerId;
        console.log(buyerId);
        $scope.style = "";
        deselectStyle();
        $scope.getStyles(buyerId)
       
    }
    $scope.getStyles = function (buyerId)
    {
        stylesService.getStyles(buyerId).then(function (res) {
            $scope.styleList = res.data;
            console.log($scope.styleList);
        });
    }

    function showStyleDetails(style) {
        console.log('style selected: ', style);
        $scope.browseFile = false;
        $scope.hideSaveOnUpdate = true;
        $scope.style = JSON.parse(style);
        console.log($scope.style);
        
        $scope.costSheetDetailsList = [{ "CostSheetNo": $scope.costSheet, "StyleID": $scope.style.StyleID }, { "CostSheetNo": $scope.costSheet, "StyleID": $scope.style.StyleID }, { "CostSheetNo": $scope.costSheet, "StyleID": $scope.style.StyleID }];
            
        stylesService.getCostSheetList($scope.style.StyleID).then(function (res) {
            $scope.costSheetList = res.data;
            $scope.costForm.$setPristine();
            $scope.costForm.$setUntouched();
        }, function (err) {
            console.log(err);
        });
    }

    function createStyle() {
        $scope.style = {};
        resetForm($scope.styleForm);
        deselectStyle();
    }

    function resetForm(form) {
        form.$setPristine();
        form.$setUntouched();
    }

    function saveStyle(style) {

        if ($scope.styleForm.$valid) {
            if (style.StyleID > 0) {
                stylesService.updateStyle(style).then(function (data) {
                    if (data.Success == 1) {
                        //$('#select2').select2("data", "");
                        alertify.alert(data.Message);
                        createStyle();
                    } else {
                        alertify.alert(data.Message);
                    }

                });
            } else {
                style.BuyerID = $scope.buyerId;
                stylesService.createStyle(style).then(function (data) {
                    if (data.Success == 1) {
                        alertify.alert(data.Message);
                        //$('#select2').select2("data", "");
                        resetForm($scope.styleForm);
                        createStyle();
                        $scope.getStyles($scope.buyerId);

                    } else {
                        alertify.alert(data.Message);
                    }

                });

            }
            $scope.style = {};
        }
        else {
            alertify.error("Please fill up the necessary informations.");
        }

    }

    function deselectStyle() {
        if (currentIndex != null) {
            $scope.styleList[currentIndex].selected = 0;
        }
        currentIndex = null;
        previousIndex = null;
    }

    function onCostSheetChange(costSheet) {
        
        stylesService.costSheetDetailsList(costSheet).then(function (data) {
            $scope.costSheetDetailsList = data;
            for (var i = 0; i < $scope.costSheetDetailsList.length; i++) {
                onItemCategoryChange($scope.costSheetDetailsList[i].ItemCategoryID, i);
            }
            console.log($scope.costSheetDetailsList);
            if($scope.costSheetDetailsList.length < 1)
            {
                $scope.costSheetDetailsList = [{ "CostSheetNo": $scope.costSheet, "StyleID": $scope.style.StyleID }, { "CostSheetNo": $scope.costSheet, "StyleID": $scope.style.StyleID }, { "CostSheetNo": $scope.costSheet, "StyleID": $scope.style.StyleID }];
            }

        });

    }

    function saveCostSheet() {
        if ($scope.costForm.$valid)
        {
            console.log('style: ',$scope.style);
            if ($scope.style.StyleID != undefined) {
                //$scope.costSheetDetailsList.StyleID = $scope.style.StyleID;
                //console.log($scope.costSheetDetailsList);
                stylesService.createCostSheet($scope.costSheetDetailsList).then(function (data) {
                    alertify.alert(data.Message);

                    if (data.Success == 1) {
                        $scope.costSheetList = "";
                        //$scope.itemList = [];
                        $scope.style.isChecked = false;
                        $scope.style = "";
                        $scope.costForm.$setPristine();
                        $scope.costForm.$setUntouched();
                    }
                });
            } else {
                alertify.alert("Please Select A Style.");
            }
        }
        else {
            alertify.error("Please fill up the required fields.");
        }
       

    }

    function onItemCategoryChange(ItemCategoryID, index) {
        stylesService.getItemDropDown(ItemCategoryID).then(function (data) {
            $scope.itemList[index] = data;
        });
    }

    function addRowToCostSheetDetailsList() {
        console.log('add row : style : ', $scope.style);
        if ($scope.style != undefined) {
            $scope.costSheetDetailsList.push({ "CostSheetNo": $scope.costSheet, "StyleID": $scope.style.StyleID });
        } else {
            alertify.alert("Please Select A Style.");
        }
    }

    function onCostSheetRowValueChange(index) {
        if ($scope.costSheetDetailsList[index].Consumption && $scope.costSheetDetailsList[index].ConversionQuantity) {
            $scope.costSheetDetailsList[index].Consumption = parseFloat($scope.costSheetDetailsList[index].Consumption);
            $scope.costSheetDetailsList[index].ConversionQuantity = parseFloat($scope.costSheetDetailsList[index].ConversionQuantity);
            $scope.costSheetDetailsList[index].ActualConsumption = parseFloat(($scope.costSheetDetailsList[index].Consumption / $scope.costSheetDetailsList[index].ConversionQuantity).toFixed(4));
        }

        if ($scope.costSheetDetailsList[index].ActualConsumption && $scope.costSheetDetailsList[index].Wastage) {
            $scope.costSheetDetailsList[index].Wastage = parseFloat($scope.costSheetDetailsList[index].Wastage);
            $scope.costSheetDetailsList[index].TotalRawMaterials = ($scope.costSheetDetailsList[index].Consumption * $scope.costSheetDetailsList[index].Wastage) / (100 * $scope.costSheetDetailsList[index].ConversionQuantity);
            $scope.costSheetDetailsList[index].TotalRawMaterials = parseFloat(($scope.costSheetDetailsList[index].TotalRawMaterials + parseFloat($scope.costSheetDetailsList[index].ActualConsumption)).toFixed(4));
        }

        if ($scope.costSheetDetailsList[index].TotalRawMaterials && $scope.costSheetDetailsList[index].UnitPrice) {
            $scope.costSheetDetailsList[index].UnitPrice = parseFloat($scope.costSheetDetailsList[index].UnitPrice);
            $scope.costSheetDetailsList[index].TotalActualCost = parseFloat(($scope.costSheetDetailsList[index].TotalRawMaterials * $scope.costSheetDetailsList[index].UnitPrice).toFixed(4));
        }
    }

    function removeCostSheetItem(index) {
        $scope.costSheetDetailsList.splice(index, 1);
        $scope.costForm.$setPristine();
        $scope.costForm.$setUntouched();

    }

    $scope.uploadSuccess = function (file,message) {
        $scope.style.Image = (JSON.parse(message)).Guid;
        //console.log($scope.style.Image);
        saveStyle($scope.style);

        file.name = "";

    };

    $scope.uploadError = (function (message, file) {
        alertify.alert('Image Upload Failed?')
            .then(function () {
                location.reload();
            });
    });

    $scope.InsertItems = function (d) {
        if ($scope.style.BuyerID == undefined || $scope.style.StyleNo == undefined || $scope.style.Febrication == undefined || $scope.style.Capacity == undefined || $scope.style.CustomerID == undefined || $scope.style.DivisionID == undefined) {
            alertify.error("Please fill up the required fields first.");
        }
        else {
            console.log(d);
            d.upload();
            console.log($scope.style.Image);
            //saveStyle($scope.style);
            //alertify.success("Image added successfully.");
        }
    };

    $scope.checkExtension = function (d) {
        //console.log(d);
        if (d.getExtension() == 'jpg' || d.getExtension() == 'jpeg') {
            $scope.status = true;
            return true;
        }
        else {
            alertify.error("Please Upload JPG File");
            return false;
        }
    };

    $scope.enableFileBrowser = function (id) {
        $('#' + id).click();
    }

    function indexOfObjectInArray(array, property, value) {
        for (var i = 0; i < array.length; i++) {
            if (array[i][property] == value) {
                return i;
            }
        }
        return -1;
    }



    $scope.showStyleDetails = showStyleDetails;
    $scope.onBuyerChange = onBuyerChange;
    $scope.saveStyle = saveStyle;
    $scope.onCostSheetChange = onCostSheetChange;
    $scope.onItemCategoryChange = onItemCategoryChange;
    $scope.saveCostSheet = saveCostSheet;
    $scope.addRowToCostSheetDetailsList = addRowToCostSheetDetailsList;
    $scope.onCostSheetRowValueChange = onCostSheetRowValueChange;
    $scope.removeCostSheetItem = removeCostSheetItem;
    $scope.createStyle = createStyle;

}])
