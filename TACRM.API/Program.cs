using Microsoft.EntityFrameworkCore;
using TACRM.Services;
using TACRM.Services.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TACRMDbContext>(options =>
	options.UseNpgsql(connectionString));

builder.Services.AddScoped<IContactsService, ContactsService>();
builder.Services.AddScoped<ISaleProductsService, SaleProductsService>();
builder.Services.AddScoped<ISalesService, SalesService>();
builder.Services.AddScoped<ICalendarEventsService, CalendarEventsService>();
builder.Services.AddScoped<IPaymentsService, PaymentsService>();
builder.Services.AddScoped<IBudgetsService, BudgetsService>();
builder.Services.AddScoped<IProvidersService, ProvidersService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
