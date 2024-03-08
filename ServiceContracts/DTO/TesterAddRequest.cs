using ServiceContracts.Enums;

namespace ServiceContracts.DTO;

/// <summary>
/// DTO to add a new Tester
/// </summary>
public class TesterAddRequest
{
    public string? TesterName { get; set; }
    public string? Email { get; set; }
    public GenderOptions? Gender { get; set; }
    public DateTime BirthDate { get; set; }
    public Guid? DevStreamId { get; set; }
    public string? Position { get; set; }
    public int MonthsOfWorkExperience { get; set; }
    public bool HasMobileDeviceExperience { get; set; }
    public IEnumerable<SkillsOptions>? Skills { get; set; }
}