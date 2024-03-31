using Microsoft.Extensions.Logging;
using TestersManager.Core.Domain.RepositoryContracts;
using TestersManager.Core.DTO;
using TestersManager.Core.Enums;
using TestersManager.Core.Helpers;
using TestersManager.Core.ServiceContracts;

namespace TestersManager.Core.Services;

public class TestersSorterService : ITestersSorterService
{
    private readonly ILogger<TestersSorterService> _logger;
    private readonly ITestersRepository _testersRepository;


    public TestersSorterService(ITestersRepository testersRepository, ILogger<TestersSorterService> logger)
    {
        _testersRepository = testersRepository;
        _logger = logger;
    }

    public async Task<List<TesterResponse>> GetSortedTesters(List<TesterResponse> allTesters, string sortBy,
        SortOrderOptions sortOrder)
    {
        _logger.LogInformation("GetSortedTesters method of TestersService invoked");

        return string.IsNullOrEmpty(sortBy)
            ? allTesters
            : await SorterHelper.SortByPropertyAsync(allTesters, sortBy, sortOrder);
    }
}