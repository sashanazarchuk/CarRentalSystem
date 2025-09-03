using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Infrastructure.Persistence.EntityConfiguration
{
    internal class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Status)
       .        HasDefaultValue(CarStatus.Available);

            builder.HasOne(c => c.Model)
               .WithMany(m => m.Cars)
               .HasForeignKey(c => c.ModelId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Bookings)
              .WithOne(b => b.Car)
              .HasForeignKey(b => b.CarId)
              .OnDelete(DeleteBehavior.Restrict);
        }
    }
}