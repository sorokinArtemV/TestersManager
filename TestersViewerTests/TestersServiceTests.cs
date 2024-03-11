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
        _testOutputHelper = testOutputHelper;
        _testersService = new TestersService();
        _devStreamsService = new DevStreamsService(false);
    }

    #region GetSortedTesters

    [Fact]
    public void GetSortedTesters_ShallReturnListWithSortedTestersByNameDesc_IfSortParamIsName()
    {
        var devStreamAddRequestOne = new DevStreamAddRequest { DevStreamName = "Crew" };
        var devStreamAddRequestTwo = new DevStreamAddRequest { DevStreamName = "New Year" };
        var devStreamResponseOne = _devStreamsService.AddDevStream(devStreamAddRequestOne);
        var devStreamResponseTwo = _devStreamsService.AddDevStream(devStreamAddRequestTwo);
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
            testerResponsesFromAdd.Add(_testersService.AddTester(testerAddRequest));

        var allTesters = _testersService.GetAllTesters();

        var testerResponsesFromSort = _testersService.GetSortedTesters(
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
    public void AddTester_ShallThrowArgumentNullException_IfTesterAddRequestIsNull()
    {
        TesterAddRequest? testerAddRequest = null;

        Assert.Throws<ArgumentNullException>(() => _testersService.AddTester(testerAddRequest));
    }

    [Fact]
    public void AddTester_ShallThrowArgumentException_IfTesterAddRequestNameIsNull()
    {
        var testerAddRequest = new TesterAddRequest { TesterName = null };

        Assert.Throws<ArgumentException>(() => _testersService.AddTester(testerAddRequest));
    }

    [Fact]
    public void AddTester_ShallInsertNewTesterToTheList_AndReturnAddedTesterResponseObj_IfTesterAddRequestIsValid()
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

        var testerResponse = _testersService.AddTester(testerAddRequest);

        Assert.True(testerResponse.DevStreamId != Guid.Empty);
        Assert.Contains(testerResponse, _testersService.GetAllTesters());
    }

    #endregion

    #region GetTesterById

    [Fact]
    public void GetTesterById_ShallReturnNull_IfIdIsNull()
    {
        Guid? testerId = null;
        Assert.Null(_testersService.GetTesterById(testerId));
    }

    [Fact]
    public void GetTesterById_ShallReturnTesterResponse_IfIdIsValid()
    {
        var devStreamAddRequest = new DevStreamAddRequest { DevStreamName = "Crew" };
        var devStreamResponse = _devStreamsService.AddDevStream(devStreamAddRequest);
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

        var testerResponseFromAdd = _testersService.AddTester(testerAddRequest);

        var testerResponseFromGet = _testersService.GetTesterById(testerResponseFromAdd.TesterId);

        Assert.Equal(testerResponseFromAdd, testerResponseFromGet);
        // Assert.Contains(testerResponseFromGet, _testersService.GetAllTesters());
    }

    #endregion

    #region GetAllTesters

    [Fact]
    public void GetAllTesters_ShallReturnEmptyList_BeforeTestersAreAdded()
    {
        Assert.Empty(_testersService.GetAllTesters());
    }

    [Fact]
    public void GetAllTesters_ShallReturnListWithAllTesters_IfTestersAreAdded()
    {
        var devStreamAddRequestOne = new DevStreamAddRequest { DevStreamName = "Crew" };
        var devStreamAddRequestTwo = new DevStreamAddRequest { DevStreamName = "New Year" };
        var devStreamResponseOne = _devStreamsService.AddDevStream(devStreamAddRequestOne);
        var devStreamResponseTwo = _devStreamsService.AddDevStream(devStreamAddRequestTwo);
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
            testerResponsesFromAdd.Add(_testersService.AddTester(testerAddRequest));

        var testerResponsesFromGet = _testersService.GetAllTesters();

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
    public void GetFilteredTesters_ShallReturnListWithAllTesters_IfSearchStringIsEmpty_AndSearchByIsTesterName()
    {
        var devStreamAddRequestOne = new DevStreamAddRequest { DevStreamName = "Crew" };
        var devStreamAddRequestTwo = new DevStreamAddRequest { DevStreamName = "New Year" };
        var devStreamResponseOne = _devStreamsService.AddDevStream(devStreamAddRequestOne);
        var devStreamResponseTwo = _devStreamsService.AddDevStream(devStreamAddRequestTwo);
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
            testerResponsesFromAdd.Add(_testersService.AddTester(testerAddRequest));

        var testerResponsesFromSearch = _testersService.GetFilteredTesters(nameof(TesterResponse.TesterName), "");

        foreach (var testerResponse in testerResponsesFromAdd)
            Assert.Contains(testerResponse, testerResponsesFromSearch); // calls equals method, so compare ref not val()


        // Check what is in the list
        _testOutputHelper.WriteLine("Expected:");
        foreach (var testers in testerResponsesFromAdd) _testOutputHelper.WriteLine(testers.ToString());

        _testOutputHelper.WriteLine("Actual:");
        foreach (var testers in testerResponsesFromSearch) _testOutputHelper.WriteLine(testers.ToString());
    }

    [Fact]
    public void GetFilteredTesters_ShallReturnListWithFilteredTesters_IfSearchStringParamIsSet()
    {
        var devStreamAddRequestOne = new DevStreamAddRequest { DevStreamName = "Crew" };
        var devStreamAddRequestTwo = new DevStreamAddRequest { DevStreamName = "New Year" };
        var devStreamResponseOne = _devStreamsService.AddDevStream(devStreamAddRequestOne);
        var devStreamResponseTwo = _devStreamsService.AddDevStream(devStreamAddRequestTwo);
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
            testerResponsesFromAdd.Add(_testersService.AddTester(testerAddRequest));

        var testerResponsesFromSearch = _testersService.GetFilteredTesters(nameof(TesterResponse.TesterName), "sa");

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
    public void UpdateTester_ShallThrowArgumentNullException_IfTesterUpdateRequestIsNull()
    {
        TesterUpdateRequest? testerUpdateRequest = null;
        Assert.Throws<ArgumentNullException>(() => _testersService.UpdateTester(testerUpdateRequest));
    }

    [Fact]
    public void UpdateTester_ShallThrowArgumentException_IfTesterUpdateRequestIdIsInvalid()
    {
        var testerUpdateRequest = new TesterUpdateRequest { TesterId = Guid.NewGuid() };

        Assert.Throws<ArgumentException>(() => _testersService.UpdateTester(testerUpdateRequest));
    }

    [Fact]
    public void UpdateTester_ShallThrowArgumentNullException_IfTesterUpdateRequestTesterNameIsNull()
    {
        var devStreamAddRequest = new DevStreamAddRequest { DevStreamName = "Crew" };
        var devStreamResponse = _devStreamsService.AddDevStream(devStreamAddRequest);
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

        var testerResponse = _testersService.AddTester(testerAddRequest);
        var testerUpdateRequest = testerResponse.ToTesterUpdateRequest();
        testerUpdateRequest.TesterName = null;

        Assert.Throws<ArgumentException>(() => _testersService.UpdateTester(testerUpdateRequest));
    }

    [Fact]
    public void UpdateTester_ShallUpdateTester_IfTesterUpdateRequestIsValid()
    {
        var devStreamAddRequest = new DevStreamAddRequest { DevStreamName = "Crew" };
        var devStreamResponse = _devStreamsService.AddDevStream(devStreamAddRequest);
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

        var testerResponse = _testersService.AddTester(testerAddRequest);
        testerResponse.TesterName = "Villanelle";
        testerResponse.Position = "Senior QA";
        var testerUpdateRequest = testerResponse.ToTesterUpdateRequest();

        var updatedTesterResponse = _testersService.UpdateTester(testerUpdateRequest);
        var testerFromGetById = _testersService.GetTesterById(testerResponse.TesterId);

        Assert.Equal(updatedTesterResponse, testerFromGetById);
    }

    #endregion

    #region DeleteTester

    [Fact]
    public void DeleteTester_ShallReturnTrue_IfTesterIdIsFound()
    {
        var devStreamAddRequest = new DevStreamAddRequest { DevStreamName = "Crew" };
        var devStreamResponse = _devStreamsService.AddDevStream(devStreamAddRequest);
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

        var testerResponse = _testersService.AddTester(testerAddRequest);

        var isDeleted = _testersService.DeleteTester(testerResponse.TesterId);
        Assert.True(isDeleted);

        var testerFromGetById = _testersService.GetTesterById(testerResponse.TesterId);
        Assert.Null(testerFromGetById);
    }

    [Fact]
    public void DeleteTester_ShallReturnFalse_IfTesterIdIsNotFound()
    {
        var isDeleted = _testersService.DeleteTester(Guid.NewGuid());
        Assert.False(isDeleted);
    }

    #endregion
}