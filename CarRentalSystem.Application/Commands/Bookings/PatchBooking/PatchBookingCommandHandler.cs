using AutoMapper;
using CarRentalSystem.Application.DTOs.Bookings;
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

namespace CarRentalSystem.Application.Commands.Bookings.PatchBooking
{
    internal class PatchBookingCommandHandler : IRequestHandler<PatchBookingCommand, BookingDto>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IBookingValidator _bookingValidator;
        private readonly ICarService _carService;
        private readonly IMapper _mapper;   
        private readonly ILogger<PatchBookingCommandHandler> _logger;
        public PatchBookingCommandHandler(IBookingRepository bookingRepository, IMapper mapper, ILogger<PatchBookingCommandHandler> logger, IBookingValidator bookingValidator, ICarService carService)
        {
            _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger;
            _bookingValidator = bookingValidator ?? throw new ArgumentNullException(nameof(bookingValidator));
            _carService = carService ?? throw new ArgumentNullException(nameof(carService));
        }
        public async Task<BookingDto> Handle(PatchBookingCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting PATCH for BookingId {BookingId}", request.Id);

            var booking = await _bookingValidator.ValidateBookingAndUserExistsAsync(request.Id, request.UserId, request.isAdmin, "modify", cancellationToken);

            var bookingDto = _mapper.Map<PatchBookingDto>(booking);
            
            _logger.LogDebug("Applying patch to BookingId {BookingId}", request.Id);
            request.PatchDoc.ApplyTo(bookingDto);
            
            _mapper.Map(bookingDto, booking);
           
            _bookingValidator.ValidateBookingDates(booking.StartDate, booking.EndDate);
            await _bookingValidator.ValidateUserNoOverlapAsync(booking.UserId, booking.StartDate, booking.EndDate, cancellationToken, booking.Id);
            await _bookingValidator.ValidateCarNoOverlapAsync(booking.CarId, booking.StartDate, booking.EndDate, cancellationToken, booking.Id);
            booking.TotalPrice = await _carService.CalculateBookingPriceAsync(booking.Car, booking.StartDate, booking.EndDate);

            await _bookingRepository.PatchAsync(booking, cancellationToken);
            
            _logger.LogInformation("Booking {BookingId} updated in repository successfully", request.Id);
            return _mapper.Map<BookingDto>(booking);
        }
    }
}