using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<CityDTO>> GetCities()
        {
            var result = CitiesDataStore.Current.Cities;
            //if (result == null) NoContent(); // This is not necessary
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<CityDTO> GetCity(int id)
        {
            var result = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);
            if (result == null) NoContent();
            return Ok(result);
        }
    }
}
