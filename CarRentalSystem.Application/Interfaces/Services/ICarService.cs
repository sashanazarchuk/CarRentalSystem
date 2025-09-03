using CarRentalSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Interfaces.Services
{
    public interface ICarService
    {
        Task UpdateCarStatusAsync(Car car, CancellationToken cancellationToken);
        Task<decimal> CalculateBookingPriceAsync(Car car, DateTime start, DateTime end);
    }
}
