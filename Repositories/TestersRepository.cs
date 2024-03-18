using System.Linq.Expressions;
using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories;

public class TestersRepository : ITestersRepository
{
    private readonly ApplicatonDbContext _db;

    public TestersRepository(ApplicatonDbContext db)
    {
        _db = db;
    }


    public async Task<Tester> AddTester(Tester tester)
    {
        _db.Add(tester);
        await _db.SaveChangesAsync();

        return tester;
    }

    public async Task<List<Tester>> GetAllTesters()
    {
        return await _db.Testers.ToListAsync();
    }

    public async Task<Tester?> GetTesterById(Guid testerId)
    {
        return await _db.Testers.Include("DevStream").FirstOrDefaultAsync(x => x.TesterId == testerId);
    }

    public async Task<List<Tester>> GetFilteredTesters(Expression<Func<Tester, bool>> predicate)
    {
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