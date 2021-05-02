using System;
using System.Collections.Generic;

namespace GlassStoreCore.BL.DTOs.WholeSaleProductsDtos
{
    public class WholeSaleSellingOrdersDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RecievingOrderDate { get; set; }
        public string UserId { get; set; }
        public ICollection<WholeSaleProductsOrderDetailsDto> WholeSaleSellingOrderDetails { get; set; }
    }
}
