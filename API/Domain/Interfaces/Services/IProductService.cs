namespace Domain.Interfaces.Services;

using Domain.DTOs.Product;
using Domain.Entities.Product;
using Domain.RequestHelpers;

public interface IProductService
{
    /// <summary>
    /// Get product from database
    /// </summary>
    /// <param name="id">Product Id</param>
    /// <param name="isTracked"> Determines whether a query is tracking </param>
    /// <returns>The product if found, otherwise null</returns>
    Task<Product?> GetProductAsync(long id, bool isTracked);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="productParams"></param>
    /// <returns>List of products according to the given parameters</returns>
    Task<PagedList<Product>> GetProductsFromQueryPagedAsync(ProductParams productParams);

    /// <summary>
    /// 
    /// </summary>
    /// <returns>All brands and types of existing products</returns>
    Task<ProductFiltersDto> GetProductFiltersAsync();

    /// <summary>
    /// Adds product to database
    /// </summary>
    /// <param name="product">Product to add</param>
    /// <returns>True if the operation was successful</returns>
    Task<bool> AddProductAsync(Product product);

    /// <summary>
    /// Updates tracked product in database
    /// </summary>
    /// <param name="existingProduct">Tracked product to update</param>
    /// <returns>True if the operation was successful</returns>
    Task<bool> UpdateProductAsync(Product existingProduct);

    /// <summary>
    /// Removes product from database
    /// </summary>
    /// <param name="id">Product id</param>
    /// <returns>Deleted product's PublicId if exists</returns>
    Task<string?> DeleteProductAsync(long id);
}
