using DataTorque.Shared.Weather;
using DataTorque.Services.Weather;
using Moq;

namespace DataTorque.UnitTests.Weather
{
    [TestClass]
    public class WeatherServiceTests
    {
        private Mock<IOpenWeatherMapClient> _mockWeatherClient = null!;
        private Mock<IGeoLocationService> _mockGeoLocationService = null!;
        private WeatherService _weatherService = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockWeatherClient = new Mock<IOpenWeatherMapClient>();
            _mockGeoLocationService = new Mock<IGeoLocationService>();
            _weatherService = new WeatherService(_mockWeatherClient.Object, _mockGeoLocationService.Object);
        }

        [TestMethod]
        public async Task When_TempatureOver25C_ReturnSuggestion()
        {
            // Arrange
            var cityCode = "TEST";
            var geoLocation = new GeoLocationResult { Latitude = -36.8485, Longitude = 174.7633 };
            var weatherData = new WeatherDataRecord(30.0, "clear sky", 10.0); // 30°C, clear, 10 km/h wind

            _mockGeoLocationService.Setup(x => x.GetCityGeoLocationFromCode(cityCode, string.Empty, string.Empty))
                .ReturnsAsync(geoLocation);
            _mockWeatherClient.Setup(x => x.GetWeatherDataByGeoLocation(geoLocation.Latitude!.Value, geoLocation.Longitude!.Value))
                .ReturnsAsync(weatherData);

            // Act
            var result = await _weatherService.GetWeatherWithSuggestions(cityCode);

            // Assert
            Assert.AreEqual(30.0, result.Temp);
            Assert.AreEqual("Sunny", result.Weather); // Should be mapped to simple condition
            Assert.AreEqual(10.0, result.WindSpeed);
            Assert.IsTrue(result.Suggestions.Contains("It's a great day for a swim."));
        }

        [TestMethod]
        public async Task When_SunnyDay_ReturnSuggestion()
        {
            // Arrange
            var cityCode = "TEST";
            var geoLocation = new GeoLocationResult { Latitude = -36.8485, Longitude = 174.7633 };
            var weatherData = new WeatherDataRecord(20.0, "sunny", 8.0);

            _mockGeoLocationService.Setup(x => x.GetCityGeoLocationFromCode(cityCode, string.Empty, string.Empty))
                .ReturnsAsync(geoLocation);
            _mockWeatherClient.Setup(x => x.GetWeatherDataByGeoLocation(geoLocation.Latitude!.Value, geoLocation.Longitude!.Value))
                .ReturnsAsync(weatherData);

            // Act
            var result = await _weatherService.GetWeatherWithSuggestions(cityCode);

            // Assert
            Assert.AreEqual(20.0, result.Temp);
            Assert.AreEqual("Sunny", result.Weather);
            Assert.AreEqual(8.0, result.WindSpeed);
            Assert.IsTrue(result.Suggestions.Contains("Don't forget to bring a hat."));
        }

        [TestMethod]
        public async Task When_ClearDay_ReturnSuggestion()
        {
            // Arrange
            var cityCode = "TEST";
            var geoLocation = new GeoLocationResult { Latitude = -36.8485, Longitude = 174.7633 };
            var weatherData = new WeatherDataRecord(22.0, "clear sky", 12.0);

            _mockGeoLocationService.Setup(x => x.GetCityGeoLocationFromCode(cityCode, string.Empty, string.Empty))
                .ReturnsAsync(geoLocation);
            _mockWeatherClient.Setup(x => x.GetWeatherDataByGeoLocation(geoLocation.Latitude!.Value, geoLocation.Longitude!.Value))
                .ReturnsAsync(weatherData);

            // Act
            var result = await _weatherService.GetWeatherWithSuggestions(cityCode);

            // Assert
            Assert.AreEqual(22.0, result.Temp);
            Assert.AreEqual("Sunny", result.Weather);
            Assert.AreEqual(12.0, result.WindSpeed);
            Assert.IsTrue(result.Suggestions.Contains("Don't forget to bring a hat."));
        }

        [TestMethod]
        public async Task When_Under15CAndSnowingOrRaining_ReturnSuggestion()
        {
            // Arrange
            var cityCode = "TEST";
            var geoLocation = new GeoLocationResult { Latitude = -36.8485, Longitude = 174.7633 };
            var weatherData = new WeatherDataRecord(10.0, "light rain", 15.0); // 10°C and raining

            _mockGeoLocationService.Setup(x => x.GetCityGeoLocationFromCode(cityCode, string.Empty, string.Empty))
                .ReturnsAsync(geoLocation);
            _mockWeatherClient.Setup(x => x.GetWeatherDataByGeoLocation(geoLocation.Latitude!.Value, geoLocation.Longitude!.Value))
                .ReturnsAsync(weatherData);

            // Act
            var result = await _weatherService.GetWeatherWithSuggestions(cityCode);

            // Assert
            Assert.AreEqual(10.0, result.Temp);
            Assert.AreEqual("Rainy", result.Weather);
            Assert.AreEqual(15.0, result.WindSpeed);
            Assert.IsTrue(result.Suggestions.Contains("Don't forget to bring a coat."));
            Assert.IsTrue(result.Suggestions.Contains("Don't forget the umbrella."));
        }

        [TestMethod]
        public async Task When_Under15CAndSnowing_ReturnSuggestion()
        {
            // Arrange
            var cityCode = "TEST";
            var geoLocation = new GeoLocationResult { Latitude = -36.8485, Longitude = 174.7633 };
            var weatherData = new WeatherDataRecord(5.0, "light snow", 18.0); // 5°C and snowing

            _mockGeoLocationService.Setup(x => x.GetCityGeoLocationFromCode(cityCode, string.Empty, string.Empty))
                .ReturnsAsync(geoLocation);
            _mockWeatherClient.Setup(x => x.GetWeatherDataByGeoLocation(geoLocation.Latitude!.Value, geoLocation.Longitude!.Value))
                .ReturnsAsync(weatherData);

            // Act
            var result = await _weatherService.GetWeatherWithSuggestions(cityCode);

            // Assert
            Assert.AreEqual(5.0, result.Temp);
            Assert.AreEqual("Snowing", result.Weather);
            Assert.AreEqual(18.0, result.WindSpeed);
            Assert.IsTrue(result.Suggestions.Contains("Don't forget to bring a coat."));
        }

        [TestMethod]
        public async Task When_Raining_ReturnSuggestion()
        {
            // Arrange
            var cityCode = "TEST";
            var geoLocation = new GeoLocationResult { Latitude = -36.8485, Longitude = 174.7633 };
            var weatherData = new WeatherDataRecord(18.0, "moderate rain", 16.0); // Above 15°C but raining

            _mockGeoLocationService.Setup(x => x.GetCityGeoLocationFromCode(cityCode, string.Empty, string.Empty))
                .ReturnsAsync(geoLocation);
            _mockWeatherClient.Setup(x => x.GetWeatherDataByGeoLocation(geoLocation.Latitude!.Value, geoLocation.Longitude!.Value))
                .ReturnsAsync(weatherData);

            // Act
            var result = await _weatherService.GetWeatherWithSuggestions(cityCode);

            // Assert
            Assert.AreEqual(18.0, result.Temp);
            Assert.AreEqual("Rainy", result.Weather);
            Assert.AreEqual(16.0, result.WindSpeed);
            Assert.IsTrue(result.Suggestions.Contains("Don't forget the umbrella."));
            // Should NOT contain coat suggestion (temp > 15°C)
            Assert.IsFalse(result.Suggestions.Contains("Don't forget to bring a coat."));
        }

        [TestMethod]
        public async Task When_MultipleSuggestions_ReturnCombinedSuggestion()
        {
            // Arrange - Sunny and over 25°C
            var cityCode = "TEST";
            var geoLocation = new GeoLocationResult { Latitude = -36.8485, Longitude = 174.7633 };
            var weatherData = new WeatherDataRecord(28.0, "sunny", 14.0);

            _mockGeoLocationService.Setup(x => x.GetCityGeoLocationFromCode(cityCode, string.Empty, string.Empty))
                .ReturnsAsync(geoLocation);
            _mockWeatherClient.Setup(x => x.GetWeatherDataByGeoLocation(geoLocation.Latitude!.Value, geoLocation.Longitude!.Value))
                .ReturnsAsync(weatherData);

            // Act
            var result = await _weatherService.GetWeatherWithSuggestions(cityCode);

            // Assert
            Assert.AreEqual(28.0, result.Temp);
            Assert.AreEqual("Sunny", result.Weather);
            Assert.AreEqual(14.0, result.WindSpeed);
            Assert.IsTrue(result.Suggestions.Contains("Don't forget to bring a hat."));
            Assert.IsTrue(result.Suggestions.Contains("It's a great day for a swim."));
        }

        [TestMethod]
        public async Task When_NoMatchingConditions_ReturnEmptySuggestion()
        {
            // Arrange - Cloudy, moderate temperature
            var cityCode = "TEST";
            var geoLocation = new GeoLocationResult { Latitude = -36.8485, Longitude = 174.7633 };
            var weatherData = new WeatherDataRecord(20.0, "few clouds", 12.0);

            _mockGeoLocationService.Setup(x => x.GetCityGeoLocationFromCode(cityCode, string.Empty, string.Empty))
                .ReturnsAsync(geoLocation);
            _mockWeatherClient.Setup(x => x.GetWeatherDataByGeoLocation(geoLocation.Latitude!.Value, geoLocation.Longitude!.Value))
                .ReturnsAsync(weatherData);

            // Act
            var result = await _weatherService.GetWeatherWithSuggestions(cityCode);

            // Assert
            Assert.AreEqual(20.0, result.Temp);
            Assert.AreEqual("Sunny", result.Weather); 
            Assert.AreEqual(12.0, result.WindSpeed);
            Assert.AreEqual(string.Empty, result.Suggestions);
        }

        [TestMethod]
        public async Task When_WindyConditions_ReturnWindyWeatherCondition()
        {
            // Arrange - High wind speed (over 20 km/h)
            var cityCode = "TEST";
            var geoLocation = new GeoLocationResult { Latitude = -36.8485, Longitude = 174.7633 };
            var weatherData = new WeatherDataRecord(18.0, "few clouds", 25.0); // High wind speed

            _mockGeoLocationService.Setup(x => x.GetCityGeoLocationFromCode(cityCode, string.Empty, string.Empty))
                .ReturnsAsync(geoLocation);
            _mockWeatherClient.Setup(x => x.GetWeatherDataByGeoLocation(geoLocation.Latitude!.Value, geoLocation.Longitude!.Value))
                .ReturnsAsync(weatherData);

            // Act
            var result = await _weatherService.GetWeatherWithSuggestions(cityCode);

            // Assert
            Assert.AreEqual(18.0, result.Temp);
            Assert.AreEqual("Windy", result.Weather); 
            Assert.AreEqual(25.0, result.WindSpeed);
            Assert.AreEqual(string.Empty, result.Suggestions); 
        }

        [TestMethod]
        public async Task When_InvalidCityCode_ThrowArgumentException()
        {
            // Arrange
            var cityCode = "INVALID";
            var geoLocation = new GeoLocationResult { Latitude = null, Longitude = null }; 

            _mockGeoLocationService.Setup(x => x.GetCityGeoLocationFromCode(cityCode, string.Empty, string.Empty))
                .ReturnsAsync(geoLocation);

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(
                () => _weatherService.GetWeatherWithSuggestions(cityCode));

            Assert.IsTrue(exception.Message.Contains($"No geolocation found for city code: {cityCode}"));
        }
    }
}