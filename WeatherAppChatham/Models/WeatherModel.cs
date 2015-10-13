using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ForecastIO;
using RestSharp;
using Newtonsoft.Json;

namespace WeatherAppChatham.Models
{
    public class WeatherModel
    {
        public string Address { get; set; }
        public class Current
        {
            public int apparentTemperature;   // Feels Like weather in farenheit
            public int dewPoint;  // Dew Point in farenheit
            public string humidity;    // Humidity in percentage
            public float pressure;  // Sea level air pressure in millibars
            public string summary;  // Text summary for current temperature
            public int temperature; // Current Temperature in farenheit
            public DateTime time;   // UNIX Time
            public int visibility;    // Average visibility in miles
            public int windSpeed;   // Wind speed in mph

            public Current(ForecastIOResponse forecast)
            {
                this.apparentTemperature = Convert.ToInt32(forecast.currently.apparentTemperature);
                this.dewPoint = Convert.ToInt32(forecast.currently.dewPoint);
                this.humidity = Convert.ToString(forecast.currently.humidity * 100);
                this.pressure = forecast.currently.pressure;
                this.summary = forecast.currently.summary;
                this.temperature = Convert.ToInt32(forecast.currently.temperature);
                this.visibility = Convert.ToInt32(forecast.currently.visibility);
                this.windSpeed = Convert.ToInt32(forecast.currently.windSpeed);

                DateTime UnixTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                this.time = UnixTime.AddSeconds(forecast.currently.time).ToLocalTime();
            }

            public Current(dynamic wunderground)
            {
                //var hTemp = dailyResults.forecast.simpleforecast.forecastday[0].high.fahrenheit;

                this.apparentTemperature = Convert.ToInt32(Convert.ToDouble(wunderground.current_observation.feelslike_f.Value));
                this.dewPoint = Convert.ToInt32(Convert.ToDouble(wunderground.current_observation.dewpoint_f.Value));
                this.humidity = wunderground.current_observation.relative_humidity.Value;
                this.pressure = float.Parse(wunderground.current_observation.pressure_mb.Value);
                this.summary = wunderground.current_observation.weather.Value;
                this.temperature = Convert.ToInt32(wunderground.current_observation.temp_f.Value);
                this.visibility = Convert.ToInt32(Convert.ToDouble(wunderground.current_observation.visibility_mi.Value));
                this.windSpeed = Convert.ToInt32(wunderground.current_observation.wind_mph.Value);

                DateTime UnixTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                this.time = UnixTime.AddSeconds(Convert.ToInt64(wunderground.current_observation.local_epoch.Value)).ToLocalTime();
            }
        }

        public class DailyForecast
        {
            public int humidity;    // Humidity in percentage
            public int precipProbablity;    // Precipitation probability in percentage
            public string summary;  // Text summary for current temperature
            public string SunriseTime;    // Sunrise Time in UNIX time
            public string SunsetTime;     // Sunset Time in UNIX time
            public int temperatureMax; // Maximum temperature for the day
            public float temperatureMin; // Minimum temperature for the day
            public string time;   // UNIX Time
            public int windSpeed;   // Wind speed in mph

            public DailyForecast(ForecastIOResponse forecast, int index)
            {
                this.humidity = Convert.ToInt32(forecast.daily.data[index].humidity * 100);
                this.precipProbablity = Convert.ToInt32(forecast.daily.data[index].precipProbability);
                this.summary = forecast.daily.data[index].summary;
                this.temperatureMax = Convert.ToInt32(forecast.daily.data[index].temperatureMax);
                this.temperatureMin = forecast.daily.data[index].temperatureMin;
                this.windSpeed = Convert.ToInt32(forecast.daily.data[index].windSpeed);

                DateTime UnixTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                this.time = UnixTime.AddSeconds(forecast.daily.data[index].time).ToLocalTime().ToLongDateString();
                this.SunriseTime = UnixTime.AddSeconds(forecast.daily.data[index].sunriseTime).ToLocalTime().ToLongTimeString();
                this.SunsetTime = UnixTime.AddSeconds(forecast.daily.data[index].sunsetTime).ToLocalTime().ToLongTimeString();
            }

            public DailyForecast(dynamic weather, dynamic planner, dynamic astronomy, int index)
            {
                this.humidity = Convert.ToInt32(weather.forecast.simpleforecast.forecastday[index].avehumidity.Value);
                var precip = planner.trip.chance_of.chanceofprecip.percentage.Value;
                this.precipProbablity = planner.trip.chance_of.chanceofprecip.percentage.Value != null ? Convert.ToInt32(planner.trip.chance_of.chanceofprecip.percentage.Value) : 0;
                this.summary = weather.forecast.simpleforecast.forecastday[index].conditions.Value;
                this.temperatureMax = Convert.ToInt32(weather.forecast.simpleforecast.forecastday[index].high.fahrenheit.Value);
                this.temperatureMin = Convert.ToInt32(weather.forecast.simpleforecast.forecastday[index].low.fahrenheit.Value);
                this.windSpeed = Convert.ToInt32(weather.forecast.simpleforecast.forecastday[index].maxwind.mph.Value);

                DateTime UnixTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                this.time = UnixTime.AddSeconds(Convert.ToInt64(weather.forecast.simpleforecast.forecastday[index].date.epoch.Value)).ToLocalTime().ToLongDateString();
                this.SunriseTime = astronomy.sun_phase.sunrise.hour.Value + ":" + astronomy.sun_phase.sunrise.minute.Value + "AM";
                this.SunsetTime = astronomy.sun_phase.sunrise.hour.Value + ":" + astronomy.sun_phase.sunrise.minute.Value + "PM";

            }
        }

        public class Daily
        {
            public List<DailyForecast> dailyWeatherList { get; set; }

            public Daily(ForecastIOResponse forecast) 
            {
                dailyWeatherList = new List<DailyForecast>();

                for (var i = 0; i < 8; i++)
                {
                    dailyWeatherList.Add(new DailyForecast(forecast, i));
                }
            }

            public Daily(dynamic weather, dynamic planner, dynamic astronomy)
            {
                dailyWeatherList = new List<DailyForecast>();

                for (var i = 0; i < 8; i++)
                {
                    dailyWeatherList.Add(new DailyForecast(weather, planner, astronomy, i));
                }
            }
        }

        public Current currentWeather;
        public Daily dailyWeather;

        public WeatherModel() { }

        public WeatherModel(ForecastIOResponse forecast, string addr)
        {
            this.currentWeather = new Current(forecast);
            this.dailyWeather = new Daily(forecast);
            this.Address = addr;
        }

        public WeatherModel(dynamic wundergroundCurrent, dynamic wundergroundDaily, dynamic planner, dynamic astronomy, string addr) 
        {
            this.currentWeather = new Current(wundergroundCurrent);
            this.dailyWeather = new Daily(wundergroundDaily, planner, astronomy);
            this.Address = addr;
        }
    }
}