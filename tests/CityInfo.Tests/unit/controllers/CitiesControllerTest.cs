using CityInfo.ASP.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitiesInfo.Tests.unit.controllers
{
    public class CitiesControllerTest
    {
        [Fact] // Define um teste unitário
        public void GetCities_ReturnsJsonResult_WithListOfCities()
        {
            // Arrange
            var controller = new CitiesController();

            // Act
            var result = controller.GetCities();

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var cities = Assert.IsType<List<object>>(jsonResult.Value);

            Assert.Equal(2, cities.Count);
            Assert.Contains(cities, city => city.ToString().Contains("New York City"));
            Assert.Contains(cities, city => city.ToString().Contains("Antwerp"));
        }
    }
}
