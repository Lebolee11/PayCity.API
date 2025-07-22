using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayCity.API.Controllers;
using PayCity.API.Data;
using PayCity.API.Models;

namespace PayCity.API.Controllers
{
    [ApiController]
    [Route("api/municipal")]
    public class MunicipalController : ControllerBase
    {
        [HttpPost("pay")]
        public IActionResult PayMunicipalAccount([FromBody] PayMunicipalRequest request)
        {
            // In a real app, validate account number, process payment
            var receiptId = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

            return Ok(new
            {
                receipt = receiptId,
                accountNumber = request.AccountNumber,
                amount = request.Amount,
                paymentMethod = request.PaymentMethod,
                status = "Municipal payment successful",
                timestamp = DateTime.UtcNow
            });
        }
    }
}
