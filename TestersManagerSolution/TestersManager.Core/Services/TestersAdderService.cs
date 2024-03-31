using Microsoft.Extensions.Logging;
using TestersManager.Core.Domain.RepositoryContracts;
using TestersManager.Core.DTO;
using TestersManager.Core.Helpers;
using TestersManager.Core.ServiceContracts;

namespace TestersManager.Core.Services;

public class TestersAdderService : ITestersAdderService
{
    private readonly ILogger<TestersAdderService> _logger;
    private readonly ITestersRepository _testersRepository;


    public TestersAdderService(ITestersRepository testersRepository, ILogger<TestersAdderService> logger)
    {
        _testersRepository = testersRepository;
        _logger = logger;
    }

    public async Task<TesterResponse> AddTester(TesterAddRequest? testerAddRequest)
    {
        ArgumentNullException.ThrowIfNull(testerAddRequest);
        ModelValidationHelper.IsValid(testerAddRequest);

        var tester = testerAddRequest.ToTester();
        tester.TesterId = Guid.NewGuid();

        await _testersRepository.AddTester(tester);

        return tester.ToTesterResponse();
    }
}