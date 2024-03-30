using ServiceContracts.DTO;

namespace ServiceContracts;

/// <summary>
///     Represents business logic for manipulating DevStream entity
/// </summary>
public interface IDevStreamsAdderService
{
    /// <summary>
    ///     Adds new DevStream
    /// </summary>
    /// <param name="devStreamAddRequest">DevStream to add</param>
    /// <returns>Added DevStream as response object</returns>
    public Task<DevStreamResponse> AddDevStream(DevStreamAddRequest? devStreamAddRequest);
}