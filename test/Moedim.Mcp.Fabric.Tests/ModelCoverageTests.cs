using FluentAssertions;
using Moedim.Mcp.Fabric.Models;
using Xunit;

namespace Moedim.Mcp.Fabric.Tests;

public class ModelCoverageTests
{
    [Fact]
    public void SemanticModelMetadata_AllowsAssigningProperties()
    {
        var model = new SemanticModelMetadata
        {
            Id = "id-1",
            Name = "Name",
            DisplayName = "Display",
            Tables = { new TableMetadata { Name = "T1" } }
        };

        model.Id.Should().Be("id-1");
        model.DisplayName.Should().Be("Display");
        model.Tables.Should().ContainSingle(t => t.Name == "T1");
    }

    [Fact]
    public void WorkspaceInfo_AllowsAssigningProperties()
    {
        var workspace = new WorkspaceInfo
        {
            Id = "w1",
            Name = "Workspace",
            Description = "Test workspace"
        };

        workspace.Description.Should().Be("Test workspace");
        workspace.Name.Should().Be("Workspace");
    }
}