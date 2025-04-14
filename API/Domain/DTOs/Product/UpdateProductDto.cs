namespace Domain.DTOs.Product;

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

public class UpdateProductDto
{
    [Required]
    public required long Id { get; set; }
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