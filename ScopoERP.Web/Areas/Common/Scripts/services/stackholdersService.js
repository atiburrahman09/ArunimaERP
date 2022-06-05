scopoAppServices.service('stackholdersService', function ($http) {

    // common logic 
    this.getStackholder = function (type) {
        switch (type) {
            case 'buyer':
                return $http.get("/Common/Stackholders/GetAllBuyer/");
                break;
            case 'customer':
                return $http.get("/Common/Stackholders/GetAllCustomer/");
                break;
            case 'supplier':
                return $http.get("/Common/Stackholders/GetAllSupplier/");
                break;
        }
    };

    this.createStackholder = function (type, info) {
        console.log(info)
        switch(type){
            case 'buyer':
                return $http.post("/Common/Stackholders/CreateBuyer/", info);
                break;
            case 'customer':
                return $http.post("/Common/Stackholders/CreateCustomer/", info);
                break;
            case 'supplier':
                return $http.post("/Common/Stackholders/CreateSupplier/", info);
                break;
        }        
    };


});// end of service