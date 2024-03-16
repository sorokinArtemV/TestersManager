using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class DevStreamsService : IDevStreamsService
{
    private readonly TestersDbContext _db;

    public DevStreamsService(TestersDbContext dbContext)
    {
        _db = dbContext;
    }

    public DevStreamResponse AddDevStream(DevStreamAddRequest? devStreamAddRequest)
    {
        ArgumentNullException.ThrowIfNull(devStreamAddRequest);
        ArgumentException.ThrowIfNullOrEmpty(devStreamAddRequest.DevStreamName);
        if (_db.DevStreams.Any(x => x.DevStreamName == devStreamAddRequest.DevStreamName))
            throw new ArgumentException("DevStream name already exists");
        // if (_db.DevStreams.Where(stream => stream.DevStreamName == devStreamAddRequest.DevStreamName).Count() > 0)
        // {
        //     throw new ArgumentException("DevStream name already exists");
        // }

        var devStream = devStreamAddRequest.ToDevStream();
        devStream.DevStreamId = Guid.NewGuid();
        _db.DevStreams.Add(devStream);
        _db.SaveChanges();

        return devStream.ToDevStreamResponse();
    }

    public List<DevStreamResponse> GetAllDevStreams()
    {
        return _db.DevStreams.Select(x => x.ToDevStreamResponse()).ToList();
    }

    public DevStreamResponse? GetDevStreamById(Guid? devStreamId)
    {
        ArgumentNullException.ThrowIfNull(devStreamId);

        var devStreamResponse =
            _db.DevStreams.FirstOrDefault(x => x.DevStreamId == devStreamId)?.ToDevStreamResponse();

        return devStreamResponse ?? null;
    }
}