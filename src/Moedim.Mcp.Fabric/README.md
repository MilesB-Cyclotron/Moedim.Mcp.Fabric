# Moedim MCP Template

A minimal Model Context Protocol (MCP) server starter. It wires up the MCP server host, stdio transport, and a couple of sample tools to get you going.

## Requirements

- .NET 8 SDK or later

## Quickstart

1. Install tools

```bash
dotnet tool restore
```

1. Restore & run

```bash
dotnet restore
dotnet run --project src/Moedim.Mcp.Fabric/Moedim.Mcp.Fabric.csproj
```

1. Wire MCP client

- VS Code: use .vscode/mcp.json entry for Moedim.Mcp.Fabric
- Other clients: configure stdio command `dotnet run --project src/Moedim.Mcp.Fabric/Moedim.Mcp.Fabric.csproj`

## Run locally

```bash
# from repo root after templating
dotnet run --project src/Moedim.Mcp.Fabric/Moedim.Mcp.Fabric.csproj
```

## Pack as an MCP server tool

```bash
dotnet pack src/Moedim.Mcp.Fabric/Moedim.Mcp.Fabric.csproj -c Release
```

## Tools (scaffolded)

- `hello(name?)` — simple greeting
- `utcNow()` — returns current UTC timestamp

## MCP client wiring

See .vscode/mcp.json for a ready-to-use configuration pointing at the stdio server.
