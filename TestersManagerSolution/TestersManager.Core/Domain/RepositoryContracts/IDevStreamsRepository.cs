using Entities;

namespace RepositoryContracts;

/// <summary>
///     Repository contract for manipulating DevStream entity
/// </summary>
public interface IDevStreamsRepository
{
    /// <summary>
    ///     Adds new DevStream to the date data storage
    /// </summary>
    /// <param name="devStream">DevStream object</param>
    /// <returns>Returns Added DevStream object after adding it to the data storage</returns>
    public Task<DevStream> AddDevStream(DevStream devStream);

    /// <summary>
    ///     Returns all DevStreams from the data storage
    /// </summary>
    /// <returns>Returns List of all DevStream objects</returns>
    public Task<List<DevStream>> GetAllDevStreams();

    /// <summary>
    ///     Gets DevStream object from the data storage by DevStreamId
    /// </summary>
    /// <param name="devStreamId">DevStreamId to find</param>
    /// <returns>Returns matching DevStream object from the data storage</returns>
    public Task<DevStream?> GetDevStreamById(Guid devStreamId);

    /// <summary>
    ///     Gets DevStream object from the data storage by DevStreamName
    /// </summary>
    /// <param name="devStreamName">DevStreamName to find</param>
    /// <returns>Returns matching DevStream object from the data storage</returns>
    public Task<DevStream?> GetDevStreamByName(string devStreamName);
}