using CarRentalSystem.Application.Exceptions;
using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Interfaces.Validator;
using CarRentalSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Validators.Cars
{
    public class CarBrandValidator:ICarBrandValidator
    {
        private readonly ICarBrandRepository _carBrandRepository;
        public CarBrandValidator(ICarBrandRepository carBrandRepository)
        {
            _carBrandRepository = carBrandRepository ?? throw new ArgumentNullException(nameof(carBrandRepository));
        }

        public async Task<CarBrand> ValidateBrandExistsAsync(int brandId, CancellationToken token)
        {
            var brand = await _carBrandRepository.GetByIdAsync(brandId, token);
            if (brand is null)
                throw new NotFoundException($"Car brand with {brandId} does not exist");

            return brand;
        }

        public async Task ValidateBrandIsUniqueAsync(string brandName, CancellationToken token)
        {
            var exists = await _carBrandRepository.ExistsByNameAsync(brandName, token);
            if (exists)
                throw new BadRequestException($"Car brand '{brandName}' already exists.");
        }

        public async Task ValidateNoExistingCarModelsAsync (int brandId, CancellationToken token)
        {
            var hasModels = await _carBrandRepository.HasCarModelsAsync(brandId, token);
            if (hasModels)
                throw new BadRequestException("Cannot delete CarBrand because there are existing CarModels linked to this brand.");
        }
    }
}
