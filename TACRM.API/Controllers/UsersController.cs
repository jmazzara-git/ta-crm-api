using Microsoft.AspNetCore.Mvc;
using TACRM.Services.Core;
using TACRM.Services.Entities;

namespace TACRM.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UsersController : ControllerBase
	{
		private readonly IUsersService _usersService;

		public UsersController(IUsersService usersService)
		{
			_usersService = usersService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllUsers()
		{
			var users = await _usersService.GetAllUsersAsync();
			return Ok(users);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetUserById(int id)
		{
			var user = await _usersService.GetUserByIdAsync(id);
			if (user == null) return NotFound();

			return Ok(user);
		}

		[HttpPost]
		public async Task<IActionResult> CreateUser([FromBody] User user)
		{
			var createdUser = await _usersService.CreateUserAsync(user);
			return CreatedAtAction(nameof(GetUserById), new { id = createdUser.UserID }, createdUser);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
		{
			if (id != user.UserID) return BadRequest();

			try
			{
				var updatedUser = await _usersService.UpdateUserAsync(user);
				return Ok(updatedUser);
			}
			catch (KeyNotFoundException)
			{
				return NotFound();
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteUser(int id)
		{
			var success = await _usersService.DeleteUserAsync(id);
			if (!success) return NotFound();

			return NoContent();
		}
	}
}
