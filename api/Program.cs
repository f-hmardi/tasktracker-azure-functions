using Azure.Monitor.OpenTelemetry.Exporter;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Azure.Functions.Worker.OpenTelemetry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry;
using TaskTrackerFunctions;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

var sqlConnectionString = builder.Configuration["TASKS_SQL_CONNECTION_STRING"];
var storageConnectionString = builder.Configuration["TASKS_STORAGE_CONNECTION_STRING"];

if (!string.IsNullOrWhiteSpace(sqlConnectionString))
{
    builder.Services.AddDbContextFactory<TaskDbContext>(options =>
        options.UseSqlServer(sqlConnectionString, sqlOptions => sqlOptions.EnableRetryOnFailure()));
    builder.Services.AddSingleton<ITaskStore, SqlTaskStore>();
}
else if (!string.IsNullOrWhiteSpace(storageConnectionString))
{
    builder.Services.AddSingleton<ITaskStore>(_ => new AzureTableTaskStore(storageConnectionString));
}
else
{
    builder.Services.AddSingleton<ITaskStore, InMemoryTaskStore>();
}

var telemetry = builder.Services.AddOpenTelemetry()
    .UseFunctionsWorkerDefaults();

// Application Insights is enabled after deployment, when Azure provides this connection string.
if (!string.IsNullOrWhiteSpace(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
{
    telemetry.UseAzureMonitorExporter();
}

builder.Build().Run();
