namespace Moedim.Mcp.Fabric.Models;

/// <summary>
/// Represents an aggregation result.
/// </summary>
public class AggregationResult
{
    /// <summary>
    /// Gets or sets the name of the table on which the aggregation was performed.
    /// </summary>
    public string? TableName { get; set; }

    /// <summary>
    /// Gets or sets the name of the column on which the aggregation was performed.
    /// </summary>
    public string? ColumnName { get; set; }

    /// <summary>
    /// Gets or sets the aggregation function used (e.g., SUM, AVG, COUNT, MIN, MAX).
    /// </summary>
    public string? AggregationFunction { get; set; }

    /// <summary>
    /// Gets or sets the result of the aggregation operation.
    /// </summary>
    public object? Result { get; set; }

    /// <summary>
    /// Gets or sets the formatted text representation of the aggregation result.
    /// </summary>
    public string FormattedText { get; set; } = string.Empty;
}
