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
    public class BookingRepository : IBookingRepository
    {
        private readonly CarRentDbContext _context;
        public BookingRepository(CarRentDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Booking> CreateAsync(Booking booking, CancellationToken cancellationToken)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync(cancellationToken);
            return booking;
        }
        public async Task PatchAsync(Booking booking, CancellationToken cancellationToken)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Booking booking, CancellationToken cancellationToken)
        {
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Booking>> GetAllAsync(CancellationToken cancellationToken)
        {
            var bookings = await _context.Bookings
                .Include(c => c.Car)
                    .ThenInclude(w => w.Model)
                    .ThenInclude(w => w.Brand)
                    .Include(u => u.User)
                    .AsSplitQuery()
                    .AsNoTracking()
                .ToListAsync(cancellationToken);

            return bookings;
        }


        public async Task<Booking?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var booking = await _context.Bookings
               .Include(c => c.Car)
               .ThenInclude(m => m.Model)
               .ThenInclude(b => b.Brand)
               .Include(u => u.User)
               .AsSplitQuery()
               .AsNoTracking()
               .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

            return booking;
        }

        public async Task<List<Booking>> GetExpiredBookingsAsync(DateTime now, CancellationToken cancellationToken)
        {
            return await _context.Bookings
            .Include(b => b.Car)
            .ThenInclude(c => c.Model)
            .ThenInclude(m => m.Brand)
            .Where(b => b.EndDate <= now)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);
        }

        public async Task<bool> IsOverlappingBookingAsync(string userId, int? bookingId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
        {
            return await _context.Bookings
                .AnyAsync(b =>
                    b.UserId == userId &&
                    (!bookingId.HasValue || b.Id != bookingId.Value) &&
                    b.StartDate < endDate &&
                    b.EndDate > startDate,
                    cancellationToken);
        }


        public async Task<bool> IsCarBookedAsync(int carId, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            return await _context.Bookings
                .AnyAsync(b => b.CarId == carId && b.EndDate > now, cancellationToken);
        }

        public async Task<List<Booking>> GetActiveOrExpiredBookingsAsync(DateTime now, CancellationToken cancellationToken)
        {
            return await _context.Bookings
                .Include(b => b.Car)
                .Where(b => b.StartDate <= now || b.EndDate <= now)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsByCarModelIdAsync(int carModelId, CancellationToken token)
        {
            return await _context.Bookings
                .AnyAsync(b => b.Car != null && b.Car.ModelId == carModelId, token);
        }
    }
}