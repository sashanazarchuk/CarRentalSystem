using CarRentalSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Interfaces.Repositories
{
    public interface IBookingRepository:IGenericRepository<Booking>
    {
        Task<List<Booking>> GetExpiredBookingsAsync(DateTime now, CancellationToken cancellationToken);
        Task<bool> IsOverlappingBookingAsync(string userId, int? bookingId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
        Task<bool> IsCarBookedAsync(int carId, CancellationToken cancellationToken);
        Task<List<Booking>> GetActiveOrExpiredBookingsAsync(DateTime now, CancellationToken token);
        Task<bool> ExistsByCarModelIdAsync(int carModelId, CancellationToken token);
    }
}