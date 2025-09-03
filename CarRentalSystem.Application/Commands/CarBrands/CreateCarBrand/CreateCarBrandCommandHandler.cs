using AutoMapper;
using CarRentalSystem.Application.DTOs.CarBrands;
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

namespace CarRentalSystem.Application.Commands.CarBrands.CreateCarBrand
{
    internal class CreateCarBrandCommandHandler : IRequestHandler<CreateCarBrandCommand, CreateCarBrandDto>
    {
        private readonly ICarBrandRepository _carBrandRepository;
        private readonly ICarBrandValidator _carBrandValidator;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCarBrandCommandHandler> _logger;
        public CreateCarBrandCommandHandler(ICarBrandRepository carBrandRepository, ICarBrandValidator carBrandValidator, IMapper mapper, ILogger<CreateCarBrandCommandHandler> logger)
        {
            _carBrandRepository = carBrandRepository ?? throw new ArgumentNullException(nameof(carBrandRepository));
            _carBrandValidator = carBrandValidator ?? throw new ArgumentNullException(nameof(carBrandValidator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger;
        }
        public async Task<CreateCarBrandDto> Handle(CreateCarBrandCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting creation of new CarBrand with Name {BrandName}", request.BrandName);

            await _carBrandValidator.ValidateBrandIsUniqueAsync(request.BrandName, cancellationToken);

            var carBrand = new CarBrand { BrandName = request.BrandName };

            var createdCarBrand = await _carBrandRepository.CreateAsync(carBrand, cancellationToken);

            _logger.LogInformation("CarBrand with Name {BrandName} created successfully", request.BrandName);
            return _mapper.Map<CreateCarBrandDto>(createdCarBrand);
        }
    }
}
