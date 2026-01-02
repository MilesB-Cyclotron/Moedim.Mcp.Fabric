using System.ComponentModel;
using System.Text.Json.Serialization;
using Moedim.Mcp.Fabric.Models;
using Moedim.Mcp.Fabric.Services;

namespace Moedim.Mcp.Fabric.Tools;

/// <summary>
/// Tools for accessing Microsoft Fabric Semantic Models via DAX queries.
/// </summary>
public class FabricSemanticModelTools(IFabricSemanticModelService fabricService)
{
    private readonly IFabricSemanticModelService _fabricService = fabricService ?? throw new ArgumentNullException(nameof(fabricService));

    /// <summary>
    /// Executes a DAX query against a semantic model.
    /// </summary>
    public async Task<string> QuerySemanticModel(
        string daxQuery,
        string? datasetId = null,
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
    public async Task<string> GetSemanticModelMetadata(
        string? datasetId = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _fabricService.GetSemanticModelMetadataAsync(datasetId, cancellationToken).ConfigureAwait(false);
        return result.Success
            ? (result.FormattedText ?? "No metadata found")
            : $"Error: {result.Error}";
    }

    /// <summary>
    /// Performs an aggregation operation on a column.
    /// </summary>
    public async Task<string> AggregateData(
        string tableName,
        string columnName,
        string aggregationFunction,
        string? datasetId = null,
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
    public async Task<string> GetDistinctValues(
        string tableName,
        string columnName,
        string? datasetId = null,
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
