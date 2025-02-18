using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using TACRM.Services;
using TACRM.Services.Business.Validators;
using TACRM.Services.Dtos;
using TACRM.Services.Localization;
using Xunit;

namespace TACRM.Tests.Business.Validators
{
	public class ContactValidatorTests
	{
		private readonly ContactValidator _validator;
		private readonly AppDbContext _dbContext;

		private readonly Mock<IStringLocalizer<Localizer>> _localizerMock;

		public ContactValidatorTests()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: "TestDatabase")
				.Options;

			_dbContext = new AppDbContext(options);
			_localizerMock = new Mock<IStringLocalizer<Localizer>>();

			// Setup localization messages
			_localizerMock.Setup(l => l["FullNameRequired"]).Returns(new LocalizedString("FullNameRequired", "Full name is required."));
			_localizerMock.Setup(l => l["FullNameLength"]).Returns(new LocalizedString("FullNameLength", "Full name must be less than 50 characters."));
			_localizerMock.Setup(l => l["EmailLength"]).Returns(new LocalizedString("EmailLength", "Email must be less than 250 characters."));
			_localizerMock.Setup(l => l["EmailFormat"]).Returns(new LocalizedString("EmailFormat", "Invalid email format."));
			_localizerMock.Setup(l => l["PhoneLength"]).Returns(new LocalizedString("PhoneLength", "Phone number must be less than 50 characters."));
			_localizerMock.Setup(l => l["PhoneFormat"]).Returns(new LocalizedString("PhoneFormat", "Invalid phone number format."));
			_localizerMock.Setup(l => l["ContactSourceInvalid"]).Returns(new LocalizedString("ContactSourceInvalid", "The specified contact source does not exist."));
			_localizerMock.Setup(l => l["ContactStatusInvalid"]).Returns(new LocalizedString("ContactStatusInvalid", "The specified contact status does not exist."));
			_localizerMock.Setup(l => l["FromDateRequired"]).Returns(new LocalizedString("FromDateRequired", "From date is required."));
			_localizerMock.Setup(l => l["ToDateRequired"]).Returns(new LocalizedString("ToDateRequired", "To date is required."));
			_localizerMock.Setup(l => l["DateRangeInvalid"]).Returns(new LocalizedString("DateRangeInvalid", "To date must be greater than or equal to from date."));
			_localizerMock.Setup(l => l["KidsAgesRequired"]).Returns(new LocalizedString("KidsAgesRequired", "The number of kids' ages must match the number of kids."));
			_localizerMock.Setup(l => l["CommentsMaxLen"]).Returns(new LocalizedString("CommentsMaxLen", "Comments must be less than 500 characters."));
			_localizerMock.Setup(l => l["AdultsRequired"]).Returns(new LocalizedString("AdultsRequired", "Adults must be between 1 and 10."));
			_localizerMock.Setup(l => l["KidsRequired"]).Returns(new LocalizedString("KidsRequired", "Kids must be between 0 and 10."));

			_validator = new ContactValidator(_localizerMock.Object, _dbContext);
		}

		[Fact]
		public async Task Should_Have_Error_When_FullName_Is_Empty()
		{
			var model = new ContactDto { FullName = string.Empty };
			var result = await _validator.TestValidateAsync(model);
			result.ShouldHaveValidationErrorFor(x => x.FullName).WithErrorMessage("Full name is required.");
		}

		[Fact]
		public async Task Should_Have_Error_When_FullName_Exceeds_Max_Length()
		{
			var model = new ContactDto { FullName = new string('a', 51) };
			var result = await _validator.TestValidateAsync(model);
			result.ShouldHaveValidationErrorFor(x => x.FullName).WithErrorMessage("Full name must be less than 50 characters.");
		}

		[Fact]
		public async Task Should_Have_Error_When_Email_Exceeds_Max_Length()
		{
			var model = new ContactDto { Email = new string('a', 251) };
			var result = await _validator.TestValidateAsync(model);
			result.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage("Email must be less than 250 characters.");
		}

		[Fact]
		public async Task Should_Have_Error_When_Email_Is_Invalid()
		{
			var model = new ContactDto { Email = "invalid-email" };
			var result = await _validator.TestValidateAsync(model);
			result.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage("Invalid email format.");
		}

		[Fact]
		public async Task Should_Have_Error_When_Phone_Exceeds_Max_Length()
		{
			var model = new ContactDto { Phone = new string('a', 51) };
			var result = await _validator.TestValidateAsync(model);
			result.ShouldHaveValidationErrorFor(x => x.Phone).WithErrorMessage("Phone number must be less than 50 characters.");
		}

		[Fact]
		public async Task Should_Have_Error_When_Phone_Is_Invalid()
		{
			var model = new ContactDto { Phone = "invalid-phone" };
			var result = await _validator.TestValidateAsync(model);
			result.ShouldHaveValidationErrorFor(x => x.Phone).WithErrorMessage("Invalid phone number format.");
		}

		[Fact]
		public async Task Should_Have_Error_When_FromDate_Is_Empty()
		{
			var model = new ContactDto { FromDate = null };
			var result = await _validator.TestValidateAsync(model);
			result.ShouldHaveValidationErrorFor(x => x.FromDate).WithErrorMessage("From date is required.");
		}

		[Fact]
		public async Task Should_Have_Error_When_ToDate_Is_Empty()
		{
			var model = new ContactDto { ToDate = null };
			var result = await _validator.TestValidateAsync(model);
			result.ShouldHaveValidationErrorFor(x => x.ToDate).WithErrorMessage("To date is required.");
		}

		[Fact]
		public async Task Should_Have_Error_When_ToDate_Is_Less_Than_FromDate()
		{
			var model = new ContactDto { FromDate = new DateOnly(2023, 1, 2), ToDate = new DateOnly(2023, 1, 1) };
			var result = await _validator.TestValidateAsync(model);
			result.ShouldHaveValidationErrorFor(x => x).WithErrorMessage("To date must be greater than or equal to from date.");
		}

		[Fact]
		public async Task Should_Have_Error_When_KidsAges_Does_Not_Match_Kids_Count()
		{
			var model = new ContactDto { Kids = 2, KidsAges = new int[] { 5 } };
			var result = await _validator.TestValidateAsync(model);
			result.ShouldHaveValidationErrorFor(x => x).WithErrorMessage("The number of kids' ages must match the number of kids.");
		}

		[Fact]
		public async Task Should_Have_Error_When_Comments_Exceed_Max_Length()
		{
			var model = new ContactDto { Comments = new string('a', 501) };
			var result = await _validator.TestValidateAsync(model);
			result.ShouldHaveValidationErrorFor(x => x.Comments).WithErrorMessage("Comments must be less than 500 characters.");
		}

		[Fact]
		public async Task Should_Have_Error_When_Adults_Less_Than_One()
		{
			var model = new ContactDto { Adults = 0 };
			var result = await _validator.TestValidateAsync(model);
			result.ShouldHaveValidationErrorFor(x => x.Adults).WithErrorMessage("Adults must be between 1 and 10.");
		}

		[Fact]
		public async Task Should_Have_Error_When_Adults_Greater_Than_Ten()
		{
			var model = new ContactDto { Adults = 11 };
			var result = await _validator.TestValidateAsync(model);
			result.ShouldHaveValidationErrorFor(x => x.Adults).WithErrorMessage("Adults must be between 1 and 10.");
		}

		[Fact]
		public async Task Should_Have_Error_When_Kids_Less_Than_Zero()
		{
			var model = new ContactDto { Kids = -1 };
			var result = await _validator.TestValidateAsync(model);
			result.ShouldHaveValidationErrorFor(x => x.Kids).WithErrorMessage("Kids must be between 0 and 10.");
		}

		[Fact]
		public async Task Should_Have_Error_When_Kids_Greater_Than_Ten()
		{
			var model = new ContactDto { Kids = 11 };
			var result = await _validator.TestValidateAsync(model);
			result.ShouldHaveValidationErrorFor(x => x.Kids).WithErrorMessage("Kids must be between 0 and 10.");
		}
	}
}