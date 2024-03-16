using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services;

public class TestersService : ITestersService
{
    private readonly TestersDbContext _db;
    private readonly IDevStreamsService _devStreamsService;

    public TestersService(TestersDbContext db, IDevStreamsService devStreamsService)
    {
        _db = db;
        _devStreamsService = devStreamsService;
    }

    public TesterResponse AddTester(TesterAddRequest? testerAddRequest)
    {
        ArgumentNullException.ThrowIfNull(testerAddRequest);
        ModelValidationHelper.IsValid(testerAddRequest);

        var tester = testerAddRequest.ToTester();
        tester.TesterId = Guid.NewGuid();

        _db.Testers.Add(tester);
        _db.SaveChanges();

        return ConvertTesterToTesterResponse(tester);
    }

    public List<TesterResponse> GetAllTesters()
    {
        var testers = _db.Testers.Include("DevStream").ToList(); 
        
        return _db.Testers
            .Select(ConvertTesterToTesterResponse)
            .ToList();
    }

    public TesterResponse? GetTesterById(Guid? id)
    {
        return id is null
            ? null
            : _db.Testers
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
            nameof(TesterResponse.TesterName) =>
                FilterHelper.FilterBy(allTesters, x => x.TesterName, searchString),

            nameof(TesterResponse.Email) =>
                FilterHelper.FilterBy(allTesters, x => x.Email, searchString),

            nameof(TesterResponse.Gender) =>
                FilterHelper.FilterBy(allTesters, x => x.Gender, searchString),

            nameof(TesterResponse.DevStream) =>
                FilterHelper.FilterBy(allTesters, x => x.DevStream, searchString),

            nameof(TesterResponse.Position) =>
                FilterHelper.FilterBy(allTesters, x => x.Position, searchString),

            nameof(TesterResponse.DevStreamId) =>
                FilterHelper.FilterBy(allTesters, x => x.DevStreamId?.ToString(), searchString),

            nameof(TesterResponse.Age) =>
                FilterHelper.FilterBy(allTesters, x => x.Age.ToString(), searchString),

            nameof(TesterResponse.BirthDate) =>
                FilterHelper.FilterBy(allTesters, x => x.BirthDate?.ToString("dd MMMM yyyy"), searchString),

            nameof(TesterResponse.MonthsOfWorkExperience) =>
                FilterHelper.FilterBy(allTesters, x => x.MonthsOfWorkExperience?.ToString(), searchString),

            nameof(TesterResponse.Skills) =>
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

        var tester = _db.Testers.FirstOrDefault(tester => tester.TesterId == testerUpdateRequest.TesterId);

        ArgumentNullException.ThrowIfNull(tester);

        tester.TesterName = testerUpdateRequest.TesterName;
        tester.Email = testerUpdateRequest.Email;
        tester.Gender = testerUpdateRequest.Gender.ToString();
        tester.BirthDate = testerUpdateRequest.BirthDate;
        tester.DevStreamId = testerUpdateRequest.DevStreamId;
        tester.Position = testerUpdateRequest.Position;
        tester.MonthsOfWorkExperience = testerUpdateRequest.MonthsOfWorkExperience;
        // tester.HasMobileDeviceExperience = testerUpdateRequest.HasMobileDeviceExperience;
        tester.Skills = string.Join(", ", testerUpdateRequest.Skills);

        _db.SaveChanges();

        return ConvertTesterToTesterResponse(tester);
    }

    public bool DeleteTester(Guid? testerId)
    {
        ArgumentNullException.ThrowIfNull(testerId);

        var tester = _db.Testers.FirstOrDefault(x => x.TesterId == testerId);

        if (tester is null) return false;

        _db.Testers.Remove(_db.Testers.First(x => x.TesterId == testerId));
        _db.SaveChanges();

        return true;
    }

    private TesterResponse ConvertTesterToTesterResponse(Tester tester)
    {
        var testerResponse = tester.ToTesterResponse();
        testerResponse.DevStream = _devStreamsService.GetDevStreamById(testerResponse.DevStreamId)?.DevStreamName;
        return testerResponse;
    }
}