namespace Infrastructure.Services;

using Domain.Entities.Basket;
using Domain.Interfaces.Services;
using Domain.Shared.Configurations;

using Microsoft.Extensions.Options;

using Stripe;

public class PaymentService(IOptionsMonitor<StripeSettings> stripeSettings) : IPaymentService
{

    public async Task<PaymentIntent> CreateOrUpdatePaymentIntent(Basket basket)
    {
        StripeConfiguration.ApiKey = stripeSettings.CurrentValue.SecretKey;

        var service = new PaymentIntentService();
        var intent = new PaymentIntent();
        var subtotal = basket.BasketItems.Sum(item => item.Quantity * item.Product.Price);
        var deliveryFee = subtotal > 10000 ? 0 : 500;

        if (string.IsNullOrEmpty(basket.PaymentIntentId))
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = subtotal + deliveryFee,
                Currency = "usd",
                PaymentMethodTypes = ["card"]
            };
            intent = await service.CreateAsync(options);
        }
        else
        {
            var options = new PaymentIntentUpdateOptions
            {
                Amount = subtotal + deliveryFee
            };
            await service.UpdateAsync(basket.PaymentIntentId, options);
        }

        return intent;

    }

}
