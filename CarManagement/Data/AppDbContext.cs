using System.Collections.Generic;
using System.Reflection.Emit;
using CarManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CarManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<CarFactory> CarFactories { get; set; }
        public DbSet<Car> Cars { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarFactory>().ToTable("carfactories");
            modelBuilder.Entity<Car>().ToTable("cars");
        }
    }
}
