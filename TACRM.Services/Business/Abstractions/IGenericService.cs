namespace TACRM.Services.Business.Abstractions
{
	public interface IGenericService<T> where T : class
	{
		Task<IEnumerable<T>> GetAsync();
		Task<T> GetByIdAsync(int id);
		Task<T> CreateAsync(T dto);
		Task UpdateAsync(T dto);
		Task DeleteAsync(int id);
	}
}