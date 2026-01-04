namespace Moedim.Mcp.Fabric.Models;

/// <summary>
/// Represents a refresh history entry for a dataset.
/// </summary>
public class DatasetRefresh
{
    /// <summary>
    /// Gets or sets the refresh type (Scheduled, OnDemand, ViaApi, ViaXmlaEndpoint, ViaEnhancedApi, OnDemandTraining).
    /// </summary>
    public string? RefreshType { get; set; }

    /// <summary>
    /// Gets or sets the refresh start time in UTC.
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// Gets or sets the refresh end time in UTC.
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// Gets or sets the status of the refresh (Completed, Failed, Unknown, Disabled, Cancelled).
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Gets or sets the request identifier for correlating refresh calls.
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// Gets or sets the error payload when a refresh fails.
    /// </summary>
    public string? ServiceExceptionJson { get; set; }

    /// <summary>
    /// Gets or sets the collection of attempts made for this refresh.
    /// </summary>
    public List<RefreshAttempt> Attempts { get; set; } = new();
}

/// <summary>
/// Represents a single refresh attempt.
/// </summary>
public class RefreshAttempt
{
    /// <summary>
    /// Gets or sets the attempt identifier.
    /// </summary>
    public int? AttemptId { get; set; }

    /// <summary>
    /// Gets or sets the attempt start time in UTC.
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// Gets or sets the attempt end time in UTC.
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// Gets or sets the attempt type (Data or Query).
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Gets or sets the error payload for the attempt when available.
    /// </summary>
    public string? ServiceExceptionJson { get; set; }
}
