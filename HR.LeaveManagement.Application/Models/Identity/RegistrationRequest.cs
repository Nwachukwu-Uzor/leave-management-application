using System.ComponentModel.DataAnnotations;

namespace HR.LeaveManagement.Application.Models.Identity;

public class RegistrationRequest
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    [MinLength(16)]
    public string UserName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}
