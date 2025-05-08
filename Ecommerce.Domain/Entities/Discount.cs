using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Entities
{
    public class Discount
    {
        public int DiscountId { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public decimal DiscountValue { get; set; }
        public string? DiscountType { get; set; } // "Percentage", "FixedAmount"
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UsageLimit { get; set; }
        public int UsageCount { get; set; }
        public bool IsActive { get; set; }

        // Navegación
        public virtual ICollection<Cart>? AppliedCarts { get; set; }
    }
}
