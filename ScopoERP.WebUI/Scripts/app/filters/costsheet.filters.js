var app = angular.module('costsheet.filters', [
	'costsheet.services'
]);
app.filter('mapUnit', ['costsheetCrudService', function (costsheetCrudService) {
	return function (input, unitDropDown) {
		if (unitDropDown != undefined) {
			for (var i in unitDropDown) {
				if (unitDropDown[i].Value == input) {
					return unitDropDown[i].Text;
				}
			}
			return 'unknown';
		}
	};
}]);

app.filter('mapItemCategory', ['costsheetCrudService', function (costsheetCrudService) {
    return function (input, itemCategoryDropDown) {
        if (itemCategoryDropDown != undefined) {
            for (var i in itemCategoryDropDown) {
                if (itemCategoryDropDown[i].Value == input) {
                    return itemCategoryDropDown[i].Text;
                }
            }
            return 'unknown';
        }
    };
}]);

app.filter('mapItem', ['costsheetCrudService', function (costsheetCrudService) {
    return function (input, itemDropDown) {
        if (itemDropDown != undefined) {
            for (var i in itemDropDown) {
                if (itemDropDown[i].Value == input) {
                    return itemDropDown[i].Text;
                }
            }
            return 'unknown';
        }
    };
}]);