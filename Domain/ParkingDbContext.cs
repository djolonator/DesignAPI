using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class ParkingDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Location> Locations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ParkingDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Random randomLocation = new Random();
            int count = 20;
            const double centralLat = 44.787197;
            const double centralLong = 20.457273;
            const double latRange = 0.1;
            const double longRange = 0.2;

            Location[] locations = new Location[count];
            List<User> users = new List<User>();
            Random randomUser = new Random(100);
            int id = -1;
            for (int i = 0; i < count; i++)
            {
                double lat = centralLat + ((randomLocation.NextDouble() - 0.5) * latRange);
                double longi = centralLong + ((randomLocation.NextDouble() - 0.5) * longRange);

                var user = new User() { Points = randomUser.Next() };

                locations[i] = new Location() { LocationId = id, Latitude = lat, Longitude = longi, UserId = user.Id };
                id--;
            }

            modelBuilder.Entity<Location>().HasData(locations);
            modelBuilder.Entity<User>().HasData(users);
        }

        public ParkingDbContext(DbContextOptions<ParkingDbContext> options)
       : base(options)
        {
        }
    }
}
