using Microsoft.AspNetCore.Mvc;
using TACRM.Services.Core;
using TACRM.Services.Entities;

namespace TACRM.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class PaymentsController : ControllerBase
	{
		private readonly IPaymentsService _paymentsService;

		public PaymentsController(IPaymentsService paymentService)
		{
			_paymentsService = paymentService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllPayments()
		{
			var payments = await _paymentsService.GetAllPaymentsAsync();
			return Ok(payments);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetPaymentById(int id)
		{
			var payment = await _paymentsService.GetPaymentByIdAsync(id);
			if (payment == null) return NotFound();

			return Ok(payment);
		}

		[HttpPost]
		public async Task<IActionResult> CreatePayment([FromBody] Payment payment)
		{
			var createdPayment = await _paymentsService.CreatePaymentAsync(payment);
			return CreatedAtAction(nameof(GetPaymentById), new { id = createdPayment.PaymentID }, createdPayment);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdatePayment(int id, [FromBody] Payment payment)
		{
			if (id != payment.PaymentID) return BadRequest();

			try
			{
				var updatedPayment = await _paymentsService.UpdatePaymentAsync(payment);
				return Ok(updatedPayment);
			}
			catch (KeyNotFoundException)
			{
				return NotFound();
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeletePayment(int id)
		{
			var success = await _paymentsService.DeletePaymentAsync(id);
			if (!success) return NotFound();

			return NoContent();
		}
	}
}
