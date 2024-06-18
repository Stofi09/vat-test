using Taxually.TechnicalTest.Services.Handlers;
using Taxually.TechnicalTest.Services.Interfaces;
using Taxually.TechnicalTest.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<ITaxuallyHttpClient, TaxuallyHttpClient>();
builder.Services.AddTransient<ITaxuallyQueueClient, TaxuallyQueueClient>();
builder.Services.AddTransient<IVatRegistrationHandler, UkVatRegistrationHandler>();
builder.Services.AddTransient<IVatRegistrationHandler, FrVatRegistrationHandler>();
builder.Services.AddTransient<IVatRegistrationHandler, DeVatRegistrationHandler>();
builder.Services.AddTransient<VatRegistrationService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
