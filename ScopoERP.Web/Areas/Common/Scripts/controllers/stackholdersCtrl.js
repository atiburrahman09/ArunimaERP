scopoAppControllers.controller('stackholdersCtrl', ['$scope', 'stackholdersService', function ($scope, stackholdersService) {
    var current = { type: "", index: 0 };
    var previous = { type: "", index: 0 };
    $scope.init = function () {
        $scope.listType = "";
        $scope.createType = "";
        $scope.detailType = "";
        $scope.updateType = "";
        $scope.stackholders = { allBuyer: [], allCustomer: [], allSupplier: [] };
        $scope.types = [{ name: 'Buyer', type: 'buyer' }, { name: 'Customer', type: 'customer' }, { name: 'Supplier', type: 'supplier' }];
        $scope.submitButton = "Edit";
        //reload($scope.listType);

    }; // end init()

    var createTypeSelector = function (type) {
        $scope.instance = {};
        $scope.createType = type;
        $scope.formButton = "Create";
        $scope.detailType = false;
        $scope[type + 'Form'].$setPristine();
        $scope[type + 'Form'].$setUntouched();
    }

    var selectionControl = function (index, type) {
        $scope.createType = false;
        $scope.formButton = "Edit";
        $scope.detailType = type.toLowerCase().substr(3);
        $scope.instance = angular.copy($scope.stackholders[type][index]);

        if (type == current.type && index == current.index) {
            $scope.instance = angular.copy($scope.stackholders[type][index]);
        } else {
            previous.type = current.type;
            previous.index = current.index;
            current.type = type;
            current.index = index;
            $scope.stackholders[current.type][current.index].selected = 1;
            if (previous.type) {
                $scope.stackholders[previous.type][previous.index].selected = 0;
            }

        }
    }

    var editInstance = function (type) {
        $scope.createType = type;
        $scope.formButton = "Update";
        $scope.oldInstance = angular.copy($scope.instance);
    };

    var createStackholder = function () {
        console.log($scope.instance)
        if ($scope[$scope.createType + 'Form'].$valid) {
            stackholdersService.createStackholder($scope.createType, $scope.instance).then(function (res) {
                $scope.listType = $scope.createType;
                reload($scope.listType);
            });
        } else {

        }
    };

    var cancelForm = function () {
        $scope.createType = "";
        $scope.instance = angular.copy($scope.oldInstance);
        $scope.submitButton = "Edit";
    }

    var clearForm = function (type) {
        $scope.instance = {};
        $scope[type + 'Form'].$setPristine();
        $scope[type + 'Form'].$setUntouched();
    }

    var reload = function (type) {
        $scope.createType = "";
        $scope.detailType = "";
        stackholdersService.getStackholder(type).then(function (res) {
            console.log(res.data);
            $scope.stackholders['all' + capWord(type)] = res.data;
            clearForm(type);
        });
    };

    $scope.reload = reload;
    $scope.createTypeSelector = createTypeSelector;
    $scope.selectionControl = selectionControl;
    $scope.editInstance = editInstance;
    $scope.createStackholder = createStackholder;
    $scope.cancelForm = cancelForm;


    var capWord = function (string) {
        return string.charAt(0).toUpperCase() + string.slice(1);
    };

}]);// end of controller