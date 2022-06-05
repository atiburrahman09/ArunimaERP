scopoAppServices.service('inventoryService', function ($http) {

    this.getAllBL = function () {
        return $http.get("/Inventory/GetAllBL/");
    }

    this.getAllChalan = function () {
        return $http.get("/Inventory/GetAllChalan/");
    }

    this.createChalan = function (blInstance) {
        return $http.post("/Inventory/CreateChalan/", blInstance);
    }

    this.getBLDetails = function (blID, piID) {
        return $http.get("/Inventory/GetBLDetails/", { params: { blID: blID, piID: piID, itemID: null } });
    }

    this.getAllPI = function () {
        return $http.get("/Inventory/GetPIDropdown/");
    }

    this.getPIbyBLID = function (blID) {
        return $http.get("/Inventory/GetPIDropdownByBL/", { params: { blID: blID } });
    }

    this.getBLDetailsForChalan = function (blID, piID) {
        return $http.get("/Inventory/GetBLDetailsForChalan/", { params: { bliD: blID, piID: piID } })
    }

    this.updateBLDetails = function (list) {
        for (var i = 0; i < list.length; i++) {
            list[i].SetupDate = new Date();
        }
        return $http.put("/Inventory/UpdateBLDetails/", list);
    }
});