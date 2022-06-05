var app = angular.module('job.services', []);

app.service('jobService', ['$http', function ($http) {

    this.getAllJob = function (year) {
        return $http({
            url: '/Job/GetJobList',
            method: 'GET',
            params: { year: year }
        });
    }

    this.updateAdvancedCM = function (jobList) {
        return $http.post('/Job/UpdateAdvancedCM', jobList);
    }
}])