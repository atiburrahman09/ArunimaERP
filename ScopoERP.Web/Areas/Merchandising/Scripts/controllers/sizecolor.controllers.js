//var myModule = angular.module("sizecolor.controllers", [
//	'sizecolor.services',
//	'globalModule',
//    "ui.select2"
//]);

scopoAppControllers.controller('sizeColorController', ['$scope', '$location', 'sizeColorService', 'alertify', function ($scope, $location, sizeColorService, alertify) {
    //$scope.sizeColorList = sizeColorList;

    $scope.init = function (purchaseOrderID) {
        console.log("POID-" + purchaseOrderID);
        $scope.purchaseOrderID = purchaseOrderID;
        sizeColorService.getSizeColorByPurchaseOrder(purchaseOrderID).then(function (res) {
            if (res.data.length > 0) {
                $scope.sizeColorList = res.data;
                $scope.PoStyleID = $scope.sizeColorList[0].PoStyleID;
                console.log(res.data);
            }
            else
                $scope.sizeColorList = [{
                    Color: "", SizeQuantity: [{ Size: "", Quantity: "" }]
                }];
            $scope.PoStyleID = purchaseOrderID;
            console.log($scope.sizeColorList);
        });

        sizeColorService.getPODropDown().then(function (res) {
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
            alertify.error("Sorry you can not remove the last color");
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
            alertify.error("Sorry you can not remove the last size");
        }
    };

    $scope.saveButtonClick = function (sizeColorList) {

        var firstSizeQuantity = sizeColorList[0].SizeQuantity;
        for (var i in sizeColorList) {
            sizeColorList[i].PoStyleID = $scope.PoStyleID;
            for (var j in sizeColorList[i].SizeQuantity) {
                $scope.sizeColorList[i].SizeQuantity[j].Size = firstSizeQuantity[j].Size;
            }
        }

        console.log(sizeColorList);
        $scope.sizeColorVM = {"PoStyleID" : $scope.PoStyleID, "SizeColorList": sizeColorList}
        
        sizeColorService.save(JSON.stringify($scope.sizeColorVM)).then(function (res) {
            if (res.data === true) {
                alertify.success("Size Color saved");
            } else {
                alertify.error("Please see inner exception.");
            }
        });
    };

    $scope.copyButtonClick = function () {
        if ($scope.copyFromPO) {
            sizeColorService.copy($scope.copyFromPO, $scope.purchaseOrderID).then(function (res) {
                if (res.data === true) {

                    sizeColorService.getSizeColorByPurchaseOrder($scope.purchaseOrderID).then(function (res) {
                        if (res.data.length > 0)
                            $scope.sizeColorList = res.data;
                        else
                            $scope.sizeColorList = [{
                                Color: "", SizeQuantity: [{ Size: "", Quantity: "" }]
                            }];
                    });

                    alertify.success("Size Color Saved");
                } else if (res.data.errorMessage) {

                    alertify.error(res.data.errorMessage);
                } else {

                }
            });
        } else {
            alertify.error("Please Select an Purchase Order");
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