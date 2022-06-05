scopoAppControllers.controller('couponCtrl', ['$scope', 'couponService','alertify', function ($scope, couponService, alertify) {

    $scope.init = function () {
        //$scope.getAllStyles();
        $scope.saveButton = true;
        $scope.couponList = [];
        $scope.operationCategoryList = [];
        $scope.accountId = 1;
        couponService.getAllBuyer().then(function (data) {
            $scope.buyerList = data;
        });

        couponService.getAllOperationCategories().then(function (res) {
            $scope.operationCategoryList = res.data;
        });
    };


    //filter criteria
    $scope.filter = {};

    //geting Buyer ID
    $scope.getBuyerIDAndStyle = function (item, model, label) {
        $scope.buyerID = item.BuyerID;
        $scope.BuyerName = item.BuyerName;
        onBuyerChange(item.BuyerID);
    };
    function onBuyerChange(buyerId) {
        if (buyerId != "") {
            couponService.getStyles($scope.accountId, buyerId).then(function (data) {
                $scope.styleList = data;
            });
        }
    }

   
    //date picker date bind with model
    $scope.bindDate = function bindDate(id, model) {
        $scope[model][id] = $('#' + id).val();
    };

    //create coupon
    $scope.coupon = {};
    $scope.newCoupon = {};
    $scope.generateClicked = function () {
        if ($scope.couponForm.$valid) {
            $scope.couponList = [];
            var index = indexOfObjectInArray($scope.cuttingPlanList, 'Value', $scope.coupon.CuttingPlanID);
            $scope.coupon.CuttingNo = $scope.cuttingPlanList[index].Text;
            couponService.generateCouponInformation($scope.coupon.CuttingPlanID, $scope.coupon.StyleID, $scope.coupon.PurchaseOrderID, $scope.coupon.CuttingNo, $scope.coupon.OperationCategoryID).then(function (res) {
                console.log(res.data.ErrorCode);
                if (res.data.ErrorCode == true) {
                    alertify.error("Coupon already generated. Please delete previous coupon to generate new coupon.");
                }
                else {
                    $scope.couponList = res.data.list;
                    console.log($scope.couponList);
                    if ($scope.couponList.length > 0) {
                        $scope.groups = _.groupBy(_.sortBy($scope.couponList, "SectionNo"), "BundleNo");
                        console.log($scope.bundle);
                        //$scope.groups = _.groupBy($scope.bundle, "SectionNo");
                        //console.log($scope.groups);
                    }
                    else {
                        alertify.error("No Data Found");
                    }

                }

            }, function (err) {
                handleHttpError(err);
            });
        }
        else {
            alertify.error("Please fill up the required informations.");
        }


    };

    //fetch Purchase order dropdown
    $scope.poList = [];
    $scope.getPOList = function (item) {
        $scope.style = item.Text;
        $scope.coupon.StyleID = item.Value;
        couponService.getPOList($scope.coupon.StyleID).then(function (res) {
            $scope.poList = res.data;
            console.log($scope.poList);
        }, function (err) { console.log(err); });
    };

    //fetch cut plan dropdown
    $scope.cuttingPlanList = [];
    $scope.getCutPlanList = function () {
        couponService.getCutPlanList($scope.coupon.PurchaseOrderID).then(function (res) {
            $scope.cuttingPlanList = res.data;
        }, function (err) { console.log(err); });
    };


    //Save Coupon
    $scope.save = function () {
        console.log($scope.couponList);
        couponService.save($scope.couponList).then(function (res) {
            alertify.success("Coupon successfully saved.");
        }, function (err) { handleHttpError(err); });
    };

    //coupon delete
    $scope.deleteCoupon = function () {
        if ($scope.couponForm.$valid) {
            alertify.confirm('Do you want to delete the Coupon?', function () {
                couponService.deleteCoupon($scope.coupon.CuttingPlanID, $scope.coupon.OperationCategoryID).then(function (res) {
                    $scope.saveButton = true;
                    $scope.couponList = [];
                    $scope.groups = [];
                    alertify.success("Coupon successfully deleted.");
                }, function (err) { console.log(err); });
            }, function () {
                //alertify.error('Cancel')
            });
        }
        else {
            alertify.error("Please fill up the required informations.");
        }


    };

    //search coupon
    $scope.searchCoupon = function () {
       
        var index = indexOfObjectInArray($scope.cuttingPlanList, 'Value', $scope.coupon.CuttingPlanID);
        $scope.coupon.CuttingNo = $scope.cuttingPlanList[index].Text;
        if ($scope.couponForm.$valid) {
            couponService.searchCoupon($scope.coupon).then(function (res) {
                if (res.data.length > 0)
                {
                    $scope.saveButton = false;
                    $scope.couponList = res.data;
                    $scope.groups = _.groupBy(_.sortBy($scope.couponList, "SectionNo"), "BundleNo");
                    console.log($scope.bundle);
                }
                else {
                    alertify.error("No Coupon Found");
                    $scope.groups = [];
                }
            }, function (err) { console.log(err); });
        }
        else {
            alertify.error("Please fill up the required informations.");
        }


    };

    function indexOfObjectInArray(array, property, value) {
        for (var i = 0; i < array.length; i++) {
            if (array[i][property] == value) {
                return i;
            }
        }
        return -1;
    }

    //method for drag & drop
    $scope.$watch('models.dropzones', function (model) {
        $scope.modelAsJson = angular.toJson(model, true);
    }, true);

    $scope.saveCoupon = function (list) {
        console.log(list);

    }

}]);
