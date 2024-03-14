using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO;

/// <summary>
/// DTO to add a new Tester
/// </summary>
public class TesterAddRequest
{
    [Required(ErrorMessage = "Tester name cannot be null or empty")]
    public string? TesterName { get; set; }
    
    [Required(ErrorMessage = "Email cannot be null or empty")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string? Email { get; set; }
    public GenderOptions? Gender { get; set; }
    public DateTime BirthDate { get; set; }
    public Guid? DevStreamId { get; set; }
    public string? Position { get; set; }
    public int MonthsOfWorkExperience { get; set; }
    public bool HasMobileDeviceExperience { get; set; }
    public string? Skills { get; set; }


    /// <summary>
    /// Converts TesterAddRequest to Tester
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