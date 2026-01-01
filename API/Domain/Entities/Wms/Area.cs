namespace Domain.Entities.Wms;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Shared.Constants;

[Table(nameof(Area), Schema = DbConstants.DbSchemaNameWms)]
public class Area()
{
    [Column]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column]
    [Required]
    [ForeignKey(nameof(Warehouse))]
    public required Guid WarehouseId { get; set; }


    [NotMapped]
    public Dictionary<Area, int> Neighbors { get; set; } = [];

    [NotMapped]
    public List<Edge> Edges { get; set; } = [];

    #region NavigationProperies

#pragma warning disable CS8618

    public Warehouse Warehouse { get; set; }

#pragma warning restore CS8618

    #endregion

    #region Methods

    // Add a neighbor to the node with a specific weight (edge)
    public void AddNeighbor(Area neighbor, int weight)
    {
        Neighbors[neighbor] = weight;
    }

    public void AddEdge(Area target, int weight)
    {
        Edges.Add(new Edge
        {
            SourceId = this.Id,
            TargetId = target.Id,
            Weight = weight
        });
    }

    #endregion
}
