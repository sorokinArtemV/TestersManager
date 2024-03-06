using Entities;

namespace ServiceContracts.DTO;

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