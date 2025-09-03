using CarRentalSystem.Application.Exceptions;
using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Validators.Bookings;
using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Domain.Enum;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Tests.UnitTests.Validators
{
    public class BookingValidatorTests
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ICarRepository _carRepository;
        private readonly BookingValidator _validator;

        public BookingValidatorTests()
        {
            _bookingRepository = A.Fake<IBookingRepository>();
            _carRepository = A.Fake<ICarRepository>();
            _validator = new BookingValidator(_bookingRepository, _carRepository);
        }

        [Fact]
        public async Task ValidateCarExistsAndAvailableAsync_ThrowsNotFound_WhenCarDoesNotExist()
        {
            A.CallTo(() => _carRepository.GetByIdAsync(1, A<CancellationToken>._))
                .Returns(Task.FromResult<Car?>(null));

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _validator.ValidateCarExistsAndAvailableAsync(1, CancellationToken.None));
        }

        [Fact]
        public async Task ValidateCarExistsAndAvailableAsync_ThrowsForbidden_WhenCarInMaintenance()
        {
            var car = new Car { Id = 1, Name = "TestCar", Status = CarStatus.Maintenance };
            A.CallTo(() => _carRepository.GetByIdAsync(1, A<CancellationToken>._))
                .Returns(Task.FromResult<Car?>(car));

            await Assert.ThrowsAsync<ForbiddenException>(() =>
                _validator.ValidateCarExistsAndAvailableAsync(1, CancellationToken.None));
        }

        [Fact]
        public void ValidateBookingDates_ThrowsBadRequest_WhenDatesInvalid()
        {
            var start = DateTime.UtcNow.AddHours(-1);
            var end = start.AddHours(-2);

            Assert.Throws<BadRequestException>(() => _validator.ValidateBookingDates(start, end));
        }

        [Fact]
        public async Task ValidateBookingAndUserExistsAsync_ThrowsForbidden_WhenUserHasNoRights()
        {
            var booking = new Booking { Id = 1, UserId = "user2" };
            A.CallTo(() => _bookingRepository.GetByIdAsync(1, A<CancellationToken>._))
                .Returns(Task.FromResult<Booking?>(booking));

            await Assert.ThrowsAsync<ForbiddenException>(() =>
                _validator.ValidateBookingAndUserExistsAsync(1, "user1", false, "edit", CancellationToken.None));
        }

        [Fact]
        public async Task ValidateBookingAndUserExistsAsync_Succeeds_WhenAdminHasRights()
        {
            var booking = new Booking { Id = 1, UserId = "user2" };
            A.CallTo(() => _bookingRepository.GetByIdAsync(1, A<CancellationToken>._))
                .Returns(Task.FromResult<Booking?>(booking));

            var result = await _validator.ValidateBookingAndUserExistsAsync(1, "user1", true, "edit", CancellationToken.None);

            Assert.Equal(booking, result);
        }

    }
}
