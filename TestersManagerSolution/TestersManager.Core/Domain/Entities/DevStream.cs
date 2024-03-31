using System.ComponentModel.DataAnnotations;

namespace TestersManager.Core.Domain.Entities;

/// <summary>
///     Domain model to store DevStream
/// </summary>
public class DevStream
{
    [Key]
    public Guid DevStreamId { get; set; }

    public string? DevStreamName { get; set; }
    
    // public virtual ICollection<Tester>? Testers { get; set; }
}