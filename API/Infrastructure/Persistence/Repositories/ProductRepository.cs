namespace Infrastructure.Persistence.Repositories;

using Domain.Entities.Product;
using Domain.Interfaces.Repository;
using Domain.RequestHelpers;
using Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using Domain.DTOs.Product;
using Domain.Entities.Basket;

public class ProductRepository(StoreContext storeContext) : IProductRepository
{

	public async Task<Product?> GetProductAsync(Guid id)
	{
		return await storeContext.Products.FindAsync(id);
	}

	public IQueryable<Product> GetProductQuery(ProductParams productParams)
	{
		return storeContext.Products
			.Sort(productParams.OrderBy)
			.Search(productParams.SearchTerm)
			.Filter(productParams.Brands, productParams.Types)
			.AsQueryable();
	}

	public async Task<ProductFiltersDto> GetProductFiltersAsync()
	{
		return new ProductFiltersDto
		{
			Brands = await storeContext.Products.Select(p => p.Brand).Distinct().ToListAsync(),
			Types = await storeContext.Products.Select(p => p.Type).Distinct().ToListAsync()
		};
	}

	public async Task<bool> AddProductAsync(Product product)
	{
		await storeContext.Products.AddAsync(product);
		return await storeContext.SaveChangesAsync() != 0;
	}

	public async Task<bool> UpdateProductAsync(Product product)
	{
		storeContext.Products.Update(product);
		return await storeContext.SaveChangesAsync() != 0;
	}

	public async Task<bool> DeleteProductAsync(Guid id)
	{
		return await storeContext.Products.Where(p => p.Id == id).ExecuteDeleteAsync() != 0;
	}

}
