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
	public class ProductService(
		AppDbContext db,
		IAppUserContext user,
		IMapper mapper,
		IValidator<ProductDto> validator,
		IStringLocalizer<Localizer> localizer) : IGenericService<ProductDto>
	{
		private readonly AppDbContext _db = db;
		private readonly IAppUserContext _user = user;
		private readonly IMapper _mapper = mapper;
		private readonly IValidator<ProductDto> _validator = validator;
		private readonly IStringLocalizer<Localizer> _localizer = localizer;

		public async Task<ApiResponse<IEnumerable<ProductDto>>> GetListAsync()
		{
			// TODO: Add sharing support

			var products = await _db
				.Product
				.Where(p => p.UserId == _user.UserId && !p.IsDisabled)
				.OrderBy(p => p.ProductName)
				.ToListAsync();

			return new ApiResponse<IEnumerable<ProductDto>>(
				_mapper.Map<IEnumerable<ProductDto>>(products)
			);
		}

		public async Task<ApiResponse<ProductDto>> GetByIdAsync(int id)
		{
			var response = new ApiResponse<ProductDto>();

			var product = await _db
				.Product
				.FirstOrDefaultAsync(p => p.ProductId == id && p.UserId == _user.UserId);

			if (product == null)
			{
				response.Errors.Add(_localizer["ProductNotFound"]);
				return response;
			}

			response.Data = _mapper.Map<ProductDto>(product);
			return response;
		}

		public async Task<ApiResponse<ProductDto>> CreateAsync(ProductDto dto)
		{
			var response = new ApiResponse<ProductDto>();

			// Validate
			ValidationResult val = await _validator.ValidateAsync(dto);
			if (!val.IsValid)
			{
				response.Errors.Add(val.Errors);
				return response;
			}

			// Map
			var product = new Product
			{
				UserId = _user.UserId,
				ProductTypeCode = dto.ProductTypeCode,
				ProductName = dto.ProductName,
				ProductDetails = dto.ProductDetails,
				IsShared = dto.IsShared,
				CreatedAt = DateTime.UtcNow
			};

			// Save
			_db.Product.Add(product);
			await _db.SaveChangesAsync();

			response.Data = _mapper.Map<ProductDto>(product);
			return response;
		}

		public async Task<ApiResponse<ProductDto>> UpdateAsync(ProductDto dto)
		{
			var response = new ApiResponse<ProductDto>();

			var product = await _db
				.Product
				.FirstOrDefaultAsync(p => p.ProductId == dto.ProductId && p.UserId == _user.UserId);

			if (product == null)
			{
				response.Errors.Add(_localizer["ProductNotFound"]);
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
			product.ProductTypeCode = dto.ProductTypeCode;
			product.ProductName = dto.ProductName;
			product.ProductDetails = dto.ProductDetails;
			product.IsShared = dto.IsShared;
			product.UpdatedAt = DateTime.UtcNow;

			// Save
			await _db.SaveChangesAsync();

			response.Data = _mapper.Map<ProductDto>(product);
			return response;
		}

		public async Task<ApiResponse> DeleteAsync(int id)
		{
			// TODO: Validate if it's being used

			var response = new ApiResponse();

			var product = await _db
				.Product
				.FirstOrDefaultAsync(p => p.ProductId == id && p.UserId == _user.UserId);

			if (product == null)
			{
				response.Errors.Add(_localizer["ProductNotFound"]);
				return response;
			}

			// Delete
			_db.Product.Remove(product);
			await _db.SaveChangesAsync();

			return response;
		}
	}
}


