using TestersManager.Core.Domain.Entities;

namespace TestersManager.Core.DTO;

/// <summary>
/// DTO class to use as a return type of DevStreamService methods
/// </summary>
public class DevStreamResponse : IEquatable<DevStreamResponse>
{
    public Guid DevStreamId { get; set; }
    public string? DevStreamName { get; set; }

    public override bool Equals(object? obj)
    {
        // implicit type conversion, could have use record instead
        if (obj is DevStreamResponse devStreamResponse)
        {
            return DevStreamId == devStreamResponse.DevStreamId
                && DevStreamName == devStreamResponse.DevStreamName;
        }
        
        return false;
    }

    public bool Equals(DevStreamResponse? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return DevStreamId.Equals(other.DevStreamId) && DevStreamName == other.DevStreamName;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(DevStreamId, DevStreamName);
    }

    public static bool operator ==(DevStreamResponse? left, DevStreamResponse? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(DevStreamResponse? left, DevStreamResponse? right)
    {
        return !Equals(left, right);
    }
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