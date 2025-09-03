using CarRentalSystem.Application.DTOs.Car;
using CarRentalSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.DTOs.Bookings
{
    public class BookingDto
    {
        public int Id { get; set; }
        public CarDto Car { get; set; } = null!;
        public string UserId { get; set; } = string.Empty;
        public string User { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string CarStatus { get; set; } = null!;  
    }
}
