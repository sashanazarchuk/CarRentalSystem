using CarRentalSystem.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Infrastructure.Persistence.Repositories
{
    public class UnitOfWorkRepository : IUnitOfWork
    {
        private readonly CarRentDbContext _context;
        public IBookingRepository Bookings { get; private set; }
        public ICarRepository Cars { get; private set; }

        public UnitOfWorkRepository(CarRentDbContext context, IBookingRepository booking, ICarRepository cars)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            Bookings = booking ?? throw new ArgumentNullException(nameof(booking));
            Cars = cars ?? throw new ArgumentNullException(nameof(cars));
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
           return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}