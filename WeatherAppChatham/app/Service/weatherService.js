﻿app.factory('weatherService', function ($http) {
    
    var getForecastIO = function (address) {
        return $http.post("Weather/GetWUnderground", address);
    };

    return {
        getForecastIO: getForecastIO
    }
});