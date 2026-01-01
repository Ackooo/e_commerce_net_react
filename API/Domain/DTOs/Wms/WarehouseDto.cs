namespace Domain.DTOs.Wms;

public class WarehouseDto
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }
   
    public required string Description { get; set; }

    public required string Location { get; set; }
    
    public required decimal Latitude { get; set; }
  
    public required decimal Longitude { get; set; }

}
