using TACRM.Services.Dtos;

namespace TACRM.Services.Business.Abstractions
{
	public interface IGenericService<T> where T : class
	{
		Task<ApiResponse<IEnumerable<T>>> GetListAsync();
		Task<ApiResponse<T>> GetByIdAsync(int id);
		Task<ApiResponse<T>> CreateAsync(T dto);
		Task<ApiResponse<T>> UpdateAsync(T dto);
		Task<ApiResponse> DeleteAsync(int id);
	}
}