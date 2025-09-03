using CarRentalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Infrastructure.Persistence.EntityConfiguration
{
    internal class CarModelConfiguration : IEntityTypeConfiguration<CarModel>
    {
        public void Configure(EntityTypeBuilder<CarModel> builder)
        {
            builder.HasKey(m => m.Id);

            
            builder.HasOne(m => m.Brand)
                   .WithMany(b => b.CarModels)
                   .HasForeignKey(m => m.BrandId)
                   .OnDelete(DeleteBehavior.Restrict);

            
            builder.HasMany(m => m.Cars)
                   .WithOne(c => c.Model)
                   .HasForeignKey(c => c.ModelId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}