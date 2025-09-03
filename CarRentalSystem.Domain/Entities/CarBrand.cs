using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Domain.Entities
{
    public class CarBrand:BaseEntity
    {
        public string BrandName { get; set; } = string.Empty;
        public List<CarModel> CarModels { get; set; } = new List<CarModel>();
    }
}