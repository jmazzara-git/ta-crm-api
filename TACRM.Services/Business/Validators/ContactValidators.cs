using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using TACRM.Services.Dtos;
using TACRM.Services.Localization;

namespace TACRM.Services.Business.Validators
{
	public class ContactValidator : AbstractValidator<ContactDto>
	{
		private readonly AppDbContext _db;
		public ContactValidator(IStringLocalizer<Localizer> localizer, AppDbContext db)
		{
			_db = db;

			// Full Name
			RuleFor(x => x.FullName)
				.NotEmpty().WithMessage(localizer["FullNameRequired"])
				.MaximumLength(50).WithMessage(localizer["FullNameLen"]);

			// Email
			RuleFor(x => x.Email)
				.MaximumLength(250).WithMessage(localizer["EmailLen"])
				.EmailAddress().WithMessage(localizer["EmailFormat"])
				.When(x => !string.IsNullOrWhiteSpace(x.Email));

			// Phone
			RuleFor(x => x.Phone)
				.MaximumLength(50).WithMessage(localizer["PhoneLen"])
				.Matches(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$").WithMessage(localizer["PhoneFormat"])
				.When(x => !string.IsNullOrWhiteSpace(x.Phone));

			// ContactSourceId
			RuleFor(x => x.ContactSourceId)
				.MustAsync(ContactSourceExists).WithMessage(localizer["ContactSourceInvalid"]);

			// ContactStatusCode	
			RuleFor(x => x.ContactStatusCode)
				.MustAsync(ContactStatusExists).WithMessage(localizer["ContactStatusInvalid"]);

			// Travel Dates
			RuleFor(x => x.FromDate)
				.NotEmpty().WithMessage(localizer["FromDateRequired"]);

			RuleFor(x => x.ToDate)
				.NotEmpty().WithMessage(localizer["ToDateRequired"]);

			RuleFor(x => x)
				.Must(x => x.FromDate == null || x.ToDate == null || x.ToDate >= x.FromDate)
				.WithMessage(localizer["DateRangeInvalid"]);

			// Adults
			RuleFor(x => x.Adults)
				.GreaterThan(0)
				.LessThanOrEqualTo(10);

			// Kids
			RuleFor(x => x.Kids)
				.LessThanOrEqualTo(10);

			// Kids Ages
			RuleFor(x => x)
				.Must(x => x.Kids == 0 || (x.KidsAges != null && x.KidsAges.Length == x.Kids))
				.WithMessage(localizer["KidsAgesRequired"]);

			// Comments
			RuleFor(x => x.Comments)
				.MaximumLength(500).WithMessage(localizer["CommentsMaxLen"]);
		}

		private async Task<bool> ContactSourceExists(int? contactSourceId, CancellationToken cancellationToken)
		{
			// Contact Source is optional
			if (contactSourceId == null)
				return true;

			return await _db.ContactSource.AnyAsync(cs => cs.ContactSourceId == contactSourceId, cancellationToken);
		}
		private async Task<bool> ContactStatusExists(string contactStatusCode, CancellationToken cancellationToken)
		{
			// Contact Status is required
			if (contactStatusCode == null)
				return false;

			return await _db.ContactStatus.AnyAsync(cs => cs.ContactStatusCode == contactStatusCode, cancellationToken);
		}
	}
}