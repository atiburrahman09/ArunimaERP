scopoAppControllers.controller('timelineCtrl', ['$scope', 'timelineService', 'alertify', function ($scope, timelineService, alertify) {

    $scope.timeline = {};
    $scope.timelineList = [];
    $scope.styleList = [];
    $scope.purchaseOrderList = [];


    $scope.init = function () {
        timelineService.getStyleList().then(function (data) {
            $scope.styleList = data;
        }, function (err) {
            handleHttpError(err);
        });
        //$scope.getAllTimeline();
    }


    $scope.onStyleChange = function (styleID) {
        $scope.pod = { StyleID: styleID };
        $scope.getPurchaseOrderListByStyle(styleID)
    }

    $scope.getPurchaseOrderListByStyle=function getPurchaseOrderListByStyle(styleID) {
        timelineService.getPurchaseOrderListByStyle(styleID).then(function (res) {
            $scope.purchaseOrderList = formatDate(res.data, ["ExitDate", "OriginalCRD"]);
        }, function (err) {
            handleHttpError(err);
        });
    }
    $scope.onPurchaseOrderChange = function (pod) {
        $scope.pod = JSON.parse(pod);
        $scope.jobId = $scope.pod.JobID;
        $scope.timelineList = [];
        $scope.getAllTimelineByPOID($scope.pod.PurchaseOrderID)
    }

    $scope.getAllTimelineByPOID = function (PurchaseOrderID) {
        console.log($scope.pod);
        timelineService.getAllTimelineByPOID(PurchaseOrderID).then(function (res) {
            console.log(res.data);
            if (res.data.length > 0) {
                $scope.timelineList = formatDate(res.data, '', ["ExpectedDate", 'ProvableDate', 'LastModified']);

                for (var i = 0; i < $scope.timelineList.length; i++)
                {
                    $scope.timelineList[i].ExpectedDate = new Date($scope.timelineList[i].ExpectedDate);
                    $scope.timelineList[i].ProvableDate = new Date($scope.timelineList[i].ProvableDate);
                    $scope.timelineList[i].LastModified = new Date($scope.timelineList[i].LastModified);
                    $scope.timelineList[i].disable = true;

                }
            }
            else {
                $scope.timelineList.push($scope.timeline);
            }
            
        });
    };


    $scope.saveTimeline = function (index) {
        console.log($scope.timelineList);
        $scope.timelineList[index].PurchaseOrderID = $scope.pod.PurchaseOrderID;
        if ($scope.timelineForm.$valid) {
            timelineService.saveTimeline($scope.timelineList[index]).then(function (res) {
                alertify.success(res.data);
                $scope.timelineList[index].disable = true;
               // $scope.reset();
            }, function (err) {
                handleHttpError(err);
            });
        }
        else {
            alertify.error("Please fill the required information.");
        }
    }


    $scope.addRow = function () {
        $scope.timelineList.push({});
        console.log($scope.timelineList);

    }

    $scope.removeRow = function (index) {
        if ($scope.timelineList[index].TimelineID != null) {
            timelineService.removeTimeline($scope.timelineList[index].TimelineID).then(function (res) {
                alertify.success(res.data);
                $scope.timelineList.splice(index, 1);
                //$scope.reset();
            }, function (err) {
                handleHttpError(err);
            });
        }
        else {
            $scope.timelineList.splice(index, 1);
        }
    }

    $scope.edit = function (index) {
        console.log(index);
        $scope.timelineList[index].disable = false;

    }

    

    $scope.reset = function () {
        $scope.timelineList = [{}];
        $scope.timelineForm.$setPristine();
        $scope.timelineForm.$setUntouched();
    }
}]);