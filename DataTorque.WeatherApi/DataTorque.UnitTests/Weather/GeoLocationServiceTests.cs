using DataTorque.Services.Weather;
using DataTorque.Shared.Weather;
using Microsoft.Extensions.Options;

namespace DataTorque.UnitTests.Weather
{
    [TestClass]
    public class GeoLocationServiceTests
    {
        [TestMethod]
        public async Task GetCityGeoLocationFromCode_ReturnsWellingtonCoordinates()
        {
            // Arrange
            var httpClient = new HttpClient();
            var options = Options.Create(new OpenWeatherMapOptions
            {
                ApiKey = "test-key",
                Domain = "https://test.api.com"
            });
            var service = new GeoLocationService(httpClient, options);

            // Act
            var result = await service.GetCityGeoLocationFromCode("WLG", string.Empty, string.Empty);

            // Assert
            Assert.IsTrue(result.HasLocation);
            Assert.AreEqual(174.775574, result.Longitude);
            Assert.AreEqual(-41.28664, result.Latitude);
        }

        [TestMethod]
        public async Task GetCityGeoLocationFromCode_WithKnownNewZealandCities_ReturnsCorrectCoordinates()
        {
            // Arrange
            var httpClient = new HttpClient();
            var options = Options.Create(new OpenWeatherMapOptions
            {
                ApiKey = "test-key",
                Domain = "https://test.api.com"
            });
            var service = new GeoLocationService(httpClient, options);

            // Act
            var wellington = await service.GetCityGeoLocationFromCode("WLG", string.Empty, string.Empty);
            var auckland = await service.GetCityGeoLocationFromCode("AKL", string.Empty, string.Empty);
            var christchurch = await service.GetCityGeoLocationFromCode("CHC", string.Empty, string.Empty);

            // Assert
            // Wellington 
            Assert.IsTrue(wellington.HasLocation);
            Assert.AreEqual(174.775574, wellington.Longitude);
            Assert.AreEqual(-41.28664, wellington.Latitude);
            
            // Auckland
            Assert.IsTrue(auckland.HasLocation);
            Assert.AreEqual(174.7633, auckland.Longitude);
            Assert.AreEqual(-36.8485, auckland.Latitude);
            
            // Christchurch
            Assert.IsTrue(christchurch.HasLocation);
            Assert.AreEqual(172.6362, christchurch.Longitude);
            Assert.AreEqual(-43.5321, christchurch.Latitude);
        }
    }
}
