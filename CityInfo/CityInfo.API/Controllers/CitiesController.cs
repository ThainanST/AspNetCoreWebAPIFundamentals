using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly CitiesDataStore _citiesDataStore;

        public CitiesController(CitiesDataStore citiesDataStore)
        {
            _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
        }

        [HttpGet]
        public ActionResult<IEnumerable<CityDTO>> GetCities()
        {
            var result = _citiesDataStore.Cities;
            //if (result == null) NoContent(); // This is not necessary
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<CityDTO> GetCity(int id)
        {
            var result = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == id);
            if (result == null) NoContent();
            return Ok(result);
        }
    }
}
