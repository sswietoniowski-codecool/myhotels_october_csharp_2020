using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyHotels.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetException()
        {
            throw new Exception("Something went wrong");

            return Ok();
        }
    }
}
