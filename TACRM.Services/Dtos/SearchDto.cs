namespace TACRM.Services.Dtos
{
	public class SearchRequestDto
	{
		public string SearchTerm { get; set; }
		public int PageNumber { get; set; } = 1;
		public int PageSize { get; set; } = 50;
		public string SortBy { get; set; }
		public string SortDirection { get; set; }
	}

	public class SearchResultDto<T>
	{
		public IEnumerable<T> Results { get; set; }

		public int Total { get; set; }
	}
}