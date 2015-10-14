app.controller("forecastioCtrl", function ($scope, $stateParams, $state, weatherService, toastr) {
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
            $scope.source = "Forecast.IO";
            findIcon($scope.forecastIOInfo.currentWeather.icon);

            $state.go('forecastio.location', { locationName: $scope.locationName });
        }, function (err) {
            toastr.error("Error retrieving weather info from forecast.io" + err);
        });
    } else {
        weatherService.getWUnderground($scope.mvcmodel).then(function (results) {
            $scope.forecastIOInfo = results.data;
            $scope.source = null;
            findIcon($scope.forecastIOInfo.currentWeather.icon);

            $state.go('forecastio.location', { locationName: $scope.locationName });
        }, function (err) {
            toastr.error("Error retrieving weather info from weather underground" + err);
        });
    }
    

    function findIcon(iconName) {
        switch (iconName.toLowerCase()) {
            case "clear-day":
            case "clear":
            case "mostlysunny":
            case "partlysunny":
            case "sunny":
                $scope.weatherIcon = "wi wi-day-sunny";
                break;
            case "clear-night":
                $scope.weatherIcon = "wi wi-night-clear";
                break;
            case "rain":
            case "chancerain":
            case "freezing rain":
                $scope.weatherIcon = "wi wi-rain";
                break;
            case "snow":
            case "chanceflurries":
            case "chancesnow":
            case "flurries":
                $scope.weatherIcon = "wi wi-snow";
                break;
            case "sleet":
            case "chancesleet":
                $scope.weatherIcon = "wi wi-sleet";
                break;
            case "wind":
                $scope.weatherIcon = "wi wi-strong-wind";
                break;
            case "fog":
            case "hazy":
                $scope.weatherIcon = "wi wi-fog";
                break;
            case "partly-cloudy-day":
            case "partlycloudy":
                $scope.weatherIcon = "wi wi-day-cloudy";
                break;
            case "partly-cloudy-night":
                $scope.weatherIcon = "wi wi-night-cloudy";
                break;
            case "hail":
                $scope.weatherIcon = "wi wi-hail";
                break;
            case "thunderstorm":
            case "chancetstorms":
            case "tstorms":
                $scope.weatherIcon = "wi wi-thunderstorm";
                break;
            case "tornado":
                $scope.weatherIcon = "wi wi-tornado";
                break;
            case "cloudy":
            case "mostlycloudy":
            case "overcast":
                $scope.weatherIcon = "wi wi-cloudy";
                break;
        }
    }

    $scope.toggleSource = function () {
        var temp = $scope.address;
        $scope.address = null;

    }
});