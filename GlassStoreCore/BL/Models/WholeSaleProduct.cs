using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GlassStoreCore.BL.Models
{
    public class WholeSaleProduct
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string SerialNumber { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int UnitsInStock { get; set; }

        public ICollection<WholeSaleSellingOrderDetail> WholeSaleSellingOrderDetails { get; set; }
    }
}
