using CarRentalSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Interfaces.Validator
{
    public interface IBookingValidator
    {
        Task<Car> ValidateCarExistsAndAvailableAsync(int carId, CancellationToken token);
        Task ValidateUserNoOverlapAsync(string userId, DateTime start, DateTime end, CancellationToken token, int? bookingId = null);
        Task ValidateCarNoOverlapAsync(int carId, DateTime start, DateTime end, CancellationToken token, int? bookingId = null);
        void ValidateBookingDates(DateTime start, DateTime end);
        void ValidateBookingDuration(DateTime start, DateTime end, TimeSpan? minDuration = null, TimeSpan? maxDuration = null);
        Task<Booking> ValidateBookingAndUserExistsAsync(int bookingId, string userId, bool isAdmin, string action, CancellationToken token);


    }

}
