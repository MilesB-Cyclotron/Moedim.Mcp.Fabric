# Moedim.Mcp.Fabric

> Model Context Protocol (MCP) server that exposes Microsoft Fabric semantic models, tables, and DAX queries to AI assistants.

[![NuGet](https://img.shields.io/nuget/v/Moedim.Mcp.Fabric.svg)](https://www.nuget.org/packages/Moedim.Mcp.Fabric)
[![Build](https://img.shields.io/badge/build-passing-brightgreen.svg)](https://github.com/kdcllc/Moedim.Mcp.Fabric/actions)

![Stand With Israel](img/IStandWithIsrael.png)

> "Moedim" is a Hebrew word that translates to "feast" or "appointed time." The feasts are signals and signs to help us know what is on the heart of HaShem.

## Hire me

Please send [email](mailto:info@kingdavidconsulting.com) if you consider to **hire me**.

[![buymeacoffee](https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png)](https://www.buymeacoffee.com/vyve0og)

## Give a Star! :star:

If you like or are using this project to learn or start your solution, please give it a star. Thanks!

## Features

- **Semantic Model Discovery**: List workspaces, datasets, tables, and columns exposed via Fabric APIs.
- **DAX Execution**: Run DAX queries against Fabric semantic models with optional dataset overrides.
- **Aggregations & Distincts**: Built-in helpers for SUM/AVG/COUNT/MIN/MAX and distinct values.
- **Typed Responses**: Structured models for query results, metadata, and formatted text outputs.
- **MCP-Ready Tools**: Five MCP tools with full metadata/description attributes for better LLM guidance.

## Quick Start

### Installation

```bash
dotnet add package Moedim.Mcp.Fabric
```

### Configuration

Provide configuration via appsettings.json or environment variables:

- `Fabric:WorkspaceId` *(required)*
- `Fabric:DefaultDatasetId` *(optional)*
- `Fabric:ApiBaseUrl` (default: `https://api.powerbi.com/v1.0/myorg`)
- `Fabric:HttpTimeoutSeconds` (default: `30`)

### Transport Modes

The server supports two transport modes:

#### Stdio Mode (Default)

Use stdio transport for local development with VS Code MCP extension:

```bash
dotnet run --project src/Moedim.Mcp.Fabric/Moedim.Mcp.Fabric.csproj
```

Or configure in your MCP client:

```json
{
  "servers": {
    "Moedim.Mcp.Fabric": {
      "type": "stdio",
      "command": "dotnet",
      "args": ["run", "--project", "Moedim.Mcp.Fabric.csproj"],
      "cwd": "${workspaceFolder}/src/Moedim.Mcp.Fabric"
    }
  }
}
```

#### HTTP Mode

Use HTTP transport for stateless deployment or container environments:

```bash
# Default port 5000
dotnet run --project src/Moedim.Mcp.Fabric/Moedim.Mcp.Fabric.csproj --http

# Custom port
dotnet run --project src/Moedim.Mcp.Fabric/Moedim.Mcp.Fabric.csproj --http --port 8080
```

Or configure in your MCP client:

```json
{
  "servers": {
    "Moedim.Mcp.Fabric": {
      "type": "http",
      "url": "http://localhost:5000/mcp"
    }
  }
}
```

For complete details on command-line options and environment variables, run:

```bash
dotnet run --project src/Moedim.Mcp.Fabric/Moedim.Mcp.Fabric.csproj --help
```

Or see [QUICKSTART.md](src/Moedim.Mcp.Fabric/QUICKSTART.md).

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

## References

- [text](https://github.com/modelcontextprotocol/csharp-sdk)
- [Model Context Protocol .NET Samples](https://github.com/microsoft/mcp-dotnet-samples)
- [Create a minimal MCP server using C# and publish to NuGet](https://learn.microsoft.com/en-us/dotnet/ai/quickstarts/build-mcp-server)
- [modelcontextprotocol/csharp-sdk](https://github.com/modelcontextprotocol/csharp-sdk)
