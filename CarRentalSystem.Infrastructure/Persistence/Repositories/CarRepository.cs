using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Infrastructure.Persistence.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly CarRentDbContext _context;
        public CarRepository(CarRentDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<Car> CreateAsync(Car car, CancellationToken token)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync(token);
            return car;
        }
        public async Task PatchAsync(Car car, CancellationToken token)
        {
            _context.Cars.Update(car);
            await _context.SaveChangesAsync(token);
        }

        public async Task DeleteAsync(Car car, CancellationToken token)
        {
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync(token);
        }

        public async Task<IEnumerable<Car>> GetAllAsync(CancellationToken token)
        {
            var cars = await _context.Cars
                .Include(c => c.Model)
                .ThenInclude(m => m.Brand)
                .AsSplitQuery()
                .AsNoTracking()
                .ToListAsync(token);
            return cars;
        }

        public async Task<IEnumerable<Car>> GetAvailableCarsAsync(CancellationToken token)
        {
            var now = DateTime.UtcNow;

            var cars = await _context.Cars
                .Include(c => c.Model)
                .ThenInclude(m => m.Brand)
                .Where(c => c.Status != CarStatus.Rented && c.Status != CarStatus.Maintenance &&
                    !c.Bookings.Any(b => b.StartDate <= now && b.EndDate >= now))
                .AsSplitQuery()
                .AsNoTracking()
                .ToListAsync(token);

            return cars;
        }

        public async Task<Car?> GetByIdAsync(int id, CancellationToken token)
        {
            var car = await _context.Cars
                .Include(c => c.Model)
                .ThenInclude(m => m.Brand)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, token);

            return car;
        }

        public async Task<bool> IsOverlappingCarAsync(int carId, int? bookingId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
        {
            return await _context.Bookings
                .AnyAsync(b =>
                    b.CarId == carId &&
                    (!bookingId.HasValue || b.Id != bookingId.Value) &&
                    b.StartDate < endDate &&
                    b.EndDate > startDate,
                    cancellationToken);
        }

    }
}