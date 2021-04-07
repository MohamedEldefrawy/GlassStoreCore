using System.Collections;

namespace GlassStoreCore.BL.DTOs.WholeSaleProductsDtos
{
    public class WholeSaleProductsOrderDetailsDto
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string WholeSaleProductId { get; set; }
        public int WholeSaleSellingOrderId { get; set; }
    }
}
