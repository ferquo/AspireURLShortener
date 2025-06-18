var builder = DistributedApplication.CreateBuilder(args);

var sqlPassword = builder.AddParameter("PostgresSqlPassword", secret: true);
var sqlUsername = builder.AddParameter("PostgresSqlUsername", secret: true);

var postgres = builder.AddPostgres("postgres",sqlUsername, sqlPassword).WithDataVolume();
var postgresDb = postgres.AddDatabase("postgresDb");

var apiService = builder
    .AddProject<Projects.AspireURLShortener_ApiService>("apiService")
    .WithReference(postgresDb)
    .WaitFor(postgresDb)
    .WithHttpHealthCheck("/health");

builder.Build().Run();