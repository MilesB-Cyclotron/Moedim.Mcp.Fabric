namespace Moedim.Mcp.Fabric.Models;

/// <summary>
/// Represents a semantic model (dataset) in Fabric.
/// </summary>
public class SemanticModelMetadata
{
    /// <summary>
    /// Gets or sets the unique identifier of the semantic model.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the semantic model.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the display name of the semantic model.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the list of tables in the semantic model.
    /// </summary>
    public List<TableMetadata> Tables { get; set; } = new();
}
