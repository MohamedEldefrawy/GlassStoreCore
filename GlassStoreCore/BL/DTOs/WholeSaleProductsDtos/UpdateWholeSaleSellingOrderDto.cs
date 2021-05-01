using System.Collections.Generic;

namespace GlassStoreCore.BL.DTOs.WholeSaleProductsDtos
{
    public class UpdateWholeSaleSellingOrderDto
    {
        public int Id { get; set; }

        public ICollection<WholeSaleProductsOrderDetailsDto> WholeSaleSellingOrderDetails { get; set; }

    }
}
