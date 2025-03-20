namespace Domain.Entities.Product;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(Product), Schema = "Store")]
public class Product
{
	[Key]
	public long Id { get; set; }

	[Required]
	[MaxLength(256)]
	public required string Name { get; set; }

	[Required]
	[MaxLength(256)]
	public required string Description { get; set; }

	[Required]
	[Range(0, long.MaxValue)]
    public long Price { get; set; }

	[MaxLength(512)]
	public string? PictureUrl { get; set; }

	[Required]
	[MaxLength(256)]
	public required string Brand { get; set; }

	[Required]
	[MaxLength(256)]
	public required string Type { get; set; }

	[Range(0, int.MaxValue)]
    public int QuantityInStock { get; set; }

	#region Cloudinary

	[MaxLength(512)]
	public string? PublicId { get; set; }

    #endregion
}
