using CarRentalSystem.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.DTOs.Cars
{
    public class PatchCarDto
    {
        public string? Name { get; set; } = string.Empty;
        public int? ModelId { get; set; }
        public string? Status { get; set; } = string.Empty;
        public CarFuelType? FuelType { get; set; } 
        public decimal? PricePerHour { get; set; }
        public int? ReleaseYear { get; set; }
    }
}
