using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities
{
    public class Address
    {
        public int AddressId { get; set; }
        public int UserId { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Country { get; set; }
        public bool IsDefault { get; set; }
        public string? AddressType { get; set; } // "Shipping", "Billing", "Both"

        // Navegación
        public virtual User? User { get; set; }
    }
}
