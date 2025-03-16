namespace Infrastructure.Services;

using Domain.DTOs.Order;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Services;
using Domain.Exceptions;

using Microsoft.Extensions.Localization;
using Localization;
using Domain.Entities.Order;
using AutoMapper;

public class OrderService(IOrderRepository orderRepository, IBasketRepository basketRepository, 
	IUserRepository userRepository, IMapper mapper, IStringLocalizer<Resource> localizer) : IOrderService
{
	public async Task<OrderDto> GetByIdAsync(Guid id, string buyerId)
	{
		var order = await orderRepository.GetByIdAsync(id, buyerId);
		//return order.ProjectOrderToOrderDto();
		return mapper.Map<OrderDto>(order);
	}

	public async Task<List<OrderDto>> GetByBuyerIdAsync(string buyerId)
	{
		var orders = await orderRepository.GetByBuyerIdAsync(buyerId);
		//return orders.ProjectOrderToOrderDto().ToList();
		return mapper.Map<List<OrderDto>>(orders);
	}

	public async Task<Guid> CreateOrder(string buyerId, CreateOrderDto orderDto)
	{
		var basket = await basketRepository.GetBasketAsync(buyerId)
			?? throw new ApiException(localizer["Basket_ProblemLocate"]);

		var items = new List<OrderItem>();

		foreach (var item in basket.Items)
		{
			var productItem = item.Product;
			//var productItem = await context.Products.FindAsync(item.ProductId);

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
			BuyerId = buyerId,
			ShippingAddress = orderDto.ShippingAddress,
			Subtotal = subtotal,
			DeliveryFee = deliveryFee,
			PaymentIntentId = basket.PaymentIntentId

		};

		await orderRepository.AddOrderAsync(order);
		await basketRepository.DeleteBasketAsync(basket.Id);

		if (orderDto.SaveAddress)
		{
			await userRepository.AddUsersAddressAsync(buyerId, orderDto);
		}

		return order.Id;
	}

	public async Task UpdateOrderStatusAsync(string paymentIntentId, string chargeStatus)
	{
		await orderRepository.UpdateOrderStatusAsync(paymentIntentId, chargeStatus);
	}

}
