using CarRentalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Infrastructure.Persistence.DataSeed
{
    internal class CarBrandSeeder
    {
        public static void SeedCarBrands(ModelBuilder builder)
        {
            builder.Entity<CarBrand>().HasData(

                new CarBrand
                {
                    Id = 1,
                    BrandName = "Toyota"
                },

                new CarBrand
                {
                    Id = 2,
                    BrandName = "Subaru"
                },
                new CarBrand
                {
                    Id = 3,
                    BrandName = "Nissan"
                }
            );
        }
    }
}
