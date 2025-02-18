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
	public class ProviderService(
		AppDbContext db,
		IAppUserContext user,
		IMapper mapper,
		IValidator<ProviderDto> validator,
		IStringLocalizer<Localizer> localizer) : IGenericService<ProviderDto>
	{
		private readonly AppDbContext _db = db;
		private readonly IAppUserContext _user = user;
		private readonly IMapper _mapper = mapper;
		private readonly IValidator<ProviderDto> _validator = validator;
		private readonly IStringLocalizer<Localizer> _localizer = localizer;

		public async Task<ApiResponse<IEnumerable<ProviderDto>>> GetListAsync()
		{
			// TODO: Add sharing support

			var providers = await _db
				.Provider
				.Where(p => p.UserId == _user.UserId && !p.IsDisabled)
				.OrderBy(p => p.ProviderName)
				.ToListAsync();

			return new ApiResponse<IEnumerable<ProviderDto>>(
				_mapper.Map<IEnumerable<ProviderDto>>(providers)
			);
		}

		public async Task<ApiResponse<ProviderDto>> GetByIdAsync(int id)
		{
			var response = new ApiResponse<ProviderDto>();

			var provider = await _db
				.Provider
				.FirstOrDefaultAsync(p => p.ProviderId == id && p.UserId == _user.UserId);

			if (provider == null)
			{
				response.Errors.Add(_localizer["ProviderNotFound"]);
				return response;
			}

			response.Data = _mapper.Map<ProviderDto>(provider);
			return response;
		}

		public async Task<ApiResponse<ProviderDto>> CreateAsync(ProviderDto dto)
		{
			var response = new ApiResponse<ProviderDto>();

			// Validate
			ValidationResult val = await _validator.ValidateAsync(dto);
			if (!val.IsValid)
			{
				response.Errors.Add(val.Errors);
				return response;
			}

			// Map
			var provider = new Provider
			{
				UserId = _user.UserId,
				ProviderName = dto.ProviderName,
				ProviderDetails = dto.ProviderDetails,
				IsShared = dto.IsShared,
				CreatedAt = DateTime.UtcNow
			};

			// Save
			_db.Provider.Add(provider);
			await _db.SaveChangesAsync();

			response.Data = _mapper.Map<ProviderDto>(provider);
			return response;
		}

		public async Task<ApiResponse<ProviderDto>> UpdateAsync(ProviderDto dto)
		{
			var response = new ApiResponse<ProviderDto>();

			var provider = await _db
				.Provider
				.FirstOrDefaultAsync(p => p.ProviderId == dto.ProviderId && p.UserId == _user.UserId);

			if (provider == null)
			{
				response.Errors.Add(_localizer["ProviderNotFound"]);
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
			provider.ProviderName = dto.ProviderName;
			provider.ProviderDetails = dto.ProviderDetails;
			provider.IsShared = dto.IsShared;
			provider.UpdatedAt = DateTime.UtcNow;

			// Save
			await _db.SaveChangesAsync();

			response.Data = _mapper.Map<ProviderDto>(provider);
			return response;
		}

		public async Task<ApiResponse> DeleteAsync(int id)
		{
			// TODO: Validate if it's being used

			var response = new ApiResponse();

			var provider = await _db
				.Provider
				.FirstOrDefaultAsync(p => p.ProviderId == id && p.UserId == _user.UserId);

			if (provider == null)
			{
				response.Errors.Add(_localizer["ProviderNotFound"]);
				return response;
			}

			// Delete
			_db.Provider.Remove(provider);
			await _db.SaveChangesAsync();

			return response;
		}
	}
}


