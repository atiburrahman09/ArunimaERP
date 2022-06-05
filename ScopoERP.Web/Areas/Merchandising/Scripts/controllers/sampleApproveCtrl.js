scopoAppControllers.controller('sampleApproveCtrl', ['$scope', 'sampleApproveService', 'alertify', function ($scope, sampleApproveService, alertify) {

    $scope.sampleApprovalVM = {};
    $scope.approve = {};


    $scope.init = function () {
        $scope.sampleApproveList = [{}];
        $scope.styleList = [];
        $scope.sampleList = [];
        $scope.getAllStyle();
        $scope.getAllSample();
        $scope.statusList = [{ 'value': 2, 'text': 'Pending' }, { 'value': 1, 'text': 'Approved' }, { 'value': 3, 'text': 'Rejected' }];
    }

    $scope.getDueDate = function (approvalDate, validityTime) {
        var new_date = moment(approvalDate, "MM-DD-YYYY").add(validityTime, 'days');
        return moment(new_date).format('MM/DD/YYYY');
    }

    $scope.getAllStyle = function () {
        sampleApproveService.getAllStyle().then(function (res) {
            $scope.styleList = res.data;
        });
    };

    $scope.getAllSample = function () {
        sampleApproveService.getAllSample().then(function (res) {
            $scope.sampleList = res.data;
        });
    };

    $scope.bindDate = function bindDate(id, model, index) {
        $scope.sampleApproveList[index][id] = $('#' + id).val();
    };

    $scope.saveApprove = function () {
        console.log($scope.StyleID);
        console.log($scope.sampleApproveList);
        if ($scope.approveForm.$valid) {
            $scope.sampleApprovalVM = { "SampleApprovalID": $scope.sampleApproveList[0].SampleApprovalID, "StyleID": $scope.StyleID, "ApprovalList": $scope.sampleApproveList }
            console.log($scope.sampleApproveList[0].SampleApprovalID);
            sampleApproveService.saveApprove($scope.sampleApprovalVM).then(function (res) {
                alertify.success(res.data);
                $scope.reset();
            }, function (err) {
                handleHttpError(err);
            });
        }
        else {
            alertify.error("Please fill the required information.");
        }


    }


    $scope.addRowToSampleApproveList = function () {
        $scope.sampleApproveList.push({});

    }


    $scope.removeRow = function (index) {

        //console.log($scope.sampleApproveList[index].SampleApprovalID);
        //return;
        if ($scope.sampleApproveList[index].SampleApprovalID != null) {
            sampleApproveService.removeSampleApprove($scope.sampleApproveList[index].SampleApprovalID).then(function (res) {
                alertify.success(res.data);
                $scope.sampleApproveList.splice(index, 1);
                //$scope.reset();
            }, function (err) {
                handleHttpError(err);
            });
        }
        else {
            $scope.sampleApproveList.splice(index, 1);
        }



    }

    $scope.reset = function () {
        $scope.sampleApprovalVM = {};
        $scope.sampleApproveList = [{}];
        $scope.StyleID = "";
        $scope.ValidityTime = "";
        $scope.ApprovalSerialNo = "";
        $scope.approveForm.$setPristine();
        $scope.approveForm.$setUntouched();
    }

    $scope.getAllSampleApprove = function (StyleID) {
        $scope.StyleID = StyleID;
        sampleApproveService.getAllSampleApprove(StyleID).then(function (res) {
            if (res.data.length > 0) {
                res.data.forEach(x => {
                    x.ApproveDate = new Date(parseInt(getMilliseconds(x.ApproveDate)));
                    x.SentDate = new Date(parseInt(getMilliseconds(x.SentDate)));
                    x.ApproximateSentDate = new Date(parseInt(getMilliseconds(x.ApproximateSentDate)));
                });
                $scope.sampleApproveList = res.data;
                $scope.ValidityTime = $scope.sampleApproveList[0].ValidityTime;
                $scope.ApprovalSerialNo = $scope.sampleApproveList[0].ApprovalSerialNo;
                console.log($scope.sampleApproveList);
            }
            else {
                $scope.sampleApproveList = [{}];
            }

        });
    };

    function getMilliseconds(date) {
        if (date) {
            return date.match(/\d+/)[0];
        } else {
            return null;
        }

    }
}]);