using TestersManager.Core.Domain.Entities;

namespace TestersManager.Core.DTO;

/// <summary>
/// DTO to add a new DevStream
/// </summary>
public class DevStreamAddRequest
{
    public string? DevStreamName { get; set; }
    
    public DevStream ToDevStream()
    {
        return new DevStream
        {
            DevStreamName = DevStreamName
        };
    }
}