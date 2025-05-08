using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string? OrderStatus { get; set; } // "Pending", "Processing", "Shipped", "Delivered", "Cancelled"
        public decimal TotalAmount { get; set; }
        public int ShippingAddressId { get; set; }
        public int? BillingAddressId { get; set; }
        public string? ShippingMethod { get; set; }
        public decimal ShippingCost { get; set; }
        public string? OrderNotes { get; set; }

        // Navegación
        public virtual User? User { get; set; }
        public virtual Address? ShippingAddress { get; set; }
        public virtual Address? BillingAddress { get; set; }
        public virtual ICollection<OrderItem>? OrderItems { get; set; }
        public virtual ICollection<Payment>? Payments { get; set; }
    }
}
