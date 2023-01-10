using System.ComponentModel.DataAnnotations;

namespace Security.Dtos
{
    public class UserRegistrationDto
    {
        [Required] public string? FirstName { get; set; }
        [Required] public string? MiddleName { get; set; }
        [Required] public string? LastName { get; set; }
        [Required] public string? UserName { get; set; }
        [Required] public string? Email { get; set; }
        [Required] public string? Password { get; set; }
        [Required] public string? PhoneNumber { get; set; }
    }
}
