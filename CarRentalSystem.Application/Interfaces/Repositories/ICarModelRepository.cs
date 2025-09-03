using CarRentalSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Interfaces.Repositories
{
    public interface ICarModelRepository: IGenericRepository<CarModel>
    {
        Task<CarModel?> GetByIdWithBrandAsync(int modelId, CancellationToken token);
        Task<bool> ExistsByNameAsync(string modelName, int brandId, CancellationToken token);
    }
}