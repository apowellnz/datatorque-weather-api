using DataTorque.Shared.Weather;
using Microsoft.Extensions.Options;

namespace DataTorque.Services.Weather
{
    /// <summary>
    /// THis is just a Mock idea. If there was a drop down that only had reference to the city code this service could be used to return the latitude and longitude. 
    /// Not required if the UI was a map or something similar. But in the case of a dropdown that had a value of the city code. This would be a handy service.
    /// ref: https://openweathermap.org/api/geocoding-api it does require state, and country code as well. But I didn't implement that part. 
    /// </summary>
    public class GeoLocationService : IGeoLocationService
    {
        private readonly HttpClient _httpClient;
        private readonly OpenWeatherMapOptions _options;

        public GeoLocationService(HttpClient httpClient, IOptions<OpenWeatherMapOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<GeoLocationResult> GetCityGeoLocationFromCode(string cityCode, string state, string code)
        {
            await Task.Delay(50); // pretend to be a async request to an API....
            
            var mockLocations = new Dictionary<string, (double Longitude, double Latitude)>
            {
                { "WLG", (174.775574, -41.28664) },  // Wellington from README
                { "AKL", (174.7633, -36.8485) },     // Auckland
                { "CHC", (172.6362, -43.5321) }     // Christchurch  
            };
            
            var upperCityCode = cityCode?.ToUpper() ?? string.Empty;
            
            if (mockLocations.TryGetValue(upperCityCode, out var location))
            {
                return new GeoLocationResult
                {
                    Longitude = location.Longitude,
                    Latitude = location.Latitude
                };
            }
            
            return new GeoLocationResult
            {
                Longitude = null,
                Latitude = null
            };
        }
    }
}
