namespace Moedim.Mcp.Fabric.Models;

/// <summary>
/// Represents a workspace in Fabric.
/// </summary>
public class WorkspaceInfo
{
    /// <summary>
    /// Gets or sets the unique identifier of the workspace.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the workspace.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the workspace.
    /// </summary>
    public string? Description { get; set; }
}
