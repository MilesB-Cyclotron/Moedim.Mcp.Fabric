using Moedim.Mcp.Fabric.Models;

namespace Moedim.Mcp.Fabric.Services;

/// <summary>
/// Interface for Fabric Semantic Model service operations.
/// </summary>
public interface IFabricSemanticModelService
{
    /// <summary>
    /// Executes a DAX query against a semantic model.
    /// </summary>
    /// <param name="daxQuery">The DAX query to execute</param>
    /// <param name="datasetId">Optional dataset ID; uses default if not provided</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Query result containing rows and columns</returns>
    Task<FabricResponse<QueryResult>> ExecuteDAXQueryAsync(
        string daxQuery,
        string? datasetId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets metadata (tables and columns) for a semantic model.
    /// </summary>
    /// <param name="datasetId">Optional dataset ID; uses default if not provided</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of tables with their columns</returns>
    Task<FabricResponse<List<TableMetadata>>> GetSemanticModelMetadataAsync(
        string? datasetId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists all datasets (semantic models) in the workspace.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of datasets</returns>
    Task<FabricResponse<List<DatasetInfo>>> ListDatasetsAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets detailed information about a specific dataset.
    /// </summary>
    /// <param name="datasetId">Optional dataset ID; uses default if not provided</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Detailed dataset information</returns>
    Task<FabricResponse<DatasetInfo>> GetDatasetDetailsAsync(
        string? datasetId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists datasources configured for a dataset.
    /// </summary>
    /// <param name="datasetId">Optional dataset ID; uses default if not provided</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of datasources</returns>
    Task<FabricResponse<List<DataSourceInfo>>> GetDatasourcesAsync(
        string? datasetId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists parameters defined for a dataset.
    /// </summary>
    /// <param name="datasetId">Optional dataset ID; uses default if not provided</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of parameters</returns>
    Task<FabricResponse<List<DatasetParameter>>> GetDatasetParametersAsync(
        string? datasetId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists refresh history entries for a dataset.
    /// </summary>
    /// <param name="datasetId">Optional dataset ID; uses default if not provided</param>
    /// <param name="top">Optional number of entries to return</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of refresh history entries</returns>
    Task<FabricResponse<List<DatasetRefresh>>> GetRefreshHistoryAsync(
        string? datasetId = null,
        int? top = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists dataset user access entries.
    /// </summary>
    /// <param name="datasetId">Optional dataset ID; uses default if not provided</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of dataset user access entries</returns>
    Task<FabricResponse<List<DatasetUserAccess>>> GetDatasetUsersAsync(
        string? datasetId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Performs an aggregation operation on a column.
    /// </summary>
    /// <param name="tableName">Name of the table</param>
    /// <param name="columnName">Name of the column to aggregate</param>
    /// <param name="aggregationFunction">Aggregation function (SUM, COUNT, AVERAGE, MIN, MAX)</param>
    /// <param name="datasetId">Optional dataset ID; uses default if not provided</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Aggregation result</returns>
    Task<FabricResponse<AggregationResult>> AggregateDataAsync(
        string tableName,
        string columnName,
        string aggregationFunction,
        string? datasetId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets distinct values from a column.
    /// </summary>
    /// <param name="tableName">Name of the table</param>
    /// <param name="columnName">Name of the column</param>
    /// <param name="datasetId">Optional dataset ID; uses default if not provided</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of distinct values</returns>
    Task<FabricResponse<DistinctValuesResult>> GetDistinctValuesAsync(
        string tableName,
        string columnName,
        string? datasetId = null,
        CancellationToken cancellationToken = default);
}
