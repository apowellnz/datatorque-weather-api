using DataTorque.Shared.Weather;
using Microsoft.AspNetCore.Mvc;

namespace DataTorque.WeatherApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly ILogger<WeatherController> _logger;
        private static int _requestCounter = 0;
        private static readonly object _lockObject = new object();

        public WeatherController(IWeatherService weatherService, ILogger<WeatherController> logger)
        {
            _weatherService = weatherService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetWeather([FromQuery] double latitude, [FromQuery] double longitude)
        {
            // request returns 503 evey 5 requests... I'd rather do a 423 or something else. 
            lock (_lockObject)
            {
                _requestCounter++;
                if (_requestCounter % 5 == 0)
                {
                    _logger.LogWarning("Simulating upstream failure for request #{RequestNumber}", _requestCounter);
                    return StatusCode(503, new { error = "Service temporarily unavailable" });
                }
            }

            try
            {
                var weatherData = await _weatherService.GetWeatherWithSuggestionsByCoordinates(latitude, longitude);
                
                var response = new
                {
                    temperature = Math.Round(weatherData.Temp, 1),
                    windSpeed = Math.Round(weatherData.WindSpeed, 1),
                    condition = weatherData.Weather,
                    recommendation = weatherData.Suggestions
                };

                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid location provided: lat={Latitude}, lon={Longitude}", latitude, longitude);
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving weather data for lat={Latitude}, lon={Longitude}", latitude, longitude);
                return StatusCode(500, new { error = "An error occurred while retrieving weather data" });
            }
        }
    }
}
