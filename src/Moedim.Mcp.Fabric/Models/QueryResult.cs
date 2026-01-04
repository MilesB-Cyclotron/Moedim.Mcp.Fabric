namespace Moedim.Mcp.Fabric.Models;

/// <summary>
/// Represents the result of a query execution.
/// </summary>
public class QueryResult
{
    /// <summary>
    /// Gets or sets the list of rows returned by the query.
    /// Each row is represented as a dictionary of column names to values.
    /// </summary>
    public List<Dictionary<string, object?>> Rows { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of column names in the query result.
    /// </summary>
    public List<string> Columns { get; set; } = new();

    /// <summary>
    /// Gets or sets the formatted text representation of the query result.
    /// </summary>
    public string FormattedText { get; set; } = string.Empty;
}
