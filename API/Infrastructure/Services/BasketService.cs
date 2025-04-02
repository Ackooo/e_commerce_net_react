namespace Infrastructure.Services;

using Domain.Entities.Basket;
using Domain.Entities.Product;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Services;

public class BasketService(IBasketRepository basketRepository) : IBasketService
{
    public Task<Basket?> GetBasketAsync(Guid userId, bool isTracked = false)
    {
        if(userId == Guid.Empty) return Task.FromResult<Basket?>(null);
        return basketRepository.GetBasketAsync(userId, isTracked);
    }

    public async Task<Basket> AddBasketAsync(Basket basket)
    {
        return await basketRepository.AddBasketAsync(basket);
    }

    public async Task<bool> UpdateBasketAsync(Basket existingBasket)
    {
        return await basketRepository.UpdateBasketAsync(existingBasket);
    }

    public async Task<bool> AddItemAsync(Basket existingBasket, Product existingProduct, int quantity)
    {
        return await basketRepository.AddItemAsync(existingBasket, existingProduct, quantity);
    }

    public async Task<bool> DeleteBasketAsync(Guid id)
    {
        return await basketRepository.DeleteBasketAsync(id);
    }

    public async Task<bool> RemoveItemAsync(Basket existingBasket, long productId, int quantity)
    {
        return await basketRepository.RemoveItemAsync(existingBasket, productId, quantity);
    }
}