using System.Threading.Tasks;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Infrastructure.Interfaces
{
    public interface IPaymentGateway
    {
        Task<PaymentGatewayResponse> ProcessPaymentAsync(PaymentGatewayRequest request);
        Task<PaymentGatewayResponse> RefundPaymentAsync(string transactionId, decimal amount);
        Task<PaymentGatewayResponse> VerifyPaymentAsync(string transactionId);
    }

    public class PaymentGatewayRequest
    {
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";

        // Información de tarjeta (si aplica)
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public string Cvv { get; set; }

        // Información de PayPal (si aplica)
        public string PayPalEmail { get; set; }

        // Información de transferencia bancaria (si aplica)
        public string BankAccountNumber { get; set; }
        public string BankRoutingNumber { get; set; }

        // Referencias
        public string OrderReference { get; set; }
        public string CustomerReference { get; set; }
    }

    public class PaymentGatewayResponse
    {
        public bool Success { get; set; }
        public string TransactionId { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string AuthorizationCode { get; set; }
        public string ErrorCode { get; set; }
    }
}