using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moedim.Mcp.Fabric.Configuration;
using Moedim.Mcp.Fabric.Extensions;
using Moedim.Mcp.Fabric.Services;
using Xunit;

namespace Moedim.Mcp.Fabric.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddFabricSemanticModel_WithValidConfig_RegistersDependencies()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Fabric:WorkspaceId"] = "workspace-1",
                ["Fabric:DefaultDatasetId"] = "dataset-1",
                ["Fabric:ApiBaseUrl"] = "https://api.powerbi.com/v1.0/myorg",
                ["Fabric:HttpTimeoutSeconds"] = "45"
            })
            .Build();

        var services = new ServiceCollection();
        services.AddFabricSemanticModel(configuration);

        var provider = services.BuildServiceProvider();

        provider.GetRequiredService<IFabricSemanticModelService>().Should().NotBeNull();
        var options = provider.GetRequiredService<IOptions<FabricOptions>>().Value;
        options.WorkspaceId.Should().Be("workspace-1");
        options.HttpTimeoutSeconds.Should().Be(45);

        var client = provider.GetRequiredService<IHttpClientFactory>().CreateClient("FabricAPI");
        client.Timeout.Should().Be(TimeSpan.FromSeconds(45));
        client.DefaultRequestHeaders.Accept.ToString().Should().Contain("application/json");
    }

    [Fact]
    public void AddFabricSemanticModel_WithMissingWorkspaceId_ThrowsValidationException()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Fabric:ApiBaseUrl"] = "https://api.powerbi.com/v1.0/myorg"
            })
            .Build();

        var services = new ServiceCollection();
        services.AddFabricSemanticModel(configuration);

        var provider = services.BuildServiceProvider();

        Action action = () => provider.GetRequiredService<IOptions<FabricOptions>>().Value.WorkspaceId.Should().NotBeNull();

        action.Should().Throw<OptionsValidationException>();
    }
}