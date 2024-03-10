using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services;

public class TestersService : ITestersService
{
    private readonly IDevStreamsService _devStreamsService;
    private readonly List<Tester> _testers;

    public TestersService()
    {
        _testers = new List<Tester>();
        _devStreamsService = new DevStreamsService();
    }

    public TesterResponse AddTester(TesterAddRequest? testerAddRequest)
    {
        ArgumentNullException.ThrowIfNull(testerAddRequest);
        ModelValidationHelper.IsValid(testerAddRequest);

        var tester = testerAddRequest.ToTester();
        tester.TesterId = Guid.NewGuid();

        _testers.Add(tester);

        return ConvertTesterToTesterResponse(tester);
    }

    public List<TesterResponse> GetAllTesters()
    {
        return _testers
            .Select(ConvertTesterToTesterResponse)
            .ToList();
    }

    public TesterResponse? GetTesterById(Guid? id)
    {
        return id is null
            ? null
            : _testers
                .Where(tester => tester.TesterId == id)
                .Select(ConvertTesterToTesterResponse)
                .FirstOrDefault();
    }

    public List<TesterResponse> GetFilteredTesters(string searchBy, string searchString)
    {
        var allTesters = GetAllTesters();
        var matchingTesters = allTesters;

        if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString)) return matchingTesters;

        matchingTesters = searchBy switch
        {
            nameof(Tester.TesterName) =>
                FilterHelper.FilterBy(allTesters, x => x.TesterName, searchString),

            nameof(Tester.Email) =>
                FilterHelper.FilterBy(allTesters, x => x.Email, searchString),

            nameof(Tester.Gender) =>
                FilterHelper.FilterBy(allTesters, x => x.Gender, searchString),

            nameof(Tester.DevStream) =>
                FilterHelper.FilterBy(allTesters, x => x.DevStream, searchString),

            nameof(Tester.Position) =>
                FilterHelper.FilterBy(allTesters, x => x.Position, searchString),

            nameof(Tester.DevStreamId) =>
                FilterHelper.FilterBy(allTesters, x => x.DevStreamId?.ToString(), searchString),

            nameof(Tester.BirthDate) =>
                FilterHelper.FilterBy(allTesters, x => x.BirthDate?.ToString("dd MMMM yyyy"), searchString),

            nameof(Tester.MonthsOfWorkExperience) =>
                FilterHelper.FilterBy(allTesters, x => x.MonthsOfWorkExperience?.ToString(), searchString),

            nameof(Tester.Skills) =>
                FilterHelper.FilterBy(allTesters, x => x.Skills, searchString),

            _ => matchingTesters
        };

        return matchingTesters;
    }


    public List<TesterResponse> GetSortedTesters(List<TesterResponse> allTesters, string sortBy,
        SortOrderOptions sortOrder)
    {
        return string.IsNullOrEmpty(sortBy) ? allTesters : SorterHelper.SortByProperty(allTesters, sortBy, sortOrder);
    }

    public TesterResponse UpdateTester(TesterUpdateRequest? testerUpdateRequest)
    {
        ArgumentNullException.ThrowIfNull(testerUpdateRequest);
        ModelValidationHelper.IsValid(testerUpdateRequest);

        var tester = _testers.FirstOrDefault(tester => tester.TesterId == testerUpdateRequest.TesterId);

        ArgumentNullException.ThrowIfNull(tester);

        tester.TesterName = testerUpdateRequest.TesterName;
        tester.Email = testerUpdateRequest.Email;
        tester.Gender = testerUpdateRequest.Gender.ToString();
        tester.BirthDate = testerUpdateRequest.BirthDate;
        tester.DevStreamId = testerUpdateRequest.DevStreamId;
        tester.Position = testerUpdateRequest.Position;
        tester.MonthsOfWorkExperience = testerUpdateRequest.MonthsOfWorkExperience;
        tester.HasMobileDeviceExperience = testerUpdateRequest.HasMobileDeviceExperience;
        tester.Skills = string.Join(", ", testerUpdateRequest.Skills);

        return ConvertTesterToTesterResponse(tester);
    }

    public bool DeleteTester(Guid? id)
    {
        throw new NotImplementedException();
    }

    private TesterResponse ConvertTesterToTesterResponse(Tester tester)
    {
        var testerResponse = tester.ToTesterResponse();
        testerResponse.DevStream = _devStreamsService.GetDevStreamById(testerResponse.DevStreamId)?.DevStreamName;
        return testerResponse;
    }
}