using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Infrastructure.Persistence.Repositories
{
    public class CarModelRepository : ICarModelRepository
    {
        private readonly CarRentDbContext _context;
        public CarModelRepository(CarRentDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<CarModel> CreateAsync(CarModel carModel, CancellationToken token)
        {
            _context.CarModels.Add(carModel);
            await _context.SaveChangesAsync(token);
            return carModel;
        }
        public async Task PatchAsync(CarModel carModel, CancellationToken token)
        {
            _context.CarModels.Update(carModel);
            await _context.SaveChangesAsync(token);
        }

        public async Task DeleteAsync(CarModel carModel, CancellationToken token)
        {
            _context.CarModels.Remove(carModel);
            await _context.SaveChangesAsync(token);
        }

        public async Task<IEnumerable<CarModel>> GetAllAsync(CancellationToken token)
        {
           return await _context.CarModels
                .Include(m => m.Brand)
                .AsNoTracking()
                .ToListAsync(token);
        }

        public async Task<CarModel?> GetByIdAsync(int id, CancellationToken token)
        {
            return await _context.CarModels.Include(c=>c.Brand)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id, token);
        }

        public async Task<CarModel?> GetByIdWithBrandAsync(int modelId, CancellationToken token)
        {
            return await _context.CarModels
                .Include(m => m.Brand)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == modelId, token);
        }

        public async Task<bool> ExistsByNameAsync(string modelName, int brandId, CancellationToken token)
        {
            return await _context.CarModels
                .AsNoTracking()
                .AnyAsync(m => m.ModelName == modelName && m.BrandId == brandId, token);
        }
    }
}