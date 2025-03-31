namespace Infrastructure.Persistence.Repositories;

using Domain.DTOs.Product;
using Domain.Entities.Product;
using Domain.Extensions;
using Domain.Interfaces.Repository;
using Domain.RequestHelpers;
using Microsoft.EntityFrameworkCore;

public class ProductRepository(StoreContext storeContext) : IProductRepository
{

    public Task<Product?> GetProductAsync(long id, bool isTracked = false)
    {
        var query = isTracked
            ? storeContext.Products.AsTracking()
            : storeContext.Products.AsNoTracking();

        return query.FirstOrDefaultAsync(x => x.Id == id);
    }

    public IQueryable<Product> GetProductQuery(ProductParams productParams)
    {
        ArgumentNullException.ThrowIfNull(productParams);

        return storeContext.Products.AsNoTracking()
            .Sort(productParams.OrderBy)
            .Search(productParams.SearchTerm)
            .Filter(productParams.Brands, productParams.Types)
            .AsQueryable();
    }

    public async Task<ProductFiltersDto> GetProductFiltersAsync()
    {
        return new ProductFiltersDto
        {
            Brands = await storeContext.Products.AsNoTracking()
                .Select(p => p.Brand).Distinct().ToListAsync(),
            Types = await storeContext.Products.AsNoTracking()
                .Select(p => p.Type).Distinct().ToListAsync()
        };
    }

    public async Task<bool> AddProductAsync(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        await storeContext.Products.AddAsync(product);
        return await storeContext.SaveChangesAsync() != 0;
    }

    public async Task<bool> UpdateProductAsync(Product existingProduct)
    {
        ArgumentNullException.ThrowIfNull(existingProduct);

        storeContext.Products.Update(existingProduct);
        return await storeContext.SaveChangesAsync() != 0;
    }

    public async Task<string?> DeleteProductAsync(long id)
    {
        var product = await storeContext.Products.AsTracking()
            .FirstAsync(x => x.Id == id);

        var publicId = product.PublicId;

        storeContext.Products.Remove(product);
        await storeContext.SaveChangesAsync();

        return publicId;
    }

}
