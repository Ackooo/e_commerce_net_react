namespace Domain.RequestHelpers;

using Domain.Entities.Product;

public class ProductParams : PaginationParams
{
    public string OrderBy { get; set; } = nameof(Product.Name);
    public string? SearchTerm { get; set; }
    public string? Types { get; set; }
    public string? Brands { get; set; }

}
