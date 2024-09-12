using Abp.Domain.Uow;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class StorageDbContext : IdentityDbContext, IUnitOfWork
    {
        public DbSet<Location> Locations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Design> Design { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=StorageDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }    

        public StorageDbContext(DbContextOptions<StorageDbContext> options)
       : base(options)
        {
        }
    }
}
