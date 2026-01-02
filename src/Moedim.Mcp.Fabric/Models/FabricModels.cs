namespace Moedim.Mcp.Fabric.Models;

/// <summary>
/// Represents a response from a Fabric API call.
/// </summary>
public class FabricResponse<T> where T : class
{
    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// The data returned from the API, if successful.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Error message if the operation failed.
    /// </summary>
    public string? Error { get; set; }

    /// <summary>
    /// Formatted text representation of the result.
    /// </summary>
    public string FormattedText { get; set; } = string.Empty;
}

/// <summary>
/// Represents a table in a semantic model.
/// </summary>
public class TableMetadata
{
    public string? Name { get; set; }
    public string? DisplayName { get; set; }
    public List<ColumnMetadata> Columns { get; set; } = new();
}

/// <summary>
/// Represents a column in a semantic model table.
/// </summary>
public class ColumnMetadata
{
    public string? Name { get; set; }
    public string? DisplayName { get; set; }
    public string? DataType { get; set; }
}

/// <summary>
/// Represents a semantic model (dataset) in Fabric.
/// </summary>
public class SemanticModelMetadata
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? DisplayName { get; set; }
    public List<TableMetadata> Tables { get; set; } = new();
}

/// <summary>
/// Represents a dataset in the workspace.
/// </summary>
public class DatasetInfo
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? WebUrl { get; set; }
}

/// <summary>
/// Represents a workspace in Fabric.
/// </summary>
public class WorkspaceInfo
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}

/// <summary>
/// Represents the result of a query execution.
/// </summary>
public class QueryResult
{
    public List<Dictionary<string, object?>> Rows { get; set; } = new();
    public List<string> Columns { get; set; } = new();
    public string FormattedText { get; set; } = string.Empty;
}

/// <summary>
/// Represents aggregation result.
/// </summary>
public class AggregationResult
{
    public string? TableName { get; set; }
    public string? ColumnName { get; set; }
    public string? AggregationFunction { get; set; }
    public object? Result { get; set; }
    public string FormattedText { get; set; } = string.Empty;
}

/// <summary>
/// Represents distinct values result.
/// </summary>
public class DistinctValuesResult
{
    public string? TableName { get; set; }
    public string? ColumnName { get; set; }
    public List<object?> Values { get; set; } = new();
    public string FormattedText { get; set; } = string.Empty;
}
