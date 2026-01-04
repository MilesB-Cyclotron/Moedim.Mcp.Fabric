# Moedim.Mcp.Fabric

> Model Context Protocol (MCP) server that exposes Microsoft Fabric semantic models, tables, and DAX queries to AI assistants.

![Project Icon](img/icon.png)

[![NuGet](https://img.shields.io/nuget/v/Moedim.Mcp.Fabric.svg)](https://www.nuget.org/packages/Moedim.Mcp.Fabric)
[![Build](https://img.shields.io/badge/build-passing-brightgreen.svg)](https://github.com/kdcllc/Moedim.Mcp.Fabric/actions)

## Features

- **Semantic Model Discovery**: List workspaces, datasets, tables, and columns exposed via Fabric APIs.
- **DAX Execution**: Run DAX queries against Fabric semantic models with optional dataset overrides.
- **Aggregations & Distincts**: Built-in helpers for SUM/AVG/COUNT/MIN/MAX and distinct values.
- **Typed Responses**: Structured models for query results, metadata, and formatted text outputs.
- **MCP-Ready Tools**: Five MCP tools with full metadata/description attributes for better LLM guidance.

## Quick Start

1) **Install the NuGet package**

```bash
dotnet add package Moedim.Mcp.Fabric
```

2) **Configure services and MCP server**

```csharp
using Moedim.Mcp.Fabric.Extensions;
using Moedim.Mcp.Fabric.Tools;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddFabricSemanticModel(builder.Configuration);

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithTools<FabricSemanticModelTools>();

await builder.Build().RunAsync();
```

3) **Provide configuration** (appsettings.json or environment variables)

- `Fabric:WorkspaceId` *(required)*
- `Fabric:DefaultDatasetId` *(optional)*
- `Fabric:ApiBaseUrl` (default: `https://api.powerbi.com/v1.0/myorg`)
- `Fabric:HttpTimeoutSeconds` (default: `30`)

## Available MCP Tools

- `query_semantic_model` — Execute ad-hoc DAX queries.
- `list_semantic_models` — Enumerate datasets in the workspace.
- `get_semantic_model_metadata` — Retrieve tables and column metadata.
- `aggregate_data` — Run SUM/AVG/COUNT/MIN/MAX on a column.
- `get_distinct_values` — Get unique values for a column.

## Local Development

```bash
dotnet build
dotnet run --project src/Moedim.Mcp.Fabric/Moedim.Mcp.Fabric.csproj
```

Debugging in VS Code: use the `.NET: Debug MCP Server` launch profile.

## Packaging

- The NuGet package embeds the project README and uses the icon at `img/icon.png`.
- Pack with `dotnet pack -c Release` to produce the MCP server package.

## License

MIT License. See LICENSE.
