using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.Application.Interfaces;
using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;
using System.Web.Http.ModelBinding;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<PaymentDTO>>> GetAllPayments()
        {
            var payments = await _paymentService.GetAllPaymentsAsync();
            var paymentDTOs = new List<PaymentDTO>();

            foreach (var payment in payments)
            {
                paymentDTOs.Add(MapToDTO(payment));
            }

            return Ok(paymentDTOs);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<PaymentDTO>> GetPaymentById(int id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null)
                return NotFound();

            return Ok(MapToDTO(payment));
        }

        [HttpGet("order/{orderId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PaymentDTO>>> GetPaymentsByOrderId(int orderId)
        {
            var payments = await _paymentService.GetPaymentsByOrderIdAsync(orderId);
            var paymentDTOs = new List<PaymentDTO>();

            foreach (var payment in payments)
            {
                paymentDTOs.Add(MapToDTO(payment));
            }

            return Ok(paymentDTOs);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PaymentDTO>> ProcessPayment([FromBody] CreatePaymentDTO createPaymentDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var payment = new Payment
                {
                    OrderId = createPaymentDTO.OrderId,
                    Amount = createPaymentDTO.Amount,
                    PaymentMethod = createPaymentDTO.PaymentMethod,
                    CardLastFour = createPaymentDTO.CardLastFour,
                    CardType = createPaymentDTO.CardType,
                    PaymentDate = DateTime.Now
                };

                var createdPayment = await _paymentService.ProcessPaymentAsync(payment);
                return CreatedAtAction(nameof(GetPaymentById), new { id = createdPayment.PaymentId }, MapToDTO(createdPayment));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("status")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PaymentDTO>> UpdatePaymentStatus([FromBody] UpdatePaymentStatusDTO updateDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedPayment = await _paymentService.UpdatePaymentStatusAsync(updateDTO.PaymentId, updateDTO.PaymentStatus);
                return Ok(MapToDTO(updatedPayment));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/refund")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> RefundPayment(int id)
        {
            try
            {
                var result = await _paymentService.RefundPaymentAsync(id);
                if (result)
                    return Ok(new { message = "Pago reembolsado correctamente" });
                else
                    return BadRequest(new { message = "No se pudo procesar el reembolso" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeletePayment(int id)
        {
            try
            {
                var result = await _paymentService.DeletePaymentAsync(id);
                if (result)
                    return Ok(new { message = "Pago eliminado correctamente" });
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Método auxiliar para mapear de entidad a DTO
        private PaymentDTO MapToDTO(Payment payment)
        {
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
    }
}