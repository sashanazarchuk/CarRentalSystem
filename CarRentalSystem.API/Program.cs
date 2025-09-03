using CarRentalSystem.API.Extensions;
using CarRentalSystem.API.Middlewares;
using CarRentalSystem.Application.Extensions;
using CarRentalSystem.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add NLog logging services
builder.AddLoggingServices();

// Add services to the container.
builder.Services.AddApiServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{      
     app.UseSwaggerDocumentation();
}

//Seed Admin
await app.SeedAllAsync();

// Use custom exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.RunWithNLog();