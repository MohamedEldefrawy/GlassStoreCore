using System;
namespace GlassStoreCore.BL.DTOs.WholeSaleProductsDtos
{
    public class DeleteWholeSaleSellingOrderDetail
    {
        public Guid WholeSaleProductId { get; set; }
        public int WholeSaleSellingOrderId { get; set; }
    }
}
