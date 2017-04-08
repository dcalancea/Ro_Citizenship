using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class CitizenshipContext : DbContext
    {
        public CitizenshipContext(DbContextOptions<CitizenshipContext> options) : base(options)
        {

        }

        public DbSet<User> users { get; set; }
    }
}
