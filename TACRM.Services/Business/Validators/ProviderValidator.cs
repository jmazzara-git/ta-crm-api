using FluentValidation;
using Microsoft.Extensions.Localization;
using TACRM.Services.Dtos;
using TACRM.Services.Localization;

namespace TACRM.Services.Business.Validators
{
	public class ProviderValidator : AbstractValidator<ProviderDto>
	{
		public ProviderValidator(IStringLocalizer<Localizer> localizer)
		{
			// ProviderName
			RuleFor(x => x.ProviderName)
				.NotEmpty().WithMessage(localizer["ProviderNameRequired"])
				.MaximumLength(100).WithMessage(localizer["ProviderNameMaxLen"]);

			// ProviderDetails
			RuleFor(x => x.ProviderDetails)
				.MaximumLength(500).WithMessage(localizer["ProviderDetailsMaxLen"]);
		}
	}
}


