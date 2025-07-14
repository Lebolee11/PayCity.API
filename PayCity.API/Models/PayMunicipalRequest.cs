using Microsoft.AspNetCore.Mvc;

namespace PayCity.API.Models
{
    public class PayMunicipalRequest
    {
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
    }
}
