using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class DevStreamsAdderService : IDevStreamsAdderService
{
    private readonly IDevStreamsRepository _devStreamRepository;

    public DevStreamsAdderService(IDevStreamsRepository devStreamsRepository)
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
}