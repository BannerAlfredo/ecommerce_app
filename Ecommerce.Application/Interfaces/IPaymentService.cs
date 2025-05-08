using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<Payment>> GetAllPaymentsAsync();
        Task<Payment> GetPaymentByIdAsync(int paymentId);
        Task<IEnumerable<Payment>> GetPaymentsByOrderIdAsync(int orderId);
        Task<Payment> ProcessPaymentAsync(Payment payment);
        Task<Payment> UpdatePaymentStatusAsync(int paymentId, string newStatus);
        Task<bool> RefundPaymentAsync(int paymentId);
        Task<bool> DeletePaymentAsync(int paymentId);
    }
}