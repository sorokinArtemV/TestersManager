namespace TestersManager.Core.ServiceContracts;

/// <summary>
///     Represents business logic to manipulate Tester entity
/// </summary>
public interface ITestersDeleterService
{
    /// <summary>
    ///     Deletes Tester by id
    /// </summary>
    /// <param name="id">Tester Guid</param>
    /// <returns>Returns true if deleted</returns>
    public Task<bool> DeleteTester(Guid? id);
}