using CarRentalSystem.Infrastructure.Persistence;
using CarRentalSystem.Infrastructure.Persistence.DataSeed;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Infrastructure.Extensions
{
    public static class SeedRunnerExtensions
    {
        public static async Task SeedAllAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                // Apply any pending migrations
                var dbContext = scope.ServiceProvider.GetRequiredService<CarRentDbContext>();
                dbContext.Database.Migrate();

                // Seed the admin
                var seeder = services.GetRequiredService<AdminSeeder>(); 
                await seeder.SeedAdminAsync();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<AdminSeeder>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;  
            }
        }
    }
}