using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TACRM.Services;
using TACRM.Services.Business;
using TACRM.Services.Business.Abstractions;
using TACRM.Services.Business.Validators;
using TACRM.Services.Dtos;

var builder = WebApplication.CreateBuilder(args);

#region Services
// Http context
builder.Services.AddHttpContextAccessor();

// Add DB context
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")));

// Add user context
builder.Services.AddScoped<IAppUserContext, AppUserContext>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Contacts
builder.Services.AddScoped<IValidator<ContactDto>, ContactValidator>();
builder.Services.AddScoped<IContactService, ContactService>();

// Add localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(builder =>
	{
		builder.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader();
	});
});
#endregion

#region App

var app = builder.Build();

var supportedCultures = new[] { "en", "es" };

var localizationOptions = new RequestLocalizationOptions()
	.SetDefaultCulture("en")
	.AddSupportedCultures(supportedCultures)
	.AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

#endregion