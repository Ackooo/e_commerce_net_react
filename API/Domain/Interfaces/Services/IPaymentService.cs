namespace Domain.Interfaces.Services;

using System.Threading.Tasks;

using Domain.Entities.Basket;

using Stripe;

public interface IPaymentService
{
    Task<PaymentIntent> CreateOrUpdatePaymentIntent(Basket basket);
}