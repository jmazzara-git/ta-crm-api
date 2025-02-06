using FluentValidation;
using Microsoft.Extensions.Localization;
using TACRM.Services.Dtos;
using TACRM.Services.Resources;

namespace TACRM.Services.Business.Validators
{
	public class ContactValidator : AbstractValidator<ContactDto>
	{
		public ContactValidator(IStringLocalizer<Messages> localizer)
		{
			// Full Name
			RuleFor(x => x.FullName)
				.NotEmpty().WithMessage(localizer["FullNameRequired"])
				.MaximumLength(255).WithMessage(localizer["FullNameMaxLength"]);

			// Email
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage(localizer["EmailRequired"])
				.MaximumLength(255).WithMessage(localizer["EmailMaxLength"])
				.EmailAddress().WithMessage(localizer["EmailInvalidFormat"]);

			// Phone
			RuleFor(x => x.Phone)
				.NotEmpty().WithMessage(localizer["PhoneRequired"])
				.MaximumLength(50).WithMessage(localizer["PhoneMaxLength"])
				.Matches(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$")
				.WithMessage(localizer["PhoneInvalidFormat"]);

			// Travel Dates
			RuleFor(x => x)
				.Must(x => x.FromDate == null || x.ToDate == null || x.ToDate >= x.FromDate)
				.WithMessage(localizer["TravelDateValidation"]);
		}
	}
}