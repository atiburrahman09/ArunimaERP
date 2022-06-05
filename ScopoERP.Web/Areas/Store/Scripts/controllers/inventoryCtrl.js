scopoAppControllers.controller('inventoryCtrl', ['$scope', 'inventoryService',
function ($scope, inventoryService) {
    var allPI = [];
    $scope.blDetailsList = [];
    $scope.blChalanList = [];
    $scope.blChalanList = [];
    $scope.disableForm = true;    
    $scope.blInstance = {};
    $scope.isBL = true;
    $scope.PI = {
        list: [],
        selected: ""
    }

    $scope.dt = {
        blDate: false     
    }

    $scope.pickDate = function (val) {
        $scope.dt[val] = true;
    }


   
    $scope.init = function () { 
        getAllPI();
        loadBLFirst();
        $scope.loadChalan();
        $scope.isBL = true;
    };


    $scope.refreshDetails = function () {
        console.log($scope.PI.selected);
        if ($scope.PI.selected != null) {
            if ($scope.isBL) {
                console.log("In BLL");
                getBLDetails($scope.blInstance.BLID, $scope.PI.selected);
            } else {
                getBLDetailsForChalan($scope.blInstance.BLID, $scope.PI.selected);
            }            
        }        
    }

    $scope.blInstance = null;
    $scope.prevInstance = null;

    $scope.selectInstance = function (index, instance) {
        if ($scope.isBL) {
            $scope.disableForm = true;
        }
        instance.selected = true;
        if ($scope.prevInstance != null) {            
            if (instance.BLNo === $scope.prevInstance.BLNo) {
                return;
            }
            $scope.prevInstance.selected = false;
        }
        $scope.prevInstance = instance;
        $scope.blInstance = angular.copy(instance);
        showBL(instance);
    }
    $scope.blSelected = function (instance)
    {
        $scope.blInstance = {};
        $scope.blInstance = JSON.parse(instance);
        $scope.isBL = true;
        $scope.disableForm = true;
        $scope.PI.list = [];
        showBL(JSON.parse(instance));
    }

    $scope.chalanSelected = function (instance)
    {
        $scope.blInstance = {};
        $scope.blInstance = JSON.parse(instance);
        $scope.PI.list = [];
        $scope.showForm();
        getAllPI();
    }

    $scope.showForm = function () {        
        $scope.isBL = false;        
        $scope.blDetailsList = [];
        $scope.disableForm = false;
        $scope.PI.list = angular.copy(allPI);
    };

    function showBL(instance) {
        console.log(instance);
        console.log(instance.BLID);
        getPIbyBLID($scope.blInstance.BLID);
    }

    $scope.loadBL = function (selectionIndex) {
        $scope.isBL = true;
        $scope.disableForm = true;
        $scope.chalanForm.$setPristine();
        $scope.chalanForm.$setUntouched();
        $scope.PI.list = [];
        loadBLFirst();
    };

    $scope.loadChalan = function () {
        //$scope.showForm();        
        inventoryService.getAllChalan()
        .then(function (res) {
            $scope.chalanList = formatDate(res.data, ['DocumentSentToCNF']);
            $scope.chalanList = formatDate(res.data, []);
            //$scope.selectInstance(0, res.data[0]);
        }, function (err) {
            handleHttpError(err);
            });
        //$scope.isBL = false;
    };

    
    $scope.updateBLDetails = function () {        
        inventoryService.updateBLDetails($scope.blDetailsList)
        .then(function (res) {
            alertify.success("Successfully Saved!");
        }, function (err) {
            handleHttpError(err);
        })
    }
    
    var getAllPI = function () {
        inventoryService.getAllPI()
        .then(function (res) {                
            allPI = res.data;                
        }, function(err){
            handleHttpError(err);
        })
    };

    var getBLDetails = function (blID, piID) {
        console.log(piID);
        inventoryService.getBLDetails(blID, piID)
        .then(function (res) {
            $scope.blDetailsList = res.data;            
        }, function (err) {
            handleHttpError(err);
        })
    }

    var getBLDetailsForChalan = function (blID, piID) {
        inventoryService.getBLDetailsForChalan(blID ,piID)
        .then(function (res) {
            $scope.blDetailsList = res.data;
        }, function (err) {
            handleHttpError(err);
        })
    }

    var getPIbyBLID = function (blID) {
        $scope.PI.list = [];
        $scope.PI.selected = "";
        inventoryService.getPIbyBLID(blID)
        .then(function (res) {
            console.log(res.data);
            if (res.data.length != 0) {
                $scope.PI.list = res.data;
                console.log($scope.PI.list);
            } else {
                $scope.PI.list = allPI;
                console.log($scope.PI.list);
            }
        }, function (err) {
            handleHttpError(err);
        });
    }

    var loadBLFirst = function () {        
        inventoryService.getAllBL().then(function (res) {
            console.log(res.data);
            $scope.blList = formatDate(res.data, ['DocumentSentToCNF']);
            $scope.blList = formatDate(res.data, []);
            //$scope.selectInstance(0, res.data[0]);
        }, function (err) {
            handleHttpError(err);
        });        
    }

    $scope.createChalan = function createChalan() {

        if (!$scope.chalanForm.$valid) {
            console.log("Invalid Form");
            return;
        }

        if ($scope.chalanForm.$valid) {                        
            inventoryService.createChalan($scope.blInstance)
            .then(function (res) {
                $scope.loadChalan();
                alertify.success('Successfully saved');
            }, function (err) {
                handleHttpError(err);
            })
        } else {
            console.log("Invalid Form");
        }
    };
    
}]);