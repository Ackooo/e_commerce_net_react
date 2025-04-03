namespace Domain.Interfaces.Services;

using Domain.DTOs.Basket;
using Domain.Entities.Basket;
using Microsoft.AspNetCore.Http;

public interface IBasketService
{
    /// <summary>
    /// Get basket by user from database
    /// </summary>
    /// <param name="userId">User id</param>
    /// <param name="isTracked">Determines whether a query is tracking</param>
    /// <returns>The basket if found, otherwise null</returns>
    Task<Basket?> GetBasketAsync(Guid userId, bool isTracked = false);

    /// <summary>
    /// Updates basket from the database
    /// </summary>
    /// <param name="existingBasket">Tracked basket to update</param>
    /// <returns>True if the operation was successful</returns>
    Task<bool> UpdateBasketAsync(Basket existingBasket);

    /// <summary>
    /// Removes basket from the database
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True if the operation was successful</returns>
    Task<bool> DeleteBasketAsync(Guid id);

    /// <summary>
    /// Add item to the Basket if basket exists, otherwise create new basket with the item
    /// </summary>
    /// <param name="response">HttpResponse</param>
    /// <param name="buyerId">User id if exists, otherwise anonymous</param>
    /// <param name="productId">Item/Product id</param>
    /// <param name="quantity">Quantity to be added</param>
    /// <returns>Updated basket</returns>
    Task<BasketDto?> AddItemToBasket(HttpResponse response, Guid? buyerId, long productId, int quantity);

    /// <summary>
    /// Removes product from the basket
    /// </summary>
    /// <param name="existingBasket">Tracked basket to update</param>
    /// <param name="productId">PRoduct id to remove</param>
    /// <param name="quantity">Quantity to remove</param>
    /// <returns></returns>
    Task<bool> RemoveItemAsync(Basket existingBasket, long productId, int quantity);
}
