namespace TACRM.Services.Dtos
{
	public class ContactDto
	{
		public int ContactId { get; set; }
		public int UserId { get; set; }
		public string ContactStatusCode { get; set; }
		public string ContactStatusName { get; set; }
		public string FullName { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public DateOnly? FromDate { get; set; }
		public DateOnly? ToDate { get; set; }
		public int Adults { get; set; }
		public int Kids { get; set; }
		public int[] KidsAges { get; set; } = [];
		public string Comments { get; set; }
		public bool EnableWhatsAppNotifications { get; set; }
		public bool EnableEmailNotifications { get; set; }
		public int? ContactSourceId { get; set; }
		public string ContactSourceName { get; set; }
	}

	public class ContactSearchRequestDto : SearchRequestDto
	{
		public string ContactStatusCode { get; set; }
	}

	public class ContactSearchResultDto : SearchResultDto<ContactDto>
	{
	}

	public class ContactSourceDto
	{
		public int ContactSourceId { get; set; }
		public string ContactSourceName { get; set; }
	}

	public class ContactStatusDto
	{
		public string ContactStatusCode { get; set; }
		public string ContactStatusName { get; set; }
	}

}
