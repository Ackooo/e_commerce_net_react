namespace Infrastructure.Services;

using Domain.DTOs.Order;
using Domain.Entities.Order;
using Domain.Exceptions;
using Domain.Extensions;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Services;
using Localization;
using Microsoft.Extensions.Localization;

public class OrderService(IOrderRepository orderRepository, IBasketRepository basketRepository,
    IUserRepository userRepository, IStringLocalizer<Resource> localizer) : IOrderService
{
    public async Task<OrderDto> GetByIdAsync(Guid id, Guid userId)
    {
        var order = await orderRepository.GetByIdAsync(id, userId);
        return order.MapToOrderDto();
    }

    public async Task<List<OrderDto>> GetByBuyerIdAsync(Guid userId)
    {
        var orders = await orderRepository.GetByBuyerIdAsync(userId);
        return [.. orders.Select(x => x.MapToOrderDto())];
    }

    public async Task<Guid> CreateOrder(Guid userId, CreateOrderDto orderDto)
    {
        var basket = await basketRepository.GetBasketAsync(userId)
            ?? throw new ApiException(localizer["Basket_ProblemLocate"]);

        var items = new List<OrderItem>();
        foreach(var item in basket.BasketItems)
        {
            var productItem = item.Product;
            var orderItem = new OrderItem
            {
                Price = productItem.Price,
                Quantity = item.Quantity,
                ProductId = productItem.Id,
                Name = productItem.Name,
                PictureUrl = productItem.PictureUrl,
            };
            items.Add(orderItem);
            productItem.QuantityInStock -= item.Quantity;
        }

        var subtotal = items.Sum(item => item.Price * item.Quantity);
        var deliveryFee = subtotal > 10000 ? 0 : 500; //5$

        var order = new Order
        {
            OrderItems = items,
            UserId = userId,
            ShippingAddress = orderDto.ShippingAddress,
            Subtotal = subtotal,
            DeliveryFee = deliveryFee,
            PaymentIntentId = basket.PaymentIntentId

        };

        await orderRepository.AddOrderAsync(order);
        await basketRepository.DeleteBasketAsync(basket.Id);

        if(orderDto.SaveAddress)
        {
            await userRepository.AddUsersAddressAsync(userId, orderDto);
        }

        return order.Id;
    }

    public async Task UpdateOrderStatusAsync(string paymentIntentId, string chargeStatus)
    {
        await orderRepository.UpdateOrderStatusAsync(paymentIntentId, chargeStatus);
    }

}
