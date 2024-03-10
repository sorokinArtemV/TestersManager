using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
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
                allTesters.Where(x =>
                        string.IsNullOrEmpty(x.TesterName) ||
                        x.TesterName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList(),

            nameof(Tester.Email) =>
                allTesters.Where(x =>
                        string.IsNullOrEmpty(x.Email) ||
                        x.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList(),

            nameof(Tester.Gender) =>
                allTesters.Where(x =>
                        string.IsNullOrEmpty(x.Gender) ||
                        x.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList(),

            nameof(Tester.DevStream) =>
                allTesters.Where(x =>
                        string.IsNullOrEmpty(x.DevStream) ||
                        x.DevStream.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList(),

            nameof(Tester.Position) =>
                allTesters.Where(x =>
                        string.IsNullOrEmpty(x.Position) ||
                        x.Position.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList(),

            nameof(Tester.DevStreamId) =>
                allTesters.Where(x => x.DevStreamId is null || x.DevStreamId.Value.ToString()
                    .Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),

            nameof(Tester.BirthDate) =>
                allTesters.Where(x => x.BirthDate is null || x.BirthDate.Value.ToString("dd MMMM yyyy")
                    .Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),

            nameof(Tester.MonthsOfWorkExperience) =>
                allTesters.Where(x => x.MonthsOfWorkExperience is null || x.MonthsOfWorkExperience.Value
                    .ToString()
                    .Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),

            nameof(Tester.Skills) =>
                allTesters.Where(x =>
                        string.IsNullOrEmpty(x.Skills) ||
                        x.Skills.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList(),
            
            _ => matchingTesters
            // matchingTesters = allTesters
            //     .Where(x => (!string.IsNullOrEmpty(x.TesterName)
            //         ? x.TesterName.Contains(searchString, StringComparison.OrdinalIgnoreCase)
            //         : true)).ToList();
        };

        return matchingTesters;
    }

    private TesterResponse ConvertTesterToTesterResponse(Tester tester)
    {
        var testerResponse = tester.ToTesterResponse();
        testerResponse.DevStream = _devStreamsService.GetDevStreamById(testerResponse.DevStreamId)?.DevStreamName;
        return testerResponse;
    }
}