using System.ComponentModel.DataAnnotations;

namespace Entities;

/// <summary>
///     Domain model to store DevStream
/// </summary>
public class DevStream
{
    [Key]
    public Guid DevStreamId { get; set; }

    public string? DevStreamName { get; set; }
}