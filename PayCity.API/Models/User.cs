using System.ComponentModel.DataAnnotations;

namespace PayCity.API.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string PasswordHash { get; set; }
        public bool TermsAccepted { get; set; }

        public List<Fine> Fines { get; set; } = new();
    }
}
