using AutoMapper;
using CarRentalSystem.Application.Commands.Cars.CreateCar;
using CarRentalSystem.Application.DTOs.Cars;
using CarRentalSystem.Application.Exceptions;
using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Interfaces.Validator;
using CarRentalSystem.Domain.Entities;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Tests.UnitTests.Handlers.Cars.Commands
{
    public class CreateCarCommandHandlerTests
    {
        private readonly ICarRepository _carRepository;
        private readonly ICarValidator _carValidator;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCarCommandHandler> _logger;
        private readonly CreateCarCommandHandler _handler;

        public CreateCarCommandHandlerTests()
        {
            _carRepository = A.Fake<ICarRepository>();
      
            _mapper = A.Fake<IMapper>();
            _carValidator = A.Fake<ICarValidator>();
            _logger = A.Fake<ILogger<CreateCarCommandHandler>>();
            _handler = new CreateCarCommandHandler(_carRepository, _carValidator, _mapper, _logger);
        }


        [Fact]
        public async Task Handle_ShouldCreateCar_WhenModelExists()
        {
            // Arrange
            var command = new CreateCarCommand
            {
                Name = "Test Car",
                ModelId = 1,
                FuelType = 0,
                PricePerHour = 10.0m,
                ReleaseYear = 2020
            };

            var carModel = new CarModel { Id = 1, ModelName = "Test Model" };
            var createdCar = new Car { Id = 1, Name = command.Name, ModelId = command.ModelId, FuelType = command.FuelType, PricePerHour = command.PricePerHour, ReleaseYear = command.ReleaseYear };
            var createCarDto = new CreateCarDto { Name = createdCar.Name };

            A.CallTo(() => _carValidator.ValidateCarModelExistsAsync(command.ModelId, A<CancellationToken>._))
                .Returns(carModel);
            A.CallTo(() => _carRepository.CreateAsync(A<Car>._, A<CancellationToken>._))
                .Returns(createdCar);
            A.CallTo(() => _mapper.Map<CreateCarDto>(createdCar))
                .Returns(createCarDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createCarDto.Name, result.Name);

            A.CallTo(() => _carValidator.ValidateCarModelExistsAsync(command.ModelId, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _carRepository.CreateAsync(A<Car>._, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _mapper.Map<CreateCarDto>(createdCar)).MustHaveHappenedOnceExactly();
        }


        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenModelDoesNotExist()
        {
            // Arrange
            A.CallTo(() => _carValidator.ValidateCarModelExistsAsync(A<int>._, A<CancellationToken>._))
                .ThrowsAsync(new NotFoundException("Car model not found"));

            var command = new CreateCarCommand { Name = "Car", ModelId = 1 };

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));

            A.CallTo(() => _carValidator.ValidateCarModelExistsAsync(A<int>._, A<CancellationToken>._))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _carRepository.CreateAsync(A<Car>._, A<CancellationToken>._)).MustNotHaveHappened();
        }
    }
}