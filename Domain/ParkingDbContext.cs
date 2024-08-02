using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class ParkingDbContext: IdentityDbContext<IdentityUser>
    {
        public DbSet<Location> Locations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseInMemoryDatabase("ParkingDB");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            Random random = new Random();
            int count = 20;
            const double centralLat = 44.787197;
            const double centralLong = 20.457273;


            const double latRange = 0.1;
            const double longRange = 0.2;

            Location[] locations = new Location[count];

            for (int i = 0; i < count; i++)
            {
                double lat = centralLat + ((random.NextDouble() - 0.5) * latRange);
                double longi = centralLong + ((random.NextDouble() - 0.5) * longRange);
                locations[i] = new Location() { Latitude = lat, Longitude = longi };     
            }

            modelBuilder.Entity<Location>().HasData(locations);
        }
    }
}
