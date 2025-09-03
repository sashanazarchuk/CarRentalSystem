using CarRentalSystem.Application.Commands.Bookings.DeleteBooking;
using CarRentalSystem.Application.Exceptions;
using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Interfaces.Services;
using CarRentalSystem.Application.Interfaces.Validator;
using CarRentalSystem.Domain.Entities;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CarRentalSystem.Tests.UnitTests.Handlers.Bookings.Commands
{
    public class DeleteBookingCommandHandlerTests
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ICarService _carService;
        private readonly IBookingValidator _bookingValidator;
        private readonly ILogger<DeleteBookingCommandHandler> _logger;
        private readonly DeleteBookingCommandHandler _handler;
        public DeleteBookingCommandHandlerTests()
        {
            _bookingRepository = A.Fake<IBookingRepository>();
            _carService = A.Fake<ICarService>();
            _bookingValidator = A.Fake<IBookingValidator>();
            _logger = A.Fake<ILogger<DeleteBookingCommandHandler>>();
            _handler = new DeleteBookingCommandHandler(_bookingRepository, _logger, _carService, _bookingValidator);
        }

        [Fact]
        public async Task Handle_ShouldDeleteBooking_WhenUserIsOwner()
        {
            // Arrange
            int bookingId = 1;
            string userId = "user1";
            bool isAdmin = false;

            var car = new Car { Id = 1, Name = "TestCar", PricePerHour = 10 };
            var booking = new Booking
            {
                CarId = 1,
                Car = car,
                UserId = userId,
                StartDate = DateTime.UtcNow.AddHours(1),
                EndDate = DateTime.UtcNow.AddHours(2),
                TotalPrice = 10
            };

            A.CallTo(() => _bookingValidator.ValidateBookingAndUserExistsAsync(bookingId, userId, isAdmin, "delete", A<CancellationToken>._))
                .Returns(Task.FromResult(booking));

            var command = new DeleteBookingCommand(bookingId, userId, isAdmin);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            A.CallTo(() => _bookingRepository.DeleteAsync(booking, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _carService.UpdateCarStatusAsync(car, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenBookingDoesNotExist()
        {
            // Arrange
            int bookingId = 1;
            string userId = "user1";
            bool isAdmin = false;

            A.CallTo(() => _bookingValidator.ValidateBookingAndUserExistsAsync(bookingId, userId, isAdmin, "delete", A<CancellationToken>._))
                .ThrowsAsync(new NotFoundException("Booking not found"));

            var command = new DeleteBookingCommand(bookingId, userId, isAdmin);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));

            A.CallTo(() => _bookingRepository.DeleteAsync(A<Booking>._, A<CancellationToken>._)).MustNotHaveHappened();
            A.CallTo(() => _carService.UpdateCarStatusAsync(A<Car>._, A<CancellationToken>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenUserNotOwner()
        {
            // Arrange
            int bookingId = 1;
            string userId = "user1";
            bool isAdmin = false;

            A.CallTo(() => _bookingValidator.ValidateBookingAndUserExistsAsync(bookingId, userId, isAdmin, "delete", A<CancellationToken>._))
                .ThrowsAsync(new UnauthorizedAccessException("User not authorized to delete this booking"));

            var command = new DeleteBookingCommand(bookingId, userId, isAdmin);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, CancellationToken.None));

            A.CallTo(() => _bookingRepository.DeleteAsync(A<Booking>._, A<CancellationToken>._)).MustNotHaveHappened();
            A.CallTo(() => _carService.UpdateCarStatusAsync(A<Car>._, A<CancellationToken>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task Handle_ShouldDeleteBooking_WhenAdminDeletesAnyBooking()
        {
            // Arrange
            int bookingId = 1;
            string adminId = "admin";
            bool isAdmin = true;

            var car = new Car { Id = 1, Name = "TestCar", PricePerHour = 10 };
            var booking = new Booking
            {
                CarId = 1,
                Car = car,
                UserId = "user1",
                StartDate = DateTime.UtcNow.AddHours(1),
                EndDate = DateTime.UtcNow.AddHours(2),
                TotalPrice = 10
            };

            A.CallTo(() => _bookingValidator.ValidateBookingAndUserExistsAsync(bookingId, adminId, isAdmin, "delete", A<CancellationToken>._))
                .Returns(Task.FromResult(booking));

            var command = new DeleteBookingCommand(bookingId, adminId, isAdmin);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            A.CallTo(() => _bookingRepository.DeleteAsync(booking, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _carService.UpdateCarStatusAsync(car, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        }
    }
}