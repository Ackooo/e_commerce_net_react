namespace Domain.Interfaces.Repository;

using Domain.DTOs.Product;
using Domain.Entities.Product;
using Domain.RequestHelpers;

public interface IProductRepository
{
    Task<Product?> GetProductAsync(int id);

    IQueryable<Product> GetProductQuery(ProductParams productParams);

    Task<ProductFiltersDto> GetProductFiltersAsync();

    Task<bool> AddProductAsync(Product product);

    Task<bool> UpdateProductAsync(Product product);

    Task<bool> DeleteProductAsync(int id);
}
