using AutoFixture;
using Entities;
using FluentAssertions;
using Moq;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Xunit.Abstractions;

namespace TestersViewerTests;

public class DevStreamsServiceTests
{
    private readonly IDevStreamsRepository _devStreamsRepository;
    private readonly Mock<IDevStreamsRepository> _devStreamsRepositoryMock;
    private readonly IDevStreamsService _devStreamsService;
    private readonly IFixture _fixture;
    private ITestOutputHelper _testOutputHelper;

    public DevStreamsServiceTests(ITestOutputHelper testOutputHelper)
    {
        _devStreamsRepositoryMock = new Mock<IDevStreamsRepository>();
        _devStreamsRepository = _devStreamsRepositoryMock.Object;
        _fixture = new Fixture();
        _devStreamsService = new DevStreamsService(_devStreamsRepository);
        _testOutputHelper = testOutputHelper;
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

        var requestResponse = request.ToDevStream();

        _devStreamsRepositoryMock.Setup(x => x.AddDevStream(It.IsAny<DevStream>())).ReturnsAsync(requestResponse);

        Func<Task> action = async () => await _devStreamsService.AddDevStream(request);

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task AddDevStream_ShallThrowArgumentException_WhenDevStreamAddRequestNameIsDuplicated()
    {
        var request = _fixture.Build<DevStreamAddRequest>()
            .With(x => x.DevStreamName, "Crew")
            .Create();

        var requestResponse = request.ToDevStream();

        _devStreamsRepositoryMock.Setup(x => x.AddDevStream(It.Is<DevStream>(ds => ds.DevStreamName == "Crew")))
            .ThrowsAsync(new ArgumentException("A DevStream with the same name already exists."));


        var action = async () => { await _devStreamsService.AddDevStream(request); };

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
        List<DevStream> emptyDevStreamsList = [];
        _devStreamsRepositoryMock.Setup(x => x.GetAllDevStreams()).ReturnsAsync(emptyDevStreamsList);

        var devStreamsList = await _devStreamsService.GetAllDevStreams();

        devStreamsList.Should().BeEmpty(); 
    }

    [Fact]
    public async Task GetAllDevStreams_ShallShowAllDevStreams_WheDevStreamsAreAdded()
    {
        List<DevStream> devStreams = 
        [
            _fixture.Build<DevStream>().With(x => x.DevStreamName, "Crew").Create(),
            _fixture.Build<DevStream>().With(x => x.DevStreamName, "New Year").Create(),
            _fixture.Build<DevStream>().With(x => x.DevStreamName, "Artillery").Create()
        ];

        var devStreamsExpectedResponses = devStreams.Select(x => x.ToDevStreamResponse()).ToList();

        _devStreamsRepositoryMock.Setup(x => x.GetAllDevStreams()).ReturnsAsync(devStreams);
        
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