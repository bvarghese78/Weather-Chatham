app.controller("homeCtrl", function ($scope, $stateParams, $state) {

    $scope.weatherSource = "forecastIO";

    // Sets users lat and lon and loads forecast ctrl
    $scope.showPosition = function (pos) {
        $scope.lat = pos.coords.latitude;
        $scope.lng = pos.coords.longitude;
        //$state.go('forecastio.location', { locationName: $scope.lat + ", " + $scope.lng });
    }

    $scope.showError = function (pos) {
        $scope.lat1 = pos.coords.latitude;
        $scope.lng1 = pos.coords.longitude;
    }

    // Gets users current location
    //navigator.geolocation.getCurrentPosition($scope.showPosition, $scope.showError);
});