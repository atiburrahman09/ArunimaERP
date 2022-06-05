scopoAppControllers.controller('bookingCtrl', ['$scope', 'bookingService', function ($scope, bookingService) {

    $scope.context = {
        contexts: {
            newBooking: false,
            editBooking: false,
            editPI: false,
            newBooking: false,
            newPI: false
        },
        currentContext: {},
        get: function (ctx) {
            return this.contexts[ctx];
        },
        set: function (ctx) {
            for (var prop in this.contexts) {
                ctx === prop ? this.contexts[prop] = true : this.contexts[prop] = false;
                this.currentContext = this.contexts[ctx];
            }
        }
    }

    $scope.init = function () {
        $scope.jobList = [];
        $scope.piSummaryList = [];
        $scope.b2bLCList = [];
        $scope.supplierList = [];
        $scope.bookingList = [];
        $scope.PI = {};
        $scope.grandTotalQuantityForRevise = 0;
        $scope.grandTotalPriceForRevise = 0;
        $scope.poList = [];
        $scope.purchaseOrderList = [];

        getJobDropDown();
        getSupplierDropDown();
        $scope.getFormula();
        $scope.RefTrue = false;
    }

    $scope.onJobChange = function (jobId) {
        //getPIDropDownByJob(jobId);
        getReferenceDropDownByJob(jobId);
        fetchPurchaseOrders(jobId);
        $scope.jobId = jobId;

        //$scope.context.set('newBooking');

    }



    $scope.onSelectPI = function (selectedPI) {
        $scope.bookingView = true;
        $scope.bookingReviseBtn = true;
        $scope.context.set('editPI');

        getPIById(selectedPI);
        $scope.getBookingByPIId(selectedPI);
    }

    $scope.handleFormSubmission = function () {

        if ($scope.context.get('newPI')) {
            console.log("in create pi");
            createPI($scope.PI);
        }
        else if ($scope.context.get('editPI')) {
            // save pi
            $scope.PI.SupplierID = 1;
            updatePI($scope.PI);
        }
        $scope.PI = {};
    }

    $scope.getBookingByPIId = function getBookingByPIId(piId) {
        bookingService.getBookingByPIId(piId).then(function (res) {
            $scope.bookingList = formatDate(res.data, []);

            angular.forEach($scope.bookingList, function (row) {
                $scope.RevisionNo = row.RevisionNo;
                $scope.grandTotalQuantityForRevise += row.TotalQuantity;
                $scope.grandTotalPriceForRevise += row.TotalQuantity * row.UnitPrice;

                row.getTotalPrice = function () {
                    row.TotalPrice = (row.TotalQuantity * row.UnitPrice).toFixed(6);
                    return row.TotalPrice;
                }
            });
        }, function (err) {
            handleHttpError(err);
        });
    }

    function updatePI(pi) {
        console.log(pi);
        bookingService.updatePI(pi).then(function (res) {
            alertify.success(res.data);
            // $scope.piSummaryList[prevIndex].PINo = pi.PINo;
        }, function (err) {
            handleHttpError(err);
        });
    }

    function getJobDropDown() {
        bookingService.getJobDropDown().then(function (res) {
            $scope.jobList = res.data;
        }, function (err) {
            handleHttpError(err);
        });
    }

    function getPIDropDownByJob(jobId) {
        bookingService.getPIDropDownByJob(jobId).then(function (res) {
            $scope.piSummaryList = res.data;
        }, function (err) {
            handleHttpError(err);
        });
    }

    function getReferenceDropDownByJob(jobId) {
        if (jobId) {
            bookingService.getReferenceDropDownByJob(jobId).then(function (res) {
                $scope.referenceDropdown = res.data;
                console.log('referenceDropdown: ', res.data);
            }, function (err) {
                handleHttpError(err);
            });
        }

    }

    function getPIById(piId) {

        bookingService.getPIById(piId).then(function (res) {
            $scope.PI = formatDate([res.data], ["PIDate", 'ApproximateInHouseDate'])[0];
            $scope.PITrue = true;
            $scope.RefTrue = true;
        }, function (err) {
            handleHttpError(err);
        });
    }

    function getSupplierDropDown() {
        bookingService.getSupplierDropDown().then(function (res) {
            $scope.supplierList = res.data;
        }, function (err) {
            handleHttpError(err);
        });
    }

    $scope.saveBookings = function saveBookings() {
        //$scope.bookingList = CommonFunc.setCurrentDate($scope.bookingList, "SetDate");
        for (var i = 0; i < $scope.bookingList.length; i++) {
            $scope.bookingList[i].SetDate = new Date();
        }
        bookingService.saveBookings($scope.bookingList).then(function (res) {
            alertify.alert("Booking successfully saved. Reference No: " + res.data);
            $scope.itemFormulaList = [{ Item: "", Formula: "" }];
            $scope.bookingView = false;
        }, function (err) {
            console.log(err);
        });
    }

    $scope.reviseBookings = function reviseBookings() {
        bookingService.reviseBookings($scope.bookingList).then(function (res) {
            alertify.success(res.data);
        }, function (err) {
            handleHttpError(err);
        });
    }

    function createPI(pi) {
        bookingService.createPI(pi).then(function (res) {
            alertify.success(res.data);
            $scope.piSummaryList[prevIndex].PINo = pi.PINo;

        }, function (err) {
            handleHttpError(err);
        });
    }

    function fetchPurchaseOrders(jobId) {
        if (jobId) {
            bookingService.getPurchaseOrderListByJob(jobId).then(function (res) {
                $scope.purchaseOrderList = res.data;
                $scope.purchaseOrderList.unshift({ Value: 'all', Text: 'All' });
            }, function (err) {
                handleHttpError(err);
            });
        }
    }

    $scope.newClicked = function newClicked() {
        $scope.searchCriteria.multiplePO = [];
        $scope.itemFormulaList = [{ Item: "", Formula: "" }];
        $scope.context.set('newBooking');
        $scope.bookingList = [];
        $scope.grandTotalPriceForRevise = 0;
        $scope.grandTotalQuantityForRevise = 0;
    }


    //Changing all row quantity on Grand Total Quantity Change
    $scope.changeAllRowTotalQuantityForRevise = function (grandTotalQuantityForRevise) {
        $scope.grandTotalQuantityForRevise = grandTotalQuantityForRevise;
        if (isNaN($scope.grandTotalQuantityForRevise)) {
            console.log('Nan');
            $scope.grandTotalQuantityForRevise = '';
        } else {
            $scope.grandTotalPriceForRevise = 0;
            for (var i in $scope.bookingList) {
                //$scope.bookingList[i].TotalQuantity = ($scope.bookingList[i].Ratio * $scope.grandTotalQuantityForRevise).toFixed(2);
                $scope.bookingList[i].TotalQuantity = ($scope.bookingList[i].Ratio * $scope.grandTotalQuantityForRevise).toFixed(2);
                //$scope.bookingList[i].TotalQuantity = "Heyyyy!";                

                $scope.grandTotalPriceForRevise += Number($scope.bookingList[i].TotalQuantity) * Number($scope.bookingList[i].UnitPrice);
            }
        }
    };

    //Changing all row Unit Price on Grand Total Price Change
    $scope.changeAllRowUnitPriceForRevise = function () {
        for (var i in $scope.bookingList) {
            console.log($scope.bookingList[i].Ratio);
            $scope.bookingList[i].UnitPrice = (($scope.bookingList[i].Ratio * $scope.grandTotalPriceForRevise) * $scope.bookingList[i].Ratio).toFixed(2);
            //$scope.grandTotalPriceForRevise += $scope.bookingList[i].TotalQuantity * $scope.bookingList[i].UnitPrice;
            console.log($scope.bookingList[i].UnitPrice);
        }
    };

    //Changing Grand Total Quantity on Quantity Change
    $scope.reviseTotalQuantityOnQuantityChange = function () {
        console.log('hello world from booking controller');
        $scope.grandTotalPriceForRevise = 0;
        $scope.grandTotalQuantityForRevise = 0.0;
        for (var i in $scope.bookingList) {
            //$scope.bookingList[i].TotalQuantity = ($scope.bookingList[i].Ratio * $scope.grandTotalQuantityForRevise).toFixed(2);

            $scope.grandTotalQuantityForRevise += parseFloat($scope.bookingList[i].TotalQuantity);
            $scope.grandTotalPriceForRevise += parseFloat(parseFloat($scope.bookingList[i].TotalQuantity) * parseFloat($scope.bookingList[i].UnitPrice));
        }

        // Assigning new Ratio
        for (var i in $scope.bookingList) {
            $scope.bookingList[i].Ratio = $scope.bookingList[i].TotalQuantity / $scope.grandTotalQuantityForRevise;
        }
    }

    //Changing Grand Total Price on Quantity Change
    $scope.reviseTotalPriceOnUnitPriceChange = function () {

        $scope.grandTotalPriceForRevise = 0;
        for (var i in $scope.bookingList) {
            var totalQuantity = ($scope.bookingList[i].Ratio * $scope.grandTotalQuantityForRevise).toFixed(2);
            $scope.grandTotalPriceForRevise += $scope.bookingList[i].TotalQuantity * $scope.bookingList[i].UnitPrice;
        }
    }

    $scope.bookingView = false;

    $scope.getFormula = function getFormula() {
        bookingService.getFormulaDropDown().then(function (res) {
            $scope.formulaList = res.data;
        }, function (err) {
            handleHttpError(err);
        });
    };

    $scope.searchCriteria = {};
    $scope.poDropDownChange = function () {
        if ($scope.searchCriteria.multiplePO[0] == "all") {
            bookingService.getItemByAllPO($scope.purchaseOrderList).then(function (res) {
                $scope.searchCriteria.itemDropDown = res.data;

                $scope.searchCriteria.itemDropDown.unshift({ Value: 'all', Text: 'All' });
            });
        }
        else if ($scope.searchCriteria.multiplePO.length > 0) {
            bookingService.getItemByMultiplePO($scope.searchCriteria.multiplePO.join(",")).then(function (res) {
                $scope.searchCriteria.itemDropDown = res.data;

                $scope.searchCriteria.itemDropDown.unshift({ Value: 'all', Text: 'All' });
            });
        } else {
            $scope.searchCriteria.itemDropDown = null;
        }
    };

    //append item-formula row to table
    $scope.addRowDisabled = false;
    $scope.itemFormulaList = [{ Item: "", Formula: "" }];
    $scope.addRow = function addRow() {
        if ($scope.itemFormulaList.slice(-1)[0].Item == "" || $scope.itemFormulaList.slice(-1)[0].Formula == "") { $scope.addRowDisabled = true; return; }
        $scope.itemFormulaList.push({ Item: "", Formula: "" });
    }
    $scope.enableAddRow = function enableAddRow() { $scope.addRowDisabled = false; }

    //on generate click
    $scope.generateClicked = function generateClicked() {
        if (!$scope.jobId) { alertify.error("Please select a job first."); return; }
        if (!$scope.forms.podForm.$valid) { alertify.error("Form submission invalid. Please fill up required fields."); return; }
        if ($scope.searchCriteria.multiplePO[0] == "all") {
            for (var i = 1; i < $scope.purchaseOrderList.length; i++) {
                $scope.poList.push($scope.purchaseOrderList[i].Value);
            }
        }
        else {
            $scope.poList = $scope.searchCriteria.multiplePO;
        }
        console.log($scope.poList);

        $scope.generateBookingVM = { poList: $scope.poList, itemFormulaList: $scope.itemFormulaList };

        bookingService.generateBooking($scope.generateBookingVM).then(function (res) {
            console.log(res);
            $scope.bookingView = true;
            $scope.bookingReviseBtn = false;
            $scope.bookingList = res.data;
            console.log("generated booking list here " + JSON.stringify($scope.bookingList));
            $scope.grandTotalQuantityForRevise = 0;
            $scope.grandTotalPriceForRevise = 0;
            angular.forEach($scope.bookingList, function (row) {
                $scope.grandTotalQuantityForRevise += row.TotalQuantity;
                $scope.grandTotalPriceForRevise += row.TotalQuantity * row.UnitPrice;
            });
        }, function (err) {
            handleHttpError(err);
        });
    }

    $scope.deleteClicked = function () {
        console.log("test");
        bookingService.deletePI($scope.PIID).then(function (res) {
            alertify.success("PI and assosiated booking deleted");
        }, function (err) {
        });
    }
}]);