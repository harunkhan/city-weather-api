using CityInfo.API.Model;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
//TODO:
// 1. Error handling Not implemented.
// 2. Need to test Put.
// 3. tasks are Syncronus.
// 4. Weather details are constant, need to create and actual AppId for realtime data.
// 5. Solution can be improved further.

/// <summary>
/// This API provide CRUD operations for City.
/// while getting the data, Along with City It Combines data for country and Weather.
/// </summary>

namespace CityInfo.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CityController : ControllerBase
	{
		private readonly ICityService _cityService;

		public CityController(ICityService cityService) {
			this._cityService = cityService;
		}

		// GET: api/<CityController>
		[HttpGet]
		public IActionResult Get() {
			var cities = _cityService.GetCity();

			if (cities.Any()) {
				return Ok(cities);
			}
			return NoContent();
		}

		// GET api/<CityController>/Delhi
		[HttpGet("{name}")]
		public IActionResult Get(string name) {
			var city = _cityService.GetCity(name);

			if (city != null) {
				return Ok(city);
			}
			return NotFound();
		}

		// POST api/<CityController>
		[HttpPost]
		public IActionResult Post([FromBody] CityModel city) {
			if (ModelState.IsValid) {
				var id = _cityService.AddCity(city);
				return Ok(id);
			}
			return BadRequest(ModelState);
		}

		// PUT api/<CityController>/5
		[HttpPut("{id}")]
		public IActionResult Put(int id, [FromBody] CityModel city) {
			if (id > 0 && ModelState.IsValid) {
				bool response = _cityService.UpdateCity(id, city);
				return Ok(response);
			}
			return BadRequest(ModelState);
		}

		// DELETE api/<CityController>/5
		[HttpDelete("{id}")]
		public IActionResult Delete(int id) {
			if (id > 0) {
				var response = _cityService.DeleteCity(id);

				return Ok(response);
			}
			return NotFound(id);
		}
	}
}
