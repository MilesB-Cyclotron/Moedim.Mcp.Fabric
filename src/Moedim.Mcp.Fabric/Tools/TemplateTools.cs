using System.ComponentModel;
using ModelContextProtocol.Server;

namespace Moedim.Mcp.Fabric.Tools;

internal sealed class TemplateTools
{
    [McpServerTool]
    [Description("Returns a friendly greeting.")]
    public string Hello(
        [Description("Name to greet. Defaults to 'world'.")] string name = "world")
    {
        var target = string.IsNullOrWhiteSpace(name) ? "world" : name.Trim();
        return $"Hello, {target}!";
    }

    [McpServerTool]
    [Description("Returns the current UTC timestamp in ISO-8601 format.")]
    public string UtcNow()
    {
        return DateTime.UtcNow.ToString("O");
    }
}
