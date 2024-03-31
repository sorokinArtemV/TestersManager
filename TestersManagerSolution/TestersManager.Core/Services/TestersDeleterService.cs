using Microsoft.Extensions.Logging;
using TestersManager.Core.Domain.RepositoryContracts;
using TestersManager.Core.ServiceContracts;

namespace TestersManager.Core.Services;

public class TestersDeleterService : ITestersDeleterService
{
    private readonly ILogger<TestersDeleterService> _logger;
    private readonly ITestersRepository _testersRepository;


    public TestersDeleterService(ITestersRepository testersRepository, ILogger<TestersDeleterService> logger)
    {
        _testersRepository = testersRepository;
        _logger = logger;
    }

    public async Task<bool> DeleteTester(Guid? testerId)
    {
        ArgumentNullException.ThrowIfNull(testerId);

        var tester = await _testersRepository.GetTesterById(testerId.Value);

        if (tester is null) return false;

        await _testersRepository.DeleteTesterById(testerId.Value);

        return true;
    }
}