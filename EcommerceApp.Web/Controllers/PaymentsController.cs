using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.Application.Interfaces;
using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;
using Ecommerce.Application.Mappers;

namespace Ecommerce.Web.Controllers
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
        public async Task<ActionResult<IEnumerable<PaymentDTO>>> GetAllPayments()
        {
            var payments = await _paymentService.GetAllPaymentsAsync();
            var dtos = payments.Select(p => p.ToDTO());
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<PaymentDTO>> GetPaymentById(int id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null)
                return NotFound();

            return Ok(payment.ToDTO());
        }

        [HttpGet("order/{orderId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PaymentDTO>>> GetPaymentsByOrderId(int orderId)
        {
            var payments = await _paymentService.GetPaymentsByOrderIdAsync(orderId);
            var dtos = payments.Select(p => p.ToDTO());
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<ActionResult<PaymentDTO>> ProcessPayment([FromBody] CreatePaymentDTO createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = createDto.ToEntity();
            try
            {
                var created = await _paymentService.ProcessPaymentAsync(entity);
                return CreatedAtAction(
                    nameof(GetPaymentById),
                    new { id = created.PaymentId },
                    created.ToDTO()
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("status")]
        public async Task<ActionResult<PaymentDTO>> UpdatePaymentStatus([FromBody] UpdatePaymentStatusDTO updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _paymentService.UpdatePaymentStatusAsync(updateDto.PaymentId, updateDto.PaymentStatus);
                return Ok(updated.ToDTO());
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
        public async Task<ActionResult> RefundPayment(int id)
        {
            try
            {
                var result = await _paymentService.RefundPaymentAsync(id);
                if (result)
                    return Ok(new { message = "Pago reembolsado correctamente" });

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
        public async Task<ActionResult> DeletePayment(int id)
        {
            try
            {
                var result = await _paymentService.DeletePaymentAsync(id);
                if (result)
                    return Ok(new { message = "Pago eliminado correctamente" });

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
