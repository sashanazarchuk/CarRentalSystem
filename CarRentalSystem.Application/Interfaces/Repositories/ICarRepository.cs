using CarRentalSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Interfaces.Repositories
{
    public interface ICarRepository:IGenericRepository<Car>
    {
        Task<IEnumerable<Car>> GetAvailableCarsAsync(CancellationToken token);
        Task<bool> IsOverlappingCarAsync(int carId, int? bookingId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
    }
}