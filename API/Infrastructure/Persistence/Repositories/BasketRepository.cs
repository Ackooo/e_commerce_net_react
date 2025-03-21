﻿namespace Infrastructure.Persistence.Repositories;

using Domain.Entities.Basket;
using Domain.Entities.Product;
using Domain.Interfaces.Repository;

using Microsoft.EntityFrameworkCore;

public class BasketRepository(StoreContext storeContext) : IBasketRepository
{

	public Task<Basket?> GetBasketAsync(string buyerId)
	{
		return storeContext.Baskets.AsNoTracking()
			.Include(i => i.BasketItems)
			.ThenInclude(p => p.Product)
			.Where(b => b.BuyerId == buyerId)
			.FirstOrDefaultAsync();
	}

	public async Task<Basket> AddBasketAsync(Basket basket)
	{
		await storeContext.Baskets.AddAsync(basket);
		await storeContext.SaveChangesAsync();
		return basket;
	}

	public async Task<bool> UpdateBasketAsync(Basket basket)
	{
		storeContext.Baskets.Update(basket);
		return await storeContext.SaveChangesAsync() != 0;
	}

	public async Task<bool> AddItemAsync(Basket basket, Product product, int quantity)
	{
		if(basket.BasketItems.All(item => item.ProductId != product.Id))
		{
			basket.BasketItems.Add(new BasketItem { Product = product, Quantity = quantity });
		}

		var existingItem =	basket.BasketItems.FirstOrDefault(item => item.ProductId == product.Id);
		if (existingItem != null)
		{
			existingItem.Quantity += quantity;
		}
		
		return await storeContext.SaveChangesAsync() > 0;
	}

	public async Task<bool> DeleteBasketAsync(Guid id)
	{
		return await storeContext.Baskets.Where(b => b.Id == id).ExecuteDeleteAsync() != 0;
	}

	public async Task<bool> RemoveItemAsync(Basket basket, long productId, int quantity)
	{
		var item = basket.BasketItems.FirstOrDefault(item => item.ProductId == productId);
		//TODO: review
		if (item == null) return false;
		item.Quantity -= quantity;
		if (item.Quantity == 0) basket.BasketItems.Remove(item);
		return await storeContext.SaveChangesAsync() > 0;
	}

}
