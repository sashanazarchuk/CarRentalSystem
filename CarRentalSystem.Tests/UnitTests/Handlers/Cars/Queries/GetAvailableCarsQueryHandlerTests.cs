using AutoMapper;
using CarRentalSystem.Application.DTOs.Car;
using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Queries.Cars.GetAvailableCars;
using CarRentalSystem.Domain.Entities;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CarRentalSystem.Tests.UnitTests.Handlers.Cars.Queries
{
    public class GetAvailableCarsQueryHandlerTests
    {
        private readonly ICarRepository _carRepository;
        private readonly IMapper _mapper;
        private readonly GetAvailableCarsQueryHandler _handler;

        public GetAvailableCarsQueryHandlerTests()
        {
            _carRepository = A.Fake<ICarRepository>();
            _mapper = A.Fake<IMapper>();
            _handler = new GetAvailableCarsQueryHandler(_carRepository, _mapper);
        }

        
        [Fact]
        public async Task Handle_ReturnsAvailableCarsList()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            
            var availableCars = new List<Car>
            {
                new Car { Id = 1, Name = "Car1" },
                new Car { Id = 2, Name = "Car2" }
            };

            var carDtos = new List<CarDto>
            {
                new CarDto { Id = 1, Name = "Car1" },
                new CarDto { Id = 2, Name = "Car2" }
            };

            A.CallTo(() => _carRepository.GetAvailableCarsAsync(cancellationToken)).Returns(availableCars);
            A.CallTo(() => _mapper.Map<List<CarDto>>(availableCars)).Returns(carDtos);

            var query = new GetAvailableCarsQuery();

            // Act
            var result = await _handler.Handle(query, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.Name == "Car1");
            Assert.Contains(result, c => c.Name == "Car2");

            A.CallTo(() => _carRepository.GetAvailableCarsAsync(A<CancellationToken>._))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _mapper.Map<List<CarDto>>(availableCars))
                .MustHaveHappenedOnceExactly();
        }

        
        [Fact]
        public async Task Handle_ReturnsEmptyList_WhenNoAvailableCars()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            
            var availableCars = new List<Car>();
            A.CallTo(() => _carRepository.GetAvailableCarsAsync(cancellationToken)).Returns(availableCars);
            A.CallTo(() => _mapper.Map<List<CarDto>>(availableCars)).Returns(new List<CarDto>());
            var query = new GetAvailableCarsQuery();
            
            // Act
            var result = await _handler.Handle(query, cancellationToken);
            
            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            A.CallTo(() => _carRepository.GetAvailableCarsAsync(A<CancellationToken>._))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _mapper.Map<List<CarDto>>(availableCars))
                .MustHaveHappenedOnceExactly();
        }
    }
}
