using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Domain.Entities
{
    public class CarModel: BaseEntity
    {
        public string ModelName { get; set; } = string.Empty;

        public int BrandId { get; set; }
        public CarBrand Brand { get; set; } = null!;

        public List<Car> Cars { get; set; } = new List<Car>();
    }
}