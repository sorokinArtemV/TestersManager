using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts;

/// <summary>
///     Represents business logic to manipulate Tester entity
/// </summary>
public interface ITestersGetterService
{
    /// <summary>
    ///     Gets all Testers
    /// </summary>
    /// <returns>Returns a List of TestersResponse objects</returns>
    public Task<List<TesterResponse>> GetAllTesters();

    /// <summary>
    ///     Gets Tester by id
    /// </summary>
    /// <param name="id">Tester Guid</param>
    /// <returns>Returns TesterResponse object</returns>
    public Task<TesterResponse?> GetTesterById(Guid? id);

    /// <summary>
    ///     Returns filtered list of Testers filtered by searchBy and searchString
    /// </summary>
    /// <param name="searchBy">Search field to filter</param>
    /// <param name="searchString">Search string to search</param>
    /// <returns>Return filtered list based on searchBy and searchString</returns>
    public Task<List<TesterResponse>> GetFilteredTesters(string searchBy, string searchString);

    /// <summary>
    ///     Returns CSV file of all Testers
    /// </summary>
    /// <returns> Returns the MemoryStream</returns>
    public Task<MemoryStream> GetTestersCsv();
}