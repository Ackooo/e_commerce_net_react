namespace Domain.DTOs.User;

using Domain.DTOs.Basket;

public class UserDto
{
    public string Email { get; set; }
    public string Token { get; set; }
    public BasketDto? Basket { get; set; }
}
