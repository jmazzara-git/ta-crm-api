namespace TACRM.Services.Dtos
{
	public class ContactDto
	{
		public int ContactId { get; set; }
		public int UserId { get; set; }
		public string ContactStatus { get; set; }
		public string FullName { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public DateOnly? FromDate { get; set; }
		public DateOnly? ToDate { get; set; }
		public int Adults { get; set; }
		public int Kids { get; set; }
		public string KidsAges { get; set; }
		public string Comments { get; set; }
		public bool EnableWhatsAppNotifications { get; set; }
		public bool EnableEmailNotifications { get; set; }
		public int? ContactSourceId { get; set; }
	}

}
