﻿using CityInfo.ASP.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.ASP.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private CitiesDataStore _citiesDataStore;

        public CitiesController(CitiesDataStore citiesDataStore)
        {
            _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
        }

        [HttpGet]
        public ActionResult<IEnumerable<CityDto>> GetCities()
        {
            var citiesToReturn = _citiesDataStore.Cities;
            return Ok(citiesToReturn);
        }

        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCity(int id)
        {
            var cityToReturn = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == id);
            if (cityToReturn == null) return NotFound();            
            return Ok(cityToReturn);
        }
    }
}
