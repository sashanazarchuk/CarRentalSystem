using CarRentalSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Interfaces.Validator
{
    public interface ICarValidator
    {
        Task<Car> ValidateCarExistsAsync(int carId, CancellationToken token);
        Task<CarModel> ValidateCarModelExistsAsync(int modelId, CancellationToken token);
    }
}
