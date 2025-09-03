using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.DTOs.Car
{
    public class CarDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ModelId { get; set; }
        public string ModelName { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string FuelType { get; set; } = string.Empty;
        public decimal PricePerHour { get; set; }
        public int ReleaseYear { get; set; }
    }
}
