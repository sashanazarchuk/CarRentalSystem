using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Domain.Entities
{
    public class Booking:BaseEntity
    {
        public int CarId { get; set; }
        public Car Car { get; set; } = null!;
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
    }
}