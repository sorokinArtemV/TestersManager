using Entities;

namespace ServiceContracts.DTO;

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