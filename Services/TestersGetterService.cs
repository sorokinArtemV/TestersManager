using System.Globalization;
using CsvHelper;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class TestersGetterService : ITestersGetterService
{
    private readonly ILogger<TestersGetterService> _logger;
    private readonly ITestersRepository _testersRepository;


    public TestersGetterService(ITestersRepository testersRepository, ILogger<TestersGetterService> logger)
    {
        _testersRepository = testersRepository;
        _logger = logger;
    }

    public async Task<List<TesterResponse>> GetAllTesters()
    {
        _logger.LogInformation("GetAllTesters method of TestersService invoked");
        var testers = await _testersRepository.GetAllTesters();

        return testers
            .Select(x => x.ToTesterResponse())
            .ToList();
    }

    public async Task<TesterResponse?> GetTesterById(Guid? testerId)
    {
        return testerId == null
            ? null
            : (await _testersRepository.GetTesterById(testerId.Value))?.ToTesterResponse();
    }

    public async Task<List<TesterResponse>> GetFilteredTesters(string searchBy, string? searchString)
    {
        _logger.LogInformation("GetFilteredTesters method of TestersService invoked");

        var allTesters = searchBy switch
        {
            // prioritize default case
            _ when searchString is null => await _testersRepository.GetAllTesters(),

            nameof(TesterResponse.TesterName) =>
                await _testersRepository.GetFilteredTesters(x =>
                    x.TesterName!.Contains(searchString)),

            nameof(TesterResponse.Email) =>
                await _testersRepository.GetFilteredTesters(x =>
                    x.Email!.Contains(searchString)),

            nameof(TesterResponse.Gender) =>
                await _testersRepository.GetFilteredTesters(x =>
                    x.Gender!.Contains(searchString)),

            nameof(TesterResponse.DevStream) =>
                await _testersRepository.GetFilteredTesters(x =>
                    x.DevStream.DevStreamName.ToString().Contains(searchString)),

            nameof(TesterResponse.Position) =>
                await _testersRepository.GetFilteredTesters(x =>
                    x.Position!.Contains(searchString)),

            nameof(TesterResponse.DevStreamId) =>
                await _testersRepository.GetFilteredTesters(x =>
                    x.DevStream.DevStreamName.Contains(searchString)),

            nameof(TesterResponse.Age) =>
                await _testersRepository.GetFilteredTesters(x =>
                    (DateTime.Now.Year - x.BirthDate.Value.Year).ToString()
                    .Contains(searchString)),


            nameof(TesterResponse.BirthDate) =>
                await _testersRepository.GetFilteredTesters(x =>
                    x.BirthDate!.Value.ToString("dd-MM-yyyy")
                        .Contains(searchString)),

            nameof(TesterResponse.MonthsOfWorkExperience) =>
                await _testersRepository.GetFilteredTesters(x =>
                    x.MonthsOfWorkExperience.ToString()!.Contains(searchString)),

            nameof(TesterResponse.Skills) =>
                await _testersRepository.GetFilteredTesters(x =>
                    x.Skills!.Contains(searchString)),

            _ => await _testersRepository.GetAllTesters()
        };

        return allTesters.Select(x => x.ToTesterResponse()).ToList();
    }

    public async Task<MemoryStream> GetTestersCsv()
    {
        var memoryStream = new MemoryStream();
        var streamWriter = new StreamWriter(memoryStream);
        var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture, true);

        csvWriter.WriteHeader<TesterResponse>();
        await csvWriter.NextRecordAsync();

        var testers = await GetAllTesters();

        await csvWriter.WriteRecordsAsync(testers);
        memoryStream.Position = 0;

        return memoryStream;
    }
}