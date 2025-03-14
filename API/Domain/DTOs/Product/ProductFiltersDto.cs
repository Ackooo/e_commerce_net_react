namespace Domain.DTOs.Product;

public class ProductFiltersDto
{

    public required List<string> Brands { get; set; }
    public required List<string> Types { get; set; }
}
