using AutoMapper;
using CarRentalSystem.Application.DTOs.Cars;
using CarRentalSystem.Application.Exceptions;
using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Interfaces.Validator;
using CarRentalSystem.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Commands.Cars.CreateCar
{
    internal class CreateCarCommandHandler : IRequestHandler<CreateCarCommand, CreateCarDto>
    {
        private readonly ICarRepository _carRepository;
        private readonly ICarValidator _carValidator;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCarCommandHandler> _logger;
        public CreateCarCommandHandler(ICarRepository carRepository,  ICarValidator carValidator, IMapper mapper, ILogger<CreateCarCommandHandler> logger)
        {
            _carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
            _carValidator = carValidator ?? throw new ArgumentNullException(nameof(carValidator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger;
        }

        public async Task<CreateCarDto> Handle(CreateCarCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating car with Name={Name}, ModelId={ModelId}", request.Name, request.ModelId);
            await _carValidator.ValidateCarModelExistsAsync(request.ModelId, cancellationToken);

            var car = new Car
            {
                Name = request.Name,
                ModelId = request.ModelId,
                FuelType = request.FuelType,
                PricePerHour = request.PricePerHour,
                ReleaseYear = request.ReleaseYear,
            };

            var createdCar = await _carRepository.CreateAsync(car, cancellationToken);

            _logger.LogInformation("Car with Name={Name}, ModelId={ModelId} created successfully", request.Name, request.ModelId);

            return _mapper.Map<CreateCarDto>(createdCar);
        }
    }
}
