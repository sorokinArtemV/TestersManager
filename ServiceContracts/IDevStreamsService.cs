using Entities;
using ServiceContracts.DTO;

namespace ServiceContracts;

/// <summary>
/// Represents business logic for manipulating DevStream entity
/// </summary>
public interface IDevStreamsService
{
    /// <summary>
    /// Adds new DevStream
    /// </summary>
    /// <param name="devStreamAddRequest">DevStream to add</param>
    /// <returns>Added DevStream as response object</returns>
    public DevStreamResponse AddDevStream(DevStreamAddRequest? devStreamAddRequest);

    /// <summary>
    /// Gets List of all DevStreams
    /// </summary>
    /// <returns>List of DevStreams</returns>
    List<DevStreamResponse> GetAllDevStreams();
    
    /// <summary>
    /// Gets DevStream by id
    /// </summary>
    /// <param name="devStreamId">DevStream id</param>
    /// <returns>Matching DevStream</returns>
    public DevStreamResponse? GetDevStreamById(Guid? devStreamId);
}