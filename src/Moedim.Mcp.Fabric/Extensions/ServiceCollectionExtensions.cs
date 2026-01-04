using System.ComponentModel.DataAnnotations;
using Moedim.Mcp.Fabric.Configuration;
using Moedim.Mcp.Fabric.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Moedim.Mcp.Fabric.Extensions;

/// <summary>
/// Extension methods for registering Fabric services in the dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Fabric Semantic Model services to the DI container with full configuration validation.
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">Application configuration</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddFabricSemanticModel(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<FabricOptions>()
            .Bind(configuration.GetSection("Fabric"))
            .Validate(options => ValidateDataAnnotations(options), "Fabric options are invalid")
            .Validate(options => !string.IsNullOrWhiteSpace(options.WorkspaceId), "Fabric:WorkspaceId must be configured")
            .Validate(options => Uri.TryCreate(options.ApiBaseUrl, UriKind.Absolute, out _), "Fabric:ApiBaseUrl must be a valid absolute URI")
            .Validate(options => options.HttpTimeoutSeconds > 0, "Fabric:HttpTimeoutSeconds must be greater than zero")
            .ValidateOnStart();

        services.AddHttpClient("FabricAPI", (sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<FabricOptions>>().Value;
            client.Timeout = TimeSpan.FromSeconds(options.HttpTimeoutSeconds);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        services.AddScoped<IFabricSemanticModelService, FabricSemanticModelService>();

        return services;
    }

    /// <summary>
    /// Validates data annotations on the FabricOptions object.
    /// </summary>
    private static bool ValidateDataAnnotations(FabricOptions options)
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(options);
        return Validator.TryValidateObject(options, context, validationResults, true);
    }
}
