﻿using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class StorageDbContext : IdentityDbContext, IUnitOfWork
    {
        public DbSet<Design> Design { get; set; }
        public DbSet<DesignCategory> DesignCategory { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<Recipient> Recipient { get; set; }


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

            modelBuilder.Entity<Order>()
               .HasOne<IdentityUser>() 
               .WithMany()              
               .HasForeignKey(o => o.UserId) 
               .OnDelete(DeleteBehavior.Restrict);
        }    

        public StorageDbContext(DbContextOptions<StorageDbContext> options)
       : base(options)
        {
        }
    }
}
