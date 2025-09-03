using CarRentalSystem.Application.DTOs.Bookings;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Commands.Bookings.CreateBooking
{
    public record CreateBookingCommand: IRequest<CreateBookingDto>
    {
        public int CarId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    
}
