namespace Domain.DTOs.User;

using System.ComponentModel.DataAnnotations;

public class LoginDto
{
    [Required]
    public required string Username { get; set; }
    [Required]
    public required string Password { get; set; }
}
