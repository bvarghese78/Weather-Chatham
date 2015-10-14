app.controller('weatherCtrl', function ($scope, $stateParams, $state, weatherService, toastr) {
    $scope.weatherSource = "forecastIO";

    $scope.toggleSource = function () {
        var temp = $scope.address;
        $scope.address = null;

        $state.go('home');
    }

    $scope.search = function () {
        if (!$scope.address) {
            toastr.error("Enter an address to search");
            return;
        }

        $state.go('forecastio.location', { locationName: $scope.address });
    }
});