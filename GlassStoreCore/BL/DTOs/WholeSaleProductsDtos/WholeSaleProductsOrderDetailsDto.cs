﻿using System.Collections;

namespace GlassStoreCore.BL.DTOs.WholeSaleProductsDtos
{
    public class WholeSaleProductsOrderDetailsDto
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public string WholeSaleProductId { get; set; }
        public string WholeSaleSellingOrderId { get; set; }
    }
}
