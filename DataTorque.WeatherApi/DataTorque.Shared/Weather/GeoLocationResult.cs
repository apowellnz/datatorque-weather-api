namespace DataTorque.Shared.Weather
{
    public record GeoLocationResult()
    {
        public bool HasLocation { 
            get => Longitude.HasValue && Latitude.HasValue;
        }
        public double? Longitude { get; init; }
        public double? Latitude { get; init; }
    }
}
