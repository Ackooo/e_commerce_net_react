namespace Domain.Interfaces.Repository;

using Domain.Entities.Wms;
using Domain.RequestHelpers;

public interface IWarehouseRepository
{
    /// <summary>
    /// Get all warehouses query 
    /// </summary>
    /// <param name="warehouseParams"></param>
    /// <returns></returns>
    IQueryable<Warehouse> GetWarehouseQuery(WarehouseParams warehouseParams);

    /// <summary>
    /// Get warehouse by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Warehouse if found, otherwise null</returns>
    Task<Warehouse?> GetById(Guid id);
}
