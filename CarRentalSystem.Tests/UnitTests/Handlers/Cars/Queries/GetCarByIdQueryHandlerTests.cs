using AutoMapper;
using CarRentalSystem.Application.DTOs.Car;
using CarRentalSystem.Application.Exceptions;
using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Queries.Cars.GetCarById;
using CarRentalSystem.Domain.Entities;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Tests.UnitTests.Handlers.Cars.Queries
{
    public class GetCarByIdQueryHandlerTests
    {
        private readonly ICarRepository _carRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCarByIdQueryHandler> _logger;
        private readonly GetCarByIdQueryHandler _handler;

        public GetCarByIdQueryHandlerTests()
        {
            _carRepository = A.Fake<ICarRepository>();
            _mapper = A.Fake<IMapper>();
            _logger = A.Fake<ILogger<GetCarByIdQueryHandler>>();
            _handler = new GetCarByIdQueryHandler(_carRepository, _mapper, _logger);
        }


        [Fact]
        public async Task Handle_ReturnsCarDto_WhenCarExists()
        {
            // Arrange
            var carId = 1;
            var cancellationToken = new CancellationToken();
            var car = new Car { Id = carId, Name = "Car1" };
            var carDto = new CarDto { Id = carId, Name = "Car1" };
            A.CallTo(() => _carRepository.GetByIdAsync(carId, cancellationToken)).Returns(car);
            A.CallTo(() => _mapper.Map<CarDto>(car)).Returns(carDto);
            var query = new GetCarByIdQuery(carId);

            // Act
            var result = await _handler.Handle(query, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(carId, result.Id);
            Assert.Equal("Car1", result.Name);
        }


        [Fact]
        public async Task Handle_ReturnsNull_WhenCarDoesNotExist()
        {
            // Arrange
            var carId = 1;
            var cancellationToken = new CancellationToken();

            A.CallTo(() => _carRepository.GetByIdAsync(carId, cancellationToken))
                .Returns((Car?)null);

            var query = new GetCarByIdQuery(carId);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => _handler.Handle(query, cancellationToken));

            Assert.Equal($"Car with id {carId} not found", exception.Message);

            A.CallTo(() => _carRepository.GetByIdAsync(carId, cancellationToken))
                .MustHaveHappenedOnceExactly();
        }
    }
}