namespace Moedim.Mcp.Fabric.Models;

/// <summary>
/// Represents a response from a Fabric API call.
/// </summary>
/// <typeparam name="T">The type of data returned from the API.</typeparam>
public class FabricResponse<T> where T : class
{
    /// <summary>
    /// Gets or sets a value indicating whether the operation was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the data returned from the API, if successful.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Gets or sets the error message if the operation failed.
    /// </summary>
    public string? Error { get; set; }

    /// <summary>
    /// Gets or sets the formatted text representation of the result.
    /// </summary>
    public string FormattedText { get; set; } = string.Empty;
}
