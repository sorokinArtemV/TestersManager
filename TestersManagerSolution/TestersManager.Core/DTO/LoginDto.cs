using System.ComponentModel.DataAnnotations;

namespace TestersManager.Core.DTO;

public class LoginDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}