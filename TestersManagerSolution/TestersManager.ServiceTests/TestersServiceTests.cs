using System.Linq.Expressions;
using AutoFixture;
using Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using Xunit.Abstractions;

namespace TestersViewerTests;

public class TestersServiceTests
{
    private readonly IFixture _fixture;
    private readonly ITestersAdderService _testersAdderService;
    private readonly ITestersDeleterService _testersDeleterService;
    private readonly ITestersGetterService _testersGetterService;
    private readonly ITestersRepository _testersRepository;
    private readonly Mock<ITestersRepository> _testersRepositoryMock;
    private readonly ITestersSorterService _testersSorterService;
    private readonly ITestersUpdaterService _testersUpdaterService;
    private readonly ITestOutputHelper _testOutputHelper;


    public TestersServiceTests(ITestOutputHelper testOutputHelper)
    {
        _testersRepositoryMock = new Mock<ITestersRepository>();
        _testersRepository = _testersRepositoryMock.Object;
        _fixture = new Fixture();
        _testOutputHelper = testOutputHelper;

        _testersGetterService =
            new TestersGetterService(_testersRepository, new Mock<ILogger<TestersGetterService>>().Object);
        _testersAdderService =
            new TestersAdderService(_testersRepository, new Mock<ILogger<TestersAdderService>>().Object);
        _testersDeleterService =
            new TestersDeleterService(_testersRepository, new Mock<ILogger<TestersDeleterService>>().Object);
        _testersSorterService =
            new TestersSorterService(_testersRepository, new Mock<ILogger<TestersSorterService>>().Object);
        _testersUpdaterService =
            new TestersUpdaterService(_testersRepository, new Mock<ILogger<TestersUpdaterService>>().Object);
    }

    #region GetSortedTesters

    [Fact]
    public async Task GetSortedTesters_ShallReturnListWithSortedTestersByNameDesc_IfSortParamIsName()
    {
        List<Tester> testers =
        [
            _fixture.Build<Tester>()
                .With(x => x.Email, "fXw5g1@example.com")
                .With(x => x.DevStream, null as DevStream)
                .Create(),

            _fixture.Build<Tester>()
                .With(x => x.Email, "fXw5g2@example.com")
                .With(x => x.DevStream, null as DevStream)
                .Create(),
            _fixture.Build<Tester>()
                .With(x => x.Email, "fXw5g3@example.com")
                .With(x => x.DevStream, null as DevStream)
                .Create()
        ];

        var testerResponsesListExpected = testers.Select(x => x.ToTesterResponse()).ToList();

        _testersRepositoryMock.Setup(x => x.GetAllTesters()).ReturnsAsync(testers);

        var allTesters = await _testersGetterService.GetAllTesters();

        var testerResponsesFromSort = await _testersSorterService.GetSortedTesters(
            allTesters, nameof(TesterResponse.TesterName), SortOrderOptions.Desc);

        testerResponsesFromSort.Should().BeInDescendingOrder(x => x.TesterName);
    }

    #endregion

    #region AddTester

    [Fact]
    public async Task AddTester_ShallThrowArgumentNullException_IfTesterAddRequestIsNull()
    {
        TesterAddRequest? testerAddRequest = null;

        Func<Task> action = async () => await _testersAdderService.AddTester(testerAddRequest);

        await action.Should().ThrowAsync<ArgumentNullException>();
        // await Assert.ThrowsAsync<ArgumentNullException>(async () => await _testersService.AddTester(testerAddRequest));
    }

    [Fact]
    public async Task AddTester_ShallThrowArgumentException_IfTesterAddRequestNameIsNull()
    {
        var testerAddRequest = _fixture.Build<TesterAddRequest>()
            .With(x => x.TesterName, null as string) // must be null as string, otherwise - error
            .Create();

        var tester = testerAddRequest.ToTester();

        _testersRepositoryMock.Setup(x => x.AddTester(It.IsAny<Tester>())).ReturnsAsync(tester);

        Func<Task> action = async () => await _testersAdderService.AddTester(testerAddRequest);

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task
        AddTester_ShallInsertNewTesterToTheList_AndReturnAddedTesterResponseObj_IfTesterAddRequestIsValid()
    {
        var testerAddRequest = _fixture.Build<TesterAddRequest>()
            .With(x => x.Email, "fXw5g@example.com")
            .Create();

        var tester = testerAddRequest.ToTester();
        var testerResponseExpected = tester.ToTesterResponse();

        _testersRepositoryMock.Setup(x => x.AddTester(It.IsAny<Tester>())).ReturnsAsync(tester);

        var testerResponse = await _testersAdderService.AddTester(testerAddRequest);
        testerResponseExpected.TesterId = testerResponse.TesterId;

        testerResponse.TesterId.Should().NotBe(Guid.Empty);
        testerResponse.Should().Be(testerResponseExpected);
    }

    #endregion

    #region GetTesterById

    [Fact]
    public async Task GetTesterById_ShallReturnNull_IfIdIsNull()
    {
        Guid? testerId = null;
        var testerResponse = await _testersGetterService.GetTesterById(testerId);

        testerResponse.Should().BeNull();
    }

    [Fact]
    public async Task GetTesterById_ShallReturnTesterResponse_IfIdIsValid()
    {
        var tester = _fixture.Build<Tester>()
            .With(x => x.Email, "fXw5g@example.com")
            .With(x => x.DevStream, null as DevStream)
            .Create();

        var testerResponseExpected = tester.ToTesterResponse();

        _testersRepositoryMock.Setup(x => x.GetTesterById(It.IsAny<Guid>())).ReturnsAsync(tester);

        var testerResponseFromGet = await _testersGetterService.GetTesterById(tester.TesterId);

        testerResponseFromGet.Should().Be(testerResponseExpected);
    }

    #endregion

    #region GetAllTesters

    [Fact]
    public async Task GetAllTesters_ShallReturnEmptyList_BeforeTestersAreAdded()
    {
        _testersRepositoryMock.Setup(x => x.GetAllTesters()).ReturnsAsync([]);

        var allTesters = await _testersGetterService.GetAllTesters();

        allTesters.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllTesters_ShallReturnListWithAllTesters_IfTestersAreAdded()
    {
        List<Tester> testers =
        [
            _fixture.Build<Tester>()
                .With(x => x.Email, "fXw5g1@example.com")
                .With(x => x.DevStream, null as DevStream)
                .Create(),

            _fixture.Build<Tester>()
                .With(x => x.Email, "fXw5g2@example.com")
                .With(x => x.DevStream, null as DevStream)
                .Create(),
            _fixture.Build<Tester>()
                .With(x => x.Email, "fXw5g3@example.com")
                .With(x => x.DevStream, null as DevStream)
                .Create()
        ];

        var testerResponsesListExpected = testers.Select(x => x.ToTesterResponse()).ToList();

        _testersRepositoryMock.Setup(x => x.GetAllTesters()).ReturnsAsync(testers);

        var testersFromGet = await _testersGetterService.GetAllTesters();

        testersFromGet.Should().BeEquivalentTo(testerResponsesListExpected);
    }

    #endregion

    #region GetFilteredTesters

    [Fact]
    public async Task GetFilteredTesters_ShallReturnListWithAllTesters_IfSearchStringIsEmpty_AndSearchByIsTesterName()
    {
        List<Tester> testers =
        [
            _fixture.Build<Tester>()
                .With(x => x.Email, "fXw5g1@example.com")
                .With(x => x.DevStream, null as DevStream)
                .Create(),

            _fixture.Build<Tester>()
                .With(x => x.Email, "fXw5g2@example.com")
                .With(x => x.DevStream, null as DevStream)
                .Create(),
            _fixture.Build<Tester>()
                .With(x => x.Email, "fXw5g3@example.com")
                .With(x => x.DevStream, null as DevStream)
                .Create()
        ];

        var testerResponsesListExpected = testers.Select(x => x.ToTesterResponse()).ToList();

        _testersRepositoryMock.Setup(x => x.GetFilteredTesters(It.IsAny<Expression<Func<Tester, bool>>>()))
            .ReturnsAsync(testers);


        var testerResponsesFromSearch = await _testersGetterService.GetFilteredTesters(nameof(TesterResponse.TesterName), "");

        testerResponsesFromSearch.Should().BeEquivalentTo(testerResponsesListExpected);
    }

    [Fact]
    public async Task GetFilteredTesters_ShallReturnListWithFilteredTesters_IfSearchStringParamIsSet()
    {
        List<Tester> testers =
        [
            _fixture.Build<Tester>()
                .With(x => x.Email, "fXw5g1@example.com")
                .With(x => x.DevStream, null as DevStream)
                .Create(),

            _fixture.Build<Tester>()
                .With(x => x.Email, "fXw5g2@example.com")
                .With(x => x.DevStream, null as DevStream)
                .Create(),
            _fixture.Build<Tester>()
                .With(x => x.Email, "fXw5g3@example.com")
                .With(x => x.DevStream, null as DevStream)
                .Create()
        ];

        var testerResponsesListExpected = testers.Select(x => x.ToTesterResponse()).ToList();

        _testersRepositoryMock.Setup(x => x.GetFilteredTesters(It.IsAny<Expression<Func<Tester, bool>>>()))
            .ReturnsAsync(testers);


        var testerResponsesFromSearch =
            await _testersGetterService.GetFilteredTesters(nameof(TesterResponse.TesterName), "sa");

        testerResponsesFromSearch.Should().BeEquivalentTo(testerResponsesListExpected);
    }

    #endregion

    #region UpdateTester

    [Fact]
    public async Task UpdateTester_ShallThrowArgumentNullException_IfTesterUpdateRequestIsNull()
    {
        TesterUpdateRequest? testerUpdateRequest = null;

        Func<Task> action = async () => await _testersUpdaterService.UpdateTester(testerUpdateRequest);

        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdateTester_ShallThrowArgumentException_IfTesterUpdateRequestIdIsInvalid()
    {
        var testerUpdateRequest = _fixture.Build<TesterUpdateRequest>()
            .With(x => x.TesterId, Guid.NewGuid())
            .Create();

        Func<Task> action = () => _testersUpdaterService.UpdateTester(testerUpdateRequest);

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task UpdateTester_ShallThrowArgumentNullException_IfTesterUpdateRequestTesterNameIsNull()
    {
        var tester = _fixture.Build<Tester>()
            .With(x => x.Email, "fXw5g@example.com")
            .With(x => x.DevStream, null as DevStream)
            .With(x => x.Gender, "Female")
            .Create();

        var testerResponse = tester.ToTesterResponse();
        var testerUpdateRequest = testerResponse.ToTesterUpdateRequest();
        testerUpdateRequest.TesterName = null;

        Func<Task> action = () => _testersUpdaterService.UpdateTester(testerUpdateRequest);

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task UpdateTester_ShallUpdateTester_IfTesterUpdateRequestIsValid()
    {
        var tester = _fixture.Build<Tester>()
            .With(x => x.Email, "fXw5g@example.com")
            .With(x => x.DevStream, null as DevStream)
            .With(x => x.Gender, "Female")
            .Create();

        var testerResponseExpected = tester.ToTesterResponse();
        testerResponseExpected.TesterName = "Tester Updated";

        var testerUpdateRequest = testerResponseExpected.ToTesterUpdateRequest();

        _testersRepositoryMock.Setup(x => x.GetTesterById(It.IsAny<Guid>())).ReturnsAsync(tester);
        _testersRepositoryMock.Setup(x => x.UpdateTester(It.IsAny<Tester>())).ReturnsAsync(tester);

        var updatedTesterResponse = await _testersUpdaterService.UpdateTester(testerUpdateRequest);

        updatedTesterResponse.Should().Be(testerResponseExpected);
    }

    #endregion

    #region DeleteTester

    [Fact]
    public async Task DeleteTester_ShallReturnTrue_IfTesterIdIsFound()
    {
        var tester = _fixture.Build<Tester>()
            .With(x => x.Email, "fXw5g@example.com")
            .With(x => x.DevStream, null as DevStream)
            .With(x => x.Gender, "Female")
            .Create();


        _testersRepositoryMock.Setup(x => x.GetTesterById(It.IsAny<Guid>())).ReturnsAsync(tester);
        _testersRepositoryMock.Setup(x => x.DeleteTesterById(It.IsAny<Guid>())).ReturnsAsync(true);

        var isDeleted = await _testersDeleterService.DeleteTester(tester.TesterId);
        isDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteTester_ShallReturnFalse_IfTesterIdIsNotFound()
    {
        var isDeleted = await _testersDeleterService.DeleteTester(Guid.NewGuid());
        isDeleted.Should().BeFalse();
    }

    #endregion
}