using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Infrastructure.Persistence.DataSeed
{
    internal class CarSeeder
    {
        public static void SeedCars(ModelBuilder builder)
        {
            builder.Entity<Car>().HasData(
                new Car
                {
                    Id = 1,
                    ModelId = 1,
                    Name = "Toyota Corolla",
                    FuelType = CarFuelType.Petrol,
                    Status = CarStatus.Available,
                    ReleaseYear = 2020,
                    PricePerHour = 95.00M,
                },
                new Car
                {
                    Id = 2,
                    ModelId = 2,
                    Name = "Toyota Camry",
                    FuelType = CarFuelType.Hybrid,
                    Status = CarStatus.Available,
                    ReleaseYear = 2021,
                    PricePerHour = 120.00M,
                },
                new Car
                {
                    Id = 3,
                    ModelId = 3,
                    Name = "Subaru Impreza",
                    FuelType = CarFuelType.Petrol,
                    Status = CarStatus.Available,
                    ReleaseYear = 2019,
                    PricePerHour = 110.00M,
                },
                new Car
                {
                    Id = 4,
                    ModelId = 4,
                    Name = "Subaru Forester",
                    FuelType = CarFuelType.Diesel,
                    Status = CarStatus.Available,
                    ReleaseYear = 2022,
                    PricePerHour = 130.00M,
                },
                new Car
                {
                    Id = 5,
                    ModelId = 5,
                    Name = "Nissan Altima",
                    FuelType = CarFuelType.Petrol,
                    Status = CarStatus.Available,
                    ReleaseYear = 2017,
                    PricePerHour = 115.00M,
                },
                new Car
                {
                    Id = 6,
                    ModelId = 6,
                    Name = "Nissan Sentra",
                    FuelType = CarFuelType.Petrol,
                    Status = CarStatus.Available,
                    ReleaseYear = 2023,
                    PricePerHour = 90.00M,
                }
            );
        }
    }
}
