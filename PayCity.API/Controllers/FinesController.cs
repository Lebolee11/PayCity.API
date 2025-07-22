/*using System.Collections.Concurrent;
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
}*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayCity.API.Data;
using PayCity.API.Models;
using System.Security.Claims;

namespace PayCity.API.Controllers;

[ApiController]
[Route("api/fines")]
public class FinesController : ControllerBase
{
    private readonly AppDbContext _context;

    public FinesController(AppDbContext context)
    {
        _context = context;
    }

    // 🔹 Get all fines for the logged-in user
    [HttpGet("view")]
    [Authorize]
    public async Task<IActionResult> GetUserFines()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdStr, out var userId))
            return BadRequest("Invalid user identifier.");

        var fines = await _context.Fines
            .Where(f => f.UserId == userId)
            .ToListAsync();
        return Ok(fines);
    }

    // 🔹 Get a specific fine
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFine(string id)
    {
        var fine = await _context.Fines.FindAsync(id);
        if (fine == null) return NotFound();
        return Ok(fine);
    }

    // 🔹 Create a new fine
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateFine([FromBody] PayCity.API.Models.Fine fine)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdStr, out var userId))
            return BadRequest("Invalid user identifier.");
        fine.Id = Guid.NewGuid().ToString();
        fine.UserId = userId;
        fine.CreatedAt = DateTime.UtcNow;

        _context.Fines.Add(fine);
        await _context.SaveChangesAsync();
        return Ok(fine);
    }

    // 🔹 Update a fine
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFine(string id, [FromBody] Fine fine)
    {
        var existing = await _context.Fines.FindAsync(id);
        if (existing == null) return NotFound();

        existing.Description = fine.Description;
        existing.Amount = fine.Amount;
        existing.IsPaid = fine.IsPaid;

        await _context.SaveChangesAsync();
        return Ok(existing);
    }

    // 🔹 Delete a fine
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFine(string id)
    {
        var fine = await _context.Fines.FindAsync(id);
        if (fine == null) return NotFound();

        _context.Fines.Remove(fine);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // 🔹 Pay a fine
    [HttpPost("pay")]
    [Authorize]
    public async Task<IActionResult> PayFine([FromBody] FinePaymentRequest request)
    {
        var fine = await _context.Fines.FindAsync(request.FineId);
        if (fine == null) return NotFound();

        fine.IsPaid = true;
        await _context.SaveChangesAsync();
        return Ok(new { message = "Fine paid successfully." });
    }
}

public class FinePaymentRequest
{
    public string FineId { get; set; }
    public string PaymentMethod { get; set; }
}

// Remove this duplicate Fine class definition.
// Use PayCity.API.Models.Fine instead.
