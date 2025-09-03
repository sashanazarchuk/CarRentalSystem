using AutoMapper;
using CarRentalSystem.Application.DTOs.Car;
using CarRentalSystem.Application.DTOs.Cars;
using CarRentalSystem.Application.Exceptions;
using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Interfaces.Validator;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Commands.Cars.PatchCar
{
    internal class PatchCarCommandHandler : IRequestHandler<PatchCarCommand, CarDto>
    {
        private readonly ICarRepository _carRepository;
        private readonly ICarValidator _carValidator;
        private readonly IMapper _mapper;
        private readonly ILogger<PatchCarCommandHandler> _logger;

        public PatchCarCommandHandler(ICarRepository carRepository, ICarValidator carValidator, IMapper mapper, ILogger<PatchCarCommandHandler> logger)
        {
            _carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
            _carValidator = carValidator ?? throw new ArgumentNullException(nameof(carValidator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger;
        }
        public async Task<CarDto> Handle(PatchCarCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting PATCH for CarId {CarId}", request.CarId);

            var car = await _carValidator.ValidateCarExistsAsync(request.CarId, cancellationToken);

            var carDto = _mapper.Map<PatchCarDto>(car);

           _logger.LogInformation("Applying patch document to CarId {CarId}", request.CarId);

            request.PatchDoc.ApplyTo(carDto);
            _mapper.Map(carDto, car);
            
            await _carRepository.PatchAsync(car, cancellationToken);
            
            _logger.LogInformation("Car updated in repository successfully");
            return _mapper.Map<CarDto>(car);
        }
    }
}