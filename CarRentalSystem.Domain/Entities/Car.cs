using CarRentalSystem.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Domain.Entities
{
    public class Car : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public int ModelId { get; set; }
        public CarModel Model { get; set; } = null!;
        public CarStatus Status { get; set; }   = CarStatus.Available;
        public CarFuelType FuelType { get; set; }
        public decimal PricePerHour { get; set; }
        public int ReleaseYear { get; set; }
        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}