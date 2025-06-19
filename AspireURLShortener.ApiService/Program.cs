using AspireURLShortener.ApiService;
using AspireURLShortener.ApiService.Data;
using AspireURLShortener.ApiService.Endpoints;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddNpgsqlDbContext<ApplicationDbContext>(connectionName: "postgresDb");

// Add DB context via aspire

// Add service defaults & Aspire client integrations.
builder.Services.AddEndpoints();
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();
app.MapHealthChecks("/health");
app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
    await app.ConfigureDatabaseAsync();
    app.MapOpenApi();
}

// Map the Scalar API reference endpoint.
app.MapScalarApiReference(options =>
{
    options.Servers = Array.Empty<ScalarServer>();
});

app.Run();