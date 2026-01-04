# Quickstart

## Installation

1. Install tools

```bash
dotnet tool restore
```

1. Restore dependencies

```bash
dotnet restore
```

## Running the MCP Server

The server supports two transport modes: **stdio** (default) and **HTTP/SSE**.

### Stdio Mode (Default)

Use stdio transport for local development with VS Code MCP extension:

```bash
dotnet run --project src/Moedim.Mcp.Fabric/Moedim.Mcp.Fabric.csproj
```

Or with explicit flag:

```bash
dotnet run --project src/Moedim.Mcp.Fabric/Moedim.Mcp.Fabric.csproj --stdio
```

### HTTP Mode

Use HTTP transport for stateless deployment or container environments:

```bash
# Default port 5000
dotnet run --project src/Moedim.Mcp.Fabric/Moedim.Mcp.Fabric.csproj --http

# Custom port
dotnet run --project src/Moedim.Mcp.Fabric/Moedim.Mcp.Fabric.csproj --http --port 8080

# Disable HTTPS redirection (useful behind reverse proxy)
dotnet run --project src/Moedim.Mcp.Fabric/Moedim.Mcp.Fabric.csproj --http --disable-https-redirect
```

### Environment Variables

Set transport and port via environment variables:

```bash
# Enable HTTP mode
UseHttp=true dotnet run --project src/Moedim.Mcp.Fabric/Moedim.Mcp.Fabric.csproj

# Set custom port
PORT=8080 dotnet run --project src/Moedim.Mcp.Fabric/Moedim.Mcp.Fabric.csproj --http
```

**Note**: Command-line arguments take precedence over environment variables.

### Get Help

```bash
dotnet run --project src/Moedim.Mcp.Fabric/Moedim.Mcp.Fabric.csproj --help
```

## Configuration

### Required Configuration

Set the Fabric workspace ID in `appsettings.json` or via environment variables:

```json
{
  "Fabric": {
    "WorkspaceId": "your-workspace-id",
    "DefaultDatasetId": "optional-default-dataset-id"
  }
}
```

Or using environment variables:

```bash
export Fabric__WorkspaceId="your-workspace-id"
export Fabric__DefaultDatasetId="optional-default-dataset-id"
```

### Authentication

The server uses `DefaultAzureCredential` for authentication. Ensure you're logged in:

```bash
az login
```

## VS Code MCP Client Configuration

### Configure Stdio Transport

Copy the stdio configuration to activate stdio transport:

```bash
cp .vscode/mcp.stdio.local.json .vscode/mcp.json
```

The stdio mode is the default configuration and requires no additional setup.

### Configure HTTP Local Transport

1. Start the server in HTTP mode:

   ```bash
   dotnet run --project src/Moedim.Mcp.Fabric/Moedim.Mcp.Fabric.csproj --http
   ```

1. Copy the HTTP local configuration:

   ```bash
   cp .vscode/mcp.http.local.json .vscode/mcp.json
   ```

### Configure HTTP Container Transport

1. Start the server in a container:

   ```bash
   docker run -p 8080:5000 moedim-mcp-fabric --http
   ```

1. Copy the HTTP container configuration:

   ```bash
   cp .vscode/mcp.http.container.json .vscode/mcp.json
   ```

## Available MCP Tools

The server provides the following tools for interacting with Microsoft Fabric Semantic Models:

- `query_semantic_model` - Execute DAX queries against semantic models
- `list_semantic_models` - List all available semantic models in the workspace
- `get_semantic_model_metadata` - Retrieve table/column schema for a semantic model
- `get_dataset_details` - Get detailed dataset information
- `get_dataset_datasources` - List datasources configured for a dataset
- `get_dataset_parameters` - List mashup parameters for a dataset
- `get_dataset_refresh_history` - Retrieve refresh history entries
- `get_dataset_users` - List users with access to a dataset
- `aggregate_data` - Perform aggregation operations (SUM, AVG, COUNT, MIN, MAX)
- `get_distinct_values` - Retrieve distinct values from a column

## Troubleshooting

### Authentication Issues

If you encounter authentication errors, verify you're logged into Azure CLI:

```bash
az login
az account show
```

### Configuration Errors

Ensure `Fabric:WorkspaceId` is configured:

```bash
dotnet run --project src/Moedim.Mcp.Fabric/Moedim.Mcp.Fabric.csproj
# Check startup logs for configuration validation errors
```

### HTTP Port Already in Use

If port 5000 is in use, specify a different port:

```bash
dotnet run --project src/Moedim.Mcp.Fabric/Moedim.Mcp.Fabric.csproj --http --port 8080
```
