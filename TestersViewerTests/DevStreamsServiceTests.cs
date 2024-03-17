using AutoFixture;
using Entities;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace TestersViewerTests;

public class DevStreamsServiceTests
{
    private readonly IDevStreamsService _devStreamsService;
    private readonly IFixture _fixture;

    public DevStreamsServiceTests()
    {
        _fixture = new Fixture();

        List<DevStream> devStreamsInitialData = [];

        var dbContextMock = new DbContextMock<ApplicatonDbContext>(
            new DbContextOptionsBuilder<ApplicatonDbContext>().Options);

        var dbContext = dbContextMock.Object;
        dbContextMock.CreateDbSetMock(x => x.DevStreams, devStreamsInitialData);

        _devStreamsService = new DevStreamsService(dbContext);
    }

    #region AddDevStream

    [Fact]
    public async Task AddDevStream_ShallThrowArgumentNullException_WhenDevStreamAddRequestIsNull()
    {
        DevStreamAddRequest? request = null;

        Func<Task> action = async () => await _devStreamsService.AddDevStream(request);

        await action.Should().ThrowAsync<ArgumentNullException>();
    }


    [Fact]
    public async Task AddDevStream_ShallThrowArgumentException_WhenDevStreamAddRequestNameIsNull()
    {
        var request = _fixture.Build<DevStreamAddRequest>()
            .With(x => x.DevStreamName, null as string)
            .Create();

        Func<Task> action = async () => await _devStreamsService.AddDevStream(request);

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task AddDevStream_ShallThrowArgumentException_WhenDevStreamAddRequestNameIsDuplicated()
    {
        var requestOne = _fixture.Build<DevStreamAddRequest>()
            .With(x => x.DevStreamName, "Crew")
            .Create();
        var requestTwo = _fixture.Build<DevStreamAddRequest>()
            .With(x => x.DevStreamName, "Crew")
            .Create();

        Func<Task> action = async () =>
        {
            await _devStreamsService.AddDevStream(requestOne);
            await _devStreamsService.AddDevStream(requestTwo);
        };

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task AddDevStream_ShallTAddDevStreamToListOfDevStreams_WhenDevStreamAddRequestIsValid()
    {
        var request = _fixture.Create<DevStreamAddRequest>();

        var response = await _devStreamsService.AddDevStream(request);

        response.DevStreamId.Should().NotBeEmpty();
    }

    #endregion

    #region GetAllDevStreams

    [Fact]
    public async Task GetAllDevStreams_ShallBeEmpty_BeforeAddingDevStreams()
    {
        var devStreamsList = await _devStreamsService.GetAllDevStreams();

        devStreamsList.Should().BeEmpty();
        
    }

    [Fact]
    public async Task GetAllDevStreams_ShallShowAllDevStreams_WheDevStreamsAreAdded()
    {
        List<DevStreamResponse> devStreamsExpectedResponses = [];

        List<DevStreamAddRequest> devStreamAddRequests =
        [
            new DevStreamAddRequest { DevStreamName = "Crew" },
            new DevStreamAddRequest { DevStreamName = "New Year" },
            new DevStreamAddRequest { DevStreamName = "Artillery" }
        ];

        foreach (var devStreamAddRequest in devStreamAddRequests)
            devStreamsExpectedResponses.Add(await _devStreamsService.AddDevStream(devStreamAddRequest));

        var devStreamsList = await _devStreamsService.GetAllDevStreams();
        
        devStreamsList.Should().BeEquivalentTo(devStreamsExpectedResponses);
    }

    #endregion

    #region GetDevStreamById

    [Fact]
    public async Task GetDevStreamById_ShallThrowArgumentNullException_IfDevStreamIdIsNull()
    {
        Guid? devStreamId = null;
        
        Func<Task> action = async () => await _devStreamsService.GetDevStreamById(devStreamId);
        
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task GetDevStreamById_ShallReturnDevStream_IfDevStreamExists()
    {
        var devStreamAddRequest = _fixture.Create<DevStreamAddRequest>();
        var devStreamResponse = await _devStreamsService.AddDevStream(devStreamAddRequest);
        var devStreamIdExpected = devStreamResponse.DevStreamId;

        var devStreamId = await _devStreamsService.GetDevStreamById(devStreamIdExpected);

        devStreamId?.DevStreamId.Should().Be(devStreamIdExpected);
    }

    #endregion
}