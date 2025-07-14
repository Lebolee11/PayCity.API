namespace PayCity.API.Models
{
    public class Fine
    {
        public string FineId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPaid { get; set; }
    }
}
