using GlassStoreCore.BL.Models;

namespace GlassStoreCore.BL.DTOs.WholeSaleProductsDtos
{
    public class WholeSaleProductsOrderDetails
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public WholeSaleProductsDto WholeSaleProduct { get; set; }
        public WholeSaleSellingOrdersDto WholeSaleSellingOrder { get; set; }


    }
}
