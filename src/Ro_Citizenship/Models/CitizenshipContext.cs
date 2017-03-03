using Microsoft.EntityFrameworkCore;

namespace Ro_Citizenship.Models
{
    public class CitizenshipContext : DbContext
    {
        public CitizenshipContext(DbContextOptions<CitizenshipContext> options) : base(options)
        {

        }

        public DbSet<User> users { get; set; }
    }
}
