namespace Infrastructure.Authentication;

using Domain.Entities.User;

using Microsoft.AspNetCore.Mvc;

public class ApiBaseController : ControllerBase
{
    public User? CurrentUser { get; set; }

    public Guid? CurrentUserId { get; set; } = Guid.Empty;

}
