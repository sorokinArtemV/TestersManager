using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services;

public class TestersService : ITestersService
{
    private readonly List<Tester> _testers;
    private readonly IDevStreamsService _devStreamsService;

    public TestersService()
    {
        _testers = new List<Tester>();
        _devStreamsService = new DevStreamsService();
    }

    private TesterResponse ConvertTesterToTesterResponse(Tester tester)
    {
        var testerResponse = tester.ToTesterResponse();
        testerResponse.DevStream = _devStreamsService.GetDevStreamById(testerResponse.DevStreamId)?.DevStreamName;
        return testerResponse;
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
        throw new NotImplementedException();
    }
}