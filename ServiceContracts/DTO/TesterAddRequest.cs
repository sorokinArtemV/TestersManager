using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO;

/// <summary>
///     DTO to add a new Tester
/// </summary>
public class TesterAddRequest
{
    [Required(ErrorMessage = "Tester name cannot be empty")]
    public string? TesterName { get; set; }

    [Required(ErrorMessage = "Email cannot empty")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Gender cannot be empty")]
    public GenderOptions? Gender { get; set; }

    [DataType(DataType.Date)]
    [Required(ErrorMessage = "BirthDate cannot be empty")]
    public DateTime? BirthDate { get; set; }

    [Required(ErrorMessage = "DevStreamId cannot be empty")]
    public Guid? DevStreamId { get; set; }

    [Required(ErrorMessage = "Position cannot be empty")]
    public string? Position { get; set; }

    [Required(ErrorMessage = "Months of work experience is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Months of work experience cannot be negative")]
    public int? MonthsOfWorkExperience { get; set; }

    [Required(ErrorMessage = "Mobile device experience is required")]
    public bool HasMobileDeviceExperience { get; set; }

    public string? Skills { get; set; }


    /// <summary>
    ///     Converts TesterAddRequest to Tester
    /// </summary>
    /// <returns>Tester object</returns>
    public Tester ToTester()
    {
        return new Tester
        {
            TesterName = TesterName,
            Email = Email,
            Gender = Gender.ToString(),
            BirthDate = BirthDate,
            DevStreamId = DevStreamId,
            Position = Position,
            MonthsOfWorkExperience = MonthsOfWorkExperience,
            HasMobileDeviceExperience = HasMobileDeviceExperience,
            Skills = Skills
        };
    }
}