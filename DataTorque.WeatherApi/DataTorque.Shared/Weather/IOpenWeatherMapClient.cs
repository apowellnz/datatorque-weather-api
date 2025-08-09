namespace DataTorque.Shared.Weather
{
    public interface IOpenWeatherMapClient
    {
        Task<WeatherDataRecord> GetWeatherDataByGeoLocation(double latitude, double longitude);
    }
}
