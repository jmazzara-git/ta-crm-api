using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using TACRM.Services;
using TACRM.Services.Business;
using TACRM.Services.Dtos;
using TACRM.Services.Entities;
using TACRM.Services.Enums;
using TACRM.Services.Localization;
using Xunit;

namespace TACRM.Tests.Business
{
	[Collection("ContactServiceTests")]
	public class ContactServiceTests
	{
		private readonly AppDbContext _dbContext;
		private readonly Mock<IAppUserContext> _mockUserContext;
		private readonly Mock<IValidator<ContactDto>> _mockValidator;
		private readonly Mock<IStringLocalizer<Localizer>> _mockLocalizer;
		private readonly ContactService _contactsService;
		private readonly IMapper _mapper;
		private readonly Random _random;

		public ContactServiceTests()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: "TestDatabase")
				.Options;

			_dbContext = new AppDbContext(options);
			_dbContext.Database.EnsureDeleted(); // Ensure the database is deleted before each test

			_mockUserContext = new Mock<IAppUserContext>();
			_mockValidator = new Mock<IValidator<ContactDto>>();
			_mockLocalizer = new Mock<IStringLocalizer<Localizer>>();

			_random = new Random();

			var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
			_mapper = config.CreateMapper();

			_contactsService = new ContactService(
				_dbContext,
				_mockUserContext.Object,
				_mapper,
				_mockValidator.Object,
				_mockLocalizer.Object);
		}

		[Fact]
		public async Task GetAsync_ShouldReturnContacts()
		{
			// Arrange
			var userId = 1;
			_mockUserContext.Setup(x => x.UserId).Returns(userId);

			var contacts = new List<Contact>
			{
				new Contact { ContactId = _random.Next(1, 10000), UserId = userId, FullName = "John Doe" },
				new Contact { ContactId = _random.Next(1, 10000), UserId = userId, FullName = "Jane Doe" }
			};

			_dbContext.Contact.AddRange(contacts);
			await _dbContext.SaveChangesAsync();

			// Act
			var result = await _contactsService.GetListAsync();

			// Assert
			Assert.Equal(2, result.Data.Count());
		}

		[Fact]
		public async Task GetByIdAsync_ShouldReturnContact()
		{
			// Arrange
			var userId = 1;
			var contactId = _random.Next(1, 10000);
			_mockUserContext.Setup(x => x.UserId).Returns(userId);

			var contact = new Contact { ContactId = contactId, UserId = userId, FullName = "John Doe" };

			_dbContext.Contact.Add(contact);
			await _dbContext.SaveChangesAsync();

			// Act
			var result = await _contactsService.GetByIdAsync(contactId);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(contactId, result.Data.ContactId);
		}

		[Fact]
		public async Task CreateAsync_ShouldAddContact()
		{
			// Arrange
			var userId = 1;
			_mockUserContext.Setup(x => x.UserId).Returns(userId);

			var contactDto = new ContactDto { FullName = "John Doe", UserId = userId };

			_mockValidator.Setup(x => x.ValidateAsync(contactDto, default))
				.ReturnsAsync(new ValidationResult());

			// Act
			var result = await _contactsService.CreateAsync(contactDto);

			// Assert
			Assert.Equal(userId, result.Data.UserId);
			Assert.Contains(_dbContext.Contact, c => c.FullName == "John Doe");
		}

		[Fact]
		public async Task UpdateAsync_ShouldUpdateContact()
		{
			// Arrange
			var userId = 1;
			var contactId = _random.Next(1, 10000);
			_mockUserContext.Setup(x => x.UserId).Returns(userId);

			var contact = new Contact { ContactId = contactId, UserId = userId, FullName = "John Doe" };

			_dbContext.Contact.Add(contact);
			await _dbContext.SaveChangesAsync();

			var contactDto = new ContactDto { ContactId = contactId, UserId = userId, FullName = "John Doe" };

			_mockValidator.Setup(x => x.ValidateAsync(contactDto, default))
				.ReturnsAsync(new ValidationResult());

			// Act
			await _contactsService.UpdateAsync(contactDto);

			// Assert
			var updatedContact = await _dbContext.Contact.FindAsync(contactId);
			Assert.Equal("John Doe", updatedContact.FullName);
		}

		[Fact]
		public async Task DeleteAsync_ShouldRemoveContact()
		{
			// Arrange
			var userId = 1;
			var contactId = _random.Next(1, 10000);
			_mockUserContext.Setup(x => x.UserId).Returns(userId);

			var contact = new Contact { ContactId = contactId, UserId = userId, FullName = "John Doe" };

			_dbContext.Contact.Add(contact);
			await _dbContext.SaveChangesAsync();

			// Act
			await _contactsService.DeleteAsync(contactId);

			// Assert
			Assert.DoesNotContain(_dbContext.Contact, c => c.ContactId == contactId);
		}

		[Fact]
		public async Task SearchAsync_ShouldReturnFilteredContacts()
		{
			// Arrange
			var userId = 1;
			_mockUserContext.Setup(x => x.UserId).Returns(userId);

			var contacts = new List<Contact>
			{
				new Contact { ContactId = _random.Next(1, 10000), UserId = userId, FullName = "John Doe", ContactStatusCode = ContactStatusEnum.NEW.ToString() },
				new Contact { ContactId = _random.Next(1, 10000), UserId = userId, FullName = "Jane Doe", ContactStatusCode = ContactStatusEnum.WIP.ToString() }
			};

			_dbContext.Contact.AddRange(contacts);
			await _dbContext.SaveChangesAsync();

			// Act
			var result = await _contactsService.SearchAsync(new ContactSearchRequestDto
			{
				SearchTerm = "John",
				ContactStatusCode = ContactStatusEnum.NEW.ToString()
			});

			// Assert
			Assert.Single(result.Data.Results);
			Assert.Equal(1, result.Data.Total);
		}
	}
}
