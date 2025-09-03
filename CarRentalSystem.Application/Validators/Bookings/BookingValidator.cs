using CarRentalSystem.Application.Exceptions;
using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Interfaces.Validator;
using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Validators.Bookings
{
    public class BookingValidator : IBookingValidator
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ICarRepository _carRepository;

        public BookingValidator(IBookingRepository bookingRepository, ICarRepository carRepository)
        {
            _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
            _carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
        }

        public async Task<Car> ValidateCarExistsAndAvailableAsync(int carId, CancellationToken token)
        {
            var car = await _carRepository.GetByIdAsync(carId, token);
            if (car == null)
                throw new NotFoundException("Car not found");

            if (car.Status == CarStatus.Maintenance)
                throw new ForbiddenException($"Car {car.Name} is not available.");

            return car;
        }

        public async Task ValidateUserNoOverlapAsync(string userId, DateTime start, DateTime end, CancellationToken token, int? bookingId = null)
        {
            bool overlapping = await _bookingRepository.IsOverlappingBookingAsync(userId, bookingId, start, end, token);
            if (overlapping)
                throw new ForbiddenException("User already has a booking during these dates.");
        }

        public async Task ValidateCarNoOverlapAsync(int carId,  DateTime start, DateTime end, CancellationToken token, int? bookingId = null)
        {
            bool overlapping = await _carRepository.IsOverlappingCarAsync(carId, bookingId, start, end, token);
            if (overlapping)
                throw new ForbiddenException("Car is already booked for these dates.");
        }

        public void ValidateBookingDates(DateTime start, DateTime end)
        {
            var now = DateTime.UtcNow;
            if (start < now || end < now)
                throw new BadRequestException("Booking dates cannot be in the past.");

            if (end <= start)
                throw new BadRequestException("End date must be after start date.");
        }

        public void ValidateBookingDuration(DateTime start, DateTime end, TimeSpan? minDuration = null, TimeSpan? maxDuration = null)
        {
            var duration = end - start;
            var min = minDuration ?? TimeSpan.FromHours(1);      
            var max = maxDuration ?? TimeSpan.FromDays(30);      

            if (duration < min)
                throw new BadRequestException($"Minimum reservation — {min.TotalHours} hour.");

            if (duration > max)
                throw new BadRequestException($"Maximum reservation — {max.TotalDays} days.");
        }


        public async Task<Booking> ValidateBookingAndUserExistsAsync(int bookingId, string userId, bool isAdmin, string action, CancellationToken token)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId, token);
            if (booking == null)
                throw new NotFoundException($"Booking with id {bookingId} not found");

            if (!isAdmin && booking.UserId != userId)
                throw new ForbiddenException($"You are not allowed to {action} this booking.");

            return booking;
        }

    }
}