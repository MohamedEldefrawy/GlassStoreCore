using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlassStoreCore.BL.DTOs.WholeSaleProductsDtos
{
    public class DeleteWholeSaleSellingOrderDetail
    {
        public Guid WholeSaleProductId { get; set; }
        public string WholeSaleSellingOrderId { get; set; }
    }
}
