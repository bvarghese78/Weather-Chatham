app.controller("forecastioCtrl", function ($scope, $stateParams, weatherService, toastr) {
    if ($stateParams.locationName) {
        $scope.locationName = $stateParams.locationName;
    }
    $scope.mvcmodel = {
        currentWeather: null,
        daily: null,
        address: $scope.locationName
    }

    if ($scope.weatherSource == "forecastIO") {
        weatherService.getForecastIO($scope.mvcmodel).then(function (results) {
            $scope.forecastIOInfo = results.data;
            findIcon($scope.forecastIOInfo.currentWeather.icon);

            $state.go('forecastio.location', { locationName: $scope.locationName });
        }, function (err) {
            toastr.error("Error retrieving weather info from forecast.io" + err);
        });
    } else {
        weatherService.getWUnderground($scope.mvcmodel).then(function (results) {
            $scope.forecastIOInfo = results.data;
            findIcon($scope.forecastIOInfo.currentWeather.icon);

            $state.go('forecastio.location', { locationName: $scope.locationName });
        }, function (err) {
            toastr.error("Error retrieving weather info from weather underground" + err);
        });
    }
    

    function findIcon(iconName) {
        switch (iconName) {
            case "clear-day":
                $scope.weatherIcon = "wi wi-day-sunny";
                break;
            case "clear-night":
                $scope.weatherIcon = "wi wi-night-clear";
                break;
            case "rain":
                $scope.weatherIcon = "wi wi-rain";
                break;
            case "snow":
                $scope.weatherIcon = "wi wi-snow";
                break;
            case "sleet":
                $scope.weatherIcon = "wi wi-sleet";
                break;
            case "wind":
                $scope.weatherIcon = "wi wi-strong-wind";
                break;
            case "fog":
                $scope.weatherIcon = "wi wi-fog";
                break;
            case "partly-cloudy-day":
                $scope.weatherIcon = "wi wi-day-cloudy";
                break;
            case "partly-cloudy-night":
                $scope.weatherIcon = "wi wi-night-cloudy";
                break;
            case "hail":
                $scope.weatherIcon = "wi wi-hail";
                break;
            case "thunderstorm":
                $scope.weatherIcon = "wi wi-thunderstorm";
                break;
            case "tornado":
                $scope.weatherIcon = "wi wi-tornado";
                break;
            case "cloudy":
                $scope.weatherIcon = "wi wi-cloudy";
                break;
        }
    }
});