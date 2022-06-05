scopoAppControllers.controller('settingsCtrl', ['$scope', 'settingsService', 'alertify', function ($scope, settingsService, alertify) {
    var currentIndex = null;
    var previousIndex = null;
    var currentSupervisorIndex = null;
    var previousSupervisorIndex = null;
    var currentJobClassIndex = null;
    var previousJobClassIndex = null;
    var currentStdOperationIndex = null;
    var previousStdOperationIndex = null;
    var currentSpecIndex = null;
    var previousSpecIndex = null;




    $scope.init = function () {
        $scope.Setting = "Job Class";
        $scope.jobClassListShow = true;
        $scope.jobClassFormShow = true;
        $scope.supervisorListShow = false;
        $scope.supervisorFormShow = false;


        $scope.getAllJobClasses();

        //settingsService.getAllOperations().then(function (res) {
        //    $scope.operationList = res.data;

        //});
        //$scope.getAllOperations();

        //settingsService.getAllJobClasses().then(function (res) {
        //    $scope.allJobClasses = res.data;
        //});
    }

    $scope.getAllFloor = function () {
        settingsService.getFloor().then(function (res) {
            $scope.floorList = res.data;
        });
    }

    $scope.getAllOperationCategories = function () {
        settingsService.getAllOperationCategories().then(function (res) {
            $scope.operationCategoryList = res.data;
        });
    }



    $scope.getAllOperations = function () {
        settingsService.getAllOperations().then(function (res) {
            $scope.operationList = res.data;
            console.log($scope.operationList)

        });
    }
    //$scope.getAllJobClasses = function () {
    //    console.log("Job Class");
    //    settingsService.getAllJobClasses().then(function (res) {
    //        $scope.jobClassList = res.data;
    //    });
    //}

    function saveJobClass() {
        if ($scope.jobClassForm.$valid) {
            settingsService.saveJobClass($scope.jobClass).then(function (res) {
                if (res.data.ErrorCode != null) {
                    alertify.error(res.data.Message);
                } else {
                    //alertify.alert(res.data.Message);
                    resetForm($scope.jobClassForm);
                    getAllJobClasses();
                }

            });
        }
    }
    //fetching All Job Class
    function getAllJobClasses() {
        console.log("Job Class");
        settingsService.getAllJobClasses().then(function (res) {
            $scope.jobClassList = res.data;
        });
    }

    function selectedJob(job) {
        var index = indexOfObjectInArray($scope.jobClassList, 'JobClassID', job.JobClassID);

        if (index == currentJobClassIndex) {
            $scope.jobClassList[currentJobClassIndex].selected = true;
        } else {
            previousJobClassIndex = currentJobClassIndex;
            currentJobClassIndex = index;
            $scope.jobClassList[currentJobClassIndex].selected = true;
            $scope.jobClass = angular.copy($scope.jobClassList[currentJobClassIndex]);

            if (previousJobClassIndex != null) {
                $scope.jobClassList[previousJobClassIndex].selected = false;
            }
            //getAllJobClass();
        }
    }


    function saveSupervisor() {
        if ($scope.supervisorForm.$valid) {
            settingsService.saveSupervisor($scope.supervisor).then(function (res) {
                if (res.data.ErrorCode != null) {
                    alertify.error(res.data.Message);
                } else {
                    //alertify.alert(res.data.Message);
                    resetForm($scope.supervisorForm);
                    getAllSupervisors();
                }

            });
        }
    }
    //fetching All Job Class
    function getAllSupervisors() {
        settingsService.getAllSupervisors().then(function (res) {
            $scope.supervisorList = res.data;
        });
    }

    function selectedSupervisor(supervisor) {
        var index = indexOfObjectInArray($scope.supervisorList, 'SupervisorID', supervisor.SupervisorID);

        if (index == currentSupervisorIndex) {
            $scope.supervisorList[currentSupervisorIndex].selected = true;
        } else {
            previousSupervisorIndex = currentSupervisorIndex;
            currentSupervisorIndex = index;
            $scope.supervisorList[currentSupervisorIndex].selected = true;
            $scope.supervisor = angular.copy($scope.supervisorList[currentSupervisorIndex]);
            console.log($scope.supervisor);
            $scope.getLine($scope.supervisor.Floor);

            if (previousSupervisorIndex != null) {
                $scope.supervisorList[previousSupervisorIndex].selected = false;
            }
            //getAllJobClass();
        }
    }

    $scope.supervisorSetting = function () {
        $scope.getAllSupervisors();
        $scope.getAllFloor();
        $scope.Setting = "Supervisor";
        $scope.jobClassListShow = false;
        $scope.jobClassFormShow = false;
        $scope.operationListShow = false;
        $scope.operationFormShow = false;
        $scope.specListShow = false;
        $scope.specFormShow = false;
        $scope.supervisorListShow = true;
        $scope.supervisorFormShow = true;
        $scope.jobClassForm.$setPristine();
        $scope.jobClassForm.$setUntouched();
    }

    $scope.jobClassSetting = function () {
        $scope.Setting = "Job Class";
        $scope.jobClassListShow = true;
        $scope.jobClassFormShow = true;
        $scope.supervisorListShow = false;
        $scope.supervisorFormShow = false;
        $scope.operationListShow = false;
        $scope.operationFormShow = false;
        $scope.specListShow = false;
        $scope.specFormShow = false;
        $scope.supervisorForm.$setPristine();
        $scope.supervisorForm.$setUntouched();
    }

    $scope.operationSetting = function () {
        $scope.getAllStdOperations();
        $scope.getAllOperationCategories();
        getAllJobClasses();
        $scope.Setting = "Operation";
        $scope.jobClassListShow = false;
        $scope.jobClassFormShow = false;
        $scope.supervisorListShow = false;
        $scope.supervisorFormShow = false;
        $scope.specListShow = false;
        $scope.specFormShow = false;
        $scope.operationListShow = true;
        $scope.operationFormShow = true;
        $scope.operationForm.$setPristine();
        $scope.operationForm.$setUntouched();
    }

    $scope.specSetting = function () {
        $scope.getAllOperations();
        $scope.getAllSpecs();
        $scope.Setting = "Spec";
        $scope.jobClassListShow = false;
        $scope.jobClassFormShow = false;
        $scope.supervisorListShow = false;
        $scope.supervisorFormShow = false;
        $scope.operationListShow = false;
        $scope.operationFormShow = false;
        $scope.specListShow = true;
        $scope.specFormShow = true;
        $scope.specForm.$setPristine();
        $scope.specForm.$setUntouched();
    }

    $scope.getLine = function (floor) {
        settingsService.getLine(floor).then(function (res) {
            $scope.lineList = res.data;
        });
    }

    function saveOperation() {
        if ($scope.operationForm.$valid) {
            settingsService.saveOperation($scope.stdOperation).then(function (res) {
                if (res.data.ErrorCode > 0) {
                    alertify.error(res.data.Message);
                } else {
                    alertify.alert("Operation successfully saved.");
                    getAllStdOperations();
                    resetForm($scope.operationForm);
                }

            });
        }
    }

    function getAllStdOperations() {
        settingsService.getAllStdOperations().then(function (res) {
            $scope.allStdOperations = res.data;
        });
    }

    function selectedOperation(operaion) {
        var index = indexOfObjectInArray($scope.allStdOperations, 'OperationID', operaion.OperationID);

        if (index == currentStdOperationIndex) {
            $scope.allStdOperations[currentStdOperationIndex].selected = true;
        } else {
            previousStdOperationIndex = currentStdOperationIndex;
            currentStdOperationIndex = index;
            $scope.allStdOperations[currentStdOperationIndex].selected = true;
            $scope.stdOperation = angular.copy($scope.allStdOperations[currentStdOperationIndex]);

            console.log($scope.stdOperation);

            if (previousStdOperationIndex != null) {
                $scope.allStdOperations[previousStdOperationIndex].selected = false;
            }
        }

        //if (index == currentStdOperationIndex) {
        //    //$scope.allStdOperations[currentStdOperationIndex].selected = true;
        //} else {
        //    //previousStdOperationIndex = currentStdOperationIndex;
        //    currentStdOperationIndex = index;
        //    //$scope.allStdOperations[currentStdOperationIndex].selected = true;
        //    $scope.stdOperation = angular.copy($scope.allStdOperations[currentStdOperationIndex]);
        //    getAllStdOperations();

        //    //if (previousStdOperationIndex != null) {
        //    //    $scope.allStdOperations[previousStdOperationIndex].selected = false;
        //    //}
        //}
    }


    function saveSpec() {
        if ($scope.specForm.$valid) {
            settingsService.saveSpec($scope.spec).then(function (res) {
                if (res.data.ErrorCode != null) {
                    alertify.error(res.data.Message);
                } else {
                    //alertify.alert(res.data.Message);
                    resetForm($scope.specForm);
                    getAllSpecs();
                }

            });
        }
        else {
            alertify.error("Please fill the required fields.");
        }
    }
    //fetching All Job Class
    function getAllSpecs() {
        settingsService.getAllSpecs().then(function (res) {
            $scope.specList = res.data;
        });
    }

    function selectedSpec(spec) {
        var index = indexOfObjectInArray($scope.specList, 'SpecID', spec.SpecID);

        if (index == currentSpecIndex) {
            $scope.specList[currentSpecIndex].selected = true;
        } else {
            previousSpecIndex = currentSpecIndex;
            currentSpecIndex = index;
            $scope.specList[currentSpecIndex].selected = true;
            $scope.spec = angular.copy($scope.specList[currentSpecIndex]);
            console.log($scope.spec);

            if (previousSpecIndex != null) {
                $scope.specList[previousSpecIndex].selected = false;
            }
            //getAllJobClass();
        }
    }



    function resetForm(form) {
        console.log(form);
        $scope.supervisor = {};
        $scope.jobClass = {};
        $scope.stdOperation = {};
        $scope.spec = {};
        if (currentSupervisorIndex != null) {
            currentSupervisorIndex = null;
            previousSupervisorIndex = null;
        }
        if (currentJobClassIndex != null) {
            currentJobClassIndex = null;
            previousJobClassIndex = null;
        }

        if (currentStdOperationIndex != null) {
            currentStdOperationIndex = null;
            previousStdOperationIndex = null;
        }

        if (currentSpecIndex != null) {
            currentSpecIndex = null;
            previousSpecIndex = null;
        }

        form.$setPristine();
        form.$setUntouched();
    }

    function indexOfObjectInArray(array, property, value) {
        for (var i = 0; i < array.length; i++) {
            if (array[i][property] == value) {
                return i;
            }
        }
        return -1;
    }


    $scope.saveJobClass = saveJobClass;
    $scope.selectedJob = selectedJob;
    $scope.getAllJobClasses = getAllJobClasses;

    $scope.saveSupervisor = saveSupervisor;
    $scope.selectedSupervisor = selectedSupervisor;
    $scope.getAllSupervisors = getAllSupervisors;

    $scope.saveOperation = saveOperation;
    $scope.selectedOperation = selectedOperation;
    $scope.getAllStdOperations = getAllStdOperations;


    $scope.saveSpec = saveSpec;
    $scope.selectedSpec = selectedSpec;
    $scope.getAllSpecs = getAllSpecs;

    $scope.resetForm = resetForm;
}])