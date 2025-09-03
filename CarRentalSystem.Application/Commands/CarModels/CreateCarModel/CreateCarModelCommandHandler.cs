using AutoMapper;
using CarRentalSystem.Application.DTOs.CarModels;
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

namespace CarRentalSystem.Application.Commands.CarModels.CreateCarModel
{
    internal class CreateCarModelCommandHandler : IRequestHandler<CreateCarModelCommand, CreateCarModelDto>
    {
        private readonly ICarModelRepository _carModelRepository;
        private readonly ICarModelValidator _carModelValidator;
        private readonly ICarBrandValidator _carBrandValidator;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCarModelCommandHandler> _logger;

        public CreateCarModelCommandHandler(ICarModelRepository carModelRepository, ICarModelValidator carModelValidator, ICarBrandValidator carBrandValidator, IMapper mapper, ILogger<CreateCarModelCommandHandler> logger)
        {
            _carModelRepository = carModelRepository ?? throw new ArgumentNullException(nameof(carModelRepository));
            _carModelValidator = carModelValidator ?? throw new ArgumentNullException(nameof(carModelValidator));
            _carBrandValidator = carBrandValidator ?? throw new ArgumentNullException(nameof(carBrandValidator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger;
        }

        public async Task<CreateCarModelDto> Handle(CreateCarModelCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating car model with Name={Name}, BrandId={BrandId}", request.ModelName, request.BrandId);
           
            await _carBrandValidator.ValidateBrandExistsAsync(request.BrandId, cancellationToken);
            await _carModelValidator.ValidateModelIsUniqueAsync(request.ModelName, request.BrandId, cancellationToken);

            var carModel = new CarModel
            {
                ModelName = request.ModelName,
                BrandId = request.BrandId,
            };
            var createdCarModel = await _carModelRepository.CreateAsync(carModel, cancellationToken);

            _logger.LogInformation("Car model with Name={Name}, BrandId={BrandId} created successfully", request.ModelName, request.BrandId);
           
            return _mapper.Map<CreateCarModelDto>(createdCarModel);
        }
    }
}