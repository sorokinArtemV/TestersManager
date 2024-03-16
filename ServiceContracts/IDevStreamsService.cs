using ServiceContracts.DTO;

namespace ServiceContracts;

/// <summary>
///     Represents business logic for manipulating DevStream entity
/// </summary>
public interface IDevStreamsService
{
    /// <summary>
    ///     Adds new DevStream
    /// </summary>
    /// <param name="devStreamAddRequest">DevStream to add</param>
    /// <returns>Added DevStream as response object</returns>
    public Task<DevStreamResponse> AddDevStream(DevStreamAddRequest? devStreamAddRequest);

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