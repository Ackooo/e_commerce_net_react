namespace Domain.Entities.Wms;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Basket;
using Domain.Entities.Order;
using Domain.Shared.Constants;

[Table(nameof(Warehouse), Schema = DbConstants.DbSchemaNameWms)]
public class Warehouse
{

    [Column]
    [Key]    
    public Guid Id { get; set; } = Guid.CreateVersion7();

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
    [MaxLength(256)]
    public required string Location { get; set; }

    [Column(TypeName = "decimal(8,6)")]
    [Required]
    [Range(0, int.MaxValue)]
    public required decimal Latitude { get; set; }

    [Column(TypeName = "decimal(9,6)")]
    [Required]
    [Range(0, int.MaxValue)]
    public required decimal Longitude { get; set; }

    #region NavigationProperies

    public ICollection<Area> Areas { get; set; } = [];

    #endregion

}
