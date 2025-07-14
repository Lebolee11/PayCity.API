using Microsoft.AspNetCore.Mvc;
using PayCity.API.Models;
using System.Collections.Generic;

namespace PayCity.API.Controllers
{

    [ApiController]
    [Route("api/utilities")]
    public class UtilitiesController : ControllerBase
    {
        // In-memory storage for demo purposes
        private static readonly List<TokenHistoryItem> _tokenHistory = new();

        [HttpPost("buy-token")]
        public IActionResult BuyToken([FromBody] BuyTokenRequest request)
        {
            // Simulate token generation
            var token = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
            var receipt = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();

            // Store in history
            var historyItem = new TokenHistoryItem
            {
                MetreId = request.MeterNumber,
                Amount = request.Amount,
                Token = token,
                Date = DateTime.Now
            };
            _tokenHistory.Add(historyItem);

            return Ok(new { token, receipt });
        }

        [HttpGet("history")]
        public IActionResult GetTokenHistory()
        {
            return Ok(_tokenHistory);
        }
    }

    // DTO for history
    
}
