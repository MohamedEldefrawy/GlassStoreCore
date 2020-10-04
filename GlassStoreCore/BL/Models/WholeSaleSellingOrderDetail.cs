using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
