namespace Moedim.Mcp.Fabric.Models;

/// <summary>
/// Represents a principal's access rights to a dataset.
/// </summary>
public class DatasetUserAccess
{
    /// <summary>
    /// Gets or sets the principal identifier (UPN for users, object ID for app/group).
    /// </summary>
    public string? Identifier { get; set; }

    /// <summary>
    /// Gets or sets the principal type (User, Group, App).
    /// </summary>
    public string? PrincipalType { get; set; }

    /// <summary>
    /// Gets or sets the dataset access right granted to the principal.
    /// </summary>
    public string? AccessRight { get; set; }
}
