using System.Globalization;
using CsvHelper;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services;

public class TestersService : ITestersService
{
    private readonly ITestersRepository _testersRepository;

    public TestersService(ITestersRepository testersRepository)
    {
        _testersRepository = testersRepository;
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

    public async Task<List<TesterResponse>> GetAllTesters()
    {
        var testers = await _testersRepository.GetAllTesters();

        return testers
            .Select(x => x.ToTesterResponse())
            .ToList();
    }

    public async Task<TesterResponse?> GetTesterById(Guid? testerId)
    {
        ArgumentNullException.ThrowIfNull(testerId);

        var tester = await _testersRepository.GetTesterById(testerId.Value);

        return tester?.ToTesterResponse();
    }

    public async Task<List<TesterResponse>> GetFilteredTesters(string searchBy, string searchString)
    {
        var allTesters = searchBy switch
        {
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
                    x.DevStream.DevStreamName.Contains(searchString)),

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

    public async Task<List<TesterResponse>> GetSortedTesters(List<TesterResponse> allTesters, string sortBy,
        SortOrderOptions sortOrder)
    {
        return string.IsNullOrEmpty(sortBy)
            ? allTesters
            : await SorterHelper.SortByPropertyAsync(allTesters, sortBy, sortOrder);
    }

    public async Task<bool> DeleteTester(Guid? testerId)
    {
        ArgumentNullException.ThrowIfNull(testerId);

        var tester = await _testersRepository.GetTesterById(testerId.Value);

        if (tester is null) return false;

        await _testersRepository.DeleteTesterById(testerId.Value);

        return true;
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

    public async Task<TesterResponse> UpdateTester(TesterUpdateRequest? testerUpdateRequest)
    {
        ArgumentNullException.ThrowIfNull(testerUpdateRequest);
        ModelValidationHelper.IsValid(testerUpdateRequest);

        var tester = await _testersRepository.GetTesterById(testerUpdateRequest.TesterId);

        ArgumentNullException.ThrowIfNull(tester);

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