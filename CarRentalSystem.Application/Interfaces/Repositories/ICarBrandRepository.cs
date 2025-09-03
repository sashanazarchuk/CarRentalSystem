using CarRentalSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Interfaces.Repositories
{
    public interface ICarBrandRepository : IGenericRepository<CarBrand>
    {
        Task<bool> ExistsByNameAsync(string brandName, CancellationToken token);
        Task<bool> HasCarModelsAsync(int brandId, CancellationToken token);
    }
}