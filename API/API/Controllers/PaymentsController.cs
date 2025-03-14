namespace API.Controllers;

using Domain.DTOs.Basket;
using Domain.Interfaces.Services;
using Domain.Extensions;

using Infrastructure.Services;

using Localization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

using Stripe;

public class PaymentsController(PaymentService paymentService, IBasketService basketService, IOrderService orderService,
    IConfiguration config, IStringLocalizer<Resource> localizer) : BaseController
{

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntent()
    {
        var basket = await basketService.GetBasketAsync(User.Identity.Name);
        if (basket == null) return NotFound();

        var intent = await paymentService.CreateOrUpdatePaymentIntent(basket);
        if (intent == null) return BadRequest(new ProblemDetails { Title = "Problem creating payment intent" });

        basket.PaymentIntentId = basket.PaymentIntentId ?? intent.Id;
        basket.ClientSecret = basket.ClientSecret ?? intent.ClientSecret;

        var result = await basketService.UpdateBasketAsync(basket);

        if (!result) return BadRequest(new ProblemDetails { Title = "Problem updating basket with intent" });

        return basket.MapBasketToDto();
    }

    [AllowAnonymous]
    [HttpPost("webhook")]
    public async Task<ActionResult> StripeWebhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"],
            config["StripeSettings:WhSecret"]);

        var charge = (Charge)stripeEvent.Data.Object;

        await orderService.UpdateOrderStatusAsync(charge.PaymentIntentId, charge.Status);
        return new EmptyResult();
    }

}
