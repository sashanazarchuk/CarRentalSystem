using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CarRentalSystem.Infrastructure.Services
{
    public class ExpiredBookingBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1);
        private readonly ILogger<ExpiredBookingBackgroundService> _logger;

        public ExpiredBookingBackgroundService(IServiceScopeFactory scopeFactory, ILogger<ExpiredBookingBackgroundService> logger )
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Expired Booking Background Service is starting.");
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var bookingRepo = scope.ServiceProvider.GetRequiredService<IBookingRepository>();
                var dbContext = scope.ServiceProvider.GetRequiredService<CarRentDbContext>();

                var now = DateTime.UtcNow;
                var expiredBookings = await bookingRepo.GetExpiredBookingsAsync(now, stoppingToken);

                _logger.LogInformation("Found {Count} expired bookings to remove.", expiredBookings.Count);
                dbContext.Bookings.RemoveRange(expiredBookings);

                await dbContext.SaveChangesAsync(stoppingToken);
                _logger.LogInformation("Expired bookings removed successfully.");

                await Task.Delay(_checkInterval, stoppingToken);
            }
        }
    }
}