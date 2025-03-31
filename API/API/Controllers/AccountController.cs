﻿namespace API.Controllers;

using API.Controllers.Common;
using API.Middleware;

using Domain.DTOs.User;
using Domain.Entities.Basket;
using Domain.Entities.User;
using Domain.Extensions;
using Domain.Interfaces.Services;
using Domain.Shared.Constants;
using Localization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[ApiBase(Order = 1)]
public class AccountController(UserManager<User> userManager, ITokenService tokenService,
    IBasketService basketService, IStringLocalizer<Resource> localizer) : ApiBaseController
{

    [HttpPost]
    [Route("Login", Name = "Login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserDto), 200)]
    public async Task<ActionResult<UserDto>> LoginAsync(LoginDto loginDto)
    {
        if(string.IsNullOrWhiteSpace(loginDto.Username) || string.IsNullOrWhiteSpace(loginDto.Password))
            return BadRequest(localizer["Login_InvalidCreds"]);

        var user = await userManager.FindByNameAsync(loginDto.Username);
        if(user == null || !await userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            return Unauthorized();
        }

        var userBasket = await RetrieveBasket(loginDto.Username);
        var anonBasket = await RetrieveBasket(Request.Cookies["buyerId"]);

        if(anonBasket != null)
        {
            if(userBasket != null) await basketService.DeleteBasketAsync(userBasket.Id);
            anonBasket.BuyerId = user.UserName;
            Response.Cookies.Delete("buyerId");
        }

        return new UserDto
        {
            Email = user.Email,
            Token = await tokenService.GenerateTokenAsync(user),
            Basket = anonBasket != null ? anonBasket.MapBasketToDto() : userBasket?.MapBasketToDto()
        };
    }

    [HttpPost]
    [Route("Register", Name = "Register")]
    [AllowAnonymous]
    public async Task<ActionResult> RegisterAsync(RegisterDto registerDto)
    {
        if(!ValidateRegisterDto(registerDto))
            return ValidationProblem(localizer["Login_InvalidCreds"]);

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

        await userManager.AddToRoleAsync(user, Roles.Member);

        return StatusCode(201);
    }

    [HttpGet("currentUser")]
    [Route("currentUser", Name = "GetCurrentUser")]
    [ProducesResponseType(typeof(UserDto), 200)]
    public async Task<ActionResult<UserDto>> GetCurrentUserAsync()
    {
        var user = await userManager.FindByNameAsync(User.Identity.Name);
        var userBasket = await RetrieveBasket(User.Identity.Name);

        return new UserDto
        {
            Email = user.Email,
            Token = await tokenService.GenerateTokenAsync(user),
            Basket = userBasket?.MapBasketToDto()
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

    //Copied from the basket controller
    private async Task<Basket?> RetrieveBasket(string buyerId)
    {
        if(string.IsNullOrEmpty(buyerId))
        {
            Response.Cookies.Delete("buyerId");
            return null;
        }

        return await basketService.GetBasketAsync(buyerId);
    }

    #endregion

}
