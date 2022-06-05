scopoAppServices.service('assignCouponService', function ($http) {

    this.assignCoupon = function (assignCoumponVM) {
        return $http.post("/Production/Coupon/SaveAssignCoupon/", assignCoumponVM);
    }

    this.isUniqueCouponEntry = function (couponID) {
        return $http.get("/Production/Coupon/IsUniqueCouponEntry/", { params: { couponID: couponID } });
    };

    this.createGumSheet = function (gumSheet, bundleObj) {
        console.log("service");
        console.log(gumSheet);
        console.log(bundleObj);
        return $http({
            url: "/Production/Coupon/CreateGumSheet/",
            method: "POST",
            data: {
                gumsheetVM: gumSheet, bundleList: bundleObj
            }
        });
        //return $http.post("/Production/Coupon/CreateGumSheet/", { params: { gumsheetVM: gumSheet, bundleList: bundleObj } });
    }

    this.updateCoupon = function (bundleList, employeeCardNo, specNo) {
        return $http({
            url: "/Production/Coupon/UpdateCoupon/",
            method: "POST",
            data: {
                bundleList: bundleList, employeeCardNo: employeeCardNo, specNo: specNo
            }
        });
        //return $http.get("/Production/Coupon/UpdateCoupon/", { params: { bundleList: bundleList, employeeCardNo: employeeCardNo, specNo: specNo } });
    }

    this.isEmpAssignedToSpec = function (empCardNo, specNo) {
        return $http.get("/Production/Coupon/IsEmpAssignedToSpec/", { params: { empCardNo: empCardNo, specNo: specNo } });
    }
    this.getValueForCalculation = function (empCardNo, completedDate, specNo) {
        return $http.get("/Production/Coupon/GetValueForCalculation/", { params: { empCardNo: empCardNo, completedDate: completedDate, specNo: specNo } });
    }

    this.getEmployeeLearningCurve = function (empCardNo) {
        return $http.get("/Production/Coupon/GetEmployeeLearningCurve/", { params: { empCardNo: empCardNo } });
    }

    this.getGumSheetData = function (empCardNo, completedDate) {
        return $http.get("/Production/Coupon/GetGumSheetData/", { params: { empCardNo: empCardNo, completedDate: completedDate } });
    }

    this.getCurveInfo = function (stage,curve) {
        return $http.get("/Production/Coupon/GetCurveInfo/", { params: {stage : stage, curve:curve}});
    }

    this.createOffStandard = function (offStandardViewModel) {
        console.log(offStandardViewModel);       
        return $http.post("/Production/Coupon/CreateOffStandard/", offStandardViewModel);
    }
})