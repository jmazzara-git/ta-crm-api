using TACRM.Services.Dtos;

namespace TACRM.Services.Business.Abstractions
{
	public interface IContactService : IGenericService<ContactDto>
	{
		Task<ApiResponse<ContactSearchResultDto>> SearchAsync(ContactSearchRequestDto dto);

		Task<ApiResponse<IEnumerable<ContactSourceDto>>> GetContactSourcesAsync();

		Task<ApiResponse<IEnumerable<ContactStatusDto>>> GetContactStatusesAsync();
	}
}
