using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GlassStoreCore.BL.Models
{
    public class WholeSaleSellingOrder
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime RecievingOrderDate { get; set; }
        public ICollection<WholeSaleSellingOrderDetails> WholeSaleSellingOrderDetails { get; set; }

    }
}
