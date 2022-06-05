scopoAppControllers.controller('assignCouponCtrl', ['$scope', '$filter', 'assignCouponService', 'couponService', '$parse', 'alertify', '$window', function ($scope, $filter, assignCouponService, couponService, $parse, alertify, $window) {

    $scope.coupon = {};
    $scope.gumsheet = {};
    $scope.codeList = [
    { OffStandardText: "", offStandanrdDuration: 0.00, Value: 0, offStandardValueText: "" },
    { OffStandardText: "", offStandanrdDuration: 0.00, Value: 0, offStandardValueText: "" },
    { OffStandardText: "", offStandanrdDuration: 0.00, Value: 0, offStandardValueText: "" },
    { OffStandardText: "", offStandanrdDuration: 0.00, Value: 0, offStandardValueText: "" },
    { OffStandardText: "", offStandanrdDuration: 0.00, Value: 0, offStandardValueText: "" },
    { OffStandardText: "", offStandanrdDuration: 0.00, Value: 0, offStandardValueText: "" },
    { OffStandardText: "", offStandanrdDuration: 0.00, Value: 0, offStandardValueText: "" },
    { OffStandardText: "", offStandanrdDuration: 0.00, Value: 0, offStandardValueText: "" },
    { OffStandardText: "", offStandanrdDuration: 0.00, Value: 0, offStandardValueText: "" },
    { OffStandardText: "", offStandanrdDuration: 0.00, Value: 0, offStandardValueText: "" }];
    $scope.totalCouponTime = 0.00;
    $scope.totalValueOfCoupon = 0.00;

    $scope.offStandardValue = 0.0;
    $scope.offStandardTime = 0.0;

    $scope.employeeList = [];
    $scope.bundleDropDownList = [];

    //Objects For Calculation Start

    $scope.gumsheetInfo = {};
    $scope.learningCurve = {};
    $scope.targetInfo = {};


    $scope.offStandardRate = [];

    //Objects For Calculation End


    $scope.offStandardObj = { Code: "", offStandanrdDuration: 0.00, Value: 0 };


    $scope.init = function () {
        $scope.offStandardList = [
          { OffStandardText: 'NW' },
          { OffStandardText: 'MT' },
          { OffStandardText: 'RP' },
          { OffStandardText: 'TC' },
          { OffStandardText: 'TB' },
          { OffStandardText: 'BC' },
          { OffStandardText: 'MISC(CC)' },
          { OffStandardText: 'MISC(OPC)' },
          { OffStandardText: 'CT' },
          { OffStandardText: 'ID' },
          { OffStandardText: 'OT' },
          { OffStandardText: 'HO' },
          { OffStandardText: 'LC-NH' },
          { OffStandardText: 'LC-RT' },
          { OffStandardText: 'LC-BD' },
          { OffStandardText: 'LC-FL' },
          { OffStandardText: 'BU' },
          { OffStandardText: 'FM' }
        ];
        $scope.specNoShow = false;

    };


    //date picker date bind with model
    $scope.bindDate = function bindDate(id, model) {
        $scope[model][id] = $('#' + id).val();
    };

    $scope.assignClicked = function () {
        if ($scope.assignCouponForm.$valid) {
            $scope.createGumSheet();
        }
        else {
            alertify.error("Please fill the required informations.");
        }
    };

    //create gumsheet
    $scope.createGumSheet = function () {
        assignCouponService.createGumSheet($scope.gumsheet, $scope.bundleList).then(function (res) {
            //alertify.success("Gumsheet create success.");
            $scope.updateCoupon();
        }, function (err) { handleHttpError(err); });
    }


    //Create Off Standard

    $scope.createOffStandard = function () {
        $scope.lc = { OffStandardText: "LC", offStandanrdDuration: 0.00, Value: $scope.gumsheet.LearningCurve };
        $scope.allowance = { OffStandardText: "AL", offStandanrdDuration: 0.00, Value: $scope.gumsheet.Allowance };
        $scope.codeList.push($scope.lc);
        $scope.codeList.push($scope.allowance);
        $scope.gumSheetOffStandanrdViewModel = {
            EmployeeCardNo: $scope.gumsheet.EmployeeCardNo,
            SpecNo: $scope.gumsheet.SpecNo,
            CompletedDate: $scope.gumsheet.CompletedDate,
            Section: $scope.gumsheet.Section,
            OffStandardVm: $scope.codeList
        };

        assignCouponService.createOffStandard($scope.gumSheetOffStandanrdViewModel).then(function (res) {
            //alertify.alert('Gum Sheet Data', 'You data has been successfully saved!', function () { $scope.reset() });
            alertify.alert('You data has been successfully saved!', function () { $scope.reset() });
        }, function (err) { handleHttpError(err); });
    }

    //update coupon 
    $scope.updateCoupon = function () {
        console.log($scope.gumsheet);
        assignCouponService.updateCoupon($scope.bundleList, $scope.gumsheet.EmployeeCardNo, $scope.gumsheet.SpecNo).then(function (res) {
            $scope.createOffStandard();
        }, function (err) { handleHttpError(err); });
    }

    //All coupon list
    $scope.couponDropDownList = [];
    $scope.getAllCoupons = function () {
        couponService.getAllCoupons().then(function (res) {
            $scope.couponDropDownList = res.data;
        }, function (err) { handleHttpError(err); });
    };

    //Bundle List
    $scope.bundleDropDownList = [];
    $scope.getBundleList = function (specNo) {
        if (specNo == undefined | specNo == null)
        {
            alertify.error("Please Enter Spec No");
            return;
        }
        couponService.getBundleList(specNo).then(function (res) {
            if (res.data.length > 0) {
                $scope.bundleDropDownList = res.data;
                //console.log($scope.bundleDropDownList);                
                $scope.isEmpAssignedToSpec($scope.gumsheet.EmployeeCardNo, specNo);
            }
            else {
                alertify.error("No data found.");
            }

        }, function (err) { handleHttpError(err); });
    };

    //Get Gum Sheet Data
    $scope.getGumSheetData = function (empCardNo, specNo, CompletedDate) {
        assignCouponService.getGumSheetData(empCardNo, specNo, CompletedDate).then(function (res) {
            console.log(res.data);
            if (res.data.length > 0) {
                $scope.gumsheetInfo = res.data[0];
            }
            else {
                $scope.gumsheetInfo.ProductionEarn = 0;
                $scope.gumsheetInfo.OffStandardEarn = 0;
            }
            console.log($scope.gumsheetInfo);
            //console.log($scope.gumsheetData);
        }, function (err) { handleHttpError(err); });
    }


    //CHeck Employee Assigned Spec and Calculate Value
    $scope.isEmpAssignedToSpec = function (empCardNo, specNo) {
        assignCouponService.isEmpAssignedToSpec(empCardNo, specNo).then(function (res) {
            if (res.data == true) {
                $scope.getValueForCalculation(empCardNo, $scope.gumsheet.CompletedDate, $scope.gumsheet.SpecNo);
            }
            else {
                $scope.getValueForCalculation(empCardNo, $scope.gumsheet.CompletedDate, $scope.gumsheet.SpecNo);
                alertify.prompt('This employee is not assigned to this spec. IF you Still want to assign then please enter TB.', 0).then(
                   function onOk(answer) {
                       console.log(typeof (answer.inputValue));
                       $scope.calculateValue({ OffStandardText: "TB", offStandanrdDuration: parseFloat(answer.inputValue) }, 9)
                   },
                   function onCancel() { }
                   );
            }

        }, function (err) { handleHttpError(err); })
    }


    $scope.getValueForCalculation = function (empCardNo, completedDate, specNo) {
        assignCouponService.getValueForCalculation(empCardNo, completedDate, specNo).then(function (res) {
            $scope.offStandardRate = res.data;
            console.log($scope.offStandardRate);
        }, function (err) { handleHttpError(err); });
    }

    //employee select & Get Learning Curve Details
    $scope.getEmployeeDetails = function (emp) {
        //$scope.gumsheet.EmployeeCardNo = emp;
        if (emp == undefined)
        {
            alertify.error("Please enter valid operator card no.");
            return;
        }
        $scope.specNoShow = true;
        $scope.getEmployeeLearningCurve($scope.gumsheet.EmployeeCardNo);
        $scope.getGumSheetData($scope.gumsheet.EmployeeCardNo, $scope.gumsheet.CompletedDate);
    };

    //Employee Learning Curve Info
    $scope.getEmployeeLearningCurve = function (empCardNo) {
        assignCouponService.getEmployeeLearningCurve(empCardNo).then(function (res) {
            $scope.learningCurve = res.data;
            $scope.getCurveInfo($scope.learningCurve.Stage, $scope.learningCurve.Curve);
            //console.log($scope.learningCurve);

        }, function (err) { handleHttpError(err); });
    }

    // Training Curve Information
    $scope.getCurveInfo = function (stage, curve) {
        assignCouponService.getCurveInfo(stage, curve).then(function (res) {
            $scope.targetInfo = res.data;
            //console.log($scope.targetInfo);
        }, function (err) { handleHttpError(err); });
    }

    //couponList
    $scope.coupon.CouponList = [];
    $scope.addCoupons = function (item) {

        //is unique entry coupon-employee
        assignCouponService.isUniqueCouponEntry(item.Value).then(function (res) {
            if (res.data) {
                alertify.error("Coupon already assigned for the employee. Entry failed.");
            }

            else {
                //if already selected once
                var results = $filter('filter')($scope.coupon.CouponList, { CouponNo: item.Text, CouponID: item.Value }, true);

                if (results.length != 0) {
                    alertify.error("Already selected once.");
                }
                else {
                    $scope.coupon.CouponList.push({ CouponNo: item.Text, CouponID: item.Value });
                }
            }
        }, function (err) { handleHttpError(err); });
    };

    //coupon table
    $scope.rowRange = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
    $scope.addRowClicked = function () {
        $scope.rowRange.push($scope.rowRange[$scope.rowRange.length - 1] + 1);
    };

    //Add bundle
    $scope.bundleList = [];
    $scope.addBundle = function (bundleObj) {
        $scope.totalCouponTime += bundleObj.CouponTime;
        $scope.totalValueOfCoupon += bundleObj.CouponValue;
        //console.log("Time" + $scope.totalCouponTime + "Value" + $scope.totalValueOfCoupon);
        $scope.bundleList.push(bundleObj.Text);

    }
    $scope.calculateValue = function (code, index) {
        //calculation O1O value
        var x = ($scope.offStandardRate[0].O2 * $scope.totalCouponTime) - $scope.offStandardRate[0].V1;
        if (x > 0) {
            $scope.offStandardRate[0].O1O = x;
        }
        else {
            $scope.offStandardRate[0].O1O = 0;
        }

        // Calculation For O11 Value
        var y = $scope.offStandardRate[0].V1 - $scope.offStandardRate[0].V3 * $scope.totalCouponTime * $scope.offStandardRate[0].MaxPaid;
        if (y > 0) {
            $scope.offStandardRate[0].O11 = y;
        }
        else {
            $scope.offStandardRate[0].O11 = 0;
        }



        if (code.OffStandardText == "NW" || code.OffStandardText == "BC" || code.OffStandardText == "MISC(OPC)" || code.OffStandardText == "CT") {
            $scope.codeList[index].OffStandardText = code.OffStandardText;
            $scope.codeList[index].offStandanrdDuration = code.offStandanrdDuration;
            $scope.codeList[index].Value = $scope.offStandardRate[0].O1A * code.offStandanrdDuration;
            $scope.codeList[index].offStandardValueText = $scope.codeList[index].Value;
            $scope.offStandardValue += $scope.codeList[index].Value;
            $scope.offStandardTime += $scope.codeList[index].offStandanrdDuration;
        }
        if (code.OffStandardText == "MT") {
            $scope.codeList[index].OffStandardText = code.OffStandardText;
            $scope.codeList[index].offStandanrdDuration = code.offStandanrdDuration;
            $scope.codeList[index].Value = $scope.offStandardRate[0].O3A;
            $scope.codeList[index].offStandardValueText = $scope.codeList[index].Value;
            $scope.offStandardValue += $scope.codeList[index].Value;
            $scope.offStandardTime += $scope.codeList[index].offStandanrdDuration;
        }
        if (code.OffStandardText == "RP" || code.OffStandardText == "TC" || code.OffStandardText == "MISC(CC)" || code.OffStandardText == "ID") {
            $scope.codeList[index].OffStandardText = code.OffStandardText;
            $scope.codeList[index].offStandanrdDuration = code.offStandanrdDuration;
            $scope.codeList[index].Value = $scope.offStandardRate[0].O1B * code.offStandanrdDuration;
            $scope.codeList[index].offStandardValueText = $scope.codeList[index].Value;
            $scope.offStandardValue += $scope.codeList[index].Value;
            $scope.offStandardTime += $scope.codeList[index].offStandanrdDuration;
        }
        if (code.OffStandardText == "TB") {
            $scope.codeList[index].OffStandardText = code.OffStandardText;
            $scope.codeList[index].offStandanrdDuration = code.offStandanrdDuration;
            $scope.codeList[index].Value = $scope.offStandardRate[0].O3;
            $scope.codeList[index].offStandardValueText = $scope.codeList[index].Value;
            $scope.offStandardValue += $scope.codeList[index].Value;
            $scope.offStandardTime += $scope.codeList[index].offStandanrdDuration;
        }
        if (code.OffStandardText == "OT") {
            $scope.codeList[index].OffStandardText = code.OffStandardText;
            $scope.codeList[index].offStandanrdDuration = code.offStandanrdDuration;
            $scope.codeList[index].Value = $scope.offStandardRate[0].O4 * (($scope.gumsheet.ClockedTime - 8.00) * 60);
            $scope.codeList[index].offStandardValueText = $scope.codeList[index].Value;
            $scope.offStandardValue += $scope.codeList[index].Value;
            $scope.offStandardTime += $scope.codeList[index].offStandanrdDuration;
        }
        if (code.OffStandardText == "HO") {
            $scope.codeList[index].OffStandardText = code.OffStandardText;
            $scope.codeList[index].offStandanrdDuration = code.offStandanrdDuration;
            $scope.codeList[index].Value = $scope.offStandardRate[0].O5 * ($scope.gumsheet.ClockedTime * 60);
            $scope.codeList[index].offStandardValueText = $scope.codeList[index].Value;
            $scope.offStandardValue += $scope.codeList[index].Value;
            $scope.offStandardTime += $scope.codeList[index].offStandanrdDuration;
        }
        if (code.OffStandardText == "BU") {
            $scope.codeList[index].OffStandardText = code.OffStandardText;
            $scope.codeList[index].offStandanrdDuration = code.offStandanrdDuration;
            $scope.codeList[index].Value = $scope.offStandardRate[0].O1O;
            $scope.codeList[index].offStandardValueText = $scope.codeList[index].Value;
            $scope.offStandardValue += $scope.codeList[index].Value;
            $scope.offStandardTime += $scope.codeList[index].offStandanrdDuration;
        }
        if (code.OffStandardText == "FM") {
            $scope.codeList[index].OffStandardText = code.OffStandardText;
            $scope.codeList[index].offStandanrdDuration = code.offStandanrdDuration;
            $scope.codeList[index].Value = $scope.offStandardRate[0].O11;
            $scope.codeList[index].offStandardValueText = $scope.codeList[index].Value;
            $scope.offStandardValue += $scope.codeList[index].Value;
            $scope.offStandardTime += $scope.codeList[index].offStandanrdDuration;
        }


        // console.log($scope.codeList);
    }

    $scope.updateData = function () {
        if ($scope.assignCouponForm.$valid) {
            $scope.offStandardRate[0].E = ($scope.totalCouponTime / (($scope.gumsheet.ClockedTime * 60) - $scope.offStandardTime)) * 100;
            if ($scope.learningCurve.RTorNHorFL == "RT") {
                if ($scope.offStandardRate[0].E < $scope.targetInfo.Target_A) {
                    var x = ($scope.offStandardRate[0].O2 - $scope.offStandardRate[0].V4);

                    if (x > 0) {
                        $scope.offStandardRate[0].O8 = $scope.gumsheet.LearningCurve = x;
                    }
                    else {
                        $scope.offStandardRate[0].O8 = $scope.gumsheet.LearningCurve = 0;
                    }
                }
                if ($scope.offStandardRate[0].E > $scope.targetInfo.Target_A && $scope.offStandardRate[0].E < $scope.targetInfo.Target_B) {
                    var x = ($scope.offStandardRate[0].V3 - $scope.offStandardRate[0].V4) * 90;

                    if (x > 0) {
                        $scope.offStandardRate[0].O8 = $scope.gumsheet.LearningCurve = x;
                    }
                    else {
                        $scope.offStandardRate[0].O8 = $scope.gumsheet.LearningCurve = 0;
                    }
                }
                if ($scope.offStandardRate[0].E > $scope.targetInfo.Target_B) {
                    var x = ($scope.offStandardRate[0].E - $scope.targetInfo.Target_B) + 100;
                    console.log($scope.targetInfo.Target_B);
                    console.log(x);
                    $scope.offStandardRate[0].O8 = $scope.gumsheet.LearningCurve = x * ($scope.offStandardRate[0].V3 - $scope.offStandardRate[0].V4);
                }
            }
            if ($scope.learningCurve.RTorNHorFL == "NH") {
                if ($scope.offStandardRate[0].E < $scope.targetInfo.Target_A) {
                    var x = $scope.offStandardRate[0].O2 - $scope.offStandardRate[0].V4;
                    if (x > 0) {
                        $scope.offStandardRate[0].O7 = $scope.gumsheet.LearningCurve = x;
                    }
                    else {
                        $scope.offStandardRate[0].O7 = $scope.gumsheet.LearningCurve = 0;
                    }
                }
                if ($scope.offStandardRate[0].E > $scope.targetInfo.Target_A && $scope.offStandardRate[0].E < $scope.targetInfo.Target_B) {
                    var x = ($scope.offStandardRate[0].V3 - $scope.offStandardRate[0].V4) * 90;

                    if (x > 0) {
                        $scope.offStandardRate[0].O7 = $scope.gumsheet.LearningCurve = x;
                    }
                    else {
                        $scope.offStandardRate[0].O7 = $scope.gumsheet.LearningCurve = 0;
                    }
                }
                if ($scope.offStandardRate[0].E > $scope.targetInfo.Target_B) {
                    $scope.offStandardRate[0].O7 = $scope.gumsheet.LearningCurve = $scope.offStandardRate[0].V3 - $scope.offStandardRate[0].V4;
                }

            }
            if ($scope.learningCurve.RTorNHorFL == "FL") {
                var x = ($scope.offStandardRate[0].E - $scope.targetInfo.Target_B) * $scope.offStandardRate[0].V3;
                $scope.offStandardRate[0].O1O = $scope.gumsheet.LearningCurve = x;
            }
            


            $scope.gumsheetInfo.ProductionEarn += $scope.totalValueOfCoupon;
            $scope.gumsheetInfo.OffStandardEarn += $scope.offStandardValue + $scope.gumsheet.LearningCurve;
        }
        else {
            alertify.error("Fill all the required information's");
        }

       
    }

    $scope.bindDate = function (id, model) {
        $scope[model][id] = $('#' + id).val();
    };

    $scope.reset = function () {

        $window.location.reload();

//        $scope.gumsheet = {};
//        $scope.gumsheetInfo = {};
//        $scope.bundleDropDownList = [];
//        $scope.bundleList = [];
//        $scope.learningCurve = {};
//        $scope.targetInfo = {};
//        $scope.offStandardRate = [];
//        $scope.codeList = [
//{ OffStandardText: "", offStandanrdDuration: 0.00, Value: 0, offStandardValueText: "" },
//{ OffStandardText: "", offStandanrdDuration: 0.00, Value: 0, offStandardValueText: "" },
//{ OffStandardText: "", offStandanrdDuration: 0.00, Value: 0, offStandardValueText: "" },
//{ OffStandardText: "", offStandanrdDuration: 0.00, Value: 0, offStandardValueText: "" },
//{ OffStandardText: "", offStandanrdDuration: 0.00, Value: 0, offStandardValueText: "" },
//{ OffStandardText: "", offStandanrdDuration: 0.00, Value: 0, offStandardValueText: "" },
//{ OffStandardText: "", offStandanrdDuration: 0.00, Value: 0, offStandardValueText: "" },
//{ OffStandardText: "", offStandanrdDuration: 0.00, Value: 0, offStandardValueText: "" },
//{ OffStandardText: "", offStandanrdDuration: 0.00, Value: 0, offStandardValueText: "" },
//{ OffStandardText: "", offStandanrdDuration: 0.00, Value: 0, offStandardValueText: "" }];
//        $scope.totalCouponTime = 0.00;
//        $scope.totalValueOfCoupon = 0.00;

//        $scope.offStandardValue = 0.00;
//        $scope.offStandardTime = 0.00;
//        $scope.assignCouponForm.$setPristine();
//        $scope.assignCouponForm.$setUntouched();
    };

}]);