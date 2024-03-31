using Microsoft.Extensions.Logging;
using TestersManager.Core.Domain.RepositoryContracts;
using TestersManager.Core.DTO;
using TestersManager.Core.Exceptions;
using TestersManager.Core.Helpers;
using TestersManager.Core.ServiceContracts;

namespace TestersManager.Core.Services;

public class TestersUpdaterService : ITestersUpdaterService
{
    private readonly ILogger<TestersUpdaterService> _logger;
    private readonly ITestersRepository _testersRepository;


    public TestersUpdaterService(ITestersRepository testersRepository, ILogger<TestersUpdaterService> logger)
    {
        _testersRepository = testersRepository;
        _logger = logger;
    }

    public async Task<TesterResponse> UpdateTester(TesterUpdateRequest? testerUpdateRequest)
    {
        ArgumentNullException.ThrowIfNull(testerUpdateRequest);
        ModelValidationHelper.IsValid(testerUpdateRequest);

        var tester = await _testersRepository.GetTesterById(testerUpdateRequest.TesterId);

        if (tester is null) throw new InvalidTesterIdException();

        tester.TesterName = testerUpdateRequest.TesterName;
        tester.Email = testerUpdateRequest.Email;
        tester.Gender = testerUpdateRequest.Gender.ToString();
        tester.BirthDate = testerUpdateRequest.BirthDate;
        tester.DevStreamId = testerUpdateRequest.DevStreamId;
        tester.Position = testerUpdateRequest.Position;
        tester.MonthsOfWorkExperience = testerUpdateRequest.MonthsOfWorkExperience;
        tester.Skills = string.Join(", ", testerUpdateRequest.Skills);

        await _testersRepository.UpdateTester(tester);

        return tester.ToTesterResponse();
    }
}