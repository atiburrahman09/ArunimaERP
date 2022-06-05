// takes a list and assumes that it contains objects that has property names with 'Date' in it
// finds those properties and formats it's value to specific date format if provided, to default otherwise
// if user provides a specific set of properties then it will operate on just those properties

function formatDate(dataList, propertyList) {
    console.log(dataList);
    var format = "DD-MMM-YYYY";
    //verify data exists return empty list otherwise
    if (dataList != null && dataList.length > 0) {
        // if propertyList contains any property then format dates for just those properties
        // this way it will be faster        
        if (propertyList != null && propertyList.length > 0) {
            for (var index in dataList) {
                for (var prop in dataList[index]) {
                    for (i in propertyList) {
                        if (propertyList[i] === prop) {
                            dataList[index][prop] != null ? dataList[index][prop] = moment(dataList[index][prop]).format(format) : dataList[index][prop] = "";

                        }
                    }
                }
            }
            return dataList;
        }
        // if propertyList is empty or null then look for properties with 'Date'
        for (var index in dataList) {
            for (var prop in dataList[index]) {
                if (prop.indexOf("Date") > -1) {
                    dataList[index][prop] != null ? dataList[index][prop] = moment(dataList[index][prop]).format(format) : dataList[index][prop] = "";
                }
            }
        }

        return dataList;
    }
    return [];
}


function handleHttpError(err) {

    if (err.status === 400) {
        alertify.error(err.data);
        return;
    }
    else {
        alertify.alert('Unknown error occured, Please contact technical support!');
        return;
    }
}
