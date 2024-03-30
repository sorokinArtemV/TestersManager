using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts;

/// <summary>
///     Represents business logic to manipulate Tester entity
/// </summary>
public interface ITestersUpdaterService
{
    /// <summary>
    ///     Updates Tester based on TesterUpdateRequest id
    /// </summary>
    /// <param name="testerUpdateRequest">Tester details to update</param>
    /// <returns>Returns TesterResponse after update</returns>
    public Task<TesterResponse> UpdateTester(TesterUpdateRequest? testerUpdateRequest);
}