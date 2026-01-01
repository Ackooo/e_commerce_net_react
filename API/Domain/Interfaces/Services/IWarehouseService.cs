namespace Domain.Interfaces.Services;

using Domain.DTOs.Wms;
using Domain.RequestHelpers;

public interface IWarehouseService
{
    /// <summary>
    /// Get all warehouses from database
    /// </summary>
    /// <returns>List of warehouses</returns>
    Task<PagedList<WarehouseDto>> GetWarehousesFromQueryAsync(WarehouseParams warehouseParams);

    /// <summary>
    /// Get warehouse by id
    /// </summary>
    /// <param name="id">Warehouse id</param>
    /// <returns>The warehouse if found, otherwise null</returns>
    Task<WarehouseDto?> GetWarehouse(Guid id);
}
