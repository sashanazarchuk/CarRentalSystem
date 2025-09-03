using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Commands.Bookings.DeleteBooking
{
    public record DeleteBookingCommand(int BookingId, string UserId, bool isAdmin) :IRequest;

}
