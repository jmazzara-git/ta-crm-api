using Microsoft.AspNetCore.Mvc;
using TACRM.Services.Core;
using TACRM.Services.Entities;

namespace TACRM.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class BudgetsController : ControllerBase
	{
		private readonly IBudgetsService _budgetsService;

		public BudgetsController(IBudgetsService budgetService)
		{
			_budgetsService = budgetService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllBudgets()
		{
			var budgets = await _budgetsService.GetAllBudgetsAsync();
			return Ok(budgets);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetBudgetById(int id)
		{
			var budget = await _budgetsService.GetBudgetByIdAsync(id);
			if (budget == null) return NotFound();

			return Ok(budget);
		}

		[HttpPost]
		public async Task<IActionResult> CreateBudget([FromBody] Budget budget)
		{
			var createdBudget = await _budgetsService.CreateBudgetAsync(budget);
			return CreatedAtAction(nameof(GetBudgetById), new { id = createdBudget.BudgetID }, createdBudget);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateBudget(int id, [FromBody] Budget budget)
		{
			if (id != budget.BudgetID) return BadRequest();

			var updatedBudget = await _budgetsService.UpdateBudgetAsync(budget);
			if (updatedBudget == null) return NotFound();

			return Ok(updatedBudget);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteBudget(int id)
		{
			var success = await _budgetsService.DeleteBudgetAsync(id);
			if (!success) return NotFound();

			return NoContent();
		}
	}
}
