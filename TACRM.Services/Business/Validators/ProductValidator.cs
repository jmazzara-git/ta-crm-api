using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using TACRM.Services.Dtos;
using TACRM.Services.Localization;

namespace TACRM.Services.Business.Validators
{
	public class ProductValidator : AbstractValidator<ProductDto>
	{
		private readonly AppDbContext _db;
		public ProductValidator(IStringLocalizer<Localizer> localizer, AppDbContext db)
		{
			_db = db;

			// ProductName
			RuleFor(x => x.ProductName)
				.NotEmpty().WithMessage(localizer["ProductNameRequired"])
				.MaximumLength(100).WithMessage(localizer["ProductNameMaxLen"]);

			// ProductDetails
			RuleFor(x => x.ProductDetails)
				.MaximumLength(500).WithMessage(localizer["ProductDetailsMaxLen"]);

			// ProductTypeCode
			RuleFor(x => x.ProductTypeCode)
				.MustAsync(ProductTypeExists).WithMessage(localizer["ProductTypeInvalid"]);

		}

		private async Task<bool> ProductTypeExists(string productTypeCode, CancellationToken cancellationToken)
		{
			// Product Type is required
			if (productTypeCode == null)
				return false;

			return await _db.ProductType.AnyAsync(x => x.ProductTypeCode == productTypeCode, cancellationToken);
		}
	}
}


