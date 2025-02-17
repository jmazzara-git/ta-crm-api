using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using TACRM.Services;
using TACRM.Services.Business;
using TACRM.Services.Business.Abstractions;
using TACRM.Services.Business.Validators;
using TACRM.Services.Dtos;
using TACRM.Services.Localization;

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

// Add localization
builder.Services.AddSingleton<IStringLocalizerFactory>(new LocalizerFactory(Path.Combine(AppContext.BaseDirectory, "Localization")));
builder.Services.AddLocalization();

// Contacts
builder.Services.AddScoped<IValidator<ContactDto>, ContactValidator>();
builder.Services.AddScoped<IContactService, ContactService>();

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

// Configure request localization
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