using ServiceContracts.DTO;

namespace ServiceContracts;

/// <summary>
/// Represents business logic to manipulate Tester entity
/// </summary>
public interface ITestersService
{
    /// <summary>
    /// Adds a new Tester to list of Testers
    /// </summary>
    /// <param name="testerAddRequest">TesterAddRequest</param>
    /// <returns>Returns TesterResponse with generated TesterId</returns>
    TesterResponse AddTester(TesterAddRequest? testerAddRequest);

    /// <summary>
    /// Gets all Testers
    /// </summary>
    /// <returns>Returns a List of TestersResponse objects</returns>
    List<TesterResponse> GetAllTesters();
}