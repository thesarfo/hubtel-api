using Hubtel.Api.Contracts;
using Hubtel.Api.Data;
using Hubtel.Api.Services;
using Hubtel.Api.Utils.Exceptions;

var builder = WebApplication.CreateBuilder(args);
var connString = builder.Configuration.GetConnectionString("Hubtel");


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSqlite<WalletContext>(connString);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<IWalletValidationService, WalletValidationService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthorization();

app.MapControllers();

app.Run();