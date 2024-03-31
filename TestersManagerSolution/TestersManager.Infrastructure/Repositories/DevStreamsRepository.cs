using Microsoft.EntityFrameworkCore;
using TestersManager.Core.Domain.Entities;
using TestersManager.Core.Domain.RepositoryContracts;
using TestersManager.Infrastructure.DbContext;

namespace TestersManager.Infrastructure.Repositories;

public class DevStreamsRepository : IDevStreamsRepository
{
    private readonly ApplicationDbContext _db;

    public DevStreamsRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<DevStream> AddDevStream(DevStream devStream)
    {
        _db.DevStreams.Add(devStream);
        await _db.SaveChangesAsync();

        return devStream;
    }

    public async Task<List<DevStream>> GetAllDevStreams()
    {
        return await _db.DevStreams.ToListAsync();
    }

    public async Task<DevStream?> GetDevStreamById(Guid devStreamId)
    {
        var devStream = await _db.DevStreams.FirstOrDefaultAsync(x => x.DevStreamId == devStreamId);

        return devStream;
    }

    public async Task<DevStream?> GetDevStreamByName(string devStreamName)
    {
        var devStream = await _db.DevStreams.FirstOrDefaultAsync(x => x.DevStreamName == devStreamName);

        return devStream;
    }
}