namespace Infrastructure.Services;

using Domain.Entities.Basket;
using Domain.Entities.Product;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Services;

public class BasketService(IBasketRepository basketRepository) : IBasketService
{
	public Task<Basket?> GetBasketAsync(string? buyerId)
	{
		if (string.IsNullOrWhiteSpace(buyerId)) return Task.FromResult<Basket?>(null);
		return basketRepository.GetBasketAsync(buyerId);
	}

	public async Task<Basket> AddBasketAsync(Basket basket)
	{
		return await basketRepository.AddBasketAsync(basket);
	}

	public async Task<bool> UpdateBasketAsync(Basket basket)
	{
		return await basketRepository.UpdateBasketAsync(basket);
	}

	public async Task<bool> AddItemAsync(Basket basket, Product product, int quantity)
	{
		return await basketRepository.AddItemAsync(basket, product, quantity);
	}

	public async Task<bool> DeleteBasketAsync(Guid id)
	{
		return await basketRepository.DeleteBasketAsync(id);
	}

	public async Task<bool> RemoveItemAsync(Basket basket, long productId, int quantity)
	{
		return await basketRepository.RemoveItemAsync(basket, productId, quantity);
	}
}