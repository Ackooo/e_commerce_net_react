namespace Domain.Interfaces.Services;

using Domain.DTOs.Product;
using Domain.Entities.Product;
using Domain.RequestHelpers;

public interface IProductService
{
    Task<Product?> GetProductAsync(Guid id);

    Task<PagedList<Product>> GetProductsFromQueryAsync(ProductParams productParams);

    Task<ProductFiltersDto> GetProductFiltersAsync();

    Task<bool> AddProductAsync(Product product);

    Task<bool> UpdateProductAsync(Product product);

    Task<bool> DeleteProductAsync(Guid id);
}
