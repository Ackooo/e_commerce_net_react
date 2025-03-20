namespace Domain.Interfaces.Repository;

using Domain.Entities.Basket;
using Domain.Entities.Product;

public interface IBasketRepository
{
    Task<Basket?> GetBasketAsync(string buyerId);

    Task<Basket> AddBasketAsync(Basket basket);

    Task<bool> UpdateBasketAsync(Basket basket);

    Task<bool> AddItemAsync(Basket basket, Product product, int quantity);

    Task<bool> DeleteBasketAsync(Guid id);

    Task<bool> RemoveItemAsync(Basket basket, long productId, int quantity);
}
