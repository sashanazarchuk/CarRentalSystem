using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Infrastructure.Persistence.DataSeed;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Infrastructure.Persistence
{
    public class CarRentDbContext : IdentityDbContext<User>
    {
        public CarRentDbContext(DbContextOptions<CarRentDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Apply all configurations from the current assembly
            builder.ApplyConfigurationsFromAssembly(typeof(CarRentDbContext).Assembly);

            // Seed initial data
            DataSeeder.SeedAll(builder);
        }

        internal DbSet<Car> Cars { get; set; }
        internal DbSet<CarBrand> CarBrands { get; set; }
        internal DbSet<CarModel> CarModels { get; set; }
        internal DbSet<Booking> Bookings { get; set; }
    }
}