namespace TACRM.Services.Dtos
{
	public class ProviderDto
	{
		public int ProviderId { get; set; }
		public int UserId { get; set; }
		public string ProviderName { get; set; }
		public string ProviderDetails { get; set; }
		public bool IsShared { get; set; } = false;
	}
}
