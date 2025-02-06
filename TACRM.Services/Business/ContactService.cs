using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using TACRM.Services.Business.Abstractions;
using TACRM.Services.Dtos;
using TACRM.Services.Entities;

namespace TACRM.Services.Business
{
	public class ContactService(
		AppDbContext db,
		IAppUserContext user,
		IMapper mapper,
		IValidator<ContactDto> validator) : IContactService
	{
		private readonly AppDbContext _db = db;
		private readonly IAppUserContext _user = user;
		private readonly IMapper _mapper = mapper;
		private readonly IValidator<ContactDto> _validator = validator;

		public async Task<IEnumerable<ContactDto>> GetAsync()
		{
			var results = await _db
				.Contact
				.Where(c => c.UserId == _user.UserId && !c.IsDisabled)
				.OrderBy(c => c.FullName)
				.ToListAsync();

			return _mapper.Map<IEnumerable<ContactDto>>(results);
		}

		public async Task<ContactDto> GetByIdAsync(int id)
		{
			var result = await _db
				.Contact
				.FirstOrDefaultAsync(c => c.ContactId == id && c.UserId == _user.UserId);

			return _mapper.Map<ContactDto>(result);
		}

		public async Task<ContactDto> CreateAsync(ContactDto dto)
		{
			// Validate
			ValidationResult result = await _validator.ValidateAsync(dto);
			if (!result.IsValid)
				throw new ValidationException(result.Errors);

			// Map
			var contact = new Contact
			{
				UserId = _user.UserId,
				FullName = dto.FullName,
				Email = dto.Email,
				Phone = dto.Phone,
				FromDate = dto.FromDate,
				ToDate = dto.ToDate,
				Adults = dto.Adults,
				Kids = dto.Kids,
				KidsAges = dto.KidsAges,
				Comments = dto.Comments,
				EnableWhatsAppNotifications = dto.EnableWhatsAppNotifications,
				EnableEmailNotifications = dto.EnableEmailNotifications,
				ContactSourceId = dto.ContactSourceId,

				ContactStatus = ContactStatusEnum.New,
				CreatedAt = DateTime.UtcNow
			};

			// Save
			_db.Contact.Add(contact);
			await _db.SaveChangesAsync();
			return _mapper.Map<ContactDto>(contact);
		}

		public async Task UpdateAsync(ContactDto dto)
		{
			var contact = await _db
				.Contact
				.FirstOrDefaultAsync(c => c.ContactId == dto.ContactId && c.UserId == _user.UserId) ?? throw new KeyNotFoundException();

			// Validate
			ValidationResult result = await _validator.ValidateAsync(dto);
			if (!result.IsValid)
				throw new ValidationException(result.Errors);

			// Map
			contact.FullName = dto.FullName;
			contact.Email = dto.Email;
			contact.Phone = dto.Phone;
			contact.FromDate = dto.FromDate;
			contact.ToDate = dto.ToDate;
			contact.Adults = dto.Adults;
			contact.Kids = dto.Kids;
			contact.KidsAges = dto.KidsAges;
			contact.Comments = dto.Comments;
			contact.EnableWhatsAppNotifications = dto.EnableWhatsAppNotifications;
			contact.EnableEmailNotifications = dto.EnableEmailNotifications;
			contact.ContactSourceId = dto.ContactSourceId;
			contact.UpdatedAt = DateTime.UtcNow;

			// Save
			await _db.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var contact = await _db
					.Contact
					.FirstOrDefaultAsync(c => c.ContactId == id && c.UserId == _user.UserId) ?? throw new KeyNotFoundException();

			// Delete
			_db.Contact.Remove(contact);
			await _db.SaveChangesAsync();
		}

		public async Task<(IEnumerable<ContactDto> Results, int TotalCount)> SearchAsync(
			string searchTerm = null,
			string contactStatus = null,
			int pageNumber = 1,
			int pageSize = 10)
		{
			// Base query with tenant filtering
			var query = _db
				.Contact
				.Where(c => c.UserId == _user.UserId && !c.IsDisabled);

			// Apply search term filter (by full name)
			if (!string.IsNullOrEmpty(searchTerm))
			{
				query = query.Where(c => c.FullName.Contains(searchTerm));
			}

			// Apply contact status filter
			if (!string.IsNullOrEmpty(contactStatus) && Enum.TryParse(contactStatus, out ContactStatusEnum statusEnum))
			{
				query = query.Where(c => c.ContactStatus == statusEnum);
			}

			// Get the total count of matching records (for pagination)
			int totalCount = await query.CountAsync();

			// Apply pagination
			var results = await query
				.OrderBy(c => c.FullName)
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			return (_mapper.Map<IEnumerable<ContactDto>>(results), totalCount);
		}
	}
}