using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Domain.Enum;
using CarRentalSystem.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Infrastructure.Services
{
    public class BookingStatusBackgroundService : BackgroundService
    {
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1);

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<BookingStatusBackgroundService> _logger;

        public BookingStatusBackgroundService(IServiceScopeFactory scopeFactory, ILogger<BookingStatusBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Booking Status Background Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var bookingRepository = scope.ServiceProvider.GetRequiredService<IBookingRepository>();
                    var context = scope.ServiceProvider.GetRequiredService<CarRentDbContext>();

                    var now = DateTime.UtcNow;
                    var bookings = await bookingRepository.GetActiveOrExpiredBookingsAsync(now, stoppingToken);

                    foreach (var b in bookings)
                    {
                        b.Car.Status = now >= b.StartDate && now < b.EndDate ? CarStatus.Rented :
                                       now >= b.EndDate ? CarStatus.Available : b.Car.Status;
                    }

                    await context.SaveChangesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while updating booking statuses.");
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }
        }
    }
}
