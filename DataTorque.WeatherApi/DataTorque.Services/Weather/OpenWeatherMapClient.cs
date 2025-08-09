using DataTorque.Shared.Weather;
using Microsoft.Extensions.Options;
using System.Text.Json;


namespace DataTorque.Services.Weather
{
    public class OpenWeatherMapClient: IOpenWeatherMapClient
    {
        private readonly HttpClient _httpClient;
        private readonly OpenWeatherMapOptions _options;
        
        public OpenWeatherMapClient(HttpClient httpClient, IOptions<OpenWeatherMapOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<WeatherDataRecord> GetWeatherDataByGeoLocation(double latitude, double longitude)
        {
            // We could include the version in the appsettings, but I left it here.. This way you could have a service for API version 2.5 and v3...
            var url = $"{_options.Domain}/data/2.5/weather?lat={latitude}&lon={longitude}&appid={_options.ApiKey}&units=metric";
            var response = await _httpClient.GetStringAsync(url);
            var json = JsonDocument.Parse(response);

            var tempCelsius = json.RootElement.GetProperty("main").GetProperty("temp").GetDouble();
            var weatherDescription = json.RootElement.GetProperty("weather")[0].GetProperty("description").GetString() ?? "Unknown";
            
            // wind.speed Wind speed. Unit Default: meter/sec, Metric: meter/sec,
            // ref: https://openweathermap.org/current serach wind.speed 
            var windSpeedMs = json.RootElement.GetProperty("wind").GetProperty("speed").GetDouble();
            var windSpeedKmh = windSpeedMs * 3.6; 

            return new WeatherDataRecord(tempCelsius, weatherDescription, windSpeedKmh);
        }
    }
}
