using CarRentalSystem.Application.Interfaces.Services;
using CarRentalSystem.Application.Interfaces.Validator;
using CarRentalSystem.Application.Services;
using CarRentalSystem.Application.Validators.Account;
using CarRentalSystem.Application.Validators.Bookings;
using CarRentalSystem.Application.Validators.Cars;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;

namespace CarRentalSystem.Application.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Add MediatR and AutoMapper
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));

            //Fluent Validation    
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<RegisterUserCommandValidator>();

            // Dependency Injection for services and validators
            services.AddScoped<IBookingValidator, BookingValidator>();
            services.AddScoped<ICarModelValidator, CarModelValidator>();
            services.AddScoped<ICarBrandValidator, CarBrandValidator>();
            services.AddScoped<ICarValidator, CarValidator>();  
            services.AddScoped<ICarService, CarService>();

            return services;
        }
    }
}
