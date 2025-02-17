using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TACRM.Services.Enums;

namespace TACRM.Services
{

	public interface IAppUserContext
	{
		int UserId { get; }
		LangEnum Lang { get; }
		bool EN => Lang == LangEnum.EN;
		int GetUserId();
		string GetUserType();
		int? GetAgencyId();
	}

	public class AppUserContext : IAppUserContext
	{
		private readonly ClaimsPrincipal _user;

		public int UserId => 2;

		public LangEnum Lang => LangEnum.EN;

		public AppUserContext(IHttpContextAccessor httpContextAccessor)
		{
			_user = httpContextAccessor.HttpContext?.User ?? throw new UnauthorizedAccessException("User not authenticated");
		}

		public int GetUserId()
		{
			var userIdClaim = _user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userIdClaim)) throw new UnauthorizedAccessException("User ID claim is missing");

			return int.Parse(userIdClaim);
		}

		public string GetUserType()
		{
			var userTypeClaim = _user.FindFirst("userType")?.Value;
			if (string.IsNullOrEmpty(userTypeClaim)) throw new UnauthorizedAccessException("User type claim is missing");

			return userTypeClaim;
		}

		public int? GetAgencyId()
		{
			var agencyIdClaim = _user.FindFirst("agencyId")?.Value;
			return agencyIdClaim != null ? int.Parse(agencyIdClaim) : (int?)null;
		}
	}
}
