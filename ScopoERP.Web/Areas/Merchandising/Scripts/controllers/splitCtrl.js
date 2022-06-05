scopoAppControllers.controller('splitCtrl', function ($scope, splitService) {

    function SplitVM() {
        this.MasterPOID = null;
        this.SplitList = [];
    }

    $scope.init = function () {
        // get styles
        getStyleDropdown();
        //get po   
        //
    }



    function getStyleDropdown() {
        splitService.getStyleDropdown().then(function (res) {
            $scope.masterStyleList = res.data;
            $scope.styleList = res.data;
            console.log(res.data);
        });
    }

    $scope.getPurchaseOrderByStyleIdMaster = function (masterStyleId) {
        splitService.getPurchaseOrderByStyleId(masterStyleId).then(function (res) {
            $scope.masterPoList = res.data;
            console.log(res.data);
        });
    }

    $scope.getPurchaseOrderByStyleId = function (styleId) {
        splitService.getPurchaseOrderByStyleId(styleId).then(function (res) {
            $scope.poList = res.data;
            console.log(res.data);
        });
    }

    $scope.getPurchaseOrderForSplit = function (styleId, poId) {
        if (!poId)
            return;
        splitService.getPurchaseOrderForSplit(styleId, poId).then(function (res) {
            console.log(res.data);
            $scope.splittedOrders = formatDate(res.data, '', ['ExitDate']);
        });
        getPurchaseOrderById(poId);
    }

    function getPurchaseOrderById(poId) {
        if (!poId)
            return;
        splitService.getPurchaseOrderById(poId).then(function (res) {
            console.log(res.data);
            $scope.selectedPO = formatDate(res.data, '', ['ExitDate']);
        });
    }

    $scope.saveSplittedPurchaseOrders = function () {
        console.log(JSON.parse($scope.masterPoId));
        console.log(JSON.parse($scope.poId));
        if (JSON.parse($scope.masterPoId) < 1 || JSON.parse($scope.poId) < 1) {
            alertify.error("Please select required data.")
        }
        else {
            splitService.saveSplittedPurchaseOrders(JSON.parse($scope.masterPoId), JSON.parse($scope.poId)).then(function (res) {
                alertify.success(res.data);
                $scope.splitForm.$setPristine();
                $scope.splitForm.$setUntouched();
            }, function (err) { handleHttpError(err); });
        }

        //var splitVM = new SplitVM();
        //splitVM.SplitList = $scope.splittedOrders;
        //splitVM.MasterPOID = JSON.parse($scope.poId);
        //splitService.saveSplittedPurchaseOrders(splitVM).then(function (res) {
        //    console.log(res.data);
        //    alertify.success(res.data);
        //}, function (err) { console.log(err);handleHttpError(err); });
    }

    $scope.addOrder = function () {
        $scope.splittedOrders.push({});
    }

    $scope.removeOrder = function (index) {
        $scope.splittedOrders.splice(index, 1);
    }
});