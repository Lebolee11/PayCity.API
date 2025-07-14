using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using PayCity.API.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[ApiController]
[Route("[controller]")]
public class FinesController : ControllerBase
{
    private static readonly ConcurrentDictionary<string, Fine> Fines = new ConcurrentDictionary<string, Fine>(
            new[]
            {
                new KeyValuePair<string, Fine>("F123", new Fine
                {
                    FineId = "F123",
                    Description = "Speeding - 120km/h in 60 zone",
                    Amount = 800,
                    ImageUrl = "https://example.com/speeding.jpg",
                    IsPaid = false
                }),
                new KeyValuePair<string, Fine>("F124", new Fine
                {
                    FineId = "F124",
                    Description = "Illegal Parking",
                    Amount = 300,
                    ImageUrl = "https://example.com/parking.jpg",
                    IsPaid = false
                })
            }
        );
    [HttpGet("view")]
    public IActionResult ViewFines()
    {
        /*var fines = new[]
        {
            new { FineId = "F123", Description = "Speeding", Amount = 500.00, ImageUrl = "http://example.com/fine1.jpg" },
            new { FineId = "F124", Description = "Illegal Parking", Amount = 300.00, ImageUrl = "http://example.com/fine2.jpg" }
        };
        return Ok(fines);*/
        return Ok(Fines.Values);
    }
    [HttpPost("pay")]
        public IActionResult PayFine([FromBody] PayFineRequest request)
        {
            if (!Fines.TryGetValue(request.FineId, out var fine))
                return NotFound(new { message = "Fine not found." });

            if (fine.IsPaid)
                return BadRequest(new { message = "Fine is already paid." });

            fine.IsPaid = true;
            return Ok(new
            {
                message = "Fine paid successfully",
                receipt = Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                fineId = fine.FineId,
                amount = fine.Amount,
                paymentMethod = request.PaymentMethod
            });
        }
}
