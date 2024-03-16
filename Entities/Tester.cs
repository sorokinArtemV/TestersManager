using System.ComponentModel.DataAnnotations;

namespace Entities;

/// <summary>
///     Domain model to store Tester
/// </summary>
public class Tester
{
    [Key]
    public Guid TesterId { get; set; }

    [StringLength(40, MinimumLength = 3)]
    public string? TesterName { get; set; }

    [StringLength(40, MinimumLength = 3)]
    public string? Email { get; set; }

    [StringLength(10, MinimumLength = 3)]
    public string? Gender { get; set; }

    public DateTime? BirthDate { get; set; }

    public Guid? DevStreamId { get; set; }

    [StringLength(40, MinimumLength = 3)]
    public string? Position { get; set; }

    public int? MonthsOfWorkExperience { get; set; }
    
    [StringLength(500)]
    public string? Skills { get; set; }

    public DevStream? DevStream { get; set; }
}