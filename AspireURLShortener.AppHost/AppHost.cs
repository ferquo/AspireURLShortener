var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.AspireURLShortener_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.Build().Run();