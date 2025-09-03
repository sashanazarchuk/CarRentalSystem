using CarRentalSystem.Application.DTOs.Bookings;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Queries.Bookings.GetBookingById
{
    public record GetBookingByIdQuery(int id, string UserId, bool isAdmin) :IRequest<BookingDto>;

}
