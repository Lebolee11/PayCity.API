using Microsoft.EntityFrameworkCore;
using PayCity.API.Models;

namespace PayCity.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Fine> Fines { get; set; }
    }
}
