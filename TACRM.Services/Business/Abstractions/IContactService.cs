using TACRM.Services.Dtos;

namespace TACRM.Services.Business.Abstractions
{
	public interface IContactService : IGenericService<ContactDto>
	{
		Task<(IEnumerable<ContactDto> Results, int TotalCount)> SearchAsync(
				string searchTerm = null,
				string status = null,
				int pageNumber = 1,
				int pageSize = 10);
	}
}
