namespace API.Controllers;

using System.Threading.Tasks;

using Domain.DTOs.Basket;
using Domain.Extensions;
using Domain.Interfaces.Extensions;
using Domain.Interfaces.Services;
using Domain.Shared.Constants;
using Infrastructure.Authentication;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
//[HasPermission(Permissions.BasketAccess)]
[ApiBase(Order = 1)]
public class BasketController(IBasketService basketService, IApiLocalizer localizer)
    : ApiBaseController
{
    #region GET

    [HttpGet]
    [Route("Basket", Name = "GetBasket")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BasketDto), 200)]
    public async Task<ActionResult<BasketDto>> GetBasketAsync()
    {
        var buyerId = GetBuyerId();
        if(!buyerId.HasValue) return NotFound();

        var basket = await basketService.GetBasketAsync(buyerId.Value);
        if(basket == null) return NotFound();

        return basket.MapToBasketDto();
    }

    #endregion

    #region POST

    [HttpPost]
    [Route("AddItem", Name = "AddItemToBasket")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BasketDto), 200)]
    public async Task<ActionResult<BasketDto>> AddItemToBasketAsync(long productId, int quantity)
    {
        var buyerId = GetBuyerId();

        var result = await basketService.AddItemToBasket(Response, buyerId, productId, quantity);

        if(result != null) return CreatedAtRoute("GetBasket", result);
        return BadRequest(new ProblemDetails { Title = localizer.Translate("Basket_ProblemSave") });
    }

    #endregion

    #region DELETE

    [HttpDelete]
    [Route("RemoveItem", Name = "RemoveBasketItem")]
    [AllowAnonymous]
    public async Task<ActionResult> RemoveBasketItemAsync(long productId, int quantity)
    {
        var buyerId = GetBuyerId();
        if(!buyerId.HasValue) return NotFound();

        var basket = await basketService.GetBasketAsync(buyerId.Value, true);
        if(basket == null) return NotFound();

        var result = await basketService.RemoveItemAsync(basket, productId, quantity);
        if(result) return Ok();

        return BadRequest(new ProblemDetails { Title = localizer.Translate("Basket_ProblemRemove") });
    }

    #endregion

    #region Helper

    private Guid? GetBuyerId()
    {
        if(CurrentUserId.HasValue && CurrentUserId != Guid.Empty) return CurrentUserId;

        var buyerId = Request.Cookies[RequestConstants.CookiesBasketUserId];
        if(string.IsNullOrWhiteSpace(buyerId))
        {
            Response.Cookies.Delete(RequestConstants.CookiesBasketUserId);
            return null;
        }

        return !Guid.TryParse(buyerId, out var userId) ? null : userId;
    }

    #endregion
}