namespace Infrastructure.Services;

using Domain.Entities.Basket;
using Domain.Interfaces.Services;

using Microsoft.Extensions.Configuration;

using Stripe;

public class PaymentService(IConfiguration config) : IPaymentService
{
    private readonly IConfiguration _config = config;

    public async Task<PaymentIntent> CreateOrUpdatePaymentIntent(Basket basket)
    {
        StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];

        var service = new PaymentIntentService();
        var intent = new PaymentIntent();
        var subtotal = basket.Items.Sum(item => item.Quantity * item.Product.Price);
        var deliveryFee = subtotal > 10000 ? 0 : 500;

        if (string.IsNullOrEmpty(basket.PaymentIntentId))
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = subtotal + deliveryFee,
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" }
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
