using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class DevStreamsService : IDevStreamsService
{
    private readonly List<DevStream> _devStreams;

    public DevStreamsService(bool initialize = true)
    {
        _devStreams = [];
        if (initialize)
            _devStreams.AddRange(new List<DevStream>
            {
                new()
                {
                    DevStreamId = Guid.Parse("1A76B36B-4B06-4A69-A368-7ADE27AB739E"),
                    DevStreamName = "Crew"
                },
                new()
                {
                    DevStreamId = Guid.Parse("248A6FE4-AC09-452C-A205-A6CC4B7E9E56"),
                    DevStreamName = "New Year"
                },

                new()
                {
                    DevStreamId = Guid.Parse("97BE8C70-E9AA-41D8-9BC6-F8832C1B485A"),
                    DevStreamName = "FrontLine"
                },
                new()
                {
                    DevStreamId = Guid.Parse("02DF3B54-16F9-44C7-9272-C57873F8A2CA"),
                    DevStreamName = "Core"
                },
                new()
                {
                    DevStreamId = Guid.Parse("78FD1D57-28E2-4CD8-82A3-5DFDBA2A212A"),
                    DevStreamName = "Tech"
                }
            });
    }

    public DevStreamResponse AddDevStream(DevStreamAddRequest? devStreamAddRequest)
    {
        ArgumentNullException.ThrowIfNull(devStreamAddRequest);
        ArgumentException.ThrowIfNullOrEmpty(devStreamAddRequest.DevStreamName);
        if (_devStreams.Any(x => x.DevStreamName == devStreamAddRequest.DevStreamName))
            throw new ArgumentException("DevStream name already exists");
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
        ArgumentNullException.ThrowIfNull(devStreamId);

        var devStreamResponse =
            _devStreams.FirstOrDefault(x => x.DevStreamId == devStreamId)?.ToDevStreamResponse();

        return devStreamResponse ?? null;
    }
}