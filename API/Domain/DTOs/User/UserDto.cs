namespace Domain.DTOs.User;

using System.ComponentModel.DataAnnotations;
using Domain.DTOs.Basket;

public class UserDto
{
    [Required]
    public required string Email { get; set; }
    [Required]
    public required string Token { get; set; }
    public BasketDto? Basket { get; set; }
}
