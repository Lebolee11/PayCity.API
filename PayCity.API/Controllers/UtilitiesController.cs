using Microsoft.AspNetCore.Mvc;
using PayCity.API.Models;

namespace PayCity.API.Controllers
{

    [ApiController]
    [Route("api/utilities")]
    public class UtilitiesController : ControllerBase
    {
        [HttpPost("buy-token")]
        public IActionResult BuyToken([FromBody] BuyTokenRequest request)
        {
            // Token generation logic here
            return Ok(new { token = "ABC123", receipt = "R123456" });
        }
    }
}
