app.controller("homeCtrl", function ($scope, $stateParams, $location) {

    $scope.weatherSource = null;

    $scope.showPosition = function (pos) {
        $scope.lat = pos.coords.latitude;
        $scope.lng = pos.coords.longitude;
    }

    $scope.showError = function (pos) {
        $scope.lat1 = pos.coords.latitude;
        $scope.lng1 = pos.coords.longitude;
    }

    navigator.geolocation.getCurrentPosition($scope.showPosition, $scope.showError);

    $scope.navigateTo = function () {
        if ($scope.weatherSource == "forecastIO") {
            $location.path('/forecastio');
        } else if ($scope.weatherSource == "wunderground") {
            $location.path('/wunderground');
        }
    };
});