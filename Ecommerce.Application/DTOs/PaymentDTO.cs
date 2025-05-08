using System;

namespace Ecommerce.Application.DTOs
{
    public class PaymentDTO
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentStatus { get; set; }
        public string TransactionId { get; set; }
        public string CardLastFour { get; set; }
        public string CardType { get; set; }
    }

    // DTO para crear un nuevo pago
    public class CreatePaymentDTO
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }

        // Información de tarjeta de crédito (si aplica)
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public string Cvv { get; set; }

        // Estos campos se extraerán de la tarjeta
        public string CardLastFour => !string.IsNullOrEmpty(CardNumber) && CardNumber.Length >= 4
            ? CardNumber.Substring(CardNumber.Length - 4)
            : null;

        public string CardType => DetermineCardType(CardNumber);

        // Método para determinar el tipo de tarjeta basado en el número
        private string DetermineCardType(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber))
                return null;

            // Algoritmo simplificado para determinar el tipo de tarjeta
            if (cardNumber.StartsWith("4"))
                return "Visa";
            else if (cardNumber.StartsWith("5"))
                return "MasterCard";
            else if (cardNumber.StartsWith("3"))
                return "Amex";
            else if (cardNumber.StartsWith("6"))
                return "Discover";
            else
                return "Unknown";
        }
    }

    // DTO para actualizar el estado de un pago
    public class UpdatePaymentStatusDTO
    {
        public int PaymentId { get; set; }
        public string PaymentStatus { get; set; }
    }
}