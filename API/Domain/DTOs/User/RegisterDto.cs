namespace Domain.DTOs.User;

using System.ComponentModel.DataAnnotations;

public class RegisterDto : LoginDto
{
    [Required]
    public required string Email { get; set; }
}
