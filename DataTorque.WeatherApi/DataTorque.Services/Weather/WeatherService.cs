using DataTorque.Shared.Weather;

namespace DataTorque.Services.Weather
{
    public class WeatherService : IWeatherService
    {
        private readonly IOpenWeatherMapClient _client;
        private readonly IGeoLocationService _geoLocation;

        public WeatherService(IOpenWeatherMapClient client, IGeoLocationService geoLocation)
        {
            _client = client;
            _geoLocation = geoLocation;
        }

        public async Task<WeatherAndSuggestionRecord> GetWeatherWithSuggestions(string cityCode)
        {
            var cityGeoLocation = await _geoLocation.GetCityGeoLocationFromCode(cityCode, string.Empty, string.Empty);
            
            if(!cityGeoLocation.HasLocation)
            {
                throw new ArgumentException($"No geolocation found for city code: {cityCode}");
            }
            return await GetWeatherWithSuggestionsByCoordinates(cityGeoLocation.Latitude, cityGeoLocation.Longitude);
        }

        public async Task<WeatherAndSuggestionRecord> GetWeatherWithSuggestionsByCoordinates(double? latitude, double? longitude)
        {
            // suggestion - documents states some data is updated every 10 minutes. So I would suggest a memory cache for 5 or 10 minutes to reduce requests. 
            
            if (!latitude.HasValue || !longitude.HasValue)
                throw new ArgumentException($"latitude & longitude are required");

            var weatherData = await _client.GetWeatherDataByGeoLocation(latitude.Value, longitude.Value);
            var weatherDataWithSuggestions = PopulateSuggestions(weatherData);
            return weatherDataWithSuggestions;
        }

        private WeatherAndSuggestionRecord PopulateSuggestions(WeatherDataRecord weatherData)
        {
            var weather = weatherData.Weather.ToLower();
            var suggestions = new List<string>();

            // just demoing a func. Could have been a private methods, or inline...
            Func<string, double, string> MapToSimpleWeatherCondition = (detailedWeather, windSpeed) =>
            {
                if (detailedWeather.Contains("snow"))
                    return "Snowing";

                if (detailedWeather.Contains("rain") || detailedWeather.Contains("drizzle"))
                    return "Rainy";

                if (windSpeed > 20)
                    return "Windy";

                if (detailedWeather.Contains("clear") || detailedWeather.Contains("sunny"))
                    return "Sunny";

                return "Fine"; // this is not in the AC, but I assume this is fine.
            };



            if (weather.Contains("sunny") || weather.Contains("clear"))
            {
                suggestions.Add("Don't forget to bring a hat.");
            }

            if (weatherData.Temp > 25)
            {
                suggestions.Add("It's a great day for a swim.");
            }

            if (weatherData.Temp < 15 && (weather.Contains("rain") || weather.Contains("snow")))
            {
                suggestions.Add("Don't forget to bring a coat.");
            }

            if (weather.Contains("rain"))
            {
                suggestions.Add("Don't forget the umbrella.");
            }

            var suggestion = suggestions.Count > 0 ? string.Join(" ", suggestions) : string.Empty;
            return new WeatherAndSuggestionRecord(weatherData.Temp, MapToSimpleWeatherCondition(weather, weatherData.WindSpeed), weatherData.WindSpeed, suggestion);
        }

    }
}
