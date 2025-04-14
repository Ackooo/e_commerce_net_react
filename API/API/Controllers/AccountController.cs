namespace API.Controllers;

using Domain.DTOs.User;
using Domain.Entities.Basket;
using Domain.Entities.User;
using Domain.Exceptions;
using Domain.Extensions;
using Domain.Interfaces.Extensions;
using Domain.Interfaces.Services;
using Domain.Shared.Constants;
using Infrastructure.Authentication;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[ApiBase(Order = 1)]
public class AccountController(UserManager<User> userManager, IUserService userService,
    ITokenService tokenService, IBasketService basketService, IHostEnvironment env,
    IApiLocalizer localizer) : ApiBaseController
{

    [HttpPost]
    [Route("Login", Name = "Login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserDto), 200)]
    public async Task<ActionResult<UserDto>> LoginAsync(LoginDto loginDto)
    {
        if(string.IsNullOrWhiteSpace(loginDto.Username) || string.IsNullOrWhiteSpace(loginDto.Password))
            return BadRequest(localizer.Translate("Login_InvalidCreds"));

        var user = await userManager.FindByNameAsync(loginDto.Username);
        if(user == null || !await userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            return Unauthorized();
        }

        var userBasket = await basketService.GetBasketAsync(user.Id, false);
        var anonBasket = await RetrieveBasket(Request.Cookies[RequestConstants.CookiesBasketUserId]);

        if(anonBasket != null)
        {
            if(userBasket != null) await basketService.DeleteBasketAsync(userBasket.Id);
            anonBasket.UserId = user.Id;
            Response.Cookies.Delete(RequestConstants.CookiesBasketUserId);
        }

        return new UserDto
        {
            Email = user.Email!,
            Token = await tokenService.GenerateTokenAsync(user),
            Basket = anonBasket != null ? anonBasket.MapToBasketDto() : userBasket?.MapToBasketDto()
        };
    }

    [HttpPost]
    [Route("Register", Name = "Register")]
    [AllowAnonymous]
    public async Task<ActionResult> RegisterAsync(RegisterDto registerDto)
    {
        if(!ValidateRegisterDto(registerDto))
            return ValidationProblem(localizer.Translate("Login_InvalidCreds"));

        var user = new User
        {
            UserName = registerDto.Username,
            Email = registerDto.Email
        };

        var result = await userManager.CreateAsync(user, registerDto.Password!);
        if(!result.Succeeded)
        {
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return ValidationProblem();
        }

        await userService.AddRoleToUserAsync(user.Id, Roles.Member);

        return StatusCode(201);
    }

    [HttpGet]
    [Route("currentUser", Name = "GetCurrentUser")]
    [ProducesResponseType(typeof(UserDto), 200)]
    public async Task<ActionResult<UserDto>> GetCurrentUserAsync()
    {
        if(CurrentUserId == null || CurrentUserId == Guid.Empty)
            throw new ApiException(localizer.Translate("Login_InvalidCreds"));

        var user = await userManager.FindByIdAsync(CurrentUserId.ToString()!) 
            ?? throw new ApiException(localizer.Translate("Login_InvalidCreds"));

        var userBasket = await basketService.GetBasketAsync(user.Id, false);
        return new UserDto
        {
            Email = user.Email!,
            Token = await tokenService.GenerateTokenAsync(user),
            Basket = userBasket?.MapToBasketDto()
        };
    }

    [HttpGet]
    [Route("savedAddress", Name = "GetSavedAddress")]
    [ProducesResponseType(typeof(Address), 200)]
    public async Task<ActionResult<Address?>> GetSavedAddress()
    {
        if(CurrentUserId == Guid.Empty) return BadRequest();

        return await userManager.Users.AsNoTracking()
            .Where(x => x.Id == CurrentUserId)
            .Select(user => user.Address)
            .FirstOrDefaultAsync();
    }

    #region Culture

    [HttpPost]
    [AllowAnonymous]
    [Route("SetCulture", Name = "SetCulture")]
    public IActionResult SetCulture(string culture)
    {
        if(!CultureInfos.SupportedCultures.Contains(culture))
        {
            culture = CultureInfos.English_US;
        }

        Response.Cookies.Append(
            CustomClaims.Culture,
            //CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            culture,
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                HttpOnly = env.IsDevelopment(),
                Secure = env.IsDevelopment(),
                SameSite = SameSiteMode.None

            });

        return Ok();
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("AvailableCultures", Name = "GetAvailableCultures")]
    public List<string> GetAvailableCultures()
    {
        return CultureInfos.SupportedCultures;
    }

    #endregion

    #region Helper

    private static bool ValidateRegisterDto(RegisterDto registerDto)
    {
        if(registerDto == null
            || string.IsNullOrWhiteSpace(registerDto.Username)
            || string.IsNullOrWhiteSpace(registerDto.Password)
            || string.IsNullOrWhiteSpace(registerDto.Email))
            return false;
        return true;
    }

    private async Task<Basket?> RetrieveBasket(string? buyerId)
    {
        if(string.IsNullOrWhiteSpace(buyerId))
        {
            Response.Cookies.Delete(RequestConstants.CookiesBasketUserId);
            return null;
        }

        return !Guid.TryParse(buyerId, out var userId)
            ? null
            : await basketService.GetBasketAsync(userId, false);
    }

    #endregion

}
