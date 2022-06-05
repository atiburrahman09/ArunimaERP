scopoAppControllers.controller('subContractCtrl', ['$scope', '$filter', 'subContractService', 'purchaseOrderService',
    function ($scope, $filter, subContractService, purchaseOrderService) {

        $scope.init = function (purchaseOrderID) {
            $scope.getFactoryList();

            purchaseOrderService.getStyleList().then(function (data) {
                $scope.styleList = data;
            }, function (err) {
                handleHttpError(err);
            });
        }

        $scope.onStyleChange = function (styleID) {
            purchaseOrderService.getPurchaseOrderListByStyle(styleID).then(function (res) {
                $scope.purchaseOrderList = formatDate(res.data, ["ExitDate", "OriginalCRD"]);
            });
        }

        $scope.onPurchaseOrderChange = function (pod) {
            $scope.puchaseOrderDetails = JSON.parse(pod);

            $scope.getAllsubContract($scope.puchaseOrderDetails.PurchaseOrderID);
        }

        $scope.getAllsubContract = function (purchaseOrderID) {
            subContractService.getAllSubContract(purchaseOrderID).then(function (res) {
                console.log(res.data);
                $scope.subContractList = formatDate(res.data, ['SetupDate', 'SubContractExitDate']);
                console.log($scope.subContractList);
            });
        }

        $scope.getFactoryList = function () {
            subContractService.getFactoryList().then(function (res) {
                $scope.factoryList = res.data;
            });
        }

        $scope.deleteRow = function (index) {
            $scope.subContractList.splice(index, 1);
        }

        $scope.addNew = function () {
            var row = {
                SubContractID: 0,
                SubContractNo: '',
                FactoryID: '',
                PurchaseOrderID: $scope.puchaseOrderDetails.PurchaseOrderID,
                SubContractQuantity: '',
                SubContractExitDate: '',
                SubContractRate: '',
                CommecialPercentage: '',
                Remarks: ''
            }

            $scope.subContractList.push(row);
        }

        $scope.save = function (subContractList) {
            subContractService.save($scope.puchaseOrderDetails.PurchaseOrderID, subContractList).then(function (res) {
                if (res.data == true) {
                    alertify.success("Successfully saved!");
                }
            });
        }
    }]);