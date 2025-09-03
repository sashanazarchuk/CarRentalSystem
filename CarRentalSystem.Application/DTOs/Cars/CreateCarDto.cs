using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.DTOs.Cars
{
    public class CreateCarDto
    {
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string FuelType { get; set; } = string.Empty;
        public decimal PricePerHour { get; set; }
        public int ReleaseYear { get; set; }
    }
}
