using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class DevStreamsService : IDevStreamsService
{
    private readonly List<DevStream> _devStreams = [];

    public DevStreamResponse AddDevStream(DevStreamAddRequest? devStreamAddRequest)
    {
        ArgumentNullException.ThrowIfNull(devStreamAddRequest);
        ArgumentException.ThrowIfNullOrEmpty(devStreamAddRequest.DevStreamName);
        if (_devStreams.Any(x => x.DevStreamName == devStreamAddRequest.DevStreamName))
        {
            throw new ArgumentException("DevStream name already exists");
        }
        // if (_devStreams.Where(stream => stream.DevStreamName == devStreamAddRequest.DevStreamName).Count() > 0)
        // {
        //     throw new ArgumentException("DevStream name already exists");
        // }

        var devStream = devStreamAddRequest.ToDevStream();
        devStream.DevStreamId = Guid.NewGuid();
        _devStreams.Add(devStream);

        return devStream.ToDevStreamResponse();
    }

    public List<DevStreamResponse> GetAllDevStreams()
    {
        return _devStreams.Select(x => x.ToDevStreamResponse()).ToList();
    }

    public DevStreamResponse? GetDevStreamById(Guid? devStreamId)
    {
        throw new NotImplementedException();
    }
}