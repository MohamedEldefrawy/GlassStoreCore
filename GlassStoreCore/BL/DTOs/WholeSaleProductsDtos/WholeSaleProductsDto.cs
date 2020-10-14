using System;

namespace GlassStoreCore.BL.DTOs.WholeSaleProductsDtos
{
    public class WholeSaleProductsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public decimal Price { get; set; }
        public int UnitsInStock { get; set; }
    }
}
