namespace API.Controllers;

using API.Controllers.Common;
using API.Middleware;

using Domain.DTOs.Basket;
using Domain.DTOs.Order;
using Domain.Extensions;
using Domain.Interfaces.Services;
using Domain.Shared.Configurations;

using Localization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

using Stripe;

[ApiController]
[Route("api/[controller]")]
[Authorize]
//[HasPermission(Permissions.PaymentAccess)]
[ApiBase(Order = 1)]
public class PaymentsController(IPaymentService paymentService, IBasketService basketService, IOrderService orderService,
	IOptionsMonitor<StripeSettings> stripeSettings, IStringLocalizer<Resource> localizer) : ApiBaseController
{

	[HttpPost]
	[Route("CreateOrUpdatePaymentIntent", Name = "CreateOrUpdatePaymentIntent")]
	//[HasPermission(Permissions.PaymentModify)]
	[ProducesResponseType(typeof(OrderDto), 200)]
	public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntentAsync()
	{
		ArgumentNullException.ThrowIfNull(CurrentUserId);
		var basket = await basketService.GetBasketAsync(CurrentUserId.Value, true);
		if (basket == null) return NotFound();

		var intent = await paymentService.CreateOrUpdatePaymentIntent(basket);
		if (intent == null) return BadRequest(new ProblemDetails { Title = "Problem creating payment intent" });

		basket.PaymentIntentId = basket.PaymentIntentId ?? intent.Id;
		basket.ClientSecret = basket.ClientSecret ?? intent.ClientSecret;

		var result = await basketService.UpdateBasketAsync(basket);

		if (!result) return BadRequest(new ProblemDetails { Title = "Problem updating basket with intent" });

		return basket.MapBasketToDto();
	}

	[HttpPost]
	[Route("webhook")]
	[AllowAnonymous]
	public async Task<ActionResult> StripeWebhookAsync()
	{
		var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
		var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"],
			stripeSettings.CurrentValue.WhSecret);

		var charge = (Charge)stripeEvent.Data.Object;

		await orderService.UpdateOrderStatusAsync(charge.PaymentIntentId, charge.Status);
		return new EmptyResult();
	}

}
