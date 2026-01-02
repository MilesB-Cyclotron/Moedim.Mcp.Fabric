using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moedim.Mcp.Fabric.Extensions;

var builder = Host.CreateApplicationBuilder(args);

// Route logs to stderr for better visibility
builder.Logging.AddConsole(options => options.LogToStandardErrorThreshold = LogLevel.Information);

// Add Fabric services
builder.Services.AddFabricSemanticModel(builder.Configuration);

var host = builder.Build();

Console.WriteLine("Fabric Semantic Model MCP Server initialized");
Console.WriteLine("Available services: QuerySemanticModel, ListSemanticModels, GetSemanticModelMetadata, AggregateData, GetDistinctValues");

await host.RunAsync();
