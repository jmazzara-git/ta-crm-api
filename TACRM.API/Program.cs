using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TACRM.Services;
using TACRM.Services.Abstractions;
using TACRM.Services.Business;
using TACRM.Services.Business.Validators;
using TACRM.Services.Core;
using TACRM.Services.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TACRMDbContext>(options =>
	options.UseNpgsql(connectionString));

builder.Services.AddScoped<ISaleProductsService, SaleProductsService>();
builder.Services.AddScoped<ISalesService, SalesService>();
builder.Services.AddScoped<ICalendarEventsService, CalendarEventsService>();
builder.Services.AddScoped<IPaymentsService, PaymentsService>();
builder.Services.AddScoped<IBudgetsService, BudgetsService>();
builder.Services.AddScoped<IProvidersService, ProvidersService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<ISubscriptionsService, SubscriptionsService>();
builder.Services.AddScoped<IUserContext, UserContext>();

// Contacts
builder.Services.AddScoped<IValidator<Contact>, ContactValidator>();
builder.Services.AddScoped<IContactsService, ContactsService>();

// Add localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
