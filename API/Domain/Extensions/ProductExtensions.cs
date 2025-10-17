namespace Domain.Extensions;

using Domain.DTOs.Product;
using Domain.Entities.Product;

public static class ProductExtensions
{
    #region Queryable

    public static IQueryable<Product> Sort(this IQueryable<Product> query, string? orderBy)
    {
        if(string.IsNullOrWhiteSpace(orderBy)) return query.OrderBy(p => p.Name);

        query = orderBy switch
        {
            "price" => query.OrderBy(p => p.Price),
            "priceDesc" => query.OrderByDescending(p => p.Price),
            _ => query.OrderBy(p => p.Name)
        };

        return query;
    }

    public static IQueryable<Product> Search(this IQueryable<Product> query, string? searchTerm)
    {
        if(string.IsNullOrEmpty(searchTerm)) return query;

        var searchTermLowerCase = searchTerm.Trim().ToLower();
        return query.Where(p => p.Name.Contains(
            searchTermLowerCase, StringComparison.CurrentCultureIgnoreCase));

    }

    public static IQueryable<Product> Filter(this IQueryable<Product> query, string? brands, string? types)
    {
        var brandList = new List<string>();
        var typeList = new List<string>();

        if(!string.IsNullOrEmpty(brands))
            brandList.AddRange(brands.ToLower().Split(",").ToList());

        if(!string.IsNullOrEmpty(types))
            typeList.AddRange(types.ToLower().Split(",").ToList());

        query = query.Where(p => brandList.Count == 0 || brandList.Contains(p.Brand.ToLower()));
        query = query.Where(p => typeList.Count == 0 || typeList.Contains(p.Type.ToLower()));

        return query;
    }

    #endregion

    #region Mapper

    public static Product MapToProduct(this CreateProductDto productDto)
    {
        return new Product
        {
            Name = productDto.Name,
            Description = productDto.Description,
            Price = productDto.Price,
            Brand = productDto.Brand,
            Type = productDto.Type,
            QuantityInStock = productDto.QuantityInStock
        };
    }

    public static void MapToProduct(this UpdateProductDto productDto, Product product)
    {
        product.Name = productDto.Name;
        product.Description = productDto.Description;
        product.Price = productDto.Price;
        product.Brand = productDto.Brand;
        product.Type = productDto.Type;
        product.QuantityInStock = productDto.QuantityInStock;
    }

    #endregion

}