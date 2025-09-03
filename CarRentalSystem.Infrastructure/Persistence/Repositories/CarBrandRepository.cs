using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Infrastructure.Persistence.Repositories
{
    public class CarBrandRepository : ICarBrandRepository
    {
        private readonly CarRentDbContext _context;
        public CarBrandRepository(CarRentDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<CarBrand> CreateAsync(CarBrand carBrand, CancellationToken token)
        {
            _context.CarBrands.Add(carBrand);
            await _context.SaveChangesAsync(token);
            return carBrand;
        }
        public async Task PatchAsync(CarBrand carBrand, CancellationToken token)
        {
            _context.CarBrands.Update(carBrand);
            await _context.SaveChangesAsync(token);
        }

        public async Task DeleteAsync(CarBrand carBrand, CancellationToken token)
        {
           _context.CarBrands.Remove(carBrand);
            await _context.SaveChangesAsync(token); 
        }

        public async Task<IEnumerable<CarBrand>> GetAllAsync(CancellationToken token)
        {
          return  await _context.CarBrands.Include(c=>c.CarModels)
                .AsNoTracking()
                .ToListAsync(token);
        }

        public async Task<CarBrand?> GetByIdAsync(int id, CancellationToken token)
        {
           return await _context.CarBrands.Include(c=>c.CarModels)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id, token);
        }

        public async Task<bool> ExistsByNameAsync(string brandName, CancellationToken token)
        {
            return await _context.CarBrands
                .AsNoTracking()
                .AnyAsync(m => m.BrandName == brandName, token);
        }

        public async Task<bool> HasCarModelsAsync(int brandId, CancellationToken token)
        {
            return await _context.CarModels.AnyAsync(m => m.BrandId == brandId, token);
        }
    }
}