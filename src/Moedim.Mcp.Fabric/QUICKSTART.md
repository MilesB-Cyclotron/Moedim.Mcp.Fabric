# Quickstart

1. Install tools

```bash
dotnet tool restore
```

2. Restore & run

```bash
dotnet restore
dotnet run --project src/Moedim.Mcp.Fabric/Moedim.Mcp.Fabric.csproj
```

3. Wire MCP client

- VS Code: use .vscode/mcp.json entry for Moedim.Mcp.Fabric
- Other clients: configure stdio command `dotnet run --project src/Moedim.Mcp.Fabric/Moedim.Mcp.Fabric.csproj`
