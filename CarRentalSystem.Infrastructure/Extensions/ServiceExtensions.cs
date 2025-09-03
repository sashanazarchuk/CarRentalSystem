using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Interfaces.Services;
using CarRentalSystem.Application.Settings;
using CarRentalSystem.Infrastructure.Persistence.DataSeed;
using CarRentalSystem.Infrastructure.Persistence.Repositories;
using CarRentalSystem.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Database Configuration
            services.AddDatabase(configuration);

            //JWT Settings
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.AddJwtAuthentication(configuration.GetSection("JwtSettings").Get<JwtSettings>()!);

            //Identity Configuration
            services.ConfigureIdentity();
            services.AddScoped<IJwtTokenService, JwtTokenService>();

            //Admin Settings
            services.Configure<AdminSettings>(configuration.GetSection("AdminSettings"));
            services.AddScoped<AdminSeeder>();

            //Dependency Injection for application services and repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<ICarModelRepository, CarModelRepository>();
            services.AddScoped<ICarBrandRepository, CarBrandRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWorkRepository>();

            // Background Services
            services.AddHostedService<BookingStatusBackgroundService>();
            services.AddHostedService<ExpiredBookingBackgroundService>();

            return services;
        }
    }
}