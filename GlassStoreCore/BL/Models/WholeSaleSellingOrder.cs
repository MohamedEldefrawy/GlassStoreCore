using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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
        public ICollection<WholeSaleSellingOrderDetails> WholeSaleSellingOrderDetails { get; set; }

    }
}
