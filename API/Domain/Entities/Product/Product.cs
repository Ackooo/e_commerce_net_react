namespace Domain.Entities.Product;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(Product), Schema = "Store")]
//[Index(Name = "", )]
public class Product
{
	[Column]
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long Id { get; set; }

	[Column]
	[Required]
	[MaxLength(256)]
	public required string Name { get; set; }

	[Column]
	[Required]
	[MaxLength(256)]
	public required string Description { get; set; }

	[Column]
	[Required]
	[Range(0, long.MaxValue)]
    public required long Price { get; set; }

	[Column]
	[MaxLength(512)]
	public string? PictureUrl { get; set; }

	[Column]
	[Required]
	[MaxLength(256)]
	public required string Brand { get; set; }

	[Column]
	[Required]
	[MaxLength(256)]
	public required string Type { get; set; }

	[Column]
	[Required]
	[Range(0, int.MaxValue)]
    public required int QuantityInStock { get; set; }

	#region Cloudinary

	[Column]
	[MaxLength(512)]
	public string? PublicId { get; set; }

    #endregion
}
