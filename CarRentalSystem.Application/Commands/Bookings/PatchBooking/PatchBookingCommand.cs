using CarRentalSystem.Application.DTOs.Bookings;

using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Commands.Bookings.PatchBooking
{
    public record PatchBookingCommand(int Id, string UserId, bool isAdmin, JsonPatchDocument<PatchBookingDto> PatchDoc) : IRequest<BookingDto>;
}
