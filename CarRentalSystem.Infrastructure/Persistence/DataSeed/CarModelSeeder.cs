using CarRentalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Infrastructure.Persistence.DataSeed
{
    internal class CarModelSeeder
    {
        public static void SeedCarModels(ModelBuilder builder)
        {
            builder.Entity<CarModel>().HasData(

                new CarModel
                {
                    Id = 1,
                    BrandId = 1,
                    ModelName = "Corolla"
                },
                new CarModel
                {
                    Id = 2,
                    BrandId = 1,
                    ModelName = "Camry"
                },

                new CarModel
                {
                    Id = 3,
                    BrandId = 2,
                    ModelName = "Impreza"
                },
                new CarModel
                {
                    Id = 4,
                    BrandId = 2,
                    ModelName = "Forester"
                },
                new CarModel
                {
                    Id = 5,
                    BrandId = 3,
                    ModelName = "Altima"
                },
                new CarModel
                {
                    Id = 6,
                    BrandId = 3,
                    ModelName = "Sentra"
                }

            );
        }
    }
}
