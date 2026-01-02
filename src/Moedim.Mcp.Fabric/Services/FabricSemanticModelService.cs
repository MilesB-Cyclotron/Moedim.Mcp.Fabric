using System.Text.Json;
using Azure.Core;
using Azure.Identity;
using Moedim.Mcp.Fabric.Configuration;
using Moedim.Mcp.Fabric.Models;
using Microsoft.Extensions.Options;

namespace Moedim.Mcp.Fabric.Services;

/// <summary>
/// Service for querying Fabric Semantic Models via REST API.
/// Handles DAX query execution, metadata retrieval, and data aggregation.
/// </summary>
public class FabricSemanticModelService : IFabricSemanticModelService
{
    private readonly string _workspaceId;
    private readonly string? _defaultDatasetId;
    private readonly string _apiBaseUrl;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly TokenCredential _tokenCredential;
    private string? _cachedAccessToken;
    private DateTime _tokenExpiryTime = DateTime.MinValue;

    /// <summary>
    /// Initializes a new instance of the FabricSemanticModelService.
    /// </summary>
    /// <param name="httpClientFactory">Factory for creating HTTP clients</param>
    /// <param name="options">Fabric configuration options</param>
    public FabricSemanticModelService(
        IHttpClientFactory httpClientFactory,
        IOptions<FabricOptions> options)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));

        var fabricOptions = options.Value;
        _workspaceId = fabricOptions.WorkspaceId;
        _defaultDatasetId = fabricOptions.DefaultDatasetId;
        _apiBaseUrl = fabricOptions.ApiBaseUrl;
        _tokenCredential = new DefaultAzureCredential();
    }

    /// <summary>
    /// Executes a DAX query against a semantic model.
    /// </summary>
    public async Task<FabricResponse<QueryResult>> ExecuteDAXQueryAsync(
        string daxQuery,
        string? datasetId = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var id = datasetId ?? _defaultDatasetId;
            if (string.IsNullOrEmpty(id))
                return new FabricResponse<QueryResult>
                {
                    Success = false,
                    Error = "Dataset ID not provided and no default dataset configured"
                };

            var query = new { query = daxQuery };
            var jsonContent = JsonSerializer.Serialize(query);
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            var url = $"{_apiBaseUrl}/groups/{_workspaceId}/datasets/{id}/executeQueries";
            var response = await PostAsync(url, content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(cancellationToken);
                return new FabricResponse<QueryResult>
                {
                    Success = false,
                    Error = $"Query execution failed: {response.StatusCode} - {error}"
                };
            }

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = ParseQueryResult(responseBody);

            return new FabricResponse<QueryResult>
            {
                Success = true,
                Data = result,
                FormattedText = FormatQueryResult(result)
            };
        }
        catch (Exception ex)
        {
            return new FabricResponse<QueryResult>
            {
                Success = false,
                Error = $"Exception executing DAX query: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Gets metadata for a semantic model.
    /// </summary>
    public async Task<FabricResponse<List<TableMetadata>>> GetSemanticModelMetadataAsync(
        string? datasetId = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var id = datasetId ?? _defaultDatasetId;
            if (string.IsNullOrEmpty(id))
                return new FabricResponse<List<TableMetadata>>
                {
                    Success = false,
                    Error = "Dataset ID not provided and no default dataset configured"
                };

            var url = $"{_apiBaseUrl}/groups/{_workspaceId}/datasets/{id}/tables";
            var response = await GetAsync(url, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(cancellationToken);
                return new FabricResponse<List<TableMetadata>>
                {
                    Success = false,
                    Error = $"Failed to get metadata: {response.StatusCode} - {error}"
                };
            }

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            var tables = ParseTableMetadata(responseBody);

            return new FabricResponse<List<TableMetadata>>
            {
                Success = true,
                Data = tables,
                FormattedText = FormatTableMetadata(tables)
            };
        }
        catch (Exception ex)
        {
            return new FabricResponse<List<TableMetadata>>
            {
                Success = false,
                Error = $"Exception getting metadata: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Lists all datasets in the workspace.
    /// </summary>
    public async Task<FabricResponse<List<DatasetInfo>>> ListDatasetsAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var url = $"{_apiBaseUrl}/groups/{_workspaceId}/datasets";
            var response = await GetAsync(url, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(cancellationToken);
                return new FabricResponse<List<DatasetInfo>>
                {
                    Success = false,
                    Error = $"Failed to list datasets: {response.StatusCode} - {error}"
                };
            }

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            var datasets = ParseDatasets(responseBody);

            return new FabricResponse<List<DatasetInfo>>
            {
                Success = true,
                Data = datasets,
                FormattedText = FormatDatasets(datasets)
            };
        }
        catch (Exception ex)
        {
            return new FabricResponse<List<DatasetInfo>>
            {
                Success = false,
                Error = $"Exception listing datasets: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Performs an aggregation on a column.
    /// </summary>
    public async Task<FabricResponse<AggregationResult>> AggregateDataAsync(
        string tableName,
        string columnName,
        string aggregationFunction,
        string? datasetId = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var aggregationFunc = aggregationFunction.ToUpper();
            var daxQuery = $"EVALUATE SUMMARIZE({tableName}, \"{columnName}\", \"{aggregationFunc}\", {aggregationFunc}({tableName}[{columnName}]))";

            var queryResponse = await ExecuteDAXQueryAsync(daxQuery, datasetId, cancellationToken);

            if (!queryResponse.Success || queryResponse.Data?.Rows.Count == 0)
            {
                return new FabricResponse<AggregationResult>
                {
                    Success = false,
                    Error = queryResponse.Error ?? "No results from aggregation"
                };
            }

            var resultValue = queryResponse.Data?.Rows[0].Values.FirstOrDefault();

            return new FabricResponse<AggregationResult>
            {
                Success = true,
                Data = new AggregationResult
                {
                    TableName = tableName,
                    ColumnName = columnName,
                    AggregationFunction = aggregationFunc,
                    Result = resultValue,
                    FormattedText = $"{aggregationFunc}({tableName}.{columnName}) = {resultValue}"
                },
                FormattedText = $"{aggregationFunc}({tableName}.{columnName}) = {resultValue}"
            };
        }
        catch (Exception ex)
        {
            return new FabricResponse<AggregationResult>
            {
                Success = false,
                Error = $"Exception during aggregation: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Gets distinct values from a column.
    /// </summary>
    public async Task<FabricResponse<DistinctValuesResult>> GetDistinctValuesAsync(
        string tableName,
        string columnName,
        string? datasetId = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var daxQuery = $"EVALUATE VALUES({tableName}[{columnName}])";
            var queryResponse = await ExecuteDAXQueryAsync(daxQuery, datasetId, cancellationToken);

            if (!queryResponse.Success || queryResponse.Data == null)
            {
                return new FabricResponse<DistinctValuesResult>
                {
                    Success = false,
                    Error = queryResponse.Error ?? "Failed to get distinct values"
                };
            }

            var values = queryResponse.Data.Rows
                .SelectMany(row => row.Values)
                .ToList();

            return new FabricResponse<DistinctValuesResult>
            {
                Success = true,
                Data = new DistinctValuesResult
                {
                    TableName = tableName,
                    ColumnName = columnName,
                    Values = values,
                    FormattedText = FormatDistinctValues(tableName, columnName, values)
                },
                FormattedText = FormatDistinctValues(tableName, columnName, values)
            };
        }
        catch (Exception ex)
        {
            return new FabricResponse<DistinctValuesResult>
            {
                Success = false,
                Error = $"Exception getting distinct values: {ex.Message}"
            };
        }
    }

    // Private helper methods

    private async Task<HttpResponseMessage> GetAsync(string url, CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient("FabricAPI");
        await SetAuthorizationHeaderAsync(client, cancellationToken);
        return await client.GetAsync(url, cancellationToken);
    }

    private async Task<HttpResponseMessage> PostAsync(string url, HttpContent content, CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient("FabricAPI");
        await SetAuthorizationHeaderAsync(client, cancellationToken);
        return await client.PostAsync(url, content, cancellationToken);
    }

    private async Task SetAuthorizationHeaderAsync(HttpClient client, CancellationToken cancellationToken)
    {
        if (DateTime.UtcNow < _tokenExpiryTime && !string.IsNullOrEmpty(_cachedAccessToken))
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _cachedAccessToken);
            return;
        }

        var token = await _tokenCredential.GetTokenAsync(
            new Azure.Core.TokenRequestContext(new[] { "https://analysis.windows.net/powerbi/api/.default" }),
            cancellationToken);

        _cachedAccessToken = token.Token;
        _tokenExpiryTime = token.ExpiresOn.UtcDateTime;
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Token);
    }

    private QueryResult ParseQueryResult(string json)
    {
        var result = new QueryResult();
        try
        {
            using (JsonDocument doc = JsonDocument.Parse(json))
            {
                var root = doc.RootElement;
                if (root.TryGetProperty("results", out var results) && results.ValueKind == System.Text.Json.JsonValueKind.Array)
                {
                    foreach (var resultItem in results.EnumerateArray())
                    {
                        if (resultItem.TryGetProperty("tables", out var tables) && tables.ValueKind == System.Text.Json.JsonValueKind.Array)
                        {
                            foreach (var table in tables.EnumerateArray())
                            {
                                if (table.TryGetProperty("columns", out var columns))
                                {
                                    foreach (var col in columns.EnumerateArray())
                                    {
                                        if (col.TryGetProperty("name", out var name))
                                        {
                                            result.Columns.Add(name.GetString() ?? "");
                                        }
                                    }
                                }

                                if (table.TryGetProperty("rows", out var rows) && rows.ValueKind == System.Text.Json.JsonValueKind.Array)
                                {
                                    foreach (var row in rows.EnumerateArray())
                                    {
                                        if (row.ValueKind == System.Text.Json.JsonValueKind.Array)
                                        {
                                            var rowDict = new Dictionary<string, object?>();
                                            var index = 0;
                                            foreach (var cell in row.EnumerateArray())
                                            {
                                                if (index < result.Columns.Count)
                                                {
                                                    rowDict[result.Columns[index]] = cell.ValueKind switch
                                                    {
                                                        System.Text.Json.JsonValueKind.Number => cell.GetDecimal(),
                                                        System.Text.Json.JsonValueKind.String => cell.GetString(),
                                                        System.Text.Json.JsonValueKind.True => true,
                                                        System.Text.Json.JsonValueKind.False => false,
                                                        System.Text.Json.JsonValueKind.Null => null,
                                                        _ => cell.GetRawText()
                                                    };
                                                }
                                                index++;
                                            }
                                            result.Rows.Add(rowDict);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch
        {
            // Parse error - return empty result
        }
        return result;
    }

    private List<TableMetadata> ParseTableMetadata(string json)
    {
        var tables = new List<TableMetadata>();
        try
        {
            using (JsonDocument doc = JsonDocument.Parse(json))
            {
                var root = doc.RootElement;
                if (root.TryGetProperty("value", out var value) && value.ValueKind == System.Text.Json.JsonValueKind.Array)
                {
                    foreach (var tableItem in value.EnumerateArray())
                    {
                        var table = new TableMetadata();
                        if (tableItem.TryGetProperty("name", out var name))
                            table.Name = name.GetString();
                        if (tableItem.TryGetProperty("displayName", out var displayName))
                            table.DisplayName = displayName.GetString();

                        if (tableItem.TryGetProperty("columns", out var columns) && columns.ValueKind == System.Text.Json.JsonValueKind.Array)
                        {
                            foreach (var col in columns.EnumerateArray())
                            {
                                var column = new ColumnMetadata();
                                if (col.TryGetProperty("name", out var colName))
                                    column.Name = colName.GetString();
                                if (col.TryGetProperty("displayName", out var colDisplayName))
                                    column.DisplayName = colDisplayName.GetString();
                                if (col.TryGetProperty("dataType", out var dataType))
                                    column.DataType = dataType.GetString();

                                table.Columns.Add(column);
                            }
                        }
                        tables.Add(table);
                    }
                }
            }
        }
        catch
        {
            // Parse error
        }
        return tables;
    }

    private List<DatasetInfo> ParseDatasets(string json)
    {
        var datasets = new List<DatasetInfo>();
        try
        {
            using (JsonDocument doc = JsonDocument.Parse(json))
            {
                var root = doc.RootElement;
                if (root.TryGetProperty("value", out var value) && value.ValueKind == System.Text.Json.JsonValueKind.Array)
                {
                    foreach (var dsItem in value.EnumerateArray())
                    {
                        var dataset = new DatasetInfo();
                        if (dsItem.TryGetProperty("id", out var id))
                            dataset.Id = id.GetString();
                        if (dsItem.TryGetProperty("name", out var name))
                            dataset.Name = name.GetString();
                        if (dsItem.TryGetProperty("webUrl", out var webUrl))
                            dataset.WebUrl = webUrl.GetString();
                        datasets.Add(dataset);
                    }
                }
            }
        }
        catch
        {
            // Parse error
        }
        return datasets;
    }

    private string FormatQueryResult(QueryResult result)
    {
        if (result.Rows.Count == 0)
            return "No results";

        var lines = new List<string> { string.Join(" | ", result.Columns) };
        foreach (var row in result.Rows)
        {
            var values = result.Columns.Select(col => row.TryGetValue(col, out var val) ? val?.ToString() ?? "" : "");
            lines.Add(string.Join(" | ", values));
        }
        return string.Join("\n", lines);
    }

    private string FormatTableMetadata(List<TableMetadata> tables)
    {
        var lines = new List<string>();
        foreach (var table in tables)
        {
            lines.Add($"Table: {table.DisplayName ?? table.Name}");
            foreach (var col in table.Columns)
            {
                lines.Add($"  - {col.DisplayName ?? col.Name} ({col.DataType})");
            }
        }
        return string.Join("\n", lines);
    }

    private string FormatDatasets(List<DatasetInfo> datasets)
    {
        return string.Join("\n", datasets.Select(ds => $"• {ds.Name} (ID: {ds.Id})"));
    }

    private string FormatDistinctValues(string tableName, string columnName, List<object?> values)
    {
        var valueStrings = values.Select(v => v?.ToString() ?? "NULL");
        return $"{tableName}.{columnName}: {string.Join(", ", valueStrings)}";
    }
}
