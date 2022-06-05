scopoAppControllers.controller('prodStatReportCtrl', function ($scope, $rootScope, prodStatReportService) {

    var dailySw;
    $scope.init = function () {

        getDailySewingStatus();
    }



    function DailySewing(actual, expected) {
        this.actual = actual;
        this.expected = expected;
    }


    function drawChart(dailySw) {

        var ctx = document.getElementById("myChart").getContext('2d');
        
        var myChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: ["Sewing Status"],
                datasets: [{
                    label: 'Actual Sewing',
                    data: [dailySw.actual],
                    backgroundColor: [
                        'rgba(255, 99, 132, 0.2)',
                    ],
                    borderColor: [
                        'rgba(255,99,132,1)',
                    ],
                    borderWidth: 1
                },
                {
                    label: 'Expected Sewing',
                    data: [dailySw.expected],
                    backgroundColor: [
                        'rgba(0,128,0,0.2)',
                    ],
                    borderColor: [
                        'rgba(255,99,132,1)',

                    ],
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                }
            }
        });
    }


    function getDailySewingStatus() {
        prodStatReportService.getDailySewingStatus().then(function (res) {
            dailySw = new DailySewing(res.data.ActualSewing, res.data.ExpectedSewing);
            drawChart(dailySw);
        }, function (err) { });
    }


});