app.controller('weatherCtrl', function ($scope, $stateParams, $state, weatherService) {
    $scope.employee = weatherService.employee;

    $scope.navigateTo = function () {
        if ($scope.weatherSource == "forecastIO") {
            $location.path('/forecastio');
        } else if ($scope.weatherSource == "wunderground") {
            $location.path('/wunderground');
        }
    };

    $scope.search = function () {
        if (!$scope.address) {
            alert("Enter an address to search");
            return;
        }
        

            $state.go('forecastio.location', { locationName: $scope.address });
    
    }
});