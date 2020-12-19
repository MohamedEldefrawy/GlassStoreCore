namespace GlassStoreCore.BL.DTOs.WholeSaleProductsDtos
{
    public class WholeSaleProductsOrderDetailsDto
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public WholeSaleProductsDto WholeSaleProduct { get; set; }
        public WholeSaleSellingOrdersDto WholeSaleSellingOrder { get; set; }
    }
}
