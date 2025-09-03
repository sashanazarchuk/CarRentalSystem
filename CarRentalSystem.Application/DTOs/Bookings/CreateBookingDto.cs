using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.DTOs.Bookings
{
    public class CreateBookingDto
    {
        public string CarName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
