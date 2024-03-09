using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

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

        ValidationContext validationContext = new(testerAddRequest);
        List<ValidationResult> validationResults = [];
        
        // Model validation
        bool isValid = Validator.TryValidateObject(testerAddRequest, validationContext, validationResults, true);
        if (!isValid) throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);


        var tester = testerAddRequest.ToTester();
        tester.TesterId = new Guid();

        _testers.Add(tester);

        return ConvertTesterToTesterResponse(tester);
    }

    public List<TesterResponse> GetAllTesters()
    {
        throw new NotImplementedException();
    }
}