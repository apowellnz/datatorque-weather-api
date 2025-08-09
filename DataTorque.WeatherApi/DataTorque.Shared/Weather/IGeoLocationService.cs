namespace DataTorque.Shared.Weather
{
    public interface IGeoLocationService
    {
        Task<GeoLocationResult> GetCityGeoLocationFromCode(string cityCode, string state, string code);
    }
}
