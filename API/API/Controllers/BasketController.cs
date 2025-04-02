namespace API.Controllers;

using System.Threading.Tasks;

using API.Controllers.Common;
using API.Middleware;

using Domain.DTOs.Basket;
using Domain.Entities.Basket;
using Domain.Extensions;
using Domain.Interfaces.Services;
using Domain.Shared.Constants;
using Localization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

[ApiController]
[Route("api/[controller]")]
[Authorize]
//[HasPermission(Permissions.BasketAccess)]
[ApiBase(Order = 1)]
public class BasketController(IBasketService basketService, IProductService productService,
    IStringLocalizer<Resource> localizer) : ApiBaseController
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

        return basket.MapBasketToDto();
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
        Basket basket;
        if(!buyerId.HasValue)
        {
            basket = await CreateBasket();
        }
        else
        {
            var existingBasket = await basketService.GetBasketAsync(buyerId.Value, true);
            basket = existingBasket ?? await CreateBasket();
        }

        var product = await productService.GetProductAsync(productId, true);
        if(product == null) return BadRequest(new ProblemDetails { Title = localizer["Product_NotFound"] });

        var result = await basketService.AddItemAsync(basket, product, quantity);
        if(result) return CreatedAtRoute("GetBasket", basket.MapBasketToDto());

        return BadRequest(new ProblemDetails { Title = localizer["Basket_ProblemSave"] });
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

        return BadRequest(new ProblemDetails { Title = localizer["Basket_ProblemRemove"] });
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

    private async Task<Basket> CreateBasket()
    {
        var buyerId = CurrentUserId;
        if(!buyerId.HasValue || buyerId == Guid.Empty)
        {
            buyerId = Guid.CreateVersion7();
            var cookieOptions = new CookieOptions
            {
                IsEssential = true,
                Expires = DateTime.Now.AddDays(30),
                HttpOnly = false
            };
            Response.Cookies.Append(RequestConstants.CookiesBasketUserId, buyerId.Value.ToString(), cookieOptions);
        }

        var basket = new Basket { UserId = buyerId.Value };
        return await basketService.AddBasketAsync(basket);//start to track
    }

    #endregion
}