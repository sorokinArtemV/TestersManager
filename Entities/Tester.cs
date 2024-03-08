namespace Entities;

/// <summary>
/// Domain model to store Tester
/// </summary>
public class Tester
{
    public Guid TesterId { get; set; }
    public string? TesterName { get; set; }
    public string? Email { get; set; }
    public string? Gender { get; set; }
    public DateTime? BirthDate { get; set; }
    public Guid? DevStreamId { get; set; }
    public string? Position { get; set; }
    public int? MonthsOfWorkExperience { get; set; }
    public bool HasMobileDeviceExperience { get; set; }
    public string? Skills { get; set; }
    public DevStream? DevStream { get; set; }
}