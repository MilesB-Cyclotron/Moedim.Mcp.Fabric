using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using Moedim.Mcp.Fabric.Tools;

var builder = Host.CreateApplicationBuilder(args);

// Route logs to stderr; stdout is reserved for MCP protocol messages.
builder.Logging.AddConsole(options => options.LogToStandardErrorThreshold = LogLevel.Information);

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithTools<TemplateTools>();

await builder.Build().RunAsync();
