using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using TACRM.Services.Business.Abstractions;
using TACRM.Services.Dtos;
using TACRM.Services.Entities;
using TACRM.Services.Localization;

namespace TACRM.Services.Business
{
	public class ContactService(
		AppDbContext db,
		IAppUserContext user,
		IMapper mapper,
		IValidator<ContactDto> validator,
		IStringLocalizer<Localizer> localizer) : IContactService
	{
		private readonly AppDbContext _db = db;
		private readonly IAppUserContext _user = user;
		private readonly IMapper _mapper = mapper;
		private readonly IValidator<ContactDto> _validator = validator;
		private readonly IStringLocalizer _localizer = localizer;

		public async Task<ApiResponse<IEnumerable<ContactDto>>> GetListAsync()
		{
			var contacts = await _db
				.Contact
				.Where(c => c.UserId == _user.UserId && !c.IsDisabled)
				.OrderBy(c => c.FullName)
				.ToListAsync();

			return new ApiResponse<IEnumerable<ContactDto>>(
				_mapper.Map<IEnumerable<ContactDto>>(contacts)
			);
		}

		public async Task<ApiResponse<ContactDto>> GetByIdAsync(int id)
		{
			var response = new ApiResponse<ContactDto>();

			var contact = await _db
				.Contact
				.FirstOrDefaultAsync(c => c.ContactId == id && c.UserId == _user.UserId);

			if (contact == null)
			{
				response.Errors.Add(_localizer["ContactNotFound"]);
				return response;
			}

			response.Data = _mapper.Map<ContactDto>(contact);
			return response;
		}

		public async Task<ApiResponse<ContactDto>> CreateAsync(ContactDto dto)
		{
			var response = new ApiResponse<ContactDto>();

			// Validate
			ValidationResult val = await _validator.ValidateAsync(dto);
			if (!val.IsValid)
			{
				response.Errors.Add(val.Errors);
				return response;
			}

			// Map
			var contact = new Contact
			{
				UserId = _user.UserId,
				ContactStatusCode = dto.ContactStatusCode,
				FullName = dto.FullName,
				Email = dto.Email,
				Phone = dto.Phone,
				FromDate = dto.FromDate,
				ToDate = dto.ToDate,
				Adults = dto.Adults,
				Kids = dto.Kids,
				KidsAges = string.Join(",", dto.KidsAges),
				Comments = dto.Comments,
				EnableWhatsAppNotifications = dto.EnableWhatsAppNotifications,
				EnableEmailNotifications = dto.EnableEmailNotifications,
				ContactSourceId = dto.ContactSourceId,
				CreatedAt = DateTime.UtcNow
			};

			// Save
			_db.Contact.Add(contact);
			await _db.SaveChangesAsync();

			response.Data = _mapper.Map<ContactDto>(contact);
			return response;
		}

		public async Task<ApiResponse<ContactDto>> UpdateAsync(ContactDto dto)
		{
			var response = new ApiResponse<ContactDto>();

			var contact = await _db
				.Contact
				.FirstOrDefaultAsync(c => c.ContactId == dto.ContactId && c.UserId == _user.UserId);

			if (contact == null)
			{
				response.Errors.Add(_localizer["ContactNotFound"]);
				return response;
			}

			// Validate
			ValidationResult result = await _validator.ValidateAsync(dto);
			if (!result.IsValid)
			{
				response.Errors.Add(result.Errors);
				return response;
			}

			// Map
			contact.ContactStatusCode = dto.ContactStatusCode;
			contact.FullName = dto.FullName;
			contact.Email = dto.Email;
			contact.Phone = dto.Phone;
			contact.FromDate = dto.FromDate;
			contact.ToDate = dto.ToDate;
			contact.Adults = dto.Adults;
			contact.Kids = dto.Kids;
			contact.KidsAges = string.Join(",", dto.KidsAges);
			contact.Comments = dto.Comments;
			contact.EnableWhatsAppNotifications = dto.EnableWhatsAppNotifications;
			contact.EnableEmailNotifications = dto.EnableEmailNotifications;
			contact.ContactSourceId = dto.ContactSourceId;
			contact.UpdatedAt = DateTime.UtcNow;

			// Save
			await _db.SaveChangesAsync();

			response.Data = _mapper.Map<ContactDto>(contact);
			return response;
		}

		public async Task<ApiResponse> DeleteAsync(int id)
		{
			var response = new ApiResponse();

			var contact = await _db
				.Contact
				.FirstOrDefaultAsync(c => c.ContactId == id && c.UserId == _user.UserId);

			if (contact == null)
			{
				response.Errors.Add(_localizer["ContactNotFound"]);
				return response;
			}

			// Delete
			_db.Contact.Remove(contact);
			await _db.SaveChangesAsync();

			return response;
		}

		public async Task<ApiResponse<ContactSearchResultDto>> SearchAsync(ContactSearchRequestDto dto)
		{
			// Base query with tenant filtering
			var query = _db
				.Contact
				.Where(c => c.UserId == _user.UserId && !c.IsDisabled);

			// Apply search term filter (by full name)
			if (!string.IsNullOrEmpty(dto.SearchTerm))
			{
				query = query.Where(c => c.FullName.Contains(dto.SearchTerm));
			}

			// Apply contact status filter
			if (!string.IsNullOrEmpty(dto.ContactStatusCode))
			{
				query = query.Where(c => c.ContactStatusCode == dto.ContactStatusCode);
			}

			// Get the total count of matching records (for pagination)
			int totalCount = await query.CountAsync();

			// Apply pagination
			var results = await query
				.OrderBy(c => c.FullName)
				.Skip((dto.PageNumber - 1) * dto.PageSize)
				.Take(dto.PageSize)
				.ToListAsync();

			return new ApiResponse<ContactSearchResultDto>(
				new ContactSearchResultDto
				{
					Results = _mapper.Map<IEnumerable<ContactDto>>(results),
					Total = totalCount
				}
			);
		}

		public async Task<ApiResponse<IEnumerable<ContactSourceDto>>> GetContactSourcesAsync()
		{
			var sources = await _db
				.ContactSource
				.Select(c => new ContactSourceDto
				{
					ContactSourceId = c.ContactSourceId,
					ContactSourceName = _user.EN ? c.ContactSourceNameEN : c.ContactSourceNameES
				})
				.ToListAsync();

			return new ApiResponse<IEnumerable<ContactSourceDto>>(sources);
		}

		public async Task<ApiResponse<IEnumerable<ContactStatusDto>>> GetContactStatusesAsync()
		{
			var statuses = await _db
				.ContactStatus
				.Select(c => new ContactStatusDto
				{
					ContactStatusCode = c.ContactStatusCode,
					ContactStatusName = _user.EN ? c.ContactStatusNameEN : c.ContactStatusNameES
				})
				.ToListAsync();

			return new ApiResponse<IEnumerable<ContactStatusDto>>(statuses);
		}
	}
}