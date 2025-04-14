namespace Domain.DTOs.Product;

using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;

public class CreateProductDto
{
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string Description { get; set; }
    [Required]
    [Range(100, double.PositiveInfinity)]
    public required long Price { get; set; }
    [Required]
    public required IFormFile File { get; set; }
    [Required]
    public required string Brand { get; set; }
    [Required]
    public required string Type { get; set; }
    [Required]
    [Range(0, int.MaxValue)]
    public required int QuantityInStock { get; set; }
}
