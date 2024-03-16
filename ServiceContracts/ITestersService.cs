using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts;

/// <summary>
///     Represents business logic to manipulate Tester entity
/// </summary>
public interface ITestersService
{
    /// <summary>
    /// Adds a new Tester to list of Testers
    /// </summary>
    /// <param name="testerAddRequest">TesterAddRequest</param>
    /// <returns>Returns TesterResponse with generated TesterId</returns>
    Task<TesterResponse> AddTester(TesterAddRequest? testerAddRequest);

    /// <summary>
    /// Gets all Testers
    /// </summary>
    /// <returns>Returns a List of TestersResponse objects</returns>
    Task<List<TesterResponse>> GetAllTesters();

    /// <summary>
    /// Gets Tester by id
    /// </summary>
    /// <param name="id">Tester Guid</param>
    /// <returns>Returns TesterResponse object</returns>
    Task<TesterResponse?> GetTesterById(Guid? id);

    /// <summary>
    /// Returns filtered list of Testers filtered by searchBy and searchString
    /// </summary>
    /// <param name="searchBy">Search field to filter</param>
    /// <param name="searchString">Search string to search</param>
    /// <returns>Return filtered list based on searchBy and searchString</returns>
    Task<List<TesterResponse>> GetFilteredTesters(string searchBy, string searchString);


    /// <summary>
    /// Returns list of sorted Testers
    /// </summary>
    /// <param name="allTesters">List of all persons</param>
    /// <param name="sortBy">Sort field to sort</param>
    /// <param name="sortOrder">Sort order</param>
    /// <returns>Returns sorted list Asc or Desc</returns>
    Task<List<TesterResponse>> GetSortedTesters(List<TesterResponse> allTesters, string sortBy, SortOrderOptions sortOrder);


    /// <summary>
    /// Updates Tester based on TesterUpdateRequest id
    /// </summary>
    /// <param name="testerUpdateRequest">Tester details to update</param>
    /// <returns>Returns TesterResponse after update</returns>
    Task<TesterResponse> UpdateTester(TesterUpdateRequest? testerUpdateRequest);

    /// <summary>
    /// Deletes Tester by id
    /// </summary>
    /// <param name="id">Tester Guid</param>
    /// <returns>Returns true if deleted</returns>
    bool DeleteTester(Guid? id);
}