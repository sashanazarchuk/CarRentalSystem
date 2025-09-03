using AutoMapper;
using CarRentalSystem.Application.Commands.Cars.PatchCar;
using CarRentalSystem.Application.DTOs.Car;
using CarRentalSystem.Application.DTOs.Cars;
using CarRentalSystem.Application.Exceptions;
using CarRentalSystem.Application.Interfaces.Repositories;
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

namespace CarRentalSystem.Tests.UnitTests.Handlers.Cars.Commands
{
    public class PatchCarCommandHandlerTests
    {
        private readonly ICarRepository _carRepository;
        private readonly ICarValidator _carValidator;
        private readonly IMapper _mapper;
        private readonly ILogger<PatchCarCommandHandler> _logger;
        private readonly PatchCarCommandHandler _handler;

        public PatchCarCommandHandlerTests()
        {
            _carRepository = A.Fake<ICarRepository>();
            _carValidator = A.Fake<ICarValidator>();
            _mapper = A.Fake<IMapper>();
            _logger = A.Fake<ILogger<PatchCarCommandHandler>>();
            _handler = new PatchCarCommandHandler(_carRepository, _carValidator, _mapper, _logger);
        }


        [Fact]
        public async Task Handle_ShouldPatchCar_WhenCarExists()
        {
            // Arrange
            var carId = 1;
            var patchDoc = new JsonPatchDocument<PatchCarDto>();
            patchDoc.Replace(c => c.Name, "Updated Name");

            var car = new Car { Id = carId, Name = "Old Name" };
            var carDto = new PatchCarDto { Name = "Old Name" };
            var updatedCarDto = new CarDto { Id = carId, Name = "Updated Name" };

            A.CallTo(() => _carValidator.ValidateCarExistsAsync(carId, A<CancellationToken>._))
                .Returns(car);
            A.CallTo(() => _mapper.Map<PatchCarDto>(car))
                .Returns(carDto);
            A.CallTo(() => _mapper.Map(carDto, car))
                .Invokes(() => car.Name = "Updated Name");
            A.CallTo(() => _mapper.Map<CarDto>(car))
                .Returns(updatedCarDto);

            var command = new PatchCarCommand(carId, patchDoc);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Name", result.Name);

            A.CallTo(() => _carValidator.ValidateCarExistsAsync(carId, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _carRepository.PatchAsync(car, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _mapper.Map<CarDto>(car)).MustHaveHappenedOnceExactly();
        }

        
        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenCarDoesNotExist()
        {
            // Arrange
            var carId = 99;
            var patchDoc = new JsonPatchDocument<PatchCarDto>();
            var command = new PatchCarCommand(carId, patchDoc);

            A.CallTo(() => _carValidator.ValidateCarExistsAsync(carId, A<CancellationToken>._))
                   .ThrowsAsync(new NotFoundException($"Car with ID {carId} not found"));

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));

             A.CallTo(() => _carRepository.PatchAsync(A<Car>._, A<CancellationToken>._)).MustNotHaveHappened();
        }
    }
}