using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDTO>> GetPointsOfInterest(int cityId)
        {
            var cityData = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (cityData == null) return NotFound();
            return Ok(cityData.PointsOfInterest);
        }

        [HttpGet("{pointOfInterestId}")]
        public ActionResult<PointOfInterestDTO> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) return NotFound();

            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
            if (pointOfInterest == null) return NotFound();

            return Ok(pointOfInterest);
        }

    }
}
