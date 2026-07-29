using Azure.Monitor.OpenTelemetry.Exporter;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Azure.Functions.Worker.OpenTelemetry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using TaskTrackerFunctions;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

var storageConnectionString = builder.Configuration["TASKS_STORAGE_CONNECTION_STRING"];
if (string.IsNullOrWhiteSpace(storageConnectionString))
{
    builder.Services.AddSingleton<ITaskStore, InMemoryTaskStore>();
}
else
{
    builder.Services.AddSingleton<ITaskStore>(_ => new AzureTableTaskStore(storageConnectionString));
}

var telemetry = builder.Services.AddOpenTelemetry()
    .UseFunctionsWorkerDefaults();

// Application Insights is enabled after deployment, when Azure provides this connection string.
if (!string.IsNullOrWhiteSpace(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
{
    telemetry.UseAzureMonitorExporter();
}

builder.Build().Run();
