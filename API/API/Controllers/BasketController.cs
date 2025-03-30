namespace API.Controllers;

using System.Threading.Tasks;

using API.Controllers.Common;
using API.Middleware;

using Domain.DTOs.Basket;
using Domain.Entities.Basket;
using Domain.Extensions;
using Domain.Interfaces.Services;

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
		if (string.IsNullOrWhiteSpace(buyerId)) return NotFound();

		var basket = await basketService.GetBasketAsync(buyerId);
		if (basket == null) return NotFound();

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
		var basket = await basketService.GetBasketAsync(buyerId);

		if (basket == null) basket = await CreateBasket();

		var product = await productService.GetProductAsync(productId);
		if (product == null) return BadRequest(new ProblemDetails { Title = localizer["Product_NotFound"] });

		var result = await basketService.AddItemAsync(basket, product, quantity);
		if (result) return CreatedAtRoute("GetBasket", basket.MapBasketToDto());

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
		var basket = await basketService.GetBasketAsync(buyerId);
		if (basket == null) return NotFound();

		var result = await basketService.RemoveItemAsync(basket, productId, quantity);
		if (result) return Ok();

		return BadRequest(new ProblemDetails { Title = localizer["Basket_ProblemRemove"] });
	}

	#endregion

	#region Helper

	private string? GetBuyerId()
	{
		var buyerId = User.Identity?.Name ?? Request.Cookies["buyerId"];
		if (string.IsNullOrEmpty(buyerId))
		{
			Response.Cookies.Delete("buyerId");
		}
		return buyerId;
	}

	private async Task<Basket> CreateBasket()
	{
		var buyerId = User.Identity?.Name;
		if (string.IsNullOrEmpty(buyerId))
		{
			buyerId = Guid.NewGuid().ToString();
			var cookieOptions = new CookieOptions
			{ IsEssential = true, Expires = DateTime.Now.AddDays(30), HttpOnly = false };
			Response.Cookies.Append("buyerId", buyerId, cookieOptions);
		}

		var basket = new Basket { BuyerId = buyerId };
		return await basketService.AddBasketAsync(basket);//start to track
	}

	#endregion
}