scopoAppControllers.controller('requisitionCtrl', ['$scope', 'requisitionService', function ($scope, requisitionService) {
    
    $scope.init = function () {
        $scope.JobID = null;
        $scope.jobList = [];
        //$scope.supplierList = [];
        $scope.piDropDownList = [];
        $scope.req = {};

        requisitionService.getJobDropDown().then(function (res) {
            $scope.jobList = res.data;
        }, function (err) { console.log(err); });

        //requisitionService.getSupplierDropDown().then(function (res) {
        //    $scope.supplierList = res.data;
        //}, function (err) { console.log(err); });
    }

    $scope.onJobChange = function (selectedJob) {
        $scope.JobID = selectedJob;
        $scope.req.JobID = selectedJob;
        requisitionService.getReqByJob(selectedJob).then(function (res) {
            $scope.requisitionList = formatDate(res.data, ["RequisitionDate", "SetupDate"]);
        }, function (err) {
            console.log(err);
        });

        requisitionService.getPISummaryByJobID(selectedJob).then(function (res) {
            $scope.piDropDownList = res.data;
        }, function (err) { console.log(err); });
    };
    
    $scope.onRequisitionChange = function (selectedRequisition) {
        $scope.req = JSON.parse(selectedRequisition);
    };
    
    $scope.selectedPIListIndex = null;

    $scope.mapPIwithReq = function () {
        if ($scope.req.PIList == undefined) {
            $scope.req.PIList = [];
        }
        for (var i = 0; i < $scope.req.PIList.length; i++)
        {
            if ($scope.req.PIList[i].PIValue == $scope.piDropDownList[$scope.selectedPIListIndex].PIValue)
            {
                alertify.error("PI already added.");
                return;
            }
        }
        $scope.req.PIList.push($scope.piDropDownList[$scope.selectedPIListIndex]);
    };

    $scope.removePi = function removePi(index) {
        $scope.req.PIList.splice(index, 1);
        $scope.requisitionForm.$setPristine();
        $scope.requisitionForm.$setUntouched();
    }

    $scope.newClicked = function () {
        $scope.requisitionForm.$setPristine();
        $scope.requisitionForm.$setUntouched();
        $scope.req = {};
    };

    $scope.saveClicked = function () {
        if ($scope.JobID == null) {
            alertify.error("Plaese select job first.");
        }
        else {
            if ($scope.requisitionForm.$valid) {
                if ($scope.req.RequisitionID != undefined) {
                    requisitionService.updateRequisition($scope.req).then(function (res) {
                        alertify.success("Requisition updated.");
                        $scope.requisitionForm.$setPristine();
                        $scope.requisitionForm.$setUntouched();
                    }, function (err) { });
                }
                else {
                    $scope.req.RequisitionID = 0;
                    $scope.req.JobID = $scope.JobID;
                    if ($scope.req.PIList == undefined) { $scope.req.PIList = []; }
                    requisitionService.createRequisition($scope.req).then(function (res) {
                        alertify.success("Requisition created.");
                        $scope.requisitionForm.$setPristine();
                        $scope.requisitionForm.$setUntouched();
                    }, function (err) { });
                }
                $scope.req = {};
            }
            else { alertify.error("Please check your inputs.");}
        }
    };
    
    $scope.bindDate = function bindDate(id, model) {
        $scope[model][id] = $('#' + id).val();
    }; 
}]);
