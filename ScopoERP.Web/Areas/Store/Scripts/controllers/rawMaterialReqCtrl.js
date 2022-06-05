scopoAppControllers.controller('rawMaterialReqCtrl', ['$scope', 'rawMaterialService', function ($scope, rawMaterialService) {

        function SrVM() {
            this.Inventories = [{}];           
            
        };

        $scope.poList = [];
        $scope.itemList = [];
        $scope.floorLineList = [];    


        $scope.srVM = new SrVM();

        $scope.prevInstance = {};
        $scope.dt = {
            issueDate: false
        }

        $scope.pickDate = function (val) {
            $scope.dt[val] = true;
        }

        $scope.init = function () {
            $scope.getPurchaseOrderDropDown();
            $scope.getAllFloorLine();
            getIssues();
        };


        function getIssueById(id) {
            rawMaterialService.getIssueById(id).then(function (res) {
                $scope.srVM = formatDate([res.data], ['CreatedDate', 'IssuedDate'])[0];
                for (var p in $scope.srVM.Inventories) {
                    $scope.getAllItemByPOID($scope.srVM.Inventories[p].PoStyleId, p);                    
                }
            }, function (err) {
                console.log(err);
            })
        }

        $scope.selectInstance = function (index, instance) {
            instance.selected = true;
            if ($scope.prevInstance) {
                $scope.prevInstance.selected = false;
            }
            $scope.prevInstance = instance;
            getIssueById(instance.Value);
        }
        $scope.issueSelected = function (instance)
        {
            var issue = JSON.parse(instance);
            getIssueById(issue.Value);
        }

        function getIssues() {
            rawMaterialService.getIssues().then(function (res) {
                $scope.issues = res.data;                
            }, function (err) {
                console.log(err);
            });
        }

        $scope.getAllFloorLine = function () {
            rawMaterialService.getAllFloorLine()
                .then(function (res) {
                    $scope.floorLineList = res.data;
                }, function (err) {
                    console.log(err);
                })
        };

        $scope.getPurchaseOrderDropDown = function () {
            rawMaterialService.getPurchaseOrderDropDown()
                .then(function (res) {
                    $scope.poList = res.data;
                }, function (err) {
                    console.log(err);
                })
        };

        $scope.getAllItemByPOID = function (PoStyleId, index) {
            rawMaterialService.getAllItemByPOID(PoStyleId)
                .then(function (res) {                    
                    $scope.itemList[index] = res.data;
                }, function (err) {
                    console.log(err);
                })
        };

        $scope.addRow = function () {
            $scope.srVM.Inventories.push({});

        }

        $scope.newSR = function () {
            $scope.srVM = new SrVM();
            $scope.rawMaterialReqForm.$setPristine();
            $scope.rawMaterialReqForm.$setUntouched();
        }
        
        $scope.removeRow = function (index) {
            console.log($scope.srVM.Inventories);
            console.log(index);
            $scope.srVM.Inventories.splice(index, 1);
        }

        $scope.saveRawMaterialReq = function () {
            if ($scope.rawMaterialReqForm.$valid) {
                console.log($scope.srVM);
                rawMaterialService.saveIssuedInventories($scope.srVM).then(function (res) {
                    if (res.data.id > 0)
                    {
                        alertify.success(res.data.msg);
                        getIssues();
                        $scope.srVM.SRNo = res.data.srNo;
                        $scope.rawMaterialReqForm.$setPristine();
                        $scope.rawMaterialReqForm.$setUntouched();
                    }
                    
                    //$scope.selectedIssue = "";
                    //$scope.srVM = new SrVM();
                    //$scope.srVM.Inventories = [];
                    //$scope.rawMaterialReqForm.$setPristine();
                    //$scope.rawMaterialReqForm.$setUntouched();
                   
                }, function (err) {
                    console.log(err);
                })
            }
        }
    }]);