using FluentValidation;
using Microsoft.Extensions.Localization;
using TACRM.Services.Entities;
using TACRM.Services.Resources;

namespace TACRM.Services.Business.Validators
{
	public class BudgetValidator : AbstractValidator<Budget>
	{
		public BudgetValidator(IStringLocalizer<Messages> localizer)
		{
			RuleFor(b => b.BudgetName)
				.NotEmpty().WithMessage(localizer["BudgetNameRequired"])
				.MaximumLength(100).WithMessage(localizer["BudgetNameMaxLength"]);

			RuleFor(b => b.BudgetDetails)
				.MaximumLength(500).WithMessage(localizer["BudgetDetailsMaxLength"]);

			RuleFor(b => b.Adults)
				.GreaterThanOrEqualTo(0).WithMessage(localizer["AdultsMinValue"]);

			RuleFor(b => b.Kids)
				.GreaterThanOrEqualTo(0).WithMessage(localizer["KidsMinValue"]);

			RuleFor(b => b.UserId)
				.GreaterThan(0).WithMessage(localizer["UserIdMinValue"]);

			RuleFor(b => b.ContactId)
				.GreaterThan(0).WithMessage(localizer["ContactIdMinValue"]);

			RuleFor(b => b.BudgetGUID)
				.NotEmpty().WithMessage(localizer["BudgetGUIDRequired"]);

			RuleFor(b => b.CreatedAt)
				.NotEmpty().WithMessage(localizer["CreatedAtRequired"]);

			RuleFor(b => b.KidsAges)
				.MaximumLength(100).WithMessage(localizer["KidsAgesMaxLength"]);
		}
	}
}
