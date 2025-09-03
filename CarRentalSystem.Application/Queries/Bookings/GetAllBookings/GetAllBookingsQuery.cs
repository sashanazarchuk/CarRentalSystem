using CarRentalSystem.Application.DTOs.Bookings;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Queries.Bookings.GetAllBookings
{
    public record GetAllBookingsQuery(string UserId, bool isAdmin):IRequest<IEnumerable<BookingDto>>;
}
