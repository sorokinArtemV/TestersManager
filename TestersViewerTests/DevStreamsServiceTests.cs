using Entities;
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
        _devStreamsService = new DevStreamsService(
            new TestersDbContext( 
                new DbContextOptionsBuilder<TestersDbContext>().Options));
    }


    #region AddDevStream

    [Fact]
    public void AddDevStream_ShallThrowArgumentNullException_WhenDevStreamAddRequestIsNull()
    {
        DevStreamAddRequest? request = null;

        Assert.Throws<ArgumentNullException>(() => _devStreamsService.AddDevStream(request));
    }

    [Fact]
    public void AddDevStream_ShallThrowArgumentException_WhenDevStreamAddRequestNameIsNull()
    {
        var request = new DevStreamAddRequest { DevStreamName = null };

        Assert.Throws<ArgumentNullException>(() => _devStreamsService.AddDevStream(request));
    }

    [Fact]
    public void AddDevStream_ShallThrowArgumentException_WhenDevStreamAddRequestNameIsDuplicated()
    {
        var requestOne = new DevStreamAddRequest { DevStreamName = "Crew" };
        var requestTwo = new DevStreamAddRequest { DevStreamName = "Crew" };

        Assert.Throws<ArgumentException>(() =>
        {
            _devStreamsService.AddDevStream(requestOne);
            _devStreamsService.AddDevStream(requestTwo);
        });
    }

    [Fact]
    public void AddDevStream_ShallTAddDevStreamToListOfDevStreams_WhenDevStreamAddRequestIsValid()
    {
        var request = new DevStreamAddRequest { DevStreamName = "Crew" };

        var response = _devStreamsService.AddDevStream(request);

        Assert.True(response.DevStreamId != Guid.Empty);
    }

    #endregion

    #region GetAllDevStreams

    [Fact]
    public void GetAllDevStreams_ShallBeEmpty_BeforeAddingDevStreams()
    {
        var devStreamsList = _devStreamsService.GetAllDevStreams();

        Assert.Empty(devStreamsList);
    }

    [Fact]
    public void GetAllDevStreams_ShallShowAllDevStreams_WheDevStreamsAreAdded()
    {
        List<DevStreamResponse> devStreamsExpectedResponses = [];

        List<DevStreamAddRequest> devStreamAddRequests =
        [
            new DevStreamAddRequest { DevStreamName = "Crew" },
            new DevStreamAddRequest { DevStreamName = "New Year" },
            new DevStreamAddRequest { DevStreamName = "Artillery" }
        ];

        foreach (var devStreamAddRequest in devStreamAddRequests)
            devStreamsExpectedResponses.Add(_devStreamsService.AddDevStream(devStreamAddRequest));

        var devStreamsList = _devStreamsService.GetAllDevStreams();

        foreach (var expectedResponse in
                 devStreamsExpectedResponses)
            Assert.Contains(expectedResponse, devStreamsList); // calls equals method, so compare ref not val!
    }

    #endregion

    #region GetDevStreamById

    [Fact]
    public void GetDevStreamById_ShallThrowArgumentNullException_IfDevStreamIdIsNull()
    {
        Guid? devStreamId = null;

        Assert.Throws<ArgumentNullException>(() => _devStreamsService.GetDevStreamById(devStreamId));
    }

    [Fact]
    public void GetDevStreamById_ShallReturnDevStream_IfDevStreamExists()
    {
        var devStreamAddRequest = new DevStreamAddRequest { DevStreamName = "Crew" };
        var devStreamResponse = _devStreamsService.AddDevStream(devStreamAddRequest);
        var devStreamIdExpected = devStreamResponse.DevStreamId;

        var devStreamId = _devStreamsService.GetDevStreamById(devStreamIdExpected);

        Assert.Equal(devStreamIdExpected, devStreamId?.DevStreamId);
    }

    #endregion
}