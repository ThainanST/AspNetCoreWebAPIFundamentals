using CityInfo.ASP;
using CityInfo.ASP.Controllers;
using CityInfo.ASP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CityInfo.Tests.unit.controllers
{
    [Collection("PointsOfInterestTests")]
    public class PointsOfInterestControllerTest : IDisposable
    {
        private readonly PointsOfInterestController _controller;
        private readonly List<CityDto> _originalData;

        public PointsOfInterestControllerTest()
        {
            _controller = new PointsOfInterestController();

            // Deep Copy usando Newtonsoft.Json
            _originalData = JsonConvert.DeserializeObject<List<CityDto>>(
                JsonConvert.SerializeObject(CitiesDataStore.Current.Cities)
            )!;
        }

        public void Dispose()
        {
            Console.WriteLine("Restaurando os dados originais...");

            // Restaura os dados originais após cada teste
            CitiesDataStore.Current.Cities = JsonConvert.DeserializeObject<List<CityDto>>(
                JsonConvert.SerializeObject(_originalData)
            )!;
        }

        [Fact]
        public void GetPointsOfInterest_ReturnsOk_WithListOfPointsOfInterest()
        {
            // Arrange

            // Act
            var result = _controller.GetPointsOfInterest(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var pointsOfInterest = Assert.IsType<List<PointOfInterestDto>>(okResult.Value);

            Assert.Equal(2, pointsOfInterest.Count);
            Assert.Contains(pointsOfInterest, poi => poi.Name == "Central Park");
            Assert.Contains(pointsOfInterest, poi => poi.Name == "Empire State Building");
        }

        [Fact]
        public void GetPointsOfInterest_NonExistingCityId_ReturnsNotFound()
        {
            // Act
            var result = _controller.GetPointsOfInterest(999); // ID inexistente

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetPointOfInterest_ReturnsOk_WithPointOfInterestDto()
        {
            // Arrange

            // Act
            var cityId = 1;
            var pointOfInterestId = 1;
            var result = _controller.GetPointOfInterest(cityId, pointOfInterestId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var pointOfInterest = Assert.IsType<PointOfInterestDto>(okResult.Value);

            Assert.Equal(1, pointOfInterest.Id);
            Assert.Equal("Central Park", pointOfInterest.Name);
        }

        [Fact]
        public void GetPointOfInterest_NonExistingCityId_ReturnsNotFound()
        {
            // Act
            var result = _controller.GetPointOfInterest(999, 1); // ID inexistente

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetPointOfInterest_NonExistingPointOfInterestId_ReturnsNotFound()
        {
            // Act
            var result = _controller.GetPointOfInterest(1, 999); // ID inexistente

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void PatchPointOfInterest_ReturnsNoContent()
        {
            // Arrange
            var testCityId = 1;
            var testPointOfInterestId = 1;
            var updatedPointOfInterestName = "Updated again - Central Park";

            var patchDocument = new JsonPatchDocument<PointOfInterestForUpdateDto>();
            patchDocument.Replace(p => p.Name, updatedPointOfInterestName);

            // Act - Aplica a atualização parcial
            var patchResponse = _controller.PartiallyUpdatePointOfInterest(testCityId, testPointOfInterestId, patchDocument);

            // Assert - Verifica se o PATCH retornou NoContent
            Assert.IsType<NoContentResult>(patchResponse);

            // Act - Obtém o recurso atualizado
            var getResponse = _controller.GetPointOfInterest(testCityId, testPointOfInterestId);

            // Assert - Verifica se o GET retornou um OkObjectResult e extrai o valor
            var okResponse = Assert.IsType<OkObjectResult>(getResponse.Result);
            var updatedPointOfInterest = Assert.IsType<PointOfInterestDto>(okResponse.Value);

            // Assert - Verifica se o nome foi atualizado corretamente
            Assert.Equal(updatedPointOfInterestName, updatedPointOfInterest.Name);
        }

        [Fact]
        public void PatchPointOfInterest_WithInvalidProperty_ShouldReturnBadRequest()
        {
            // Arrange
            var testCityId = 1;
            var testPointOfInterestId = 1;

            var patchDocument = new JsonPatchDocument<PointOfInterestForUpdateDto>();
            patchDocument.Operations.Add(new Operation<PointOfInterestForUpdateDto>(
            "replace", "/invalidproperty", "", "Updated - Central Park"));

            // Act
            var patchResponse = _controller.PartiallyUpdatePointOfInterest(testCityId, testPointOfInterestId, patchDocument);

            // Assert - Verifica se o PATCH retorna BadRequest
            var badRequestResponse = Assert.IsType<BadRequestObjectResult>(patchResponse);
            Assert.Equal(400, badRequestResponse.StatusCode);
        }

        [Fact]
        public void DeletePointOfInterest_ReturnsNoContent()
        {
            // Arrange
            int testCityId = 1;
            int testPointOfInterestId = 1;

            // Act
            IActionResult deleteResponse = _controller.DeletePointOfInterest(testCityId, testPointOfInterestId);

            // Assert
            Assert.IsType<NoContentResult>(deleteResponse);

            // Act
            ActionResult<PointOfInterestDto> getResponse = _controller.GetPointOfInterest(testCityId, testPointOfInterestId);

            // Assert
            Assert.IsType<NotFoundResult>(getResponse.Result);
        }
    }
}
