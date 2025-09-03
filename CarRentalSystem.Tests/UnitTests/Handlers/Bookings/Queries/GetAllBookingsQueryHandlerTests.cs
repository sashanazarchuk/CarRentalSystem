using AutoMapper;
using CarRentalSystem.Application.DTOs.Bookings;
using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Queries.Bookings.GetAllBookings;
using CarRentalSystem.Domain.Entities;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Tests.UnitTests.Handlers.Bookings.Queries
{
    public class GetAllBookingsQueryHandlerTests
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;
        private readonly GetAllBookingsQueryHandler _handler;

        public GetAllBookingsQueryHandlerTests()
        {
            _bookingRepository = A.Fake<IBookingRepository>();
            _mapper = A.Fake<IMapper>();
            _handler = new GetAllBookingsQueryHandler(_bookingRepository, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnAllBookings_ForAdminUser()
        {
            // Arrange
            var bookings = new List<Booking>
            {
                new Booking { Id = 1, UserId = "user1" },
                new Booking { Id = 2, UserId = "user2" }
            };
            A.CallTo(() => _bookingRepository.GetAllAsync(A<CancellationToken>._))
                .Returns(Task.FromResult((IEnumerable<Booking>)bookings));
            var query = new GetAllBookingsQuery(isAdmin: true, UserId: "admin");

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);
           
            // Assert
            A.CallTo(() => _bookingRepository.GetAllAsync(A<CancellationToken>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _mapper.Map<IEnumerable<BookingDto>>(bookings)).MustHaveHappenedOnceExactly();
        }


        [Fact]
        public async Task Handle_ShouldReturnUserBookings_ForNonAdminUser()
        {
            // Arrange
            var bookings = new List<Booking>
            {
                new Booking { Id = 1, UserId = "user1" },
                new Booking { Id = 2, UserId = "user2" }
            };
            A.CallTo(() => _bookingRepository.GetAllAsync(A<CancellationToken>._))
                .Returns(Task.FromResult((IEnumerable<Booking>)bookings));
            var query = new GetAllBookingsQuery(isAdmin: false, UserId: "user1");
           
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);
            
            // Assert
            A.CallTo(() => _bookingRepository.GetAllAsync(A<CancellationToken>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _mapper.Map<IEnumerable<BookingDto>>(A<IEnumerable<Booking>>.That.Matches(b => b.Count() == 1 && b.All(bo => bo.UserId == "user1"))))
                .MustHaveHappenedOnceExactly();
        }


        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoBookingsExist()
        {
            // Arrange
            var bookings = new List<Booking>();
            A.CallTo(() => _bookingRepository.GetAllAsync(A<CancellationToken>._))
                .Returns(Task.FromResult((IEnumerable<Booking>)bookings));
            var query = new GetAllBookingsQuery(isAdmin: true, UserId: "admin");
            
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);
            
            // Assert
            A.CallTo(() => _bookingRepository.GetAllAsync(A<CancellationToken>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _mapper.Map<IEnumerable<BookingDto>>(bookings)).MustHaveHappenedOnceExactly();
        }
    }
}