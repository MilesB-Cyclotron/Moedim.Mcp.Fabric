using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using Moedim.Mcp.Fabric.Extensions;
using Moedim.Mcp.Fabric.Tools;

var builder = Host.CreateApplicationBuilder(args);

// Configure logging to route to stderr for MCP protocol compliance
builder.Logging.AddConsole(options => options.LogToStandardErrorThreshold = LogLevel.Information);

// Register Fabric services for dependency injection
builder.Services.AddFabricSemanticModel(builder.Configuration);

// Configure MCP server with stdio transport and Fabric tools
builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithTools<FabricSemanticModelTools>();

var host = builder.Build();

Console.WriteLine("Fabric Semantic Model MCP Server initialized");
Console.WriteLine("Available tools: query_semantic_model, list_semantic_models, get_semantic_model_metadata, aggregate_data, get_distinct_values");

await host.RunAsync();
