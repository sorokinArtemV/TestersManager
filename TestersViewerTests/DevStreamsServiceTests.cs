using Entities;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace TestersViewerTests;

public class DevStreamsServiceTests
{
    private readonly IDevStreamsService _devStreamsService;

    public DevStreamsServiceTests()
    {
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

        await Assert.ThrowsAsync<ArgumentNullException>(async () => await _devStreamsService.AddDevStream(request));
    }

    [Fact]
    public async Task AddDevStream_ShallThrowArgumentException_WhenDevStreamAddRequestNameIsNull()
    {
        var request = new DevStreamAddRequest { DevStreamName = null };

        await Assert.ThrowsAsync<ArgumentNullException>(async () => await _devStreamsService.AddDevStream(request));
    }

    [Fact]
    public async Task AddDevStream_ShallThrowArgumentException_WhenDevStreamAddRequestNameIsDuplicated()
    {
        var requestOne = new DevStreamAddRequest { DevStreamName = "Crew" };
        var requestTwo = new DevStreamAddRequest { DevStreamName = "Crew" };

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _devStreamsService.AddDevStream(requestOne);
            await _devStreamsService.AddDevStream(requestTwo);
        });
    }

    [Fact]
    public async Task AddDevStream_ShallTAddDevStreamToListOfDevStreams_WhenDevStreamAddRequestIsValid()
    {
        var request = new DevStreamAddRequest { DevStreamName = "Crew" };

        var response = await _devStreamsService.AddDevStream(request);

        Assert.True(response.DevStreamId != Guid.Empty);
    }

    #endregion

    #region GetAllDevStreams

    [Fact]
    public async Task GetAllDevStreams_ShallBeEmpty_BeforeAddingDevStreams()
    {
        var devStreamsList = await _devStreamsService.GetAllDevStreams();

        Assert.Empty(devStreamsList);
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

        foreach (var expectedResponse in
                 devStreamsExpectedResponses)
            Assert.Contains(expectedResponse, devStreamsList); // calls equals method, so compare ref not val!
    }

    #endregion

    #region GetDevStreamById

    [Fact]
    public async Task GetDevStreamById_ShallThrowArgumentNullException_IfDevStreamIdIsNull()
    {
        Guid? devStreamId = null;

        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _devStreamsService.GetDevStreamById(devStreamId));
    }

    [Fact]
    public async Task GetDevStreamById_ShallReturnDevStream_IfDevStreamExists()
    {
        var devStreamAddRequest = new DevStreamAddRequest { DevStreamName = "Crew" };
        var devStreamResponse = await _devStreamsService.AddDevStream(devStreamAddRequest);
        var devStreamIdExpected = devStreamResponse.DevStreamId;

        var devStreamId = await _devStreamsService.GetDevStreamById(devStreamIdExpected);

        Assert.Equal(devStreamIdExpected, devStreamId?.DevStreamId);
    }

    #endregion
}