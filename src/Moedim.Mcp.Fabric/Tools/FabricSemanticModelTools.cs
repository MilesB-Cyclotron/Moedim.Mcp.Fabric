using System.ComponentModel;
using Moedim.Mcp.Fabric.Services;
using ModelContextProtocol.Server;

namespace Moedim.Mcp.Fabric.Tools;

/// <summary>
/// Tools for accessing Microsoft Fabric Semantic Models via DAX queries.
/// </summary>
[McpServerToolType]
public class FabricSemanticModelTools(IFabricSemanticModelService fabricService)
{
    private readonly IFabricSemanticModelService _fabricService = fabricService ?? throw new ArgumentNullException(nameof(fabricService));

    /// <summary>
    /// Executes a DAX query against a semantic model.
    /// </summary>
    [McpServerTool(Name = "query_semantic_model")]
    [Description("Executes a DAX query against a Microsoft Fabric semantic model and returns the results")]
    public async Task<string> QuerySemanticModel(
        [Description("The DAX query to execute against the semantic model")] string daxQuery,
        [Description("Optional dataset ID. If not provided, uses the default dataset")] string? datasetId = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _fabricService.ExecuteDAXQueryAsync(daxQuery, datasetId, cancellationToken).ConfigureAwait(false);
        return result.Success
            ? (result.FormattedText ?? "Query executed successfully")
            : $"Error: {result.Error}";
    }

    /// <summary>
    /// Lists all semantic models (datasets) in the configured Fabric workspace.
    /// </summary>
    [McpServerTool(Name = "list_semantic_models")]
    [Description("Lists all available semantic models (datasets) in the Microsoft Fabric workspace")]
    public async Task<string> ListSemanticModels(CancellationToken cancellationToken = default)
    {
        var result = await _fabricService.ListDatasetsAsync(cancellationToken).ConfigureAwait(false);
        return result.Success
            ? (result.FormattedText ?? "No datasets found")
            : $"Error: {result.Error}";
    }

    /// <summary>
    /// Gets the metadata (tables and columns) for a semantic model.
    /// </summary>
    [McpServerTool(Name = "get_semantic_model_metadata")]
    [Description("Retrieves the metadata (tables and columns schema) for a Microsoft Fabric semantic model")]
    public async Task<string> GetSemanticModelMetadata(
        [Description("Optional dataset ID. If not provided, uses the default dataset")] string? datasetId = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _fabricService.GetSemanticModelMetadataAsync(datasetId, cancellationToken).ConfigureAwait(false);
        return result.Success
            ? (result.FormattedText ?? "No metadata found")
            : $"Error: {result.Error}";
    }

    /// <summary>
    /// Gets detailed information about a specific dataset.
    /// </summary>
    [McpServerTool(Name = "get_dataset_details")]
    [Description("Retrieves detailed information about a Microsoft Fabric dataset including configuration, refresh settings, and security properties")]
    public async Task<string> GetDatasetDetails(
        [Description("Optional dataset ID. If not provided, uses the default dataset")] string? datasetId = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _fabricService.GetDatasetDetailsAsync(datasetId, cancellationToken).ConfigureAwait(false);
        return result.Success
            ? (result.FormattedText ?? "No details found")
            : $"Error: {result.Error}";
    }

    /// <summary>
    /// Performs an aggregation operation on a column.
    /// </summary>
    [McpServerTool(Name = "aggregate_data")]
    [Description("Performs an aggregation operation (SUM, AVG, COUNT, MIN, MAX) on a column in a semantic model table")]
    public async Task<string> AggregateData(
        [Description("The name of the table containing the column")] string tableName,
        [Description("The name of the column to aggregate")] string columnName,
        [Description("The aggregation function to apply (SUM, AVG, COUNT, MIN, MAX)")] string aggregationFunction,
        [Description("Optional dataset ID. If not provided, uses the default dataset")] string? datasetId = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _fabricService.AggregateDataAsync(
            tableName,
            columnName,
            aggregationFunction,
            datasetId,
            cancellationToken).ConfigureAwait(false);
        return result.Success
            ? (result.FormattedText ?? "Aggregation completed")
            : $"Error: {result.Error}";
    }

    /// <summary>
    /// Gets distinct values from a column.
    /// </summary>
    [McpServerTool(Name = "get_distinct_values")]
    [Description("Retrieves all distinct (unique) values from a specified column in a semantic model table")]
    public async Task<string> GetDistinctValues(
        [Description("The name of the table containing the column")] string tableName,
        [Description("The name of the column to get distinct values from")] string columnName,
        [Description("Optional dataset ID. If not provided, uses the default dataset")] string? datasetId = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _fabricService.GetDistinctValuesAsync(
            tableName,
            columnName,
            datasetId,
            cancellationToken).ConfigureAwait(false);
        return result.Success
            ? (result.FormattedText ?? "No distinct values found")
            : $"Error: {result.Error}";
    }
}
