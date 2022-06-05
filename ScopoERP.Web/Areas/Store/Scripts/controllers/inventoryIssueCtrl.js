scopoAppControllers.controller('inventoryIssueCtrl', ['$scope', 'inventoryIssueService', function ($scope, inventoryIssueService) {

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


        function getIssueBySR(id) {
            inventoryIssueService.getIssueBySR(id).then(function (res) {
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
        $scope.issueSelected = function (sr)
        {
            //var issue = JSON.parse(instance);
            getIssueBySR(sr);
        }

        function getIssues() {
            inventoryIssueService.getIssues().then(function (res) {
                $scope.issues = res.data;                
            }, function (err) {
                console.log(err);
            });
        }

        $scope.getAllFloorLine = function () {
            inventoryIssueService.getAllFloorLine()
                .then(function (res) {
                    $scope.floorLineList = res.data;
                }, function (err) {
                    console.log(err);
                })
        };

        $scope.getPurchaseOrderDropDown = function () {
            inventoryIssueService.getPurchaseOrderDropDown()
                .then(function (res) {
                    $scope.poList = res.data;
                }, function (err) {
                    console.log(err);
                })
        };

        $scope.getAllItemByPOID = function (PoStyleId, index) {
            inventoryIssueService.getAllItemByPOID(PoStyleId)
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
        }
        
        $scope.removeRow = function (index) {
            console.log($scope.srVM.Inventories);
            console.log(index);
            $scope.srVM.Inventories.splice(index, 1);
        }

        $scope.saveInventoryIssue = function () {
            if ($scope.inventoryIssueForm.$valid) {
                console.log($scope.srVM);
                inventoryIssueService.saveIssuedInventories($scope.srVM).then(function (res) {
                    alertify.success(res.data);
                    $scope.selectedIssue = "";
                    $scope.srVM = new SrVM();
                    $scope.srVM.Inventories = [];
                    $scope.inventoryIssueForm.$setPristine();
                    $scope.inventoryIssueForm.$setUntouched();
                    getIssues();
                }, function (err) {
                    console.log(err);
                })
            }
        }
    }]);