# Moedim.Mcp.Fabric - AI Agent Instructions

## Project Overview

This is a **Model Context Protocol (MCP) server** that exposes Microsoft Fabric semantic models (Power BI datasets) to AI assistants via DAX queries. Built with .NET 10.0, it follows MCP protocol standards for stdio-based communication.

**Key Architecture**: MCP server → Service layer → HTTP client → Fabric REST API

## Critical Conventions

### 1. MCP Tool Pattern

All MCP tools in [FabricSemanticModelTools.cs](../src/Moedim.Mcp.Fabric/Tools/FabricSemanticModelTools.cs) follow this mandatory structure:

```csharp
[McpServerTool(Name = "tool_name")]
[Description("Clear description for LLM understanding")]
public async Task<string> ToolMethod(
    [Description("Parameter description")] string param,
    CancellationToken cancellationToken = default)
{
    var result = await _fabricService.ServiceMethod(param, cancellationToken);
    return result.Success ? (result.FormattedText ?? "Default message") : $"Error: {result.Error}";
}
```

**Key rules**:
- Tools ALWAYS return `string` (formatted text for AI consumption)
- Use `[Description]` attributes on both methods and parameters for LLM guidance
- Service layer returns `FabricResponse<T>` with `Success`, `Data`, `Error`, and `FormattedText` properties
- Optional `datasetId` parameter defaults to configured `DefaultDatasetId`

### 2. Response Pattern

All service methods return `FabricResponse<T>` defined in [Models/FabricResponse.cs](../src/Moedim.Mcp.Fabric/Models/FabricResponse.cs):

```csharp
return new FabricResponse<QueryResult>
{
    Success = true,
    Data = result,
    FormattedText = FormatQueryResult(result)  // Human-readable output
};
```

This pattern separates structured data (`Data`) from AI-friendly text (`FormattedText`).

### 3. Configuration & Authentication

Configuration is validated at startup using [FabricOptions](../src/Moedim.Mcp.Fabric/Configuration/FabricOptions.cs):
- **Required**: `Fabric:WorkspaceId` - Must be set via appsettings.json or environment variables
- **Optional**: `Fabric:DefaultDatasetId` - Can be overridden per-query
- **Authentication**: Uses `DefaultAzureCredential` (Azure CLI, Managed Identity, VS Code, etc.)

**Setup reference**: See [QUICKSTART.md](../src/Moedim.Mcp.Fabric/QUICKSTART.md)

### 4. Dependency Injection

Register services via [ServiceCollectionExtensions.cs](../src/Moedim.Mcp.Fabric/Extensions/ServiceCollectionExtensions.cs):

```csharp
builder.Services.AddFabricSemanticModel(builder.Configuration);
builder.Services.AddMcpServer().WithStdioServerTransport().WithTools<FabricSemanticModelTools>();
```

Uses:
- `IHttpClientFactory` for HTTP communication
- `IOptions<FabricOptions>` for configuration injection
- Full validation with `ValidateOnStart()` to fail fast

### 5. Testing Standards

Follow [dotnet-architecture-good-practices.instructions.md](instructions/dotnet-architecture-good-practices.instructions.md):

**Naming convention**: `MethodName_Condition_ExpectedResult()`

Example from [FabricSemanticModelServiceTests.cs](../test/Moedim.Mcp.Fabric.Tests/FabricSemanticModelServiceTests.cs):

```csharp
[Fact]
public async Task ExecuteDAXQueryAsync_WithValidResponse_ReturnsParsedResult()
{
    var handler = new StubHttpMessageHandler(CreateJsonResponse(SampleQueryResultJson()));
    var service = CreateService(handler);

    var response = await service.ExecuteDAXQueryAsync("EVALUATE ROW('Total')");

    response.Success.Should().BeTrue();
    response.Data.Should().NotBeNull();
}
```

Use `FluentAssertions` for test assertions and stub HTTP handlers for service tests.

## Development Workflows

### Build & Run

```bash
dotnet build                                # Builds solution
dotnet run --project src/Moedim.Mcp.Fabric/Moedim.Mcp.Fabric.csproj  # Runs MCP server
```

The server outputs to **stderr** for MCP protocol compliance (see [Program.cs](../src/Moedim.Mcp.Fabric/Program.cs)).

### Testing

```bash
dotnet test                                 # Runs all tests
dotnet test --collect:"XPlat Code Coverage" # With coverage
```

Coverage reports saved to `test/*/TestResults/*/coverage.cobertura.xml`.

### Debugging in VS Code

Use the `.NET: Debug MCP Server` launch profile to attach debugger while maintaining stdio protocol.

### Packaging

```bash
dotnet pack -c Release  # Creates NuGet package with embedded README
```

Package configuration in [Directory.Build.props](../Directory.Build.props):
- `TreatWarningsAsErrors=true` - Strict compilation
- `GenerateDocumentationFile=true` - XML docs for IntelliSense
- Central package management via [Directory.Packages.props](../Directory.Packages.props)

## Project Structure

```
src/Moedim.Mcp.Fabric/
  ├── Tools/                  # MCP tool definitions (AI-facing API)
  ├── Services/               # Business logic layer
  ├── Models/                 # DTOs and response wrappers
  ├── Configuration/          # Strongly-typed options
  └── Extensions/             # DI registration helpers
test/Moedim.Mcp.Fabric.Tests/
  ├── *Tests.cs               # Unit tests with mocked dependencies
  └── TestResults/            # Coverage reports
```

## Common Pitfalls

1. **Don't return complex objects from MCP tools** - Always return formatted strings for AI consumption
2. **Don't forget optional dataset override** - All query tools accept optional `datasetId` parameter
3. **Authentication requires Azure credentials** - Ensure `az login` or other credential provider is configured
4. **Logging goes to stderr** - Console writes must not interfere with MCP stdio protocol
5. **Configuration validation happens at startup** - Invalid config causes immediate failure

## Adding New MCP Tools

1. Add method to `IFabricSemanticModelService` interface
2. Implement in `FabricSemanticModelService` with `FabricResponse<T>` return type
3. Add wrapper in `FabricSemanticModelTools` with `[McpServerTool]` and `[Description]` attributes
4. Return formatted string from tool method
5. Write tests following `MethodName_Condition_ExpectedResult()` pattern

## External Dependencies

- **ModelContextProtocol**: Version 0.5.0-preview.1 - Core MCP protocol implementation
- **Azure.Identity**: Default credential chain for Fabric authentication
- **Fabric REST API**: `https://api.powerbi.com/v1.0/myorg` (Power BI API)

## Additional Context

- **"Moedim"** (Hebrew: "appointed times") - Reflects the project's thematic naming
- **Commit format**: See [commit-message.instructions.md](instructions/commit-message.instructions.md)
- **Architecture principles**: See [dotnet-architecture-good-practices.instructions.md](instructions/dotnet-architecture-good-practices.instructions.md)
