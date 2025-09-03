using AutoMapper;
using CarRentalSystem.Application.DTOs.Bookings;
using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Interfaces.Services;
using CarRentalSystem.Application.Interfaces.Validator;
using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Domain.Enum;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Commands.Bookings.CreateBooking
{
    internal class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, CreateBookingDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateBookingCommandHandler> _logger;
        private readonly IBookingValidator _bookingValidator;
        private readonly ICarService _carService;

        public CreateBookingCommandHandler( IUnitOfWork unitOfWork,  IMapper mapper,  ILogger<CreateBookingCommandHandler> logger, IBookingValidator bookingValidator, ICarService carService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger;
            _bookingValidator = bookingValidator ?? throw new ArgumentNullException(nameof(bookingValidator));
            _carService = carService ?? throw new ArgumentNullException(nameof(carService));
        }

        public async Task<CreateBookingDto> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting creation of booking for UserId: {UserId} and CarId: {CarId}", request.UserId, request.CarId);
            _bookingValidator.ValidateBookingDates(request.StartDate, request.EndDate);
            _bookingValidator.ValidateBookingDuration(request.StartDate, request.EndDate);
            var car = await _bookingValidator.ValidateCarExistsAndAvailableAsync(request.CarId, cancellationToken);
            await _bookingValidator.ValidateUserNoOverlapAsync(request.UserId, request.StartDate, request.EndDate, cancellationToken);
            await _bookingValidator.ValidateCarNoOverlapAsync(request.CarId, request.StartDate, request.EndDate, cancellationToken);

            var totalPrice = await _carService.CalculateBookingPriceAsync(car, request.StartDate, request.EndDate);

            var booking = new Booking
            {
                CarId = request.CarId,
                UserId = request.UserId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                TotalPrice = totalPrice
            };

            var created = await _unitOfWork.Bookings.CreateAsync(booking, cancellationToken);

            car.Status = CarStatus.Booked;
           
            await _unitOfWork.Cars.PatchAsync(car, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Booking created successfully with Id {BookingId} for UserId: {UserId}", created.Id, request.UserId);

            return _mapper.Map<CreateBookingDto>(created);
        }
    }
}