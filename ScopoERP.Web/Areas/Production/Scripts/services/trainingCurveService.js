scopoAppServices.service('trainingCurveService', function ($http) {

    this.saveTrainingCurve = function (curve) {
        return $http.post("/Production/TrainingCurve/SaveTrainingCurve/", curve);
    }

    this.getAllTrainingCurve = function () {
        return $http.get("/Production/TrainingCurve/GetAllTrainingCurve/");
    }
});