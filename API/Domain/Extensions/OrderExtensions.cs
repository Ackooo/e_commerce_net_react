namespace Domain.Extensions;

using Domain.DTOs.Order;
using Domain.Entities.Order;

public static class OrderExtensions
{
    #region Queryable

    public static IQueryable<OrderDto> ProjectOrderToOrderDto(this IQueryable<Order> query)
    {
        return query
            .Select(order => new OrderDto
            {
                Id = order.Id,
                BuyerId = order.UserId,
                OrderDate = order.OrderDate,
                ShippingAddress = order.ShippingAddress,
                DeliveryFee = order.DeliveryFee,
                Subtotal = order.Subtotal,
                OrderStatus = order.OrderStatus.ToString(),
                Total = order.GetTotal(),
                OrderItems = order.OrderItems.Select(item => new OrderItemDto
                {
                    ProductId = item.ProductId,
                    Name = item.Name,
                    PictureUrl = item.PictureUrl,
                    Price = item.Price,
                    Quantity = item.Quantity
                }).ToList()
            });
    }

    #endregion

    #region Mapper

    public static OrderDto MapToOrderDto(this Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            BuyerId = order.UserId,
            OrderDate = order.OrderDate,
            ShippingAddress = order.ShippingAddress,
            DeliveryFee = order.DeliveryFee,
            Subtotal = order.Subtotal,
            OrderStatus = order.OrderStatus.ToString(),
            Total = order.GetTotal(),
            OrderItems = order.OrderItems.Select(item => new OrderItemDto
            {
                ProductId = item.ProductId,
                Name = item.Name,
                PictureUrl = item.PictureUrl,
                Price = item.Price,
                Quantity = item.Quantity
            }).ToList()
        };
    }

    #endregion
}
