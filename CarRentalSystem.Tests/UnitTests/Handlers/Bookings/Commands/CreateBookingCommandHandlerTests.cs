using AutoMapper;
using CarRentalSystem.Application.Commands.Bookings.CreateBooking;
using CarRentalSystem.Application.DTOs.Bookings;
using CarRentalSystem.Application.Exceptions;
using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Interfaces.Services;
using CarRentalSystem.Application.Interfaces.Validator;
using CarRentalSystem.Domain.Entities;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Tests.UnitTests.Handlers.Bookings.Commands
{
    public class CreateBookingCommandHandlerTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateBookingCommandHandler> _logger;
        private readonly IBookingValidator _bookingValidator;
        private readonly ICarService _carService;
        private readonly CreateBookingCommandHandler _handler;

        public CreateBookingCommandHandlerTests()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            _mapper = A.Fake<IMapper>();
            _logger = A.Fake<ILogger<CreateBookingCommandHandler>>();
            _bookingValidator = A.Fake<IBookingValidator>();
            _carService = A.Fake<ICarService>();
            _handler = new CreateBookingCommandHandler(_unitOfWork, _mapper, _logger, _bookingValidator, _carService);
        }

        [Fact]
        public async Task Handle_ShouldCreateBooking_WhenValidRequest()
        {
            // Arrange
            var command = new CreateBookingCommand
            {
                CarId = 1,
                UserId = "user1",
                StartDate = DateTime.UtcNow.AddHours(1),
                EndDate = DateTime.UtcNow.AddHours(2)
            };

            var car = new Car { Id = 1, Name = "TestCar", PricePerHour = 10 };
            var booking = new Booking { CarId = 1, UserId = "user1", StartDate = command.StartDate, EndDate = command.EndDate, TotalPrice = 10 };
            var bookingDto = new CreateBookingDto { CarName = "TestCar", UserName = "user1", StartDate = command.StartDate, EndDate = command.EndDate, TotalPrice = 10 };

            A.CallTo(() => _bookingValidator.ValidateCarExistsAndAvailableAsync(command.CarId, A<CancellationToken>._)).Returns(car);
            A.CallTo(() => _bookingValidator.ValidateUserNoOverlapAsync(command.UserId, command.StartDate, command.EndDate, A<CancellationToken>._, null)).Returns(Task.CompletedTask);
            A.CallTo(() => _bookingValidator.ValidateCarNoOverlapAsync(command.CarId, command.StartDate, command.EndDate, A<CancellationToken>._, null)).Returns(Task.CompletedTask);
            A.CallTo(() => _carService.CalculateBookingPriceAsync(car, command.StartDate, command.EndDate)).Returns(10);
            A.CallTo(() => _mapper.Map<Booking>(A<object>._)).Returns(booking);
            A.CallTo(() => _mapper.Map<CreateBookingDto>(A<Booking>._)).Returns(bookingDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<CreateBookingDto>(result);
            Assert.Equal(bookingDto.TotalPrice, result.TotalPrice);
            A.CallTo(() => _unitOfWork.Bookings.CreateAsync(A<Booking>._, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _unitOfWork.SaveChangesAsync(A<CancellationToken>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _bookingValidator.ValidateCarExistsAndAvailableAsync(command.CarId, A<CancellationToken>._)).MustHaveHappened();
            
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenCarDoesNotExist()
        {
            // Arrange
            var command = new CreateBookingCommand
            {
                CarId = 99,
                UserId = "user1",
                StartDate = DateTime.UtcNow.AddHours(1),
                EndDate = DateTime.UtcNow.AddHours(2)
            };

            A.CallTo(() => _bookingValidator.ValidateCarExistsAndAvailableAsync(command.CarId, A<CancellationToken>._))
            .Throws(new NotFoundException("Car not found"));

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
            A.CallTo(() => _unitOfWork.Bookings.CreateAsync(A<Booking>._, A<CancellationToken>._))
                .MustNotHaveHappened();
            A.CallTo(() => _unitOfWork.SaveChangesAsync(A<CancellationToken>._))
                .MustNotHaveHappened();

        }

        [Fact]
        public async Task Handle_ShouldThrowForbiddenException_WhenCarAlreadyBookedInPeriod()
        {
            var command = new CreateBookingCommand
            {
                CarId = 1,
                UserId = "user1",
                StartDate = DateTime.UtcNow.AddHours(1),
                EndDate = DateTime.UtcNow.AddHours(2)
            };

            A.CallTo(() => _bookingValidator.ValidateCarNoOverlapAsync(
                command.CarId, command.StartDate, command.EndDate, A<CancellationToken>._, null))
               .Throws(new ForbiddenException("Car is already booked for these dates."));

            // Act & Assert
            await Assert.ThrowsAsync<ForbiddenException>(() => _handler.Handle(command, CancellationToken.None));

            A.CallTo(() => _unitOfWork.Bookings.CreateAsync(A<Booking>._, A<CancellationToken>._))
                .MustNotHaveHappened();
            A.CallTo(() => _unitOfWork.SaveChangesAsync(A<CancellationToken>._))
                .MustNotHaveHappened();

        }

        [Fact]
        public async Task Handle_ShouldThrowForbiddenException_WhenUserAlreadyHasBookingInPeriod()
        {
            // Arrange
            var command = new CreateBookingCommand
            {
                CarId = 1,
                UserId = "user1",
                StartDate = DateTime.UtcNow.AddHours(1),
                EndDate = DateTime.UtcNow.AddHours(2)
            };


            A.CallTo(() => _bookingValidator.ValidateUserNoOverlapAsync(
                    command.UserId, command.StartDate, command.EndDate, A<CancellationToken>._, null))
                .Throws(new ForbiddenException("User already has a booking during these dates."));

            // Act & Assert
            await Assert.ThrowsAsync<ForbiddenException>(() => _handler.Handle(command, CancellationToken.None));


            A.CallTo(() => _unitOfWork.Bookings.CreateAsync(A<Booking>._, A<CancellationToken>._))
                .MustNotHaveHappened();
            A.CallTo(() => _unitOfWork.SaveChangesAsync(A<CancellationToken>._))
                .MustNotHaveHappened();

        }
    }
}
