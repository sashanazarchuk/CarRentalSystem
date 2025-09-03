using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IBookingRepository Bookings { get; }
        ICarRepository Cars { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
