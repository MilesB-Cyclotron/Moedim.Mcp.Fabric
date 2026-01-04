using FluentAssertions;
using Moedim.Mcp.Fabric.Models;
using Moedim.Mcp.Fabric.Services;
using Moedim.Mcp.Fabric.Tools;
using Xunit;

namespace Moedim.Mcp.Fabric.Tests;

public class FabricSemanticModelToolsTests
{
    [Fact]
    public async Task QuerySemanticModel_WhenServiceSucceeds_ReturnsFormattedText()
    {
        var service = new StubService
        {
            QueryResponse = new FabricResponse<QueryResult>
            {
                Success = true,
                FormattedText = "ok"
            }
        };
        var tools = new FabricSemanticModelTools(service);

        var result = await tools.QuerySemanticModel("EVALUATE ROW('x')");

        result.Should().Be("ok");
    }

    [Fact]
    public async Task ListSemanticModels_WhenServiceFails_ReturnsError()
    {
        var service = new StubService
        {
            ListDatasetsResponse = new FabricResponse<List<DatasetInfo>>
            {
                Success = false,
                Error = "failure"
            }
        };
        var tools = new FabricSemanticModelTools(service);

        var result = await tools.ListSemanticModels();

        result.Should().Be("Error: failure");
    }

    [Fact]
    public async Task AggregateData_WhenServiceReturnsData_ForwardsFormattedText()
    {
        var service = new StubService
        {
            AggregationResponse = new FabricResponse<AggregationResult>
            {
                Success = true,
                FormattedText = "SUM(Sales.Amount)=10"
            }
        };
        var tools = new FabricSemanticModelTools(service);

        var result = await tools.AggregateData("Sales", "Amount", "SUM");

        result.Should().Be("SUM(Sales.Amount)=10");
    }

    [Fact]
    public async Task GetSemanticModelMetadata_WhenServiceSucceeds_ReturnsFormattedText()
    {
        var service = new StubService
        {
            MetadataResponse = new FabricResponse<List<TableMetadata>>
            {
                Success = true,
                FormattedText = "Tables"
            }
        };
        var tools = new FabricSemanticModelTools(service);

        var result = await tools.GetSemanticModelMetadata();

        result.Should().Be("Tables");
    }

    [Fact]
    public async Task GetDatasetDetails_WhenServiceSucceeds_ReturnsFormattedText()
    {
        var service = new StubService
        {
            DatasetDetailsResponse = new FabricResponse<DatasetInfo>
            {
                Success = true,
                FormattedText = "Details"
            }
        };
        var tools = new FabricSemanticModelTools(service);

        var result = await tools.GetDatasetDetails();

        result.Should().Be("Details");
    }

    [Fact]
    public async Task GetDatasetDatasources_WhenServiceSucceeds_ReturnsFormattedText()
    {
        var service = new StubService
        {
            DatasourcesResponse = new FabricResponse<List<DataSourceInfo>>
            {
                Success = true,
                FormattedText = "Sources"
            }
        };
        var tools = new FabricSemanticModelTools(service);

        var result = await tools.GetDatasetDatasources();

        result.Should().Be("Sources");
    }

    [Fact]
    public async Task GetDatasetParameters_WhenServiceSucceeds_ReturnsFormattedText()
    {
        var service = new StubService
        {
            ParametersResponse = new FabricResponse<List<DatasetParameter>>
            {
                Success = true,
                FormattedText = "Parameters"
            }
        };
        var tools = new FabricSemanticModelTools(service);

        var result = await tools.GetDatasetParameters();

        result.Should().Be("Parameters");
    }

    [Fact]
    public async Task GetDatasetRefreshHistory_WhenServiceSucceeds_ReturnsFormattedText()
    {
        var service = new StubService
        {
            RefreshHistoryResponse = new FabricResponse<List<DatasetRefresh>>
            {
                Success = true,
                FormattedText = "History"
            }
        };
        var tools = new FabricSemanticModelTools(service);

        var result = await tools.GetDatasetRefreshHistory();

        result.Should().Be("History");
    }

    [Fact]
    public async Task GetDatasetUsers_WhenServiceSucceeds_ReturnsFormattedText()
    {
        var service = new StubService
        {
            DatasetUsersResponse = new FabricResponse<List<DatasetUserAccess>>
            {
                Success = true,
                FormattedText = "Users"
            }
        };
        var tools = new FabricSemanticModelTools(service);

        var result = await tools.GetDatasetUsers();

        result.Should().Be("Users");
    }

    [Fact]
    public async Task GetDistinctValues_WhenServiceFails_ReturnsError()
    {
        var service = new StubService
        {
            DistinctValuesResponse = new FabricResponse<DistinctValuesResult>
            {
                Success = false,
                Error = "bad"
            }
        };
        var tools = new FabricSemanticModelTools(service);

        var result = await tools.GetDistinctValues("Sales", "Category");

        result.Should().Be("Error: bad");
    }

    private sealed class StubService : IFabricSemanticModelService
    {
        public FabricResponse<QueryResult>? QueryResponse { get; init; }
        public FabricResponse<List<TableMetadata>>? MetadataResponse { get; init; }
        public FabricResponse<List<DatasetInfo>>? ListDatasetsResponse { get; init; }
        public FabricResponse<DatasetInfo>? DatasetDetailsResponse { get; init; }
        public FabricResponse<List<DataSourceInfo>>? DatasourcesResponse { get; init; }
        public FabricResponse<List<DatasetParameter>>? ParametersResponse { get; init; }
        public FabricResponse<List<DatasetRefresh>>? RefreshHistoryResponse { get; init; }
        public FabricResponse<List<DatasetUserAccess>>? DatasetUsersResponse { get; init; }
        public FabricResponse<AggregationResult>? AggregationResponse { get; init; }
        public FabricResponse<DistinctValuesResult>? DistinctValuesResponse { get; init; }

        public Task<FabricResponse<QueryResult>> ExecuteDAXQueryAsync(string daxQuery, string? datasetId = null, CancellationToken cancellationToken = default)
            => Task.FromResult(QueryResponse!);

        public Task<FabricResponse<List<TableMetadata>>> GetSemanticModelMetadataAsync(string? datasetId = null, CancellationToken cancellationToken = default)
            => Task.FromResult(MetadataResponse!);

        public Task<FabricResponse<List<DatasetInfo>>> ListDatasetsAsync(CancellationToken cancellationToken = default)
            => Task.FromResult(ListDatasetsResponse!);

        public Task<FabricResponse<DatasetInfo>> GetDatasetDetailsAsync(string? datasetId = null, CancellationToken cancellationToken = default)
            => Task.FromResult(DatasetDetailsResponse!);

        public Task<FabricResponse<List<DataSourceInfo>>> GetDatasourcesAsync(string? datasetId = null, CancellationToken cancellationToken = default)
            => Task.FromResult(DatasourcesResponse!);

        public Task<FabricResponse<List<DatasetParameter>>> GetDatasetParametersAsync(string? datasetId = null, CancellationToken cancellationToken = default)
            => Task.FromResult(ParametersResponse!);

        public Task<FabricResponse<List<DatasetRefresh>>> GetRefreshHistoryAsync(string? datasetId = null, int? top = null, CancellationToken cancellationToken = default)
            => Task.FromResult(RefreshHistoryResponse!);

        public Task<FabricResponse<List<DatasetUserAccess>>> GetDatasetUsersAsync(string? datasetId = null, CancellationToken cancellationToken = default)
            => Task.FromResult(DatasetUsersResponse!);

        public Task<FabricResponse<AggregationResult>> AggregateDataAsync(string tableName, string columnName, string aggregationFunction, string? datasetId = null, CancellationToken cancellationToken = default)
            => Task.FromResult(AggregationResponse!);

        public Task<FabricResponse<DistinctValuesResult>> GetDistinctValuesAsync(string tableName, string columnName, string? datasetId = null, CancellationToken cancellationToken = default)
            => Task.FromResult(DistinctValuesResponse!);
    }
}