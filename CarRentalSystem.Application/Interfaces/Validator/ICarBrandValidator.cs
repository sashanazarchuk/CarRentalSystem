using CarRentalSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Interfaces.Validator
{
    public interface ICarBrandValidator
    {
        Task<CarBrand> ValidateBrandExistsAsync(int brandId, CancellationToken token);
        Task ValidateBrandIsUniqueAsync(string brandName, CancellationToken token);
        Task ValidateNoExistingCarModelsAsync(int brandId, CancellationToken token);

    }
}
