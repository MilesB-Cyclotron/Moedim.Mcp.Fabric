namespace Moedim.Mcp.Fabric.Models;

/// <summary>
/// Represents a table in a semantic model.
/// </summary>
public class TableMetadata
{
    /// <summary>
    /// Gets or sets the name of the table.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the display name of the table.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the list of columns in the table.
    /// </summary>
    public List<ColumnMetadata> Columns { get; set; } = new();
}
