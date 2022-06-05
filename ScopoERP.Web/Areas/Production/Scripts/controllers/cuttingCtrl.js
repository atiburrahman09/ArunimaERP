scopoAppControllers.controller('cuttingCtrl', ['$scope', 'cuttingService', 'styleConfigureService', 'alertify', '$filter', function ($scope, cuttingService, styleConfigureService, alertify, $filter) {
    var currentIndex = null;
    var previousIndex = null;

    $scope.POList = [];
    $scope.buyerList = [];
    $scope.cutting = {};
    $scope.cuttingList = [];
    $scope.bundleInformation = [];
    
    $scope.init = function () {
        $scope.accountId = 1;
        $scope.NoOfBundle = false;
        $scope.BundlePerQuantity = false;
        $scope.GenerateButton = false;
        cuttingService.getAllBuyer().then(function (data) {
            $scope.buyerList = data;
        });

    }



    function saveCutting() {
        if ($scope.cuttingForm.$valid) {
            console.log($scope.POID);
            if ($scope.POID != null || $scope.POID != undefined) {
                $scope.cutting.PurchaseOrderID = $scope.POID;
                if ($scope.cutting.CuttingPlanID > 0) {
                    cuttingService.updateCutting($scope.cutting).then(function (res) {
                        if (res.data == false) {
                            alertify.error("Coupon already generated. Please delete the coupon first.");
                        }
                        else {
                            //$scope.bundleInformation[0].CuttingPLanID = $scope.cutting.CuttingPlanID;
                            cuttingService.updateBundleInfo($scope.bundleInformation).then(function (res) {
                                alertify.alert("Bundle & cutting plan successfully updated.");
                            });


                            getAllCuttingsByPOID($scope.POID);
                        }

                    });
                }
                else {

                    cuttingService.saveCutting($scope.cutting).then(function (res) {
                        if (res.data.ErrorCode != null) {
                            alertify.error(res.data.Message);
                        } else {
                            if (res.data.ID > 0) {
                                $scope.bundleInformation[0].CuttingPLanID = res.data.ID;
                                console.log($scope.bundleInformation);
                                cuttingService.saveBundleInfo($scope.bundleInformation).then(function (res) {
                                    alertify.alert("Bundle & cutting plan successfully saved.");
                                });
                            }
                            else {
                                alertify.error(res.data.Message);
                            }
                            getAllCuttingsByPOID($scope.POID);
                        }

                    });
                }


            }
            else {
                alertify.error("Please select purchase order.");
            }
        }

        else {
            alertify.error("Invalid form submission.");
        }
    }
    //fetching All Cutting Plans By POID
    function getAllCuttingsByPOID(POID) {
        cuttingService.getCuttingList(POID).then(function (res) {
            $scope.cuttingList = formatDate(res.data, "YYYY-MM-DD", ["CuttingDate"]);
        });
    }

    function selectedCuttingPlan(cuttingPlan) {
        var index = indexOfObjectInArray($scope.cuttingList, 'CuttingPlanID', cuttingPlan.CuttingPlanID);
        console.log(index);
        if (index == currentIndex) {
            $scope.cuttingList[currentIndex].selected = true;
        } else {
            previousIndex = currentIndex;
            currentIndex = index;
            $scope.cuttingList[currentIndex].selected = true;

            cuttingService.getBundleInfoByCuttingID($scope.cuttingList[currentIndex].CuttingPlanID).then(function (res) {
                console.log(res);
                $scope.cutting = angular.copy($scope.cuttingList[currentIndex]);
                $scope.bundleInformation = res;
                $scope.NoOfBundle = true;
                $scope.BundlePerQuantity = true;
                $scope.GenerateButton = true;
            });
            //$scope.cutting = angular.copy($scope.allCuttings[currentIndex]);

            if (previousIndex != null) {
                $scope.cuttingList[previousIndex].selected = false;
            }
        }
    }

    $scope.getBuyerIDAndStyle = function (item, model, label) {
        $scope.buyerID = item.BuyerID;
        $scope.BuyerName = item.BuyerName;
        onBuyerChange(item.BuyerID);
    };

    $scope.getStyleIDAndPO = function (item, model, label) {
        $scope.StyleID = item.StyleID;
        onStyleChange(item.StyleID);
    };

    $scope.getPOIDAndCuttingList = function (item, model, label) {
        $scope.POID = item.PurchaseOrderID;
        onPOChange(item.PurchaseOrderID);
    };


    function onBuyerChange(buyerId) {
        if (buyerId != "") {
            cuttingService.getStyles($scope.accountId, buyerId).then(function (data) {
                $scope.styleList = data;
            });
        }
    }

    function onStyleChange(StyleID) {
        currentIndex = null;
        previousIndex = null;
        if (StyleID != "") {
            cuttingService.getPurchaseOrders(StyleID).then(function (data) {
                $scope.POList = data;
                console.log($scope.POList);
            });
        }
    }

    function onPOChange(POID) {
        if (POID != "") {
            cuttingService.getCuttingList(POID).then(function (res) {
                //formatDate(res.data, []);
                $scope.cuttingList = formatDate(res.data, []);
                if (res.data.length > 0) {
                    console.log(res.data);
                    $scope.cutting.CuttingNo = $scope.cuttingList[0].CuttingNo + 1;
                }
                else {
                    $scope.cutting.CuttingNo = 1;
                }
            });
        }
    }

    $scope.bindDate = function (id, model) {
        $scope[model][id] = $('#' + id).val();
    };

    function addCutting() {
        console.log($scope.PurchaseOrderID);
        if ($scope.PurchaseOrderID) {
            $scope.cuttingList.push({ PurchaseOrderId: $scope.PurchaseOrderID });
            resetForm($scope.cuttingForm);
        } else {
            alertify.error("No Purchase Order is Selected!");
        }


    }

    function removeCutting(index) {
        $scope.cuttingList.splice(index, 1);
    }

    function resetForm(form) {
        $scope.cutting = {};

        if (currentIndex != null) {
            currentIndex = null;
            previousIndex = null;
        }
        form.$setPristine();
        form.$setUntouched();
    }

    $scope.generateBundle = function (bundleNo, bundlePerQuantity) {
        var lastBundleNo;
        //bundle = { "BundleNo": "", "Quantity": 0, "Size": "" };
        cuttingService.getLastBundleNo().then(function (res) {
            lastBundleNo = res.data;
            $scope.bundleInformation = [];

            for (var i = 1; i <= bundleNo; i++) {
                $scope.bundleInformation.push({ "BundleNo": lastBundleNo + i, "Quantity": bundlePerQuantity, "Size": "" });
            }
        });
    }

    function indexOfObjectInArray(array, property, value) {
        for (var i = 0; i < array.length; i++) {
            if (array[i][property] == value) {
                return i;
            }
        }
        return -1;
    }

    $scope.onBuyerChange = onBuyerChange;
    $scope.onStyleChange = onStyleChange;
    $scope.saveCutting = saveCutting;
    $scope.selectedCuttingPlan = selectedCuttingPlan;
    //$scope.getAllCuttingsByPOID = getAllCuttingsByPOID;
    $scope.addCutting = addCutting;
    $scope.removeCutting = removeCutting;
}]);