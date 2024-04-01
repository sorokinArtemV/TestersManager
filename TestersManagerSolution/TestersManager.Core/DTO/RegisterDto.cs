using System.ComponentModel.DataAnnotations;

namespace TestersManager.Core.DTO;

public class RegisterDto
{
    [Required(ErrorMessage = "Tester name is required")]
    public string TesterName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is not valid")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirm password is required")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }

    // [Required(ErrorMessage = "Phone number is required")]
    // [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number can contain only numbers")]
    // [DataType(DataType.PhoneNumber)]
    // public string PhoneNumber { get; set; }
}