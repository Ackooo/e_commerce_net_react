namespace Infrastructure.Services;

using System.Threading.Tasks;

using Domain.DTOs.Product;
using Domain.Entities.Product;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Services;
using Domain.RequestHelpers;

public class ProductService(IProductRepository productRepository) : IProductService
{
    public async Task<Product?> GetProductAsync(long id, bool isTracked = false)
    {
        return await productRepository.GetProductAsync(id, isTracked);
    }

    public async Task<PagedList<Product>> GetProductsFromQueryPagedAsync(ProductParams productParams)
    {
        ArgumentNullException.ThrowIfNull(productParams);
        var query = productRepository.GetProductQuery(productParams);
        return await PagedList<Product>.ToPagedList(query, productParams.PageNumber, productParams.PageSize);
    }

    public async Task<ProductFiltersDto> GetProductFiltersAsync()
    {
        return await productRepository.GetProductFiltersAsync();
    }

    public async Task<bool> AddProductAsync(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);
        return await productRepository.AddProductAsync(product);
    }

    public async Task<bool> UpdateProductAsync(Product existingProduct)
    {
        ArgumentNullException.ThrowIfNull(existingProduct);
        return await productRepository.UpdateProductAsync(existingProduct);
    }

    public async Task<string?> DeleteProductAsync(long id)
    {
        return await productRepository.DeleteProductAsync(id);
    }
}
