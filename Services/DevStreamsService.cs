using Entities;
using Microsoft.EntityFrameworkCore;
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

    public async Task<DevStreamResponse> AddDevStream(DevStreamAddRequest? devStreamAddRequest)
    {
        ArgumentNullException.ThrowIfNull(devStreamAddRequest);
        ArgumentException.ThrowIfNullOrEmpty(devStreamAddRequest.DevStreamName);
        if (await _db.DevStreams.AnyAsync(x => x.DevStreamName == devStreamAddRequest.DevStreamName))
            throw new ArgumentException("DevStream name already exists");
        // if (_db.DevStreams.Where(stream => stream.DevStreamName == devStreamAddRequest.DevStreamName).Count() > 0)
        // {
        //     throw new ArgumentException("DevStream name already exists");
        // }

        var devStream = devStreamAddRequest.ToDevStream();
        devStream.DevStreamId = Guid.NewGuid();
        await _db.DevStreams.AddAsync(devStream);
        await _db.SaveChangesAsync();

        return devStream.ToDevStreamResponse();
    }

    public async Task<List<DevStreamResponse>> GetAllDevStreams()
    {
        return await _db.DevStreams.Select(x => x.ToDevStreamResponse()).ToListAsync();
    }

    public async Task<DevStreamResponse?> GetDevStreamById(Guid? devStreamId)
    {
        ArgumentNullException.ThrowIfNull(devStreamId);

        var devStreamResponse =
            await _db.DevStreams.FirstOrDefaultAsync(x => x.DevStreamId == devStreamId);

        return devStreamResponse?.ToDevStreamResponse() ?? null;
    }
}