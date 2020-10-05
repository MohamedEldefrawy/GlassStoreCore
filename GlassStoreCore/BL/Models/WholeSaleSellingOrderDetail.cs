using System.ComponentModel.DataAnnotations;

namespace GlassStoreCore.BL.Models
{
    public class WholeSaleSellingOrderDetail
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Price { get; set; }
        public WholeSaleProduct WholeSaleProduct { get; set; }
        public WholeSaleSellingOrder WholeSaleSellingOrder { get; set; }
    }
}
