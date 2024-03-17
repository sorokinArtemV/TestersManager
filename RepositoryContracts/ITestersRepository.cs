using Entities;

namespace RepositoryContracts;

/// <summary>
/// Repository contract for manipulating Tester entity
/// </summary>
public interface ITestersRepository
{
    public Task<Tester> AddTester(Tester tester);
}