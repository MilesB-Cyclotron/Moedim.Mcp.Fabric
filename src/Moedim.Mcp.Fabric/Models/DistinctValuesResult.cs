namespace Moedim.Mcp.Fabric.Models;

/// <summary>
/// Represents a distinct values result.
/// </summary>
public class DistinctValuesResult
{
    /// <summary>
    /// Gets or sets the name of the table from which distinct values were retrieved.
    /// </summary>
    public string? TableName { get; set; }

    /// <summary>
    /// Gets or sets the name of the column from which distinct values were retrieved.
    /// </summary>
    public string? ColumnName { get; set; }

    /// <summary>
    /// Gets or sets the list of distinct values found in the column.
    /// </summary>
    public List<object?> Values { get; set; } = new();

    /// <summary>
    /// Gets or sets the formatted text representation of the distinct values result.
    /// </summary>
    public string FormattedText { get; set; } = string.Empty;
}
