using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlassStoreCore.BL.DTOs.WholeSaleProductsDtos
{
    public class WholeSaleProductsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int UnitsInStock { get; set; }
    }
}
