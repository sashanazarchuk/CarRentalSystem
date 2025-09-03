using AutoMapper;
using CarRentalSystem.Application.DTOs.Bookings;
using CarRentalSystem.Application.DTOs.Car;
using CarRentalSystem.Application.Exceptions;
using CarRentalSystem.Application.Interfaces.Validator;
using CarRentalSystem.Application.Queries.Bookings.GetBookingById;
using CarRentalSystem.Domain.Entities;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Tests.UnitTests.Handlers.Bookings.Queries
{
    public class GetBookingByIdQueryHandlerTests
    {
        private readonly IBookingValidator _bookingValidator;
        private readonly IMapper _mapper;
        private readonly ILogger<GetBookingByIdQueryHandler> _logger;

        public GetBookingByIdQueryHandlerTests()
        {
            _bookingValidator = A.Fake<IBookingValidator>();
            _mapper = A.Fake<IMapper>();
            _logger = A.Fake<ILogger<GetBookingByIdQueryHandler>>();
        }

        [Fact]
        public async Task Handle_ShouldReturnBookingDto_WhenBookingExists()
        {
            // Arrange
            var bookingId = 1;
            var userId = "user1";
            bool isAdmin = false;
           
            var booking = new Booking
            {
                Id = bookingId,
                UserId = userId,
                CarId = 2,
                Car = new Car { Id = 2, Name = "TestCar", PricePerHour = 10 },
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                TotalPrice = 10
            };

            var bookingDto = new BookingDto
            {
                Id = bookingId,
                UserId = userId,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                TotalPrice = 10,
                Car = new CarDto { Id = 2, Name = "TestCar", PricePerHour = 10 }
            };

            A.CallTo(() => _bookingValidator.ValidateBookingAndUserExistsAsync(bookingId, userId, isAdmin, "view", A<CancellationToken>._))
                .Returns(Task.FromResult(booking));
            
            A.CallTo(() => _mapper.Map<BookingDto>(booking)).Returns(bookingDto);
            var handler = new GetBookingByIdQueryHandler(_mapper, _logger, _bookingValidator);
            var query = new GetBookingByIdQuery(bookingId, userId, isAdmin);
            
            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(bookingDto.Id, result.Id);
            Assert.Equal(bookingDto.UserId, result.UserId);
            Assert.Equal(bookingDto.Car.Name, result.Car.Name);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenBookingDoesNotExist()
        {
            // Arrange
            var bookingId = 1;
            var userId = "user1";
            bool isAdmin = false;
            A.CallTo(() => _bookingValidator.ValidateBookingAndUserExistsAsync(bookingId, userId, isAdmin, "view", A<CancellationToken>._))
                .ThrowsAsync(new NotFoundException("Booking not found"));
            var handler = new GetBookingByIdQueryHandler(_mapper, _logger, _bookingValidator);
            var query = new GetBookingByIdQuery(bookingId, userId, isAdmin);
            
            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }


        [Fact]
        public async Task Handle_ShouldReturnBookingDto_WhenAdminAccessesOtherUsersBooking()
        {
            // Arrange
            var bookingId = 1;
            var adminId = "admin";
            bool isAdmin = true;

            var booking = new Booking
            {
                Id = bookingId,
                UserId = "user1",  
                CarId = 2,
                Car = new Car { Id = 2, Name = "TestCar", PricePerHour = 10 },
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                TotalPrice = 10
            };

            var bookingDto = new BookingDto
            {
                Id = bookingId,
                UserId = "user1",
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                TotalPrice = 10,
                Car = new CarDto { Id = 2, Name = "TestCar", PricePerHour = 10 }
            };

            A.CallTo(() => _bookingValidator.ValidateBookingAndUserExistsAsync(bookingId, adminId, isAdmin, "view", A<CancellationToken>._))
                .Returns(Task.FromResult(booking));

            A.CallTo(() => _mapper.Map<BookingDto>(booking)).Returns(bookingDto);

            var handler = new GetBookingByIdQueryHandler(_mapper, _logger, _bookingValidator);
            var query = new GetBookingByIdQuery(bookingId, adminId, isAdmin);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(bookingDto.Id, result.Id);
            Assert.Equal(bookingDto.UserId, result.UserId);
        }

    }
}