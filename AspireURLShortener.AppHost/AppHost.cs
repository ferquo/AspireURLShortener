var builder = DistributedApplication.CreateBuilder(args);

// Add the postgres database.
var sqlPassword = builder.AddParameter("PostgresSqlPassword", secret: true);
var sqlUsername = builder.AddParameter("PostgresSqlUsername", secret: true);
var postgres = builder
    .AddPostgres("postgres",sqlUsername, sqlPassword)
    .WithDataVolume()
    .WithPgWeb(pgWeb => pgWeb.WithHostPort(5050));
var postgresDb = postgres.AddDatabase("postgresDb");

// Add the Aspire URL Shortener API service.
var apiService = builder
    .AddProject<Projects.AspireURLShortener_ApiService>("apiService")
    .WithReference(postgresDb)
    .WaitFor(postgresDb)
    .WithHttpHealthCheck("/health");

builder.Build().Run();