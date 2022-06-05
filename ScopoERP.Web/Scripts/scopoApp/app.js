var scopoApp = angular.module('scopoApp', [
    'scopoApp.Controllers', 'ngAlertify', 'flow', 'ngSanitize', 'ui.bootstrap', 'AngularPrint'
]);
var scopoAppControllers = angular.module('scopoApp.Controllers', ['scopoApp.Services']);
var scopoAppServices = angular.module('scopoApp.Services', []);