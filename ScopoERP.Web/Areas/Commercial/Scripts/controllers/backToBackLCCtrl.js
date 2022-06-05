scopoAppControllers.controller('backToBackLCCtrl', ['$scope', 'backToBackLCService', function ($scope, backToBackLCService) {
    
    $scope.init = function () {
        $scope.backToBackLCList = [];
        $scope.piDropDownList = [];
        $scope.lc = {};
        $scope.searchPiActive = true;

        backToBackLCService.getJobDropDown().then(function (res) {
            $scope.jobList = res.data;
        }, function (err) {
            console.log(err);
        });

        backToBackLCService.getLCTypeDropDown().then(function (res) {
            $scope.lcTypeList = res.data;
        }, function (err) {
            console.log(err);
        });
    }
        
    $scope.onJobChange = function (selectedJob) {
        getBackToBackLCByJob(selectedJob);
        getPiListByJob(selectedJob);
    };
    
    function getBackToBackLCByJob(jobId) {
        backToBackLCService.getBackToBackLCByJob(jobId).then(function (res) {         
            $scope.backToBackLCList = formatDate(res.data, []);
        }, function (err) {
            console.log(err);
        });
    };
    
    function getPiListByJob(jobId) {
        backToBackLCService.getPISummaryByJobID(jobId).then(function (res) {
            $scope.piDropDownList = res.data;
        }, function (err) { console.log(err); });
    };
    
    $scope.onBackToBackSelect = function (selectedB2B) {
        if (selectedB2B != null)
        {
            $scope.lc = angular.copy(selectedB2B);

            if ($scope.lc.PIList.length > 0) {
                $scope.searchPiActive = false;
            } else {
                $scope.searchPiActive = true;
            }
        }
       
    };
    
    $scope.mapPIwithLC = function () {
        var pi = JSON.parse($scope.selectedPI);

        if ($scope.lc.PIList == undefined) {
            $scope.lc.PIList = [];
        }

        for (index in $scope.lc.PIList) {
            if (pi.PINo === $scope.lc.PIList[index].PINo) {
                alertify.error("PI already exists.");
                return;
            }
        }

        $scope.lc.PIList.push(pi);
    };

    $scope.deletePIClicked = function (index) {
        $scope.lc.PIList.splice(index, 1);
    }

    $scope.save = function () {
        if ($scope.selectedJob == null) {
            alertify.error("Plaese select job first."); return;
        }

        if ($scope.backToBackLCForm.$valid) {
            if ($scope.lc.BackToBackLCID != undefined) {
                backToBackLCService.updateBackToBackLC($scope.lc).then(function (res) {
                    alertify.success("Back to back LC updated.");
                    getBackToBackLCByJob($scope.selectedJob);
                    getPiListByJob($scope.selectedJob);
                }, function (err) { });
            }
            else {
                $scope.lc.BackToBackLCID = 0;
                $scope.lc.MaturityDate = null;
                $scope.lc.JobID = $scope.selectedJob;
                $scope.lc.JobNo = null;
                $scope.lc.Status = false;
                $scope.lc.LCTypeTitle = null;
                if ($scope.lc.PIList == undefined) {
                    $scope.lc.PIList = [];
                }


                backToBackLCService.createBackToBackLC($scope.lc).then(function (res) {
                    
                    if (res.data == false)
                    {
                        alertify.error("Back to back LC creation failed.");
                        return;
                    }
                    alertify.success("Back to back LC created.");
                    getBackToBackLCByJob($scope.selectedJob);
                    getPiListByJob($scope.selectedJob);
                }, function (err) { });
            }

            $scope.backToBackLCForm.$setPristine();
            $scope.backToBackLCForm.$setUntouched();
            $scope.lc = {};
        }
        else {
            alertify.error("Please check your inputs.");
        }
    };

    $scope.newClicked = function () {
        $scope.lc = {};
    }; 
}]);
