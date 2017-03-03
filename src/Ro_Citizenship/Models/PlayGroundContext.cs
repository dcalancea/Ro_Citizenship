using Microsoft.EntityFrameworkCore;

namespace Ro_Citizenship.Models
{
    public partial class PlayGroundContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlServer(@"Data Source=localhost;Initial Catalog=PlayGround;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.DossierNr)
                    .HasName("PK_Users");

                entity.Property(e => e.DossierNr).HasColumnType("varchar(10)");

                entity.Property(e => e.FirstName).HasColumnType("varchar(30)");

                entity.Property(e => e.LastName).HasColumnType("varchar(30)");

                entity.Property(e => e.OrderNr).HasColumnType("varchar(10)");

                entity.Property(e => e.RegisterDate).HasColumnType("datetime");

                entity.Property(e => e.ResolutionDate).HasColumnType("datetime");

                entity.Property(e => e.Term).HasColumnType("datetime");
            });
        }
    }
}