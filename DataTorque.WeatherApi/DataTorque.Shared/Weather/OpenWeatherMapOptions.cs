using System.ComponentModel.DataAnnotations;

namespace DataTorque.Shared.Weather
{
    public class OpenWeatherMapOptions
    {
        public const string SectionName = "OpenWeatherMap";
        
        [Required(ErrorMessage = "OpenWeatherMap API Key is required")]
        public string ApiKey { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "OpenWeatherMap Domain is required")]
        [Url(ErrorMessage = "OpenWeatherMap Domain must be a valid URL")]
        public string Domain { get; set; } = string.Empty;
        
        [Range(1, 300, ErrorMessage = "Timeout must be between 1 and 300 seconds")]
        public int TimeoutSeconds { get; set; } = 30; // DIDN'T IMPLEMENT
        
        [Range(0, 10, ErrorMessage = "Retry count must be between 0 and 10")]
        public int RetryCount { get; set; } = 3; // DIDN'T IMPLEMENT
    }
}
