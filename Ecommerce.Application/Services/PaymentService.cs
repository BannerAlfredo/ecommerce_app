using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Interfaces;

namespace Ecommerce.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            return await _paymentRepository.GetAllPaymentsAsync();
        }

        public async Task<Payment> GetPaymentByIdAsync(int paymentId)
        {
            return await _paymentRepository.GetPaymentByIdAsync(paymentId);
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByOrderIdAsync(int orderId)
        {
            return await _paymentRepository.GetPaymentsByOrderIdAsync(orderId);
        }

        public async Task<Payment> ProcessPaymentAsync(Payment payment)
        {
            if (payment == null)
                throw new ArgumentNullException(nameof(payment));

            // Validaciones
            if (payment.Amount <= 0)
                throw new ArgumentException("El monto del pago debe ser mayor a cero.", nameof(payment.Amount));

            if (string.IsNullOrWhiteSpace(payment.PaymentMethod))
                throw new ArgumentException("El método de pago es requerido.", nameof(payment.PaymentMethod));

            // Establecer valores por defecto
            payment.PaymentDate = DateTime.Now;
            payment.PaymentStatus = "Pending"; // Estado inicial

            // Lógica de procesamiento según el método de pago
            switch (payment.PaymentMethod?.ToLower())
            {
                case "creditcard":
                    // Simulación de procesamiento de tarjeta de crédito
                    if (ValidateCreditCardPayment(payment))
                    {
                        payment.PaymentStatus = "Completed";
                        payment.TransactionId = $"CC-{Guid.NewGuid().ToString().Substring(0, 8)}";
                    }
                    else
                    {
                        payment.PaymentStatus = "Failed";
                        throw new InvalidOperationException("La transacción con tarjeta de crédito fue rechazada.");
                    }
                    break;

                case "paypal":
                    // Simulación de procesamiento PayPal
                    payment.PaymentStatus = "Completed";
                    payment.TransactionId = $"PP-{Guid.NewGuid().ToString().Substring(0, 8)}";
                    break;

                case "banktransfer":
                    // Las transferencias bancarias quedan en pendiente hasta su confirmación
                    payment.PaymentStatus = "Pending";
                    payment.TransactionId = $"BT-{Guid.NewGuid().ToString().Substring(0, 8)}";
                    break;

                default:
                    payment.PaymentStatus = "Pending";
                    payment.TransactionId = $"OP-{Guid.NewGuid().ToString().Substring(0, 8)}";
                    break;
            }

            // Persistir el pago
            return await _paymentRepository.CreatePaymentAsync(payment);
        }

        public async Task<Payment> UpdatePaymentStatusAsync(int paymentId, string newStatus)
        {
            if (string.IsNullOrWhiteSpace(newStatus))
                throw new ArgumentException("El nuevo estado es requerido.", nameof(newStatus));

            var validStatuses = new[] { "Pending", "Completed", "Failed", "Refunded" };
            if (!validStatuses.Contains(newStatus))
                throw new ArgumentException($"Estado de pago no válido. Los estados válidos son: {string.Join(", ", validStatuses)}", nameof(newStatus));

            var payment = await _paymentRepository.GetPaymentByIdAsync(paymentId);
            if (payment == null)
                throw new KeyNotFoundException($"No se encontró el pago con ID {paymentId}.");

            payment.PaymentStatus = newStatus;
            return await _paymentRepository.UpdatePaymentAsync(payment);
        }

        public async Task<bool> RefundPaymentAsync(int paymentId)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(paymentId);
            if (payment == null)
                throw new KeyNotFoundException($"No se encontró el pago con ID {paymentId}.");

            if (payment.PaymentStatus != "Completed")
                throw new InvalidOperationException("Solo se pueden reembolsar pagos completados.");

            // Lógica de reembolso según el método de pago
            switch (payment.PaymentMethod?.ToLower())
            {
                case "creditcard":
                    // Simulación de reembolso a tarjeta de crédito
                    payment.PaymentStatus = "Refunded";
                    break;

                case "paypal":
                    // Simulación de reembolso a PayPal
                    payment.PaymentStatus = "Refunded";
                    break;

                case "banktransfer":
                    // Reembolso por transferencia bancaria
                    payment.PaymentStatus = "Refunded";
                    break;

                default:
                    throw new InvalidOperationException($"No se puede procesar el reembolso para el método de pago '{payment.PaymentMethod}'.");
            }

            await _paymentRepository.UpdatePaymentAsync(payment);
            return true;
        }

        public async Task<bool> DeletePaymentAsync(int paymentId)
        {
            return await _paymentRepository.DeletePaymentAsync(paymentId);
        }

        // Métodos privados de ayuda
        private bool ValidateCreditCardPayment(Payment payment)
        {
            // Simulación de validación de tarjeta
            // En un entorno real, aquí se realizaría la comunicación con el procesador de pagos

            // Simulación: rechazar tarjetas que terminan en "0000"
            if (payment.CardLastFour == "0000")
                return false;

            return true;
        }
    }
}