using CityInfo.ASP;
using CityInfo.ASP.Controllers;
using CityInfo.ASP.Models;
using Microsoft.AspNetCore.Mvc;

namespace CitiesInfo.Tests.unit.controllers
{
    public class CitiesControllerTest
    {
        private readonly CitiesController _controller;
        private readonly CitiesDataStore _dataStore;
        public CitiesControllerTest()
        {
            _controller = new CitiesController();
            _dataStore = new CitiesDataStore();
        }

        [Fact] // Define um teste unitário
        public void GetCities_ReturnsOk_WithListOfCities()
        {
            // Arrange

            // Act
            var result = _controller.GetCities();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var cities = Assert.IsType<List<CityDto>>(okResult.Value);

            Assert.Equal(3, cities.Count);
            Assert.Contains(cities, city => city.Name == "New York City");
            Assert.Contains(cities, city => city.Name == "Antwerp");
            Assert.Contains(cities, city => city.Name == "Paris");
        }

        [Fact]
        public void GetCity_ExistingId_ReturnsOk_WithCityDto()
        {
            // Arrange

            // Act
            var cityId = 1;
            var result = _controller.GetCity(cityId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var city = Assert.IsType<CityDto>(okResult.Value);

            Assert.Equal(1, city.Id);
            Assert.Equal("New York City", city.Name);
        }

        [Fact]
        public void GetCity_NonExistingId_ReturnsNotFound()
        {
            // Act
            var result = _controller.GetCity(999); // ID inexistente

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
