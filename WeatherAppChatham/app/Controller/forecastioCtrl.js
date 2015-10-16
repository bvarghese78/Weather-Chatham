app.controller("forecastioCtrl", function ($scope, $stateParams, $state, weatherService, toastr) {

    // Params provided by the state
    if ($stateParams.locationName) {
        $scope.weatherProvider = $stateParams.sourceName;
        $scope.locationName = $stateParams.locationName;
    }

    // Empty MVC weather model
    $scope.mvcmodel = {
        currentWeather: null,
        daily: null,
        address: $scope.locationName
    }

    // Retrieves weather from either getForecastIO or getWUnderground
    if ($scope.weatherProvider == "forecastIO") {
        weatherService.getForecastIO($scope.mvcmodel).then(function (results) {
            $scope.forecastIOInfo = results.data;
            findIcon($scope.forecastIOInfo.currentWeather.icon);

        }, function (err) {
            toastr.error("Error retrieving weather info from forecast.io" + err);
        });
    } else {
        weatherService.getWUnderground($scope.mvcmodel).then(function (results) {
            $scope.forecastIOInfo = results.data;
            findIcon($scope.forecastIOInfo.currentWeather.icon);
            $scope.weatherSource = "wunderground";

        }, function (err) {
            toastr.error("Error retrieving weather info from weather underground" + err);
        });
    }
    
    // Finds icon based on icon name provided by weather APIs
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
});