using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string? PaymentMethod { get; set; } // "CreditCard", "PayPal", "BankTransfer", etc.
        public DateTime PaymentDate { get; set; }
        public string? PaymentStatus { get; set; } // "Pending", "Completed", "Failed", "Refunded"
        public string? TransactionId { get; set; }

        // Información de tarjeta (sólo últimos 4 dígitos para seguridad)
        public string? CardLastFour { get; set; }
        public string? CardType { get; set; }

        // Navegación
        public virtual Order? Order { get; set; }
    }
}
