using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyHotels.WebApi.Domain;
using MyHotels.WebApi.Infrastructure;
using MyHotels.WebApi.Models;

namespace MyHotels.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<CountriesController> _logger;

        public CountriesController(IUnitOfWork uow, IMapper mapper, ILogger<CountriesController> logger)
        {
            this._uow = uow;
            this._mapper = mapper;
            this._logger = logger;
        }

        // GET ... api/countries
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IList<CountryDto>>> GetCountries()
        {
            _logger.LogInformation($"{nameof(GetCountries)} called...");

            try
            {
                var countries = await _uow.Countries.GetAll();
                var results = _mapper.Map<IList<CountryDto>>(countries);

                return Ok(results);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Something went wrong in {nameof(GetCountries)}");

                //return StatusCode(StatusCodes.Status500InternalServerError,
                //    "Internal server error, please try again later...");

                return Problem("Internal server error, please try again later...");
            }
        }

        // GET ... api/countries/1
        [HttpGet("{id:int}", Name="GetCountry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CountryDto>> GetCountry(int id)
        {
            _logger.LogInformation($"{nameof(GetCountry)} called...");

            try
            {
                var country = await _uow.Countries.Get(c => c.Id == id, new List<string> { "Hotels"});

                if (country == null)
                {
                    return NotFound($"Not found country with id = {id}");
                }

                var result = _mapper.Map<CountryDto>(country);

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Something went wrong in {nameof(GetCountry)}");

                return Problem("Internal server error, please try again later...");
            }
        }

        // POST ... api/countries
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDto countryDto)
        {
            _logger.LogInformation($"{nameof(CreateCountry)} called...");

            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(CreateCountry)}");

                return BadRequest(ModelState);
            }

            try
            {
                var country = _mapper.Map<Country>(countryDto);
                await _uow.Countries.Add(country);
                await _uow.Save();

                return CreatedAtRoute("GetCountry", new {id = country.Id}, country);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Something went wrong in {nameof(CreateCountry)}");

                return Problem("Internal server error, please try again later...");
            }
        }

        // PUT ... api/countries/1
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryDto countryDto)
        {
            _logger.LogInformation($"{nameof(UpdateCountry)} called...");

            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid PUT attempt in {nameof(UpdateCountry)}");

                return BadRequest(ModelState);
            }

            try
            {
                var country = await _uow.Countries.Get(c => c.Id == id);

                if (country == null)
                {
                    return BadRequest("Submitted data is invalid!");
                }

                _mapper.Map(countryDto, country);
                _uow.Countries.Modify(country);
                await _uow.Save();

                return NoContent();

            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Something went wrong in {nameof(UpdateCountry)}");

                return Problem("Internal server error, please try again later...");
            }
        }

        // DELETE ../api/countries/1
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            _logger.LogInformation($"{nameof(DeleteCountry)} called...");

            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteCountry)}");

                return BadRequest();
            }

            try
            {
                var country = await _uow.Countries.Get(c => c.Id == id);

                if (country == null)
                {
                    return NotFound($"There is no country with id = {id}");
                }

                await _uow.Countries.Remove(id);
                await _uow.Save();

                return NoContent();

            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Something went wrong in {nameof(DeleteCountry)}");

                return Problem("Internal server error, please try again later...");
            }
        }
    }
}
