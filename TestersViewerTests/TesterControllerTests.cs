using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using TestersViewer.Controllers;
using Xunit.Abstractions;

namespace TestersViewerTests;

public class TesterControllerTests
{
    private readonly IDevStreamsService _devStreamsService;
    private readonly Mock<IDevStreamsService> _devStreamsServiceMock;
    private readonly Fixture _fixture;
    private readonly ILogger<TestersController> _logger;
    private readonly Mock<ILogger<TestersController>> _loggerMock;
    private readonly ITestersService _testersService;
    private readonly Mock<ITestersService> _testersServiceMock;
    private readonly ITestOutputHelper _testOutputHelper;


    public TesterControllerTests(ITestOutputHelper testOutputHelper)
    {
        _testersServiceMock = new Mock<ITestersService>();
        _devStreamsServiceMock = new Mock<IDevStreamsService>();
        _testersService = _testersServiceMock.Object;
        _devStreamsService = _devStreamsServiceMock.Object;
        _testOutputHelper = testOutputHelper;
        _fixture = new Fixture();
        _loggerMock = new Mock<ILogger<TestersController>>();
        _logger = _loggerMock.Object;
    }

    #region Index

    [Fact]
    public async Task Index_ShallReturnViewResult_WithTestersList()
    {
        var testersResponseList = _fixture.Create<List<TesterResponse>>();
        var testersController = new TestersController(_testersService, _devStreamsService, _logger);

        _testersServiceMock
            .Setup(x => x.GetFilteredTesters(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(testersResponseList);

        _testersServiceMock
            .Setup(x => x.GetSortedTesters(
                It.IsAny<List<TesterResponse>>(),
                It.IsAny<string>(),
                It.IsAny<SortOrderOptions>()))
            .ReturnsAsync(testersResponseList);

        var actionResult = await testersController.Index(
            _fixture.Create<string>(),
            _fixture.Create<string>(),
            _fixture.Create<string>(),
            _fixture.Create<SortOrderOptions>());

        var viewResult = Assert.IsType<ViewResult>(actionResult);

        viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<TesterResponse>>();
        viewResult.ViewData.Model.Should().BeEquivalentTo(testersResponseList);
    }

    #endregion

    #region Create
    
    [Fact]
    public async Task Create_ShallRedirectToIndex_IfNoModelErrorsWereFound()
    {
        var testerAddRequest = _fixture.Create<TesterAddRequest>();
        var testerResponse = _fixture.Create<TesterResponse>();
        var devStreams = _fixture.Create<List<DevStreamResponse>>();

        _devStreamsServiceMock
            .Setup(x => x.GetAllDevStreams())
            .ReturnsAsync(devStreams);

        _testersServiceMock
            .Setup(x => x.AddTester(It.IsAny<TesterAddRequest>()))
            .ReturnsAsync(testerResponse);

        var testersController = new TestersController(_testersService, _devStreamsService, _logger);

        var actionResult = await testersController.Create(testerAddRequest);

        var redirectResult = Assert.IsType<RedirectToActionResult>(actionResult);

        redirectResult.ActionName.Should().Be("Index");
    }

    #endregion
}