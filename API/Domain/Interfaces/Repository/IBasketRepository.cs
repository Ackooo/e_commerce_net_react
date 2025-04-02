namespace Domain.Interfaces.Repository;

using Domain.Entities.Basket;
using Domain.Entities.Product;

public interface IBasketRepository
{
    /// <summary>
    /// Get basket by user from database
    /// </summary>
    /// <param name="userId">User id</param>
    /// <param name="isTracked">Determines whether a query is tracking</param>
    /// <returns>The basket if found, otherwise null</returns>
    Task<Basket?> GetBasketAsync(Guid userId, bool isTracked = false);

    /// <summary>
    /// Adds basket to database
    /// </summary>
    /// <param name="basket">Basket to add</param>
    /// <returns>Created basket</returns>
    Task<Basket> AddBasketAsync(Basket basket);

    /// <summary>
    /// Updates basket from the database
    /// </summary>
    /// <param name="existingBasket">Tracked basket to update</param>
    /// <returns>True if the operation was successful</returns>
    Task<bool> UpdateBasketAsync(Basket existingBasket);

    /// <summary>
    /// Adds product to the basket
    /// </summary>
    /// <param name="existingBasket">Tracked basket to update</param>
    /// <param name="existingProduct">Tracked product to add</param>
    /// <param name="quantity">Quantity of products</param>
    /// <returns>True if the operation was successful</returns>
    Task<bool> AddItemAsync(Basket existingBasket, Product existingProduct, int quantity);

    /// <summary>
    /// Removes basket from the database
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True if the operation was successful</returns>
    Task<bool> DeleteBasketAsync(Guid id);

    /// <summary>
    /// Removes product from the basket
    /// </summary>
    /// <param name="existingBasket">Tracked basket to update</param>
    /// <param name="productId">PRoduct id to remove</param>
    /// <param name="quantity">Quantity to remove</param>
    /// <returns></returns>
    Task<bool> RemoveItemAsync(Basket existingBasket, long productId, int quantity);
}