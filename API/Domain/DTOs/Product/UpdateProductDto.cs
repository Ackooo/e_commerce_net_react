namespace Domain.DTOs.Product;

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

public class UpdateProductDto
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    [Range(100, double.PositiveInfinity)]
    public long Price { get; set; }
    public IFormFile File { get; set; }
    [Required]
    public string Brand { get; set; }
    [Required]
    public string Type { get; set; }
    [Required]
    [Range(0, int.MaxValue)]
    public int QuantityInStock { get; set; }
}