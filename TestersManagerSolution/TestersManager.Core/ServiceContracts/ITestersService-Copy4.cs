using TestersManager.Core.DTO;
using TestersManager.Core.Enums;

namespace TestersManager.Core.ServiceContracts;

/// <summary>
///     Represents business logic to manipulate Tester entity
/// </summary>
public interface ITestersSorterService
{
    /// <summary>
    ///     Returns list of sorted Testers
    /// </summary>
    /// <param name="allTesters">List of all persons</param>
    /// <param name="sortBy">Sort field to sort</param>
    /// <param name="sortOrder">Sort order</param>
    /// <returns>Returns sorted list Asc or Desc</returns>
    public Task<List<TesterResponse>> GetSortedTesters(List<TesterResponse> allTesters, string sortBy,
        SortOrderOptions sortOrder);
}