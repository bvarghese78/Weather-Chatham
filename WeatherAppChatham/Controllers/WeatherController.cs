﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WeatherAppChatham.Models;
using ForecastIO;
using RestSharp;
using System.Configuration;

namespace WeatherAppChatham.Controllers
{
    public class WeatherController : Controller
    {
        private const string WUNDERGROUND_URL = "http://api.wunderground.com";
        string _forecastAPI = null;
        private static string _wundergroundAPI;
        AddressModel addrOutput = null;

        private string forecastAPI
        {
            get
            {
                if (string.IsNullOrEmpty(_forecastAPI))
                    _forecastAPI = ConfigurationManager.AppSettings["forecastAPIKey"];
                return _forecastAPI;
            }
        }

        private static string wundergroundAPIProp
        {
            get
            {
                _wundergroundAPI = ConfigurationManager.AppSettings["wundergroundAPIKey"];
                return _wundergroundAPI;
            }
        }

        public ActionResult GetForecastIO(WeatherModel weather)
        {
            try
            {
                string address = weather.Address;
                double lat;
                double lon;
                string formattedAddress;
                
                FindAddress(address, out lat, out lon, out formattedAddress);
                GetWUnderground(weather);

                // Get weather info from Forecast.io
                WeatherModel forecast = GetForecastIO((float)lat, (float)lon, formattedAddress);

                var camelCaseFormatter = new JsonSerializerSettings();
                camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
                var jsonResult = new ContentResult
                {
                    Content = JsonConvert.SerializeObject(forecast, camelCaseFormatter),
                    ContentType = "application/json"
                };

                return jsonResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ActionResult GetWUnderground(WeatherModel weather)
        {
            try
            {
                string address = weather.Address;
                //string url = wundergroundAPIProp + "/forecast/q/OK/Oklahoma_City.json";
                string apiKey = "c48aee81124a9dd8";
                double lat, lon;
                string formattedAddress;
                
                FindAddress(address, out lat, out lon, out formattedAddress);
                WeatherModel forecast = GetWUnderground(apiKey, formattedAddress);

                var camelCaseFormatter = new JsonSerializerSettings();
                camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
                var jsonResult = new ContentResult
                {
                    Content = JsonConvert.SerializeObject(forecast, camelCaseFormatter),
                    ContentType = "application/json"
                };

                return jsonResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string FormatAddress(string url)
        {
            string city = null;
            string state = null;
            string country = null;
            string zip = null;

            for (var i = 0; i < addrOutput.results[0].address_components.Length; i++)
            {
                if (addrOutput.results[0].address_components[i].types[0].Contains("postal_code"))
                {
                    zip = addrOutput.results[0].address_components[i].short_name;
                    break;
                }

                if (addrOutput.results[0].address_components[i].types[0].Contains("administrative_area_level_1"))
                    state = addrOutput.results[0].address_components[i].short_name;

                if (addrOutput.results[0].address_components[i].types[0].Contains("locality"))
                    city = addrOutput.results[0].address_components[i].short_name;

                if (addrOutput.results[0].address_components[i].types[0].Contains("country"))
                    country = addrOutput.results[0].address_components[i].short_name;
            }

            if (zip != null)
                url += zip + ".json";
            else if (city != null && state != null)
            {
                if (city.Contains(" "))
                    city = city.Replace(" ", "_");

                url += state + "/" + city + ".json";
            }
            else if (city != null && country != null)
                url += country + "/" + city + ".json";
            else
            {
                throw new Exception("Invalid location. Please provide a valid location");
            }
            return url;
        }

        private void FindAddress(string address, out double lat, out double lon, out string formattedAddress)
        {
            // Find lat and lon from user provided address
            addrOutput = GetGoogleGeocode(address);
            lat = Convert.ToDouble(addrOutput.results[0].geometry.location.lat);
            lon = Convert.ToDouble(addrOutput.results[0].geometry.location.lng);
            formattedAddress = addrOutput.results[0].formatted_address;
        }

        private AddressModel GetGoogleGeocode(string address)
        {
            try
            {
                string url = "https://maps.googleapis.com";
                string api = string.Format("/maps/api/geocode/json?address={0}", address);
                var client = new RestClient(url);
                var request = new RestRequest(api, Method.GET);
                var queryResult = client.Execute(request);

                AddressModel addr = JsonConvert.DeserializeObject<AddressModel>(queryResult.Content.ToString());
                return addr;
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        private WeatherModel GetForecastIO(float lat, float lon, string formattedAddress)
        {
            var request = new ForecastIORequest("d1d02d9b39d4125af3216ea665368a5c", lat, lon, Unit.us);
            var response = request.Get();

            WeatherModel weather = new WeatherModel(response, formattedAddress);
            return weather;
        }

        private WeatherModel GetWUnderground(string apiKey, string addr)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            // 7 Day weather
            string dailyUrl = apiKey + "/forecast10day/q/";
            dailyUrl = FormatAddress(dailyUrl);

            RestClient dailyClient = new RestClient(WUNDERGROUND_URL);
            RestRequest dailyRequest = new RestRequest("/api/" + dailyUrl, Method.GET);
            var dailyResult = dailyClient.Execute(dailyRequest);
            var dailyResults = JsonConvert.DeserializeObject<dynamic>(dailyResult.Content);

            // Current conditions
            string currentConditionUrl = apiKey + "/conditions/q/";
            currentConditionUrl = FormatAddress(currentConditionUrl);

            RestClient currentConditionClient = new RestClient(WUNDERGROUND_URL);
            RestRequest currentConditionRequest = new RestRequest("/api/" + currentConditionUrl, Method.GET);
            RestResponse currentConditionResult = (RestResponse)currentConditionClient.Execute(currentConditionRequest);
            var currentConditionResults = JsonConvert.DeserializeObject<dynamic>(currentConditionResult.Content);

            // Planner (chance of precipitation
            string plannerUrl = apiKey + "/planner_";
            string tempUrl = FormatAddress(plannerUrl);
            DateTime now = DateTime.Now;
            string date = now.Month + "" + now.Day + "" + now.AddDays(7).Month + "" + now.AddDays(7).Day;
            string temp = tempUrl.Split('_')[1];
            plannerUrl = plannerUrl + date + "/q/" + temp;

            RestClient plannerClient = new RestClient(WUNDERGROUND_URL);
            RestRequest plannerRequest = new RestRequest("/api/" + plannerUrl, Method.GET);
            RestResponse plannerResult = (RestResponse)plannerClient.Execute(plannerRequest);
            var plannerResults = JsonConvert.DeserializeObject<dynamic>(plannerResult.Content);

            // Astronomy
            string astronomyUrl = apiKey + "/astronomy/q/";
            astronomyUrl = FormatAddress(astronomyUrl);

            RestClient astronomyClient = new RestClient(WUNDERGROUND_URL);
            RestRequest astronomyRequest = new RestRequest("/api/" + astronomyUrl, Method.GET);
            RestResponse astronomyResult = (RestResponse)astronomyClient.Execute(astronomyRequest);
            var astronomyResults = JsonConvert.DeserializeObject<dynamic>(astronomyResult.Content);

            stopwatch.Stop();
            
            
            WeatherModel weather = new WeatherModel(currentConditionResults, dailyResults, plannerResults, astronomyResults, addr);
            return weather;
        }
    }
}