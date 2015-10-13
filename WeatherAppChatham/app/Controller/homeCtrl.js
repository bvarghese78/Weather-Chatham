app.controller("homeCtrl", function ($scope, $stateParams, $location) {

    $scope.weatherSource = null;

    $scope.navigateTo = function () {
        if ($scope.weatherSource == "forecastIO") {
            $location.path('/forecastio');
        } else if ($scope.weatherSource == "wunderground") {
            $location.path('/wunderground');
        }
    };
});