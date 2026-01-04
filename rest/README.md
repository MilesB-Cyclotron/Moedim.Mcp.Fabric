# REST API Testing

This folder contains HTTP request files for testing the Moedim.Mcp.Fabric MCP server HTTP transport.

## Prerequisites

1. **Start the MCP server in HTTP mode:**
   ```bash
   dotnet run --project src/Moedim.Mcp.Fabric/Moedim.Mcp.Fabric.csproj --http
   ```

2. **Configure Fabric settings:**
   Ensure your `appsettings.json` or environment variables include:
   - `Fabric:WorkspaceId` (required)
   - `Fabric:DefaultDatasetId` (optional)

3. **Authenticate with Azure:**
   ```bash
   az login
   ```

## MCP Protocol Format

The MCP HTTP transport uses **JSON-RPC 2.0** format with Server-Sent Events (SSE).

**Important**: HTTP requests must accept **both** content types:
- `Accept: application/json, text/event-stream`

All requests use JSON-RPC 2.0 format (POST or GET):
{
  "jsonrpc": "2.0",
  "id": 1,
  "method": "method_name",
  "params": { }
}
```

## Using HTTP Files

### VS Code (REST Client Extension)

1. Install the [REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) extension
2. Open `mcp-server.http`
3. Click "Send Request" above any `###` section
4. Start with "Initialize MCP Session" first

### IntelliJ / Rider

- HTTP files are natively supported
- Open `mcp-server.http` and use the play button next to requests

### cURL Alternative

If you prefer command-line testing, use cURL with JSON-RPC format:

```bash
# Initialize session (do this first)
curl -X POST http://localhost:5000/mcp \
  -H "Content-Type: application/json" \
  -H "Accept: application/json, text/event-stream" \
  -d '{
    "jsonrpc": "2.0",
    "id": 1,
    "method": "initialize",
    "params": {
      "protocolVersion": "2024-11-05",
      "capabilities": {},
      "clientInfo": {"name": "curl", "version": "1.0.0"}
    }
  }'

# List available tools
curl -X POST http://localhost:5000/mcp \
  -H "Content-Type: application/json" \
  -H "Accept: application/json, text/event-stream" \
  -d '{
    "jsonrpc": "2.0",
    "id": 2,
    "method": "tools/list",
    "params": {}
  }'

# Call a tool (query semantic model)
curl -X POST http://localhost:5000/mcp \
  -H "Content-Type: application/json" \
  -H "Accept: application/json, text/event-stream" \
  -d '{
    "jsonrpc": "2.0",
    "id": 3,
    "method": "tools/call",
    "params": {
      "name": "query_semantic_model",
      "arguments": {
        "daxQuery": "EVALUATE ROW(\"Total\", 1)",
        "datasetId": null
      }
    }
  }'
```

## Available MCP Methods

### Protocol Methods
- `initialize` - Initialize MCP session (required first)
- `tools/list` - List all available tools

### Tool Invocation
- `tools/call` - Execute a specific tool with parameters

## Available Tools

Use `tools/call` method with these tool names:

- `list_semantic_models` - List all datasets
- `query_semantic_model` - Execute DAX queries
- `get_semantic_model_metadata` - Get table/column schema
- `get_dataset_details` - Get dataset information
- `get_dataset_datasources` - List datasources
- `get_dataset_parameters` - List parameters
- `get_dataset_refresh_history` - Get refresh history
- `get_dataset_users` - List users with access
- `aggregate_data` - Perform aggregations (SUM/AVG/COUNT/MIN/MAX)
- `get_distinct_values` - Get distinct column values

## Custom Port

If running on a different port:

```bash
dotnet run --project src/Moedim.Mcp.Fabric/Moedim.Mcp.Fabric.csproj --http --port 8080
```

Update the `@host` variable in `mcp-server.http` or use the custom port section at the bottom of the file.

## Troubleshooting

**Connection Refused:**
- Ensure the server is running in HTTP mode
- Check that the port matches (default: 5000)

**404 Not Found:**

- Ensure you're POST to `/mcp` endpoint (not `/mcp/messages` or `/mcp/tools/...`)
- Use JSON-RPC 2.0 format with `method` and `params`
- Try the diagnostic endpoints: `GET /` and `GET /mcp` with Accept: text/event-stream

**Authentication Errors:**
- Run `az login` and verify your Azure credentials
- Ensure your account has access to the Fabric workspace

**Configuration Errors:**
- Verify `Fabric:WorkspaceId` is set in `appsettings.json` or environment variables
- Check server startup logs for configuration validation errors

**Tool Invocation Errors:**
- Ensure `datasetId` is either set in configuration as `Fabric:DefaultDatasetId` or provided in the request
- Verify the workspace and dataset IDs are correct
- Check that table/column names exist in your semantic model

## Response Format

Successful responses follow JSON-RPC 2.0 format:

```json
{
  "jsonrpc": "2.0",
  "id": 1,
  "result": {
    // Tool-specific result data
  }
}
```

Error responses:

```json
{
  "jsonrpc": "2.0",
  "id": 1,
  "error": {
    "code": -32000,
    "message": "Error message",
    "data": {}
  }
}
```
