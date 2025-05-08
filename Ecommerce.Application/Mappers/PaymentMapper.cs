using System;
using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Mappers
{
    public static class PaymentMapper
    {
        /// <summary>
        /// Convierte una entidad Payment en su DTO.
        /// </summary>
        public static PaymentDTO ToDTO(this Payment payment)
        {
            if (payment == null)
                return null;

            return new PaymentDTO
            {
                PaymentId = payment.PaymentId,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                PaymentDate = payment.PaymentDate,
                PaymentStatus = payment.PaymentStatus,
                TransactionId = payment.TransactionId,
                CardLastFour = payment.CardLastFour,
                CardType = payment.CardType
            };
        }

        /// <summary>
        /// Crea una entidad Payment a partir del DTO de creación.
        /// </summary>
        public static Payment ToEntity(this CreatePaymentDTO dto)
        {
            if (dto == null)
                return null;

            return new Payment
            {
                OrderId = dto.OrderId,
                Amount = dto.Amount,
                PaymentMethod = dto.PaymentMethod,
                CardLastFour = dto.CardLastFour,
                CardType = dto.CardType,
                PaymentDate = DateTime.Now
            };
        }
    }
}
