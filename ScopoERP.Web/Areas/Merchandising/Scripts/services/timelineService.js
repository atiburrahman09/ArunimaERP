scopoAppServices.service('timelineService', function ($http) {

   
    this.getAllTimelineByPOID = function (purchaseOrderID) {
        return $http.get("/Merchandising/Timeline/GetAllTimelineByPOID", { params: {purchaseOrderID : purchaseOrderID}});
    }
    this.saveTimeline = function (timeline) {
        return $http.post("/Merchandising/Timeline/SaveTimeline", timeline);
    }


    this.removeTimeline = function (timelineID) {
        return $http.get("/Merchandising/Timeline/RemoveTimeline", { params: { timelineID: timelineID } });
    }

    this.getStyleList = function () {
        return $http.get("/Merchandising/Timeline/GetStyleDropDown/")
        .then(function (result) {
            return result.data;
        },
        function (err) {
            console.log(err);
        });
    }


    this.getPurchaseOrderListByStyle = function (styleID) {
        return $http.get("/Merchandising/Timeline/GetPurchaseOrderListByStyle/?styleID=" + styleID);
    }
});