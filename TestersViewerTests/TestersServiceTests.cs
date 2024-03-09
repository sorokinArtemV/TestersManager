using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;

namespace TestersViewerTests;

public class TestersServiceTests
{
    private readonly ITestersService _testersService;
    private readonly IDevStreamsService _devStreamsService;

    public TestersServiceTests()
    {
        _testersService = new TestersService();
        _devStreamsService = new DevStreamsService();
    }

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
        var devStreamAddRequest = new DevStreamAddRequest() { DevStreamName = "Crew" };
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
        var devStreamAddRequestOne = new DevStreamAddRequest() { DevStreamName = "Crew" };
        var devStreamAddRequestTwo = new DevStreamAddRequest() { DevStreamName = "New Year" };
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
        {
            testerResponsesFromAdd.Add(_testersService.AddTester(testerAddRequest));
        }

        var testerResponsesFromGet = _testersService.GetAllTesters();

        foreach (var testerResponse in testerResponsesFromAdd)
        {
            Assert.Contains(testerResponse, testerResponsesFromGet); // calls equals method, so compare ref not val()
        }
    }

    #endregion
}