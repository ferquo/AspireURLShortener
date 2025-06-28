using AspireURLShortener.ApiService;
using AspireURLShortener.ApiService.Data;
using AspireURLShortener.ApiService.Endpoints;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddNpgsqlDbContext<ApplicationDbContext>(connectionName: "postgresDb");

// Add service defaults & Aspire client integrations.
builder.Services.AddEndpoints();
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowLocalhost",
            policyBuilder =>
            {
                policyBuilder.WithOrigins("http://localhost:3000")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
    });
}


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();
app.MapHealthChecks("/health");
app.MapEndpoints();


if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowLocalhost");
    await app.ConfigureDatabaseAsync();
    app.MapOpenApi();
}

// Map the Scalar API reference endpoint.
app.MapScalarApiReference(options =>
{
    options.Servers = Array.Empty<ScalarServer>();
});

app.Run();