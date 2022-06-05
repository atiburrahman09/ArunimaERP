scopoAppControllers.controller('dashboardCtrl', ['$scope', 'dashboardService', function ($scope, dashboardService) {

    $scope.init = function () {
        $scope.dashboard = {};

        getDashboardData();
        getShipmentPerdayDataSet();
    }

    function getDashboardData() {
        dashboardService.getDashboardData().then(function (res) {
            $scope.dashboard = res.data;
            drawOrderShipmentPie({
                order: res.data.TotalOrder,
                shipment: res.data.CurrentShipment
            });
        });    
    }

    function getShipmentPerdayDataSet() {
        dashboardService.getShipmentPerDayDataSet().then(function (res) {
            console.log(res.data);
            drawShipmentPerDayChart(res.data);
        }, function () {

        });
    }

    function drawShipmentPerDayChart(data) {
        var ctx = document.getElementById('_shipmentQty').getContext('2d');
        var shipmentQty = new Chart(ctx,
            {
                type: 'bar',
                data: {
                    labels: data.Dates,
                    datasets: [
                        {
                            label: 'Shipment Quantity',
                            backgroundColor: '#3e95cd',
                            data: data.Amounts
                        }
                    ]
                },
                options: {
                    title: {
                        text: 'Shipment per day',
                        display: true
                    }
                }
        })
    }



    function drawOrderShipmentPie(data) {

        var ctx = document.getElementById("order_shiptment_report").getContext('2d');

        var myChart = new Chart(ctx, {
            type: 'pie',
            data: {
                labels: ["Order", "Shipment"],
                datasets: [{
                    label: "Order vs shipment",
                    backgroundColor: ["#ffb22b", "#7460ee"],
                    data: [data.order, data.shipment]
                }]
            },
            options: {
                title: {
                    display: true,
                    text: 'Total order vs this month shipment pie'
                }
            }
        });
    }

}]);