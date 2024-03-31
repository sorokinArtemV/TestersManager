using TestersManager.Core.Domain.RepositoryContracts;
using TestersManager.Core.DTO;
using TestersManager.Core.ServiceContracts;

namespace TestersManager.Core.Services;

public class DevStreamsGetterService : IDevStreamsGetterService
{
    private readonly IDevStreamsRepository _devStreamRepository;

    public DevStreamsGetterService(IDevStreamsRepository devStreamsRepository)
    {
        _devStreamRepository = devStreamsRepository;
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