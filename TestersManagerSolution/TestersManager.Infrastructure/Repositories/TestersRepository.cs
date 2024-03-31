using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestersManager.Core.Domain.Entities;
using TestersManager.Core.Domain.RepositoryContracts;
using TestersManager.Infrastructure.DbContext;

namespace TestersManager.Infrastructure.Repositories;

public class TestersRepository : ITestersRepository
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<TestersRepository> _logger;


    public TestersRepository(ApplicationDbContext db, ILogger<TestersRepository> logger)
    {
        _db = db;
        _logger = logger;
    }


    public async Task<Tester> AddTester(Tester tester)
    {
        _db.Add(tester);
        await _db.SaveChangesAsync();

        return tester;
    }

    public async Task<List<Tester>> GetAllTesters()
    {
        _logger.LogInformation("GetAllTesters method of TestersRepository invoked");

        return await _db.Testers.Include("DevStream").ToListAsync();
    }

    public async Task<Tester?> GetTesterById(Guid testerId)
    {
        return await _db.Testers.Include("DevStream").FirstOrDefaultAsync(x => x.TesterId == testerId);
    }

    public async Task<List<Tester>> GetFilteredTesters(Expression<Func<Tester, bool>> predicate)
    {
        _logger.LogInformation("GetFilteredTesters method of TestersRepository invoked");

        return await _db.Testers.Include("DevStream").Where(predicate).ToListAsync();
    }

    public async Task<bool> DeleteTesterById(Guid testerId)
    {
        _db.Testers.RemoveRange(_db.Testers.Where(x => x.TesterId == testerId));
        var rowsDeleted = await _db.SaveChangesAsync();

        return rowsDeleted > 0;
    }

    public async Task<Tester> UpdateTester(Tester tester)
    {
        var matchingTester = await _db.Testers.FirstOrDefaultAsync(x => x.TesterId == tester.TesterId);

        if (matchingTester is null) return tester;

        matchingTester.TesterName = tester.TesterName;
        matchingTester.Email = tester.Email;
        matchingTester.Gender = tester.Gender;
        matchingTester.BirthDate = tester.BirthDate;
        matchingTester.DevStreamId = tester.DevStreamId;
        matchingTester.Position = tester.Position;
        matchingTester.MonthsOfWorkExperience = tester.MonthsOfWorkExperience;
        matchingTester.Skills = string.Join(", ", tester.Skills);

        var rowsUpdated = await _db.SaveChangesAsync();

        return matchingTester;
    }
}