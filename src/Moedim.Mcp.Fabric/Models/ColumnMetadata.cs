namespace Moedim.Mcp.Fabric.Models;

/// <summary>
/// Represents a column in a semantic model table.
/// </summary>
public class ColumnMetadata
{
    /// <summary>
    /// Gets or sets the name of the column.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the display name of the column.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the data type of the column.
    /// </summary>
    public string? DataType { get; set; }
}
