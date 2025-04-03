namespace Domain.Extensions;

using Domain.DTOs.Basket;
using Domain.Entities.Basket;

public static class BasketExtensions
{
    #region Mapper

    public static BasketDto MapToBasketDto(this Basket basket)
    {
        return new BasketDto
        {
            Id = basket.Id,
            PaymentItentnId = basket.PaymentIntentId,
            ClientSecret = basket.ClientSecret,
            Items = basket.BasketItems.Select(item => new BasketItemDto
            {
                ProductId = item.ProductId,
                Name = item.Product.Name,
                Price = item.Product.Price,
                PictureUrl = item.Product.PictureUrl,
                Type = item.Product.Type,
                Brand = item.Product.Brand,
                Quantity = item.Quantity
            }).ToList()
        };
    }

    #endregion

}
