namespace Moedim.Mcp.Fabric.Models;

/// <summary>
/// Represents a Power BI dataset parameter (mashup parameter).
/// </summary>
public class DatasetParameter
{
    /// <summary>
    /// Gets or sets the parameter name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the parameter type (Text, Number, DateTime, Logical, Any).
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Gets or sets whether the parameter is required.
    /// </summary>
    public bool IsRequired { get; set; }

    /// <summary>
    /// Gets or sets the current value of the parameter.
    /// </summary>
    public string? CurrentValue { get; set; }

    /// <summary>
    /// Gets or sets the list of suggested values for the parameter.
    /// </summary>
    public List<string> SuggestedValues { get; set; } = new();
}
