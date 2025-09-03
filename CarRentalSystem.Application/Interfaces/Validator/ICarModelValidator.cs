using CarRentalSystem.Application.DTOs.CarModels;
using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Interfaces.Validator
{
    public interface ICarModelValidator
    {
        Task<CarModel> ValidateCarModelExistsAsync(int carModelId, CancellationToken token);
        Task ValidateModelIsUniqueAsync(string modelName, int brandId, CancellationToken token);
        Task ValidateNoExistingBookingAsync(int carModelId, CancellationToken token);
    }
}
