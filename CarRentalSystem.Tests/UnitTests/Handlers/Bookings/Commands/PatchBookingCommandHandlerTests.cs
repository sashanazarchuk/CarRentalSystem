using AutoMapper;
using CarRentalSystem.Application.Commands.Bookings.PatchBooking;
using CarRentalSystem.Application.DTOs.Bookings;
using CarRentalSystem.Application.Exceptions;
using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Interfaces.Services;
using CarRentalSystem.Application.Interfaces.Validator;
using CarRentalSystem.Domain.Entities;
using FakeItEasy;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CarRentalSystem.Tests.UnitTests.Handlers.Bookings.Commands
{
    public class PatchBookingCommandHandlerTests
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IBookingValidator _bookingValidator;
        private readonly ICarService _carService;
        private readonly IMapper _mapper;
        private readonly ILogger<PatchBookingCommandHandler> _logger;
        private readonly PatchBookingCommandHandler _handler;

        public PatchBookingCommandHandlerTests()
        {
            _bookingRepository = A.Fake<IBookingRepository>();
            _bookingValidator = A.Fake<IBookingValidator>();
            _carService = A.Fake<ICarService>();
            _mapper = A.Fake<IMapper>();
            _logger = A.Fake<ILogger<PatchBookingCommandHandler>>();
            _handler = new PatchBookingCommandHandler(_bookingRepository, _mapper, _logger, _bookingValidator, _carService);
        }

        [Fact]
        public async Task Handle_ShouldPatchBooking_WhenValidRequest()
        {
            // Arrange
            var bookingId = 1;
            var userId = "user1";
            var isAdmin = false;

            var patchDoc = new JsonPatchDocument<PatchBookingDto>();
            patchDoc.Replace(b => b.StartDate, DateTime.UtcNow.AddDays(1));
            patchDoc.Replace(b => b.EndDate, DateTime.UtcNow.AddDays(2));

            var booking = new Booking
            {
                Id = bookingId,
                UserId = userId,
                Car = new Car { Id = 2, PricePerHour = 10 },
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                TotalPrice = 10
            };

            var expectedDto = new BookingDto { Id = bookingId, UserId = userId, TotalPrice = 20 };

            A.CallTo(() => _bookingValidator.ValidateBookingAndUserExistsAsync(bookingId, userId, isAdmin, "modify", A<CancellationToken>._))
                .Returns(booking);

            A.CallTo(() => _carService.CalculateBookingPriceAsync(booking.Car, A<DateTime>._, A<DateTime>._))
                .Returns(20);

            A.CallTo(() => _mapper.Map<BookingDto>(booking)).Returns(expectedDto);

            // Act
            var result = await _handler.Handle(new PatchBookingCommand(bookingId, userId, isAdmin, patchDoc), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(20, result.TotalPrice);

            A.CallTo(() => _bookingRepository.PatchAsync(booking, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        }


        [Fact]
        public async Task Handle_ShouldThrowNotFoundOrForbidden_WhenBookingDoesNotExistOrUserNoAccess()
        {
            // Arrange
            var bookingId = 1;
            var userId = "user1";
            var patchDoc = new JsonPatchDocument<PatchBookingDto>();
            bool isAdmin = false;


            A.CallTo(() => _bookingValidator.ValidateBookingAndUserExistsAsync( bookingId, userId, isAdmin, "modify", A<CancellationToken>._))
                .Throws(new NotFoundException("Booking not found"));

            var command = new PatchBookingCommand(bookingId, userId, isAdmin, patchDoc);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
 
            A.CallTo(() => _bookingRepository.PatchAsync(A<Booking>._, A<CancellationToken>._)).MustNotHaveHappened();
        }


        [Fact]
        public async Task Handle_ShouldThrowBadRequest_WhenBookingDatesAreInvalid()
        {
            // Arrange
            var bookingId = 1;
            var userId = "user1";
            bool isAdmin = false;

            var booking = new Booking { Id = bookingId, UserId = userId, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddHours(1) };
            var patchDto = new PatchBookingDto { EndDate = DateTime.UtcNow.AddHours(-1) };
            var patchDoc = new JsonPatchDocument<PatchBookingDto>();
            patchDoc.Replace(b => b.EndDate, patchDto.EndDate);

            A.CallTo(() => _bookingValidator.ValidateBookingAndUserExistsAsync(bookingId, userId, isAdmin, "modify", A<CancellationToken>._))
                .Returns(booking);

            // Ensure the patch updates the booking's EndDate to the invalid value
            A.CallTo(() => _mapper.Map(patchDto, booking)).Invokes(() => {
                if (patchDto.StartDate.HasValue) booking.StartDate = patchDto.StartDate.Value;
                if (patchDto.EndDate.HasValue) booking.EndDate = patchDto.EndDate.Value;
            });

            // Throw for any ValidateBookingDates call
            A.CallTo(() => _bookingValidator.ValidateBookingDates(A<DateTime>._, A<DateTime>._))
                .Throws(new BadRequestException("Invalid booking dates"));

            var command = new PatchBookingCommand(bookingId, userId, isAdmin, patchDoc);

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));

            A.CallTo(() => _bookingRepository.PatchAsync(A<Booking>._, A<CancellationToken>._)).MustNotHaveHappened();
        }


        [Fact]
        public async Task Handle_ShouldThrowForbidden_WhenCarAlreadyBooked()
        {
            // Arrange
            var bookingId = 1;
            var userId = "user1";
            bool isAdmin = false;

            var booking = new Booking
            {
                Id = bookingId,
                UserId = userId,
                CarId = 10,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddHours(2),
                Car = new Car { Id = 10 }
            };

            var patchDto = new PatchBookingDto
            {
                EndDate = DateTime.UtcNow.AddHours(3)
            };

            var patchDoc = new JsonPatchDocument<PatchBookingDto>();
            patchDoc.Replace(b => b.EndDate, patchDto.EndDate);

            A.CallTo(() => _bookingValidator.ValidateBookingAndUserExistsAsync(bookingId, userId, isAdmin, "modify", A<CancellationToken>._))
                .Returns(booking);

            A.CallTo(() => _mapper.Map<PatchBookingDto>(booking)).Returns(patchDto);
            A.CallTo(() => _mapper.Map(patchDto, booking)).Invokes(() => booking.EndDate = patchDto.EndDate.Value);
            A.CallTo(() => _bookingValidator.ValidateBookingDates(booking.StartDate, booking.EndDate)).DoesNothing();
            A.CallTo(() => _bookingValidator.ValidateUserNoOverlapAsync(booking.UserId, booking.StartDate, booking.EndDate, A<CancellationToken>._, bookingId))
                .Returns(Task.CompletedTask);

            // Simulate car already booked
            A.CallTo(() => _bookingValidator.ValidateCarNoOverlapAsync(booking.CarId, A<DateTime>._, A<DateTime>._, A<CancellationToken>._,bookingId))
                .Throws(new ForbiddenException("Car is already booked"));

            var command = new PatchBookingCommand(bookingId, userId, isAdmin, patchDoc);

            // Act & Assert
            await Assert.ThrowsAsync<ForbiddenException>(() => _handler.Handle(command, CancellationToken.None));

            A.CallTo(() => _bookingRepository.PatchAsync(A<Booking>._, A<CancellationToken>._)).MustNotHaveHappened();
        }
    }
}