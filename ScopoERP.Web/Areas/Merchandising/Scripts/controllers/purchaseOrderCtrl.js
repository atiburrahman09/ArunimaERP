scopoAppControllers.controller('purchaseOrderCtrl', ['$scope', 'purchaseOrderService', function ($scope, purchaseOrderService) {

    $scope.forms = {};
    $scope.shipModes = [{ id: 1, ShipMode: 'Sea' }, { id: 2, ShipMode: 'Air' }];    
    $scope.context = {
        contexts: {
            newPO: false,
            editPO: false,
            amendment: false,
            workSheetsView: false,
            sizeOfColorView: false,
            subContractView: false,
            sizeWiseFOBView: false
        },
        get: function (ctx) {
            return this.contexts[ctx];
        },
        set: function (ctx) {
            for (var prop in this.contexts) {
                ctx === prop ? this.contexts[prop] = true : this.contexts[prop] = false;
            }
        }
    }

    $scope.state = {
        purchaseOrder: true,
        workSheet: false,
        batchEdit: false,
        sizeOfColor: false,
        subContract: false,
        sizeWiseFOB: false,
    }
    $scope.jobId = null;
    $scope.jobList = [];
    $scope.purchaseOrderList = [];
    $scope.workSheetList = [];
    $scope.csList = [];

    $scope.onJobSelect = function (pod) {
        console.log('on job select: ', pod);
        $scope.jobId = pod.JobID;
    }

    $scope.init = function () {
        purchaseOrderService.getJobList().then(function (data) {
            $scope.jobList = data;
            console.log('joblist; ',data);
        }, function (err) {
            handleHttpError(err);
        });

        purchaseOrderService.getStyleList().then(function (data) {
            $scope.styleList = data;
        }, function (err) {
            handleHttpError(err);
        });

        purchaseOrderService.getFactoryList().then(function (data) {
            $scope.factoryList = data;
        }, function (err) {
            handleHttpError(err);
        });

        purchaseOrderService.getSeasonList().then(function (data) {
            $scope.seasonList = data;
        }, function (err) {
            handleHttpError(err);
        });

        purchaseOrderService.getCurrentStatusList().then(function (data) {
            $scope.currentStatusList = data;
        }, function (err) {
            handleHttpError(err);
        });
    }

    $scope.resetForm = function (form) {
        form.$setPristine();
        form.$setUntouched();
    }

    $scope.newPurchaseOrder = function () {
        if (!$scope.selectedStyle) { alertify.error("Please select Style first."); return; }

        $scope.pod = {
            StyleID: $scope.selectedStyle
        };
        $scope.csList = [];
        $scope.context.set('newPO');

        console.log('new pod: ', $scope.pod, 'style: ', $scope.selectedStyle);
    }

    $scope.onStyleChange = function (styleID) {
        $scope.pod = { StyleID: styleID };
        $scope.csList = [];

        getPurchaseOrderListByStyle(styleID)
        getCSByStyleID(styleID);
    }

    $scope.onPurchaseOrderChange = function (pod) {
        $scope.pod = JSON.parse(pod);
        $scope.jobId = $scope.pod.JobID;
        console.log($scope.pod);
        console.log($scope.jobId);
        $scope.context.set('editPO');
    }

    function getPurchaseOrderListByStyle(styleID) {
        purchaseOrderService.getPurchaseOrderListByStyle(styleID).then(function (res) {
            $scope.purchaseOrderList = formatDate(res.data, ["ExitDate", "OriginalCRD"]);
        }, function (err) {
            handleHttpError(err);
        });
    }

    function getCSByStyleID(styleID) {
        purchaseOrderService.getCSByStyleID(styleID).then(function (res) {
            $scope.csList = res.data;
            if (res.data.length == 1 && $scope.context.get('newPO')) {
                console.log("in if csss");
                $scope.pod.CostSheetNo = res.data[0].ValueString;
            }
        }, function (err) {
            handleHttpError(err);
        });
    }

    function createPurchaseOrder(po) {
        console.log('po : ', po);
        po.JobID = $scope.jobId;
        
        console.log("job id here " + $scope.jobId + "   po jobid here  " + po.JobID);

        if (po.JobID == null || po.JobID == undefined) {
            alertify.error("Please select job.");
        }
        else {
            console.log('all clear, submitting : ', po, $scope.style);            
            purchaseOrderService.createPurchaseOrder(po).then(function (res) {
                alertify.success(res);
                $scope.purchaseOrderList.unshift(po);
                $scope.resetForm($scope.forms.podForm);
                $scope.pod = {};
            }, function (err) {
                handleHttpError(err);
            });
        }

    }

    function updatePurchaseOrder(po) {
        purchaseOrderService.updatePurchaseOrder(po).then(function (res) {
            alertify.success(res);
            $scope.resetForm($scope.forms.podForm);
            $scope.pod = {};

        }, function (err) {
            handleHttpError(err);
        });
    }

    $scope.getWorkSheetsByCostSheetAndPurchaseOrderId = function (pod) {
        $scope.submitOperations = true;

        if (!pod.CostSheetNo) {
            console.log("in IF");
            $scope.context.set('workSheetsView');
            getCSByStyleID($scope.pod.StyleID);
            //alertify.error('Assign Cost Sheet First!');
            return;
        }
        else {
            console.log($scope.pod.PurchaseOrderID);
            purchaseOrderService.getCostSheetOrWorkSheet($scope.pod.CostSheetNo, $scope.pod.PurchaseOrderID).then(function (res) {
                console.log(res.data);
                $scope.workSheetList = res.data;
                console.log($scope.state.workSheet);
                $scope.context.set('workSheetsView');
                console.log($scope.state.workSheet);
            }, function (err) { console.log(err) })
        }

    }

    $scope.savePod = function (pod) {
        console.log('form submited: ', pod);
        if (!$scope.forms.podForm.$submitted) {
            return;
        }

        if (!$scope.forms.podForm.$valid) {
            alertify.error('Invalid Form Submission!');
            return;
        }

        if ($scope.context.get('amendment')) {
            saveAmendment($scope.bulkPurchaseOrder);
            return;
        }

        if ($scope.context.get('newPO')) {
            createPurchaseOrder(pod);
            return;
        }

        if ($scope.context.get('editPO')) {
            updatePurchaseOrder(pod);
            return;
        }

        if ($scope.context.get('workSheetsView')) {
            updateWorkSheets($scope.workSheetList);
        }
    }

    $scope.updateWorkSheets = function (workSheets) {
        purchaseOrderService.updateWorkSheets(workSheets).then(function (res) {
            alertify.success(res.data);
        }, function (err) {
            handleHttpError(err);
        });
    }

    $scope.addRow = function () {
        if ($scope.pod != null) {
            $scope.workSheetList.push({ PostyleID: $scope.pod.PurchaseOrderID });
            console.log($scope.pod.PurchaseOrderID);
        } else {
            alertify.alert("Please Select a Purchase Order!");
        }
    }

    $scope.sizeOfColor = function (poID) {
        $scope.context.set('sizeOfColorView');
    }

    $scope.subContract = function (poID) {
        $scope.context.set('subContractView');
    }

    $scope.sizeWiseFOB = function (poID) {
        $scope.context.set('sizeWiseFOBView');
    }
}]);