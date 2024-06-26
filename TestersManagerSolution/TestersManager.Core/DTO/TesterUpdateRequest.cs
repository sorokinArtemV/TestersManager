using System.ComponentModel.DataAnnotations;
using TestersManager.Core.Domain.Entities;
using TestersManager.Core.Enums;

namespace TestersManager.Core.DTO;

/// <summary>
///     DTO object to update a Tester
/// </summary>
public class TesterUpdateRequest
{
    public Guid TesterId { get; set; }

    [Required(ErrorMessage = "Tester name cannot be empty")]
    public string? TesterName { get; set; }

    [Required(ErrorMessage = "Email cannot empty")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Gender cannot be empty")]
    public GenderOptions? Gender { get; set; }

    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Date of birth cannot be empty")]
    public DateTime? BirthDate { get; set; }

    [Required(ErrorMessage = "Stream cannot be empty")]
    public Guid? DevStreamId { get; set; }

    [Required(ErrorMessage = "Position cannot be empty")]
    public string? Position { get; set; }

    [Required(ErrorMessage = "Months of work experience is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Months of work experience cannot be negative")]
    public int? MonthsOfWorkExperience { get; set; }

    // public bool HasMobileDeviceExperience { get; set; }

    public string? Skills { get; set; }


    /// <summary>
    ///     Converts TesterUpdateRequest to Tester
    /// </summary>
    /// <returns>Tester object</returns>
    public Tester ToTester()
    {
        return new Tester
        {
            TesterId = TesterId,
            TesterName = TesterName,
            Email = Email,
            Gender = Gender.ToString(),
            BirthDate = BirthDate,
            DevStreamId = DevStreamId,
            Position = Position,
            MonthsOfWorkExperience = MonthsOfWorkExperience,
            // HasMobileDeviceExperience = HasMobileDeviceExperience,
            Skills = string.Join(", ", Skills != null ? Skills.Select(x => x.ToString()) : "")
        };
    }
}