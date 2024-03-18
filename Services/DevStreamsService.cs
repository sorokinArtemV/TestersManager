using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class DevStreamsService : IDevStreamsService
{
    private readonly IDevStreamsRepository _devStreamRepository;

    public DevStreamsService(IDevStreamsRepository devStreamsRepository)
    {
        _devStreamRepository = devStreamsRepository;
    }

    public async Task<DevStreamResponse> AddDevStream(DevStreamAddRequest? devStreamAddRequest)
    {
        ArgumentNullException.ThrowIfNull(devStreamAddRequest);
        ArgumentException.ThrowIfNullOrEmpty(devStreamAddRequest.DevStreamName);
        if (await _devStreamRepository.GetDevStreamByName(devStreamAddRequest.DevStreamName) is not null)
            throw new ArgumentException("DevStream name already exists");

        var devStream = devStreamAddRequest.ToDevStream();
        devStream.DevStreamId = Guid.NewGuid();
        await _devStreamRepository.AddDevStream(devStream);

        return devStream.ToDevStreamResponse();
    }

    public async Task<List<DevStreamResponse>> GetAllDevStreams()
    {
        return (await _devStreamRepository.GetAllDevStreams())
            .Select(x => x.ToDevStreamResponse()).ToList();
    }

    public async Task<DevStreamResponse?> GetDevStreamById(Guid? devStreamId)
    {
        ArgumentNullException.ThrowIfNull(devStreamId);

        var devStreamResponse = await _devStreamRepository.GetDevStreamById(devStreamId.Value);

        return devStreamResponse?.ToDevStreamResponse() ?? null;
    }
}