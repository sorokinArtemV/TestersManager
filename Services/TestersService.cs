using System.Globalization;
using CsvHelper;
using Entities;
using Microsoft.EntityFrameworkCore;
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
        var allTesters = await GetAllTesters();
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

        var tester = await _testersRepository.Testers.FirstOrDefaultAsync(x => x.TesterId == testerId);

        if (tester is null) return false;

        _testersRepository.Testers.Remove(await _testersRepository.Testers.FirstAsync(x => x.TesterId == testerId));
        await _testersRepository.SaveChangesAsync();

        return true;
    }

    public async Task<MemoryStream> GetTestersCsv()
    {
        var memoryStream = new MemoryStream();
        var streamWriter = new StreamWriter(memoryStream);
        var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture, true);

        csvWriter.WriteHeader<TesterResponse>();
        await csvWriter.NextRecordAsync();

        var testers = _testersRepository.Testers
            .Include("DevStream")
            .Select(x => x.ToTesterResponse()).ToList();

        await csvWriter.WriteRecordsAsync(testers);
        memoryStream.Position = 0;

        return memoryStream;
    }

    public async Task<TesterResponse> UpdateTester(TesterUpdateRequest? testerUpdateRequest)
    {
        ArgumentNullException.ThrowIfNull(testerUpdateRequest);
        ModelValidationHelper.IsValid(testerUpdateRequest);

        var tester = await _testersRepository.Testers.FirstOrDefaultAsync(tester => tester.TesterId == testerUpdateRequest.TesterId);

        ArgumentNullException.ThrowIfNull(tester);

        tester.TesterName = testerUpdateRequest.TesterName;
        tester.Email = testerUpdateRequest.Email;
        tester.Gender = testerUpdateRequest.Gender.ToString();
        tester.BirthDate = testerUpdateRequest.BirthDate;
        tester.DevStreamId = testerUpdateRequest.DevStreamId;
        tester.Position = testerUpdateRequest.Position;
        tester.MonthsOfWorkExperience = testerUpdateRequest.MonthsOfWorkExperience;
        tester.Skills = string.Join(", ", testerUpdateRequest.Skills);

        await _testersRepository.SaveChangesAsync();

        return tester.ToTesterResponse();
    }
}