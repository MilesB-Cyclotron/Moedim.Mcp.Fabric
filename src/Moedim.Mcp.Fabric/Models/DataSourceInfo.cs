using System.Text.Json.Serialization;

namespace Moedim.Mcp.Fabric.Models;

/// <summary>
/// Represents a Power BI datasource definition for a dataset.
/// </summary>
public class DataSourceInfo
{
    /// <summary>
    /// Gets or sets the datasource type (for example, Sql, OData, AnalysisServices).
    /// </summary>
    public string? DatasourceType { get; set; }

    /// <summary>
    /// Gets or sets the datasource identifier.
    /// </summary>
    public string? DatasourceId { get; set; }

    /// <summary>
    /// Gets or sets the gateway identifier when bound to a gateway.
    /// </summary>
    public string? GatewayId { get; set; }

    /// <summary>
    /// Gets or sets the deprecated connection string value.
    /// </summary>
    public string? ConnectionString { get; set; }

    /// <summary>
    /// Gets or sets the deprecated datasource name value.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the connection details dictionary for the datasource.
    /// </summary>
    [JsonPropertyName("connectionDetails")]
    public Dictionary<string, string?> ConnectionDetails { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}
