namespace Infrastructure.Services;

using System.Threading.Tasks;

using Domain.DTOs.Product;
using Domain.Entities.Basket;
using Domain.Entities.Product;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Services;
using Domain.RequestHelpers;

using Infrastructure.Persistence.Repositories;

public class ProductService(IProductRepository productRepository) : IProductService
{
	public async Task<Product?> GetProductAsync(long id)
	{
		return await productRepository.GetProductAsync(id);
	}

	public async Task<PagedList<Product>> GetProductsFromQueryAsync(ProductParams productParams)
	{
		var query = productRepository.GetProductQuery(productParams);
		return await PagedList<Product>.ToPagedList(query, productParams.PageNumber, productParams.PageSize);
	}

	public async Task<ProductFiltersDto> GetProductFiltersAsync()
	{
		return await productRepository.GetProductFiltersAsync();
	}

	public async Task<bool> AddProductAsync(Product product)
	{
		return await productRepository.AddProductAsync(product);
	}

	public async Task<bool> UpdateProductAsync(Product product)
	{
		return await productRepository.UpdateProductAsync(product);
	}

	public async Task<bool> DeleteProductAsync(long id)
	{
		return await productRepository.DeleteProductAsync(id);
	}
}
