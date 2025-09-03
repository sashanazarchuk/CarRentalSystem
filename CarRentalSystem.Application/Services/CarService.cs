using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Interfaces.Services;
using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Domain.Enum;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Services
{
    public class CarService : ICarService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ICarRepository _carRepository;
        private readonly ILogger<CarService> _logger;

        public CarService(IBookingRepository bookingRepository, ICarRepository carRepository, ILogger<CarService> logger)
        {
            _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
            _carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
            _logger = logger;
        }
        public async Task UpdateCarStatusAsync(Car car, CancellationToken cancellationToken)
        {

            if (car == null) return;

            bool isOtherBookings = await _bookingRepository.IsCarBookedAsync(car.Id, cancellationToken);
            if (!isOtherBookings && car.Status != CarStatus.Maintenance)
            {
                _logger.LogInformation("No other active bookings for CarId {CarId}. Updating status to Available.", car.Id);
                car.Status = CarStatus.Available;
                await _carRepository.PatchAsync(car, cancellationToken);
                _logger.LogInformation("Car {CarId} status updated to Available", car.Id);
            }
        }

        public Task<decimal> CalculateBookingPriceAsync(Car car, DateTime start, DateTime end)
        {
            var totalHours = (decimal)(end - start).TotalHours;
            var totalPrice = Math.Round(car.PricePerHour * totalHours, 2);
            return Task.FromResult(totalPrice);
        }
    }
}