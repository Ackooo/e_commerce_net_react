namespace Domain.Entities.Product;

using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(Product), Schema = "Store")]
public class Product
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public long CIId { get; set; }

    public required string Name { get; set; }
    public required string Description { get; set; }
    public long Price { get; set; }
    public string? PictureUrl { get; set; }
    public required string Brand { get; set; }
    public required string Type { get; set; }
    public int QuantityInStock { get; set; }

    #region Cloudinary

    public string? PublicId { get; set; }

    #endregion
}
