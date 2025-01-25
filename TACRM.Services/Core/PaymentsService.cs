using Microsoft.EntityFrameworkCore;
using TACRM.Services.Entities;

namespace TACRM.Services.Core
{
	public interface IPaymentsService
	{
		Task<IEnumerable<Payment>> GetAllPaymentsAsync();
		Task<Payment> GetPaymentByIdAsync(int id);
		Task<Payment> CreatePaymentAsync(Payment payment);
		Task<Payment> UpdatePaymentAsync(Payment payment);
		Task<bool> DeletePaymentAsync(int id);
	}

	public class PaymentsService : IPaymentsService
	{
		private readonly TACRMDbContext _dbContext;

		public PaymentsService(TACRMDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
		{
			return await _dbContext.Payments.ToListAsync();
		}

		public async Task<Payment> GetPaymentByIdAsync(int id)
		{
			return await _dbContext.Payments.FindAsync(id);
		}

		public async Task<Payment> CreatePaymentAsync(Payment payment)
		{
			_dbContext.Payments.Add(payment);
			await _dbContext.SaveChangesAsync();
			return payment;
		}

		public async Task<Payment> UpdatePaymentAsync(Payment payment)
		{
			var existingPayment = await _dbContext.Payments.FindAsync(payment.PaymentID);
			if (existingPayment == null) throw new KeyNotFoundException("Payment not found");

			_dbContext.Entry(existingPayment).CurrentValues.SetValues(payment);
			await _dbContext.SaveChangesAsync();
			return existingPayment;
		}

		public async Task<bool> DeletePaymentAsync(int id)
		{
			var payment = await _dbContext.Payments.FindAsync(id);
			if (payment == null) return false;

			_dbContext.Payments.Remove(payment);
			await _dbContext.SaveChangesAsync();
			return true;
		}
	}
}
