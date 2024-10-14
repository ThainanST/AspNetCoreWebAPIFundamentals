using CityInfo.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // This is another way to get the logger
            //HttpContext.RequestServices.GetService<ILogger<PointsOfInterestController>>();
        }

        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDTO>> GetPointsOfInterest(int cityId)
        {
            throw new Exception("Exception sample");
            try
            {
                //throw new Exception("Exception sample");
                var cityData = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
                if (cityData == null)
                {
                    _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
                    return NotFound();
                }
                return Ok(cityData.PointsOfInterest);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(
                    $"Exception while getting points of interest for city with id {cityId}.",
                    ex
                );
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDTO> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) return NotFound();

            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
            if (pointOfInterest == null) return NotFound();

            return Ok(pointOfInterest);
        }

        [HttpPost]
        public ActionResult<PointOfInterestDTO> CreatePointOfInterest(
            int cityId,
            [FromBody] PointOfInterestForCreationDTO pointOfInterest)
        {
            // This is not needed because the [ApiController] attribute is used
            //if (pointOfInterest == null) return BadRequest();


            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) return NotFound();

            var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);
            var finalPointOfInterest = new PointOfInterestDTO
            {
                Id = ++maxPointOfInterestId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            city.PointsOfInterest.Add(finalPointOfInterest);
            return CreatedAtRoute(
                    "GetPointOfInterest",
                    new { cityId = cityId, pointOfInterestId = finalPointOfInterest.Id },
                    finalPointOfInterest
                );
        }

        [HttpPut("{pointOfInterestId}")]
        public ActionResult UpdatePointOfInterest(
            int cityId,
            int pointOfInterestId,
            [FromBody] PointOfInterestForUpdateDTO pointOfInterest)
        {
            if (pointOfInterest == null) return BadRequest();

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) return NotFound();

            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
            if (pointOfInterestFromStore == null) return NotFound();

            pointOfInterestFromStore.Name = pointOfInterest.Name;
            pointOfInterestFromStore.Description = pointOfInterest.Description;

            return NoContent();
        }

        [HttpPatch("{pointOfInterestId}")]
        public ActionResult PartiallyUpdatePointOfInterest(
            int cityId,
            int pointOfInterestId,
            [FromBody] JsonPatchDocument<PointOfInterestForUpdateDTO> patchDocument)
        {

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) return NotFound();

            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
            if (pointOfInterestFromStore == null) return NotFound();

            var pointOfInterestToPatch = new PointOfInterestForUpdateDTO
            {
                Name = pointOfInterestFromStore.Name,
                Description = pointOfInterestFromStore.Description
            };

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!TryValidateModel(pointOfInterestToPatch)) return BadRequest(ModelState);

            pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]
        public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) return NotFound();

            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
            if (pointOfInterestFromStore == null) return NotFound();

            city.PointsOfInterest.Remove(pointOfInterestFromStore);

            return NoContent();
        }
    }
}
