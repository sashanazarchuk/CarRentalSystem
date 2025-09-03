using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Services;
using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Domain.Enum;
using FakeItEasy;
using Microsoft.Extensions.Logging;


namespace CarRentalSystem.Tests.UnitTests.Services
{
    public class CarServiceTests
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ICarRepository _carRepository;
        private readonly ILogger<CarService> _logger;
        private readonly CarService _service;

        public CarServiceTests()
        {
            _bookingRepository = A.Fake<IBookingRepository>();
            _carRepository = A.Fake<ICarRepository>();
            _logger = A.Fake<ILogger<CarService>>();
            _service = new CarService(_bookingRepository, _carRepository, _logger);
        }


        [Fact]
        public async Task UpdateCarStatusAsync_UpdatesStatusToAvailable_WhenNoOtherBookings()
        {
            // Arrange
            var car = new Car { Id = 1, Status = CarStatus.Rented };
            A.CallTo(() => _bookingRepository.IsCarBookedAsync(car.Id, A<CancellationToken>.Ignored))
                .Returns(Task.FromResult(false));

            // Act
            await _service.UpdateCarStatusAsync(car, CancellationToken.None);

            // Assert
            Assert.Equal(CarStatus.Available, car.Status);
            A.CallTo(() => _carRepository.PatchAsync(car, A<CancellationToken>.Ignored)).MustHaveHappenedOnceExactly();
        }

        
        [Fact]
        public async Task UpdateCarStatusAsync_DoesNothing_WhenCarHasOtherBookings()
        {
            // Arrange
            var car = new Car { Id = 1, Status = CarStatus.Rented };
            A.CallTo(() => _bookingRepository.IsCarBookedAsync(car.Id, A<CancellationToken>.Ignored))
                .Returns(Task.FromResult(true));

            // Act
            await _service.UpdateCarStatusAsync(car, CancellationToken.None);

            // Assert
            Assert.Equal(CarStatus.Rented, car.Status);
            A.CallTo(() => _carRepository.PatchAsync(A<Car>._, A<CancellationToken>.Ignored)).MustNotHaveHappened();
        }

        
        [Fact]
        public async Task UpdateCarStatusAsync_DoesNothing_WhenCarInMaintenance()
        {
            // Arrange
            var car = new Car { Id = 1, Status = CarStatus.Maintenance };
            A.CallTo(() => _bookingRepository.IsCarBookedAsync(car.Id, A<CancellationToken>.Ignored))
                .Returns(Task.FromResult(false));

            // Act
            await _service.UpdateCarStatusAsync(car, CancellationToken.None);

            // Assert
            Assert.Equal(CarStatus.Maintenance, car.Status);
            A.CallTo(() => _carRepository.PatchAsync(A<Car>._, A<CancellationToken>.Ignored)).MustNotHaveHappened();
        }
    }
}