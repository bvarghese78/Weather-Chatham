app.controller("homeCtrl", function ($scope, $stateParams, $state) {

    $scope.weatherSource = "forecastIO";

    $scope.showPosition = function (pos) {
        $scope.lat = pos.coords.latitude;
        $scope.lng = pos.coords.longitude;
        //$state.go('forecastio.location', { locationName: $scope.lat + ", " + $scope.lng });
    }

    $scope.showError = function (pos) {
        $scope.lat1 = pos.coords.latitude;
        $scope.lng1 = pos.coords.longitude;
    }

    //navigator.geolocation.getCurrentPosition($scope.showPosition, $scope.showError);

    //$scope.navigateTo = function () {
    //    if ($scope.weatherSource == "forecastIO") {
    //        $location.path('/forecastio');
    //    } else if ($scope.weatherSource == "wunderground") {
    //        $location.path('/wunderground');
    //    }
    //};
});