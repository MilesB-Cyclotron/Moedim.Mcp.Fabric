namespace Moedim.Mcp.Fabric.Models;

/// <summary>
/// Represents a dataset in the workspace.
/// </summary>
public class DatasetInfo
{
    /// <summary>
    /// Gets or sets the unique identifier of the dataset.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the dataset.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the web URL of the dataset.
    /// </summary>
    public string? WebUrl { get; set; }
}
