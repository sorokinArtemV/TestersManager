using System.Linq.Expressions;
using Entities;

namespace RepositoryContracts;

/// <summary>
///     Repository contract for manipulating Tester entity
/// </summary>
public interface ITestersRepository
{
    /// <summary>
    ///     Adds new Tester to the data storage
    /// </summary>
    /// <param name="tester">Tester object to add</param>
    /// <returns>Returns Added Tester object after adding it to the data storage</returns>
    public Task<Tester> AddTester(Tester tester);

    /// <summary>
    ///     Returns all Testers from the data storage
    /// </summary>
    /// <returns>Returns List of all Tester objects</returns>
    public Task<List<Tester>> GetAllTesters();

    /// <summary>
    ///     Gets Tester object from the data storage by TesterId
    /// </summary>
    /// <param name="testerId">TesterId to find</param>
    /// <returns>Returns matching Tester object from the data storage</returns>
    public Task<Tester?> GetTesterById(Guid testerId);

    /// <summary>
    ///     Gets Tester object from the data storage based on given predicate
    /// </summary>
    /// <param name="predicate">LINQ expression to filter</param>
    /// <returns>Returns matching Tester object from the data storage</returns>
    public Task<List<Tester>> GetFilteredTesters(Expression<Func<Tester, bool>> predicate);

    /// <summary>
    ///     Deletes Tester from the data storage
    /// </summary>
    /// <param name="testerId">TesterId to search</param>
    /// <returns>Returns true if Tester was found and deleted</returns>
    public Task<bool> DeleteTesterById(Guid testerId);

    /// <summary>
    ///     Updates Tester in the data storage
    /// </summary>
    /// <param name="testerId">TesterId to search</param>
    /// <returns>Returns Updated Tester object</returns>
    public Task<Tester> UpdateTester(Tester tester);
}