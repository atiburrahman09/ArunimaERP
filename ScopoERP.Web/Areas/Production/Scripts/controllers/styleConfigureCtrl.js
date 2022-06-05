scopoAppControllers.controller('styleConfigureCtrl', ['$scope', 'styleConfigureService', 'alertify', function ($scope, styleConfigureService,alertify) {
    var currentIndex = null;
    var previousIndex = null;
    var currentStdOperationIndex = null;
    var previousStdOperationIndex = null;
    $scope.specList = [];




    $scope.init = function () {

        $scope.standardOperationCreate = false;
        $scope.widget = false;
        $scope.column = 9;
        $scope.styleDetails = {};
        $scope.styleWiseMachineMapping = [];
        $scope.styleList = "";
        $scope.machineList = "";
        $scope.operationList = [];
        $scope.allCuttings = [];
        $scope.operationCategoryList = [];
        $scope.accountId = 1;

        $scope.allStdOperations = [];

        styleConfigureService.getAllSupervisors().then(function (res) {
            $scope.supervisorList = res.data;
        });
        styleConfigureService.getAllStandardOperations().then(function (res) {
            $scope.allStdOperations = res.data;
        });

        styleConfigureService.getAllCuttings().then(function (res) {
            $scope.allCuttings = formatDate(res.data, "YYYY-MM-DD", ["CuttingDate"]);

        });

        //styleConfigureService.getAllOperationCategories().then(function (res) {
        //    $scope.operationCategoryList = res.data;

        //});

        styleConfigureService.getAllJobClasses().then(function (res) {
            $scope.allJobClasses = res.data;
        });

        $scope.totalOperationTime = null;

        styleConfigureService.getMachineDropDown().then(function (res) {
            $scope.machineList = res.data;
        }, function (err) {
            console.log(err);
        });

        styleConfigureService.getAllBuyer().then(function (data) {
            $scope.buyerList = data;
        });
        styleConfigureService.getSpec().then(function (res) {
            $scope.specList = res.data;
            console.log($scope.specList);
        });
        //$scope.getSpecByOperationID = function (operationID,index) {
        //   , function (err) {
        //        handleHttpError(err);
        //    });
        //};

        $scope.stdOperationList = [];
        $scope.stdOperation = null;
    }


    function getOperationsByStyleID(styleID) {
        $scope.totalOperationTime = null;
        styleConfigureService.GetStyleOperationListByStyleID(styleID).then(
            function (res) {
                $scope.operationList = res.data;
                console.log(res.data);
                return;
                for (var i = 0; i < $scope.operationList.length; i++) {
                    $scope.totalOperationTime += $scope.operationList[i].Sam + $scope.operationList[i].AuxSam;
                }
            }, function (err) {
                console.log(err);
            })
    }

    function selectedStyle(index, style) {

        var index = indexOfObjectInArray($scope.styleList, 'StyleID', style.StyleID);
        if (index == currentIndex) {
            $scope.styleList[currentIndex].selected = true;
        } else {
            previousIndex = currentIndex;
            currentIndex = index;
            $scope.styleList[currentIndex].selected = true;
            $scope.styleDetails = style;//$scope.styleList[currentIndex];
            getOperationsByStyleID($scope.styleDetails.StyleID);
            if (previousIndex != null) {
                $scope.styleList[previousIndex].selected = false;
            }

        }
    }

    function onBuyerChange(buyerId) {
        $scope.operationList = [];
        $scope.styleDetails = {};
        if (buyerId != "") {
            styleConfigureService.getStyles($scope.accountId, buyerId).then(function (data) {
                $scope.styleList = data;
            });
        }
    }

    function saveStyleOperation() {
        if ($scope.operationForm.$valid) {
            styleConfigureService.saveStyleOperation($scope.operationList).then(
                function (res) {
                    alertify.alert(res.data);
                }, function (err) {
                    alertify.error("Please check server side error.");
                    handleHttpError(err);
                })
        } else {
            alertify.error("Invalid Form Submission");
        }

    }

    function resetForm(form) {
        form.$setPristine();
        form.$setUntouched();
    }

    function addOperation() {
        if ($scope.styleDetails.StyleID) {
            var sizeList = [];
            if ($scope.operationList.length > 0) {
                for (var i = 0; i < $scope.operationList[0].SizeListVM.length; i++) {
                    sizeList.push({ Size: "", Sam: 0 });
                }
                $scope.operationList.push({ StyleID: $scope.styleDetails.StyleID, SizeListVM: sizeList });
            }
            else {
                $scope.operationList.push({ StyleID: $scope.styleDetails.StyleID, SizeListVM: [{ Size: "", Sam: 0 }, { Size: "", Sam: 0 }, { Size: "", Sam: 0 }] });
            }
           
        } else {
            alertify.error("No Style is Selected!");
        }


    }

    function removeOperation(index) {
        $scope.operationList.splice(index, 1);
    }

    function getAllStdOperations() {
        if ($scope.showStdOperationList == true) {
            $scope.showStdOperationList = false;
            $scope.standardOperationCreate = true;
            //$scope.allStdOperations[currentStdOperationIndex].selected = false;
        } else {
            $scope.showStdOperationList = true;
            $scope.standardOperationCreate = false;
        }
    }


    function resetForm(form) {
        $scope.stdOperation = {};
        if (currentStdOperationIndex != null) {
            currentStdOperationIndex = null;
            previousStdOperationIndex = null;
        }
        form.$setPristine();
        form.$setUntouched();
    }

   

    $scope.bindDate = function (id, model) {
        $scope[model][id] = $('#' + id).val();
    };

    $scope.getBuyerIDAndStyle = function (item, model, label) {
        $scope.buyerID = item.BuyerID;
        $scope.BuyerName = item.BuyerName;
        onBuyerChange(item.BuyerID);
    };

    function indexOfObjectInArray(array, property, value) {
        for (var i = 0; i < array.length; i++) {
            if (array[i][property] == value) {
                return i;
            }
        }
        return -1;
    }

    function addSize() {
        for (var i = 0; i < $scope.operationList.length; i++) {
            $scope.operationList[i].SizeListVM.push({ Size: "", Sam: 0 });
        }


    };

 




        // $scope.newOperation = newOperation;
        $scope.onBuyerChange = onBuyerChange;
        $scope.saveStyleOperation = saveStyleOperation;
        $scope.addOperation = addOperation;
        $scope.removeOperation = removeOperation;
        $scope.selectedStyle = selectedStyle;
        //$scope.selectOperation = selectOperation;
        //$scope.saveOperation = saveOperation;
        $scope.getAllStdOperations = getAllStdOperations;
        $scope.resetForm = resetForm;
        $scope.addSize = addSize;
    }])