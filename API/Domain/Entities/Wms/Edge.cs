namespace Domain.Entities.Wms;

using Domain.Shared.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(Edge), Schema = DbConstants.DbSchemaNameWms)]
public class Edge
{
    [Column]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column]
    [Required]
    [ForeignKey(nameof(Warehouse))]
    public required int SourceId { get; set; }

    [Column]
    [Required]
    [ForeignKey(nameof(Warehouse))]
    public required int TargetId { get; set; }

    [Column]
    [Required]
    [Range(0, long.MaxValue)]
    public required int Weight { get; set; }

    #region NavigationProperies

#pragma warning disable CS8618

    public Area Source { get; set; } 
    public Area Target { get; set; }

#pragma warning restore CS8618

    #endregion

}
