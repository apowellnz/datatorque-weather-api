namespace DataTorque.Shared.Weather
{
    public interface IWeatherService
    {
        Task<WeatherAndSuggestionRecord> GetWeatherWithSuggestions(string cityCode);
        Task<WeatherAndSuggestionRecord> GetWeatherWithSuggestionsByCoordinates(double? latitude, double? longitude);
    }
}
