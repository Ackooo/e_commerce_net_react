namespace Infrastructure.Persistence.Repositories;

using Domain.Entities.Basket;
using Domain.Entities.Product;
using Domain.Interfaces.Repository;

using Microsoft.EntityFrameworkCore;

public class BasketRepository(StoreContext storeContext) : IBasketRepository
{

    public Task<Basket?> GetBasketAsync(Guid userId, bool isTracked = false)
    {
        var query = isTracked
            ? storeContext.Baskets.AsTracking()
            : storeContext.Baskets.AsNoTracking();

        return query
            .Include(i => i.BasketItems)
            .ThenInclude(p => p.Product)
            .Where(b => b.UserId == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<Basket> AddBasketAsync(Basket basket)
    {
        await storeContext.Baskets.AddAsync(basket);
        await storeContext.SaveChangesAsync();
        return basket;
    }

    public async Task<bool> UpdateBasketAsync(Basket existingBasket)
    {
        storeContext.Baskets.Update(existingBasket);
        return await storeContext.SaveChangesAsync() != 0;
    }

    public async Task<bool> AddItemAsync(Basket existingBasket, Product existingProduct, int quantity)
    {
        var existingItem = existingBasket.BasketItems
            .FirstOrDefault(item => item.ProductId == existingProduct.Id);

        if(existingItem == null)
        {
            existingBasket.BasketItems.Add(new BasketItem { Product = existingProduct, Quantity = quantity });
        }
        else
        {
            existingItem.Quantity += quantity;
        }

        return await storeContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteBasketAsync(Guid id)
    {
        return await storeContext.Baskets.Where(b => b.Id == id).ExecuteDeleteAsync() != 0;
    }

    public async Task<bool> RemoveItemAsync(Basket existingBasket, long productId, int quantity)
    {
        var item = existingBasket.BasketItems
            .FirstOrDefault(item => item.ProductId == productId);
        if(item == null) return false;

        item.Quantity -= quantity;
        if(item.Quantity <= 0) existingBasket.BasketItems.Remove(item);
        return await storeContext.SaveChangesAsync() > 0;
    }

}
