using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Infrastructure.Persistence.DataSeed
{
    internal class DataSeeder
    {
        public static void SeedAll(ModelBuilder builder)
        {
            CarBrandSeeder.SeedCarBrands(builder);
            CarModelSeeder.SeedCarModels(builder);
            CarSeeder.SeedCars(builder);
            RoleSeeder.SeedRoles(builder);
        }
    }
}