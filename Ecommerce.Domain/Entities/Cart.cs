using System;
using System.Collections.Generic;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Entities
{
    public class Cart
    {
        public int CartId { get; set; }
        public int? UserId { get; set; }
        public string? SessionId { get; set; } // Para usuarios no autenticados
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public decimal TotalAmount { get; set; }

        // Navegación
        public virtual User? User { get; set; }
        public virtual ICollection<CartItem>? CartItems { get; set; }
        public virtual ICollection<Discount>? AppliedDiscounts { get; set; }
    }
}