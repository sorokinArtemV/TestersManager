using TestersManager.Core.DTO;

namespace TestersManager.Core.ServiceContracts;

/// <summary>
///     Represents business logic to manipulate Tester entity
/// </summary>
public interface ITestersAdderService
{
    /// <summary>
    ///     Adds a new Tester to list of Testers
    /// </summary>
    /// <param name="testerAddRequest">TesterAddRequest</param>
    /// <returns>Returns TesterResponse with generated TesterId</returns>
    public Task<TesterResponse> AddTester(TesterAddRequest? testerAddRequest);
}