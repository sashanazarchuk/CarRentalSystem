using CarRentalSystem.Application.Commands.Bookings.CreateBooking;
using CarRentalSystem.Application.Commands.Bookings.DeleteBooking;
using CarRentalSystem.Application.Commands.Bookings.PatchBooking;
using CarRentalSystem.Application.DTOs.Bookings;
using CarRentalSystem.Application.Queries.Bookings.GetAllBookings;
using CarRentalSystem.Application.Queries.Bookings.GetBookingById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarRentalSystem.API.Controllers
{
    [Authorize]
    [Route("api/booking")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var isAdmin = User.IsInRole("Admin");  
            var bookings = await _mediator.Send(new GetAllBookingsQuery(userId, isAdmin));
            return Ok(bookings);
        }

   
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var isAdmin = User.IsInRole("Admin");
            var query = new GetBookingByIdQuery(id, userId, isAdmin);
            var booking = await _mediator.Send(query);

            return Ok(booking);
        }


      
        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingCommand command)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            command.UserId = userId;
            var booking = await _mediator.Send(command);
            return Ok(booking);
        }

       
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchBooking(int id, [FromBody] JsonPatchDocument<PatchBookingDto> patchDoc)
        {
            if (patchDoc == null) return BadRequest();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var isAdmin = User.IsInRole("Admin");
            var bookingDto = await _mediator.Send(new PatchBookingCommand(id, userId, isAdmin, patchDoc));
            return Ok(bookingDto);
        }

 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var isAdmin = User.IsInRole("Admin");
            await _mediator.Send(new DeleteBookingCommand(id, userId, isAdmin));
            return NoContent();
        }

    }
}