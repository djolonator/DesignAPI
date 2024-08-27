using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class ParkingDbContext : DbContext
    {
        public DbSet<Location> Locations { get; set; }
        public DbSet<User> Users { get; set; }

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
            
            int count = 20;

            var users = CreateUsers(count);
            var locations = CreateLocations(count, users);
            
            modelBuilder.Entity<User>().HasData(users);
            modelBuilder.Entity<Location>().HasData(locations);
        }

        private User[] CreateUsers(int count)
        {
            User[] users = new User[count];
            Random randomUserPoints = new Random(100);
            for (int i = 0; i < 20; i++)
            {
                users[i] = new User
                {
                    UserId = Guid.NewGuid(),
                    UserName = $"User{i + 1}",
                    PasswordHash = "hashedPassword",
                    Email = $"user{i + 1}@example.com",
                    Points = randomUserPoints.Next()
                };
            }

            return users;
        }

        private Location[] CreateLocations(int count, User[] users)
        {
            Location[] locations = new Location[count];
            Random randomLocation = new Random();

            const double centralLat = 44.787197;
            const double centralLong = 20.457273;
            const double latRange = 0.1;
            const double longRange = 0.2;

            for (int i = 0; i < count; i++)
            {
                double lat = centralLat + ((randomLocation.NextDouble() - 0.5) * latRange);
                double longi = centralLong + ((randomLocation.NextDouble() - 0.5) * longRange);

                locations[i] = new Location() 
                { 
                    LocationId = Guid.NewGuid(), 
                    Latitude = lat, 
                    Longitude = longi, 
                    UserId = users[i].UserId 
                };
            }

            return locations;
        }

        public ParkingDbContext(DbContextOptions<ParkingDbContext> options)
       : base(options)
        {
        }
    }
}
