using TestersManager.Core.DTO;

namespace TestersManager.Core.ServiceContracts;

/// <summary>
///     Represents business logic for manipulating DevStream entity
/// </summary>
public interface IDevStreamsGetterService
{
    /// <summary>
    ///     Gets List of all DevStreams
    /// </summary>
    /// <returns>List of DevStreams</returns>
    public Task<List<DevStreamResponse>> GetAllDevStreams();

    /// <summary>
    ///     Gets DevStream by id
    /// </summary>
    /// <param name="devStreamId">DevStream id</param>
    /// <returns>Matching DevStream</returns>
    public Task<DevStreamResponse?> GetDevStreamById(Guid? devStreamId);
}