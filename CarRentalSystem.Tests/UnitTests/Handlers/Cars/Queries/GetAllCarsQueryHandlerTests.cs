using AutoMapper;
using CarRentalSystem.Application.DTOs.Car;
using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Queries.Cars.GetAllCars;
using CarRentalSystem.Domain.Entities;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Tests.UnitTests.Handlers.Cars.Queries
{
    public class GetAllCarsQueryHandlerTests
    {
        private readonly ICarRepository _carRepository;
        private readonly IMapper _mapper;
        private readonly GetAllCarsQueryHandler _handler;

        public GetAllCarsQueryHandlerTests()
        {
            _carRepository = A.Fake<ICarRepository>();
            _mapper = A.Fake<IMapper>();
            _handler = new GetAllCarsQueryHandler(_carRepository, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnAllCars()
        {
            // Arrange
            var cars = new List<Car>
            {
                new Car { Id = 1, Name = "Car1" },
                new Car { Id = 2, Name = "Car2" }
            };
            var carDtos = new List<CarDto>
            {
                new  CarDto { Id = 1, Name = "Car1" },
                new CarDto { Id = 2, Name = "Car2" }
            };
            
            A.CallTo(() => _carRepository.GetAllAsync(A<CancellationToken>._)).Returns(cars);
            A.CallTo(() => _mapper.Map<IEnumerable<CarDto>>(cars)).Returns(carDtos);
            
            var query = new GetAllCarsQuery();
           
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);
            
            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, c => c.Name == "Car1");
            Assert.Contains(result, c => c.Name == "Car2");
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoCarsExist()
        {
            // Arrange
            var cars = new List<Car>();
            var carDtos = new List<CarDto>();
            
            A.CallTo(() => _carRepository.GetAllAsync(A<CancellationToken>._)).Returns(cars);
            A.CallTo(() => _mapper.Map<IEnumerable<CarDto>>(cars)).Returns(carDtos);
            
            var query = new GetAllCarsQuery();
           
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);
            
            // Assert
            Assert.Empty(result);
        }
    }
}
