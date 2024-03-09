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
}