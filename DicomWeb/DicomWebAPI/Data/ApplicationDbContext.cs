using DicomWebAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace DicomWebAPI.Data
{
    public class ApplicationDbContext :DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<LocalUser> LocalUsers { get; set; }

        public DbSet<Study> Studies { get; set; }

        public DbSet<Series> Series { get; set; }

        public DbSet<Image> Images { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions )
            : base( dbContextOptions )
        { }
    }
}
