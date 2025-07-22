
namespace PayCity.API.Models
{
    public class Fine
    {
        public string FineId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPaid { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public string Id { get; internal set; }
        public DateTime CreatedAt { get; internal set; }
    }
}
