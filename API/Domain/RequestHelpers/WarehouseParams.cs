namespace Domain.RequestHelpers;

using Domain.Entities.Wms;

public class WarehouseParams : PaginationParams
{
    public string OrderBy { get; set; } = nameof(Warehouse.Name);
    public string? SearchTerm { get; set; }
    public string? Locations { get; set; }

}
