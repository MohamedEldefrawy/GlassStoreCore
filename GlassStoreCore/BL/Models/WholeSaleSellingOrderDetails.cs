using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlassStoreCore.BL.Models
{
    public class WholeSaleSellingOrderDetails
    {
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Price { get; set; }
        public Guid WholeSaleProductId { get; set; }

        [ForeignKey("WholeSaleProductId")]
        public WholeSaleProducts WholeSaleProduct { get; set; }
        public string WholeSaleSellingOrderId { get; set; }

        [ForeignKey("WholeSaleSellingOrderId")]
        public WholeSaleSellingOrder WholeSaleSellingOrder { get; set; }
    }
}
