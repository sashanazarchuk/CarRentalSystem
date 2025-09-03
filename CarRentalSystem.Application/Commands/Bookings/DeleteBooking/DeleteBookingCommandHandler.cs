using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Interfaces.Services;
using CarRentalSystem.Application.Interfaces.Validator;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Commands.Bookings.DeleteBooking
{
    internal class DeleteBookingCommandHandler : IRequestHandler<DeleteBookingCommand>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ICarService _carService;
        private readonly IBookingValidator _bookingValidator;
        private readonly ILogger<DeleteBookingCommandHandler> _logger;
        public DeleteBookingCommandHandler(IBookingRepository bookingRepository, ILogger<DeleteBookingCommandHandler> logger, ICarService carService, IBookingValidator bookingValidator)
        {
            _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
            _bookingValidator = bookingValidator ?? throw new ArgumentNullException(nameof(bookingValidator));
            _logger = logger;
            _carService = carService ?? throw new ArgumentNullException(nameof(carService));    
        }
        public async Task Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting delete for BookingId {BookingId}", request.BookingId);

            var booking = await _bookingValidator.ValidateBookingAndUserExistsAsync(request.BookingId, request.UserId, request.isAdmin, "delete", cancellationToken);

            await _bookingRepository.DeleteAsync(booking, cancellationToken);
            _logger.LogInformation("Booking with ID {BookingId} deleted successfully", request.BookingId);


            _logger.LogInformation("Updating status for CarId {CarId} after booking deletion", booking.CarId);
            await _carService.UpdateCarStatusAsync(booking.Car, cancellationToken);
        }
    }
}