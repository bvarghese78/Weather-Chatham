app.controller('weatherCtrl', function ($scope, $stateParams, $state, weatherService, toastr) {

    $scope.weatherSource = "forecastIO";
    
    // Navigate to home state when weather source is altered.
    $scope.toggleSource = function () {
        var temp = $scope.address;
        $scope.address = null;

        $state.go('home');
    }

    // Search for weather for a specific location. Loads forecast controller
    $scope.search = function () {
        if (!$scope.address) {
            toastr.error("Enter an address to search");
            return;
        }

        $state.go('forecastio.location', { sourceName:$scope.weatherSource, locationName: $scope.address });
    }
});