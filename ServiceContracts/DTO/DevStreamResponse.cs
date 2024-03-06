using Entities;

namespace ServiceContracts.DTO;

/// <summary>
/// DTO class to use as a return type of DevStreamService methods
/// </summary>
public class DevStreamResponse
{
    public Guid DevStreamId { get; set; }

    public string? DevStreamName { get; set; }
}

public static class DevStreamExtensions
{
    public static DevStreamResponse ToDevStreamResponse(this DevStream devStream)
    {
        return new DevStreamResponse
        {
            DevStreamId = devStream.DevStreamId,
            DevStreamName = devStream.DevStreamName
        };
    }
}