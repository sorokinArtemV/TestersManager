using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;

namespace TestersViewerTests;

public class TestersServiceTests
{
    private readonly ITestersService _testersService;

    public TestersServiceTests()
    {
        _testersService = new TestersService();
    }

    #region AddTester

    [Fact]
    public void AddTester_ShouldThrowArgumentNullException_IfTesterAddRequestIsNull()
    {
        TesterAddRequest? testerAddRequest = null;

        Assert.Throws<ArgumentNullException>(() => _testersService.AddTester(testerAddRequest));
    }

    [Fact]
    public void AddTester_ShouldThrowArgumentException_IfTesterAddRequestNameIsNull()
    {
        var testerAddRequest = new TesterAddRequest { TesterName = null };

        Assert.Throws<ArgumentException>(() => _testersService.AddTester(testerAddRequest));
    }

    [Fact]
    public void AddTester_ShouldInsertNewTesterToTheList_AndReturnAddedTesterResponseObj_IfTesterAddRequestIsValid()
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
}