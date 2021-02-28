using System;
using System.Collections.Generic;
using GlassStoreCore.BL.Models;

namespace GlassStoreCore.BL.DTOs.WholeSaleProductsDtos
{
    public class WholeSaleSellingOrdersDto
    {
        public string Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserId { get; set; }
        public ICollection<WholeSaleProductsOrderDetailsDto> WholeSaleSellingOrderDetails { get; set; }
    }
}
