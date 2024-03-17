using Entities;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using Xunit.Abstractions;

namespace TestersViewerTests;

public class TestersServiceTests
{
    private readonly IDevStreamsService _devStreamsService;
    private readonly ITestersService _testersService;
    private readonly ITestOutputHelper _testOutputHelper;

    public TestersServiceTests(ITestOutputHelper testOutputHelper)
    {
        List<DevStream> devStreamsInitialData = [];
        List<Tester> testersInitialData = [];

        var dbContextMock = new DbContextMock<ApplicatonDbContext>(
            new DbContextOptionsBuilder<ApplicatonDbContext>().Options);

        var dbContext = dbContextMock.Object;
        dbContextMock.CreateDbSetMock(x => x.DevStreams, devStreamsInitialData);
        dbContextMock.CreateDbSetMock(x => x.Testers, testersInitialData);

        _devStreamsService = new DevStreamsService(dbContext);
        _testersService = new TestersService(dbContext, _devStreamsService);
        
        _testOutputHelper = testOutputHelper;
    }

    #region GetSortedTesters

    [Fact]
    public async Task GetSortedTesters_ShallReturnListWithSortedTestersByNameDesc_IfSortParamIsName()
    {
        var devStreamAddRequestOne = new DevStreamAddRequest { DevStreamName = "Crew" };
        var devStreamAddRequestTwo = new DevStreamAddRequest { DevStreamName = "New Year" };
        var devStreamResponseOne = await _devStreamsService.AddDevStream(devStreamAddRequestOne);
        var devStreamResponseTwo = await _devStreamsService.AddDevStream(devStreamAddRequestTwo);
        var testerAddRequestOne = new TesterAddRequest
        {
            TesterName = "Sakura",
            Email = "fXw5g@example.com",
            Gender = GenderOptions.Female,
            BirthDate = DateTime.Now,
            DevStreamId = devStreamResponseOne.DevStreamId,
            Position = "Tester",
            MonthsOfWorkExperience = 1,
            HasMobileDeviceExperience = true,
            Skills = "C#"
        };

        var testerAddRequestTwo = new TesterAddRequest
        {
            TesterName = "Sayaka",
            Email = "fXw5g@example.com",
            Gender = GenderOptions.Female,
            BirthDate = DateTime.Now,
            DevStreamId = devStreamResponseTwo.DevStreamId,
            Position = "Junior QA",
            MonthsOfWorkExperience = 1,
            HasMobileDeviceExperience = true,
            Skills = "C#"
        };

        List<TesterAddRequest> testerAddRequests =
        [
            testerAddRequestOne,
            testerAddRequestTwo
        ];

        List<TesterResponse> testerResponsesFromAdd = [];

        foreach (var testerAddRequest in testerAddRequests)
            testerResponsesFromAdd.Add(await _testersService.AddTester(testerAddRequest));

        var allTesters = await _testersService.GetAllTesters();

        var testerResponsesFromSort = await _testersService.GetSortedTesters(
            allTesters, nameof(TesterResponse.TesterName), SortOrderOptions.Desc);

        testerResponsesFromAdd = testerResponsesFromAdd
            .OrderByDescending(x => x.TesterName).ToList();

        for (var i = 0; i < testerResponsesFromAdd.Count; i++)
            Assert.Equal(testerResponsesFromAdd[i], testerResponsesFromSort[i]);

        _testOutputHelper.WriteLine("Actual:");
        foreach (var testers in testerResponsesFromSort) _testOutputHelper.WriteLine(testers.ToString());

        _testOutputHelper.WriteLine("Expected:");
        foreach (var testers in testerResponsesFromAdd) _testOutputHelper.WriteLine(testers.ToString());
    }

    #endregion

    #region AddTester

    [Fact]
    public async Task AddTester_ShallThrowArgumentNullException_IfTesterAddRequestIsNull()
    {
        TesterAddRequest? testerAddRequest = null;

        await Assert.ThrowsAsync<ArgumentNullException>(async () => await _testersService.AddTester(testerAddRequest));
    }

    [Fact]
    public async Task AddTester_ShallThrowArgumentException_IfTesterAddRequestNameIsNull()
    {
        var testerAddRequest = new TesterAddRequest { TesterName = null };

        await Assert.ThrowsAsync<ArgumentException>(async () => await _testersService.AddTester(testerAddRequest));
    }

    [Fact]
    public async Task
        AddTester_ShallInsertNewTesterToTheList_AndReturnAddedTesterResponseObj_IfTesterAddRequestIsValid()
    {
        var testerAddRequest = new TesterAddRequest
        {
            TesterName = "Tester",
            Email = "fXw5g@example.com",
            Gender = GenderOptions.Male,
            BirthDate = DateTime.Now,
            DevStreamId = Guid.NewGuid(),
            Position = "Tester",
            MonthsOfWorkExperience = 1,
            HasMobileDeviceExperience = true,
            Skills = "C#"
        };

        var testerResponse = await _testersService.AddTester(testerAddRequest);

        Assert.True(testerResponse.DevStreamId != Guid.Empty);
        Assert.Contains(testerResponse, await _testersService.GetAllTesters());
    }

    #endregion

    #region GetTesterById

    [Fact]
    public async Task GetTesterById_ShallReturnNull_IfIdIsNull()
    {
        Guid? testerId = null;
        Assert.Null(await _testersService.GetTesterById(testerId));
    }

    [Fact]
    public async Task GetTesterById_ShallReturnTesterResponse_IfIdIsValid()
    {
        var devStreamAddRequest = new DevStreamAddRequest { DevStreamName = "Crew" };
        var devStreamResponse = await _devStreamsService.AddDevStream(devStreamAddRequest);
        var testerAddRequest = new TesterAddRequest
        {
            TesterName = "Tester",
            Email = "fXw5g@example.com",
            Gender = GenderOptions.Male,
            BirthDate = DateTime.Now,
            DevStreamId = devStreamResponse.DevStreamId,
            Position = "Tester",
            MonthsOfWorkExperience = 1,
            HasMobileDeviceExperience = true,
            Skills = "C#"
        };

        var testerResponseFromAdd = await _testersService.AddTester(testerAddRequest);

        var testerResponseFromGet = await _testersService.GetTesterById(testerResponseFromAdd.TesterId);

        Assert.Equal(testerResponseFromAdd, testerResponseFromGet);
        // Assert.Contains(testerResponseFromGet, _testersService.GetAllTesters());
    }

    #endregion

    #region GetAllTesters

    [Fact]
    public async Task GetAllTesters_ShallReturnEmptyList_BeforeTestersAreAdded()
    {
        Assert.Empty(await _testersService.GetAllTesters());
    }

    [Fact]
    public async Task GetAllTesters_ShallReturnListWithAllTesters_IfTestersAreAdded()
    {
        var devStreamAddRequestOne = new DevStreamAddRequest { DevStreamName = "Crew" };
        var devStreamAddRequestTwo = new DevStreamAddRequest { DevStreamName = "New Year" };
        var devStreamResponseOne = await _devStreamsService.AddDevStream(devStreamAddRequestOne);
        var devStreamResponseTwo = await _devStreamsService.AddDevStream(devStreamAddRequestTwo);
        var testerAddRequestOne = new TesterAddRequest
        {
            TesterName = "Tester",
            Email = "fXw5g@example.com",
            Gender = GenderOptions.Male,
            BirthDate = DateTime.Now,
            DevStreamId = devStreamResponseOne.DevStreamId,
            Position = "Tester",
            MonthsOfWorkExperience = 1,
            HasMobileDeviceExperience = true,
            Skills = "C#"
        };

        var testerAddRequestTwo = new TesterAddRequest
        {
            TesterName = "Sayaka",
            Email = "fXw5g@example.com",
            Gender = GenderOptions.Female,
            BirthDate = DateTime.Now,
            DevStreamId = devStreamResponseTwo.DevStreamId,
            Position = "Junior QA",
            MonthsOfWorkExperience = 1,
            HasMobileDeviceExperience = true,
            Skills = "C#"
        };

        List<TesterAddRequest> testerAddRequests =
        [
            testerAddRequestOne,
            testerAddRequestTwo
        ];

        List<TesterResponse> testerResponsesFromAdd = [];

        foreach (var testerAddRequest in testerAddRequests)
            testerResponsesFromAdd.Add(await _testersService.AddTester(testerAddRequest));

        var testerResponsesFromGet = await _testersService.GetAllTesters();

        foreach (var testerResponse in
                 testerResponsesFromAdd)
            Assert.Contains(testerResponse, testerResponsesFromGet); // calls equals method, so compare ref not val()


        _testOutputHelper.WriteLine("Expected:");
        foreach (var testers in testerResponsesFromAdd) _testOutputHelper.WriteLine(testers.ToString());

        _testOutputHelper.WriteLine("Actual:");
        foreach (var testers in testerResponsesFromGet) _testOutputHelper.WriteLine(testers.ToString());
    }

    #endregion

    #region GetFilteredTesters

    [Fact]
    public async Task GetFilteredTesters_ShallReturnListWithAllTesters_IfSearchStringIsEmpty_AndSearchByIsTesterName()
    {
        var devStreamAddRequestOne = new DevStreamAddRequest { DevStreamName = "Crew" };
        var devStreamAddRequestTwo = new DevStreamAddRequest { DevStreamName = "New Year" };
        var devStreamResponseOne = await _devStreamsService.AddDevStream(devStreamAddRequestOne);
        var devStreamResponseTwo = await _devStreamsService.AddDevStream(devStreamAddRequestTwo);
        var testerAddRequestOne = new TesterAddRequest
        {
            TesterName = "Sakura",
            Email = "fXw5g@example.com",
            Gender = GenderOptions.Female,
            BirthDate = DateTime.Now,
            DevStreamId = devStreamResponseOne.DevStreamId,
            Position = "Tester",
            MonthsOfWorkExperience = 1,
            HasMobileDeviceExperience = true,
            Skills = "C#"
        };

        var testerAddRequestTwo = new TesterAddRequest
        {
            TesterName = "Sayaka",
            Email = "fXw5g@example.com",
            Gender = GenderOptions.Female,
            BirthDate = DateTime.Now,
            DevStreamId = devStreamResponseTwo.DevStreamId,
            Position = "Junior QA",
            MonthsOfWorkExperience = 1,
            HasMobileDeviceExperience = true,
            Skills = "C#"
        };

        List<TesterAddRequest> testerAddRequests =
        [
            testerAddRequestOne,
            testerAddRequestTwo
        ];

        List<TesterResponse> testerResponsesFromAdd = [];

        foreach (var testerAddRequest in testerAddRequests)
            testerResponsesFromAdd.Add(await _testersService.AddTester(testerAddRequest));

        var testerResponsesFromSearch = await _testersService.GetFilteredTesters(nameof(TesterResponse.TesterName), "");

        foreach (var testerResponse in testerResponsesFromAdd)
            Assert.Contains(testerResponse, testerResponsesFromSearch); // calls equals method, so compare ref not val()


        // Check what is in the list
        _testOutputHelper.WriteLine("Expected:");
        foreach (var testers in testerResponsesFromAdd) _testOutputHelper.WriteLine(testers.ToString());

        _testOutputHelper.WriteLine("Actual:");
        foreach (var testers in testerResponsesFromSearch) _testOutputHelper.WriteLine(testers.ToString());
    }

    [Fact]
    public async Task GetFilteredTesters_ShallReturnListWithFilteredTesters_IfSearchStringParamIsSet()
    {
        var devStreamAddRequestOne = new DevStreamAddRequest { DevStreamName = "Crew" };
        var devStreamAddRequestTwo = new DevStreamAddRequest { DevStreamName = "New Year" };
        var devStreamResponseOne = await _devStreamsService.AddDevStream(devStreamAddRequestOne);
        var devStreamResponseTwo = await _devStreamsService.AddDevStream(devStreamAddRequestTwo);
        var testerAddRequestOne = new TesterAddRequest
        {
            TesterName = "Sakura",
            Email = "fXw5g@example.com",
            Gender = GenderOptions.Female,
            BirthDate = DateTime.Now,
            DevStreamId = devStreamResponseOne.DevStreamId,
            Position = "Tester",
            MonthsOfWorkExperience = 1,
            HasMobileDeviceExperience = true,
            Skills = "C#"
        };

        var testerAddRequestTwo = new TesterAddRequest
        {
            TesterName = "Sayaka",
            Email = "fXw5g@example.com",
            Gender = GenderOptions.Female,
            BirthDate = DateTime.Now,
            DevStreamId = devStreamResponseTwo.DevStreamId,
            Position = "Junior QA",
            MonthsOfWorkExperience = 1,
            HasMobileDeviceExperience = true,
            Skills = "C#"
        };

        List<TesterAddRequest> testerAddRequests =
        [
            testerAddRequestOne,
            testerAddRequestTwo
        ];

        List<TesterResponse> testerResponsesFromAdd = [];

        foreach (var testerAddRequest in testerAddRequests)
            testerResponsesFromAdd.Add(await _testersService.AddTester(testerAddRequest));

        var testerResponsesFromSearch =
            await _testersService.GetFilteredTesters(nameof(TesterResponse.TesterName), "sa");

        foreach (var testerResponse in testerResponsesFromAdd)
            if (testerResponse.TesterName is not null &&
                testerResponse.TesterName.Contains("sa", StringComparison.OrdinalIgnoreCase))
                Assert.Contains(testerResponse, testerResponsesFromSearch);

        // Check what is in the list
        _testOutputHelper.WriteLine("Expected:");
        foreach (var testers in testerResponsesFromAdd) _testOutputHelper.WriteLine(testers.ToString());

        _testOutputHelper.WriteLine("Actual:");
        foreach (var testers in testerResponsesFromSearch) _testOutputHelper.WriteLine(testers.ToString());
    }

    #endregion

    #region UpdateTester

    [Fact]
    public async Task UpdateTester_ShallThrowArgumentNullException_IfTesterUpdateRequestIsNull()
    {
        TesterUpdateRequest? testerUpdateRequest = null;
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _testersService.UpdateTester(testerUpdateRequest));
    }

    [Fact]
    public async Task UpdateTester_ShallThrowArgumentException_IfTesterUpdateRequestIdIsInvalid()
    {
        var testerUpdateRequest = new TesterUpdateRequest { TesterId = Guid.NewGuid() };

        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _testersService.UpdateTester(testerUpdateRequest));
    }

    [Fact]
    public async Task UpdateTester_ShallThrowArgumentNullException_IfTesterUpdateRequestTesterNameIsNull()
    {
        var devStreamAddRequest = new DevStreamAddRequest { DevStreamName = "Crew" };
        var devStreamResponse = await _devStreamsService.AddDevStream(devStreamAddRequest);
        var testerAddRequest = new TesterAddRequest
        {
            TesterName = "Sakura",
            Email = "fXw5g@example.com",
            Gender = GenderOptions.Female,
            BirthDate = DateTime.Now,
            DevStreamId = devStreamResponse.DevStreamId,
            Position = "Tester",
            MonthsOfWorkExperience = 1,
            HasMobileDeviceExperience = true,
            Skills = "C#"
        };

        var testerResponse = await _testersService.AddTester(testerAddRequest);
        var testerUpdateRequest = testerResponse.ToTesterUpdateRequest();
        testerUpdateRequest.TesterName = null;

        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _testersService.UpdateTester(testerUpdateRequest));
    }

    [Fact]
    public async Task UpdateTester_ShallUpdateTester_IfTesterUpdateRequestIsValid()
    {
        var devStreamAddRequest = new DevStreamAddRequest { DevStreamName = "Crew" };
        var devStreamResponse = await _devStreamsService.AddDevStream(devStreamAddRequest);
        var testerAddRequest = new TesterAddRequest
        {
            TesterName = "Sakura",
            Email = "fXw5g@example.com",
            Gender = GenderOptions.Female,
            BirthDate = DateTime.Now,
            DevStreamId = devStreamResponse.DevStreamId,
            Position = "Tester",
            MonthsOfWorkExperience = 1,
            HasMobileDeviceExperience = true,
            Skills = "C#"
        };

        var testerResponse = await _testersService.AddTester(testerAddRequest);
        testerResponse.TesterName = "Villanelle";
        testerResponse.Position = "Senior QA";
        var testerUpdateRequest = testerResponse.ToTesterUpdateRequest();

        var updatedTesterResponse = await _testersService.UpdateTester(testerUpdateRequest);
        var testerFromGetById = await _testersService.GetTesterById(testerResponse.TesterId);

        Assert.Equal(updatedTesterResponse, testerFromGetById);
    }

    #endregion

    #region DeleteTester

    [Fact]
    public async Task DeleteTester_ShallReturnTrue_IfTesterIdIsFound()
    {
        var devStreamAddRequest = new DevStreamAddRequest { DevStreamName = "Crew" };
        var devStreamResponse = await _devStreamsService.AddDevStream(devStreamAddRequest);
        var testerAddRequest = new TesterAddRequest
        {
            TesterName = "Sayaka",
            Email = "fXw5g@example.com",
            Gender = GenderOptions.Female,
            BirthDate = DateTime.Now,
            DevStreamId = devStreamResponse.DevStreamId,
            Position = "Tester",
            MonthsOfWorkExperience = 1,
            HasMobileDeviceExperience = true,
            Skills = "C#"
        };

        var testerResponse = await _testersService.AddTester(testerAddRequest);

        var isDeleted = await _testersService.DeleteTester(testerResponse.TesterId);
        Assert.True(isDeleted);

        var testerFromGetById = await _testersService.GetTesterById(testerResponse.TesterId);
        Assert.Null(testerFromGetById);
    }

    [Fact]
    public async Task DeleteTester_ShallReturnFalse_IfTesterIdIsNotFound()
    {
        var isDeleted = await _testersService.DeleteTester(Guid.NewGuid());
        Assert.False(isDeleted);
    }

    #endregion
}