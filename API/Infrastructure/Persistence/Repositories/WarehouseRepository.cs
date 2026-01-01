namespace Infrastructure.Persistence.Repositories;

using Domain.Entities.Wms;
using Domain.Extensions;
using Domain.Interfaces.Repository;
using Domain.RequestHelpers;
using Microsoft.EntityFrameworkCore;

public class WarehouseRepository(StoreContext storeContext) : IWarehouseRepository
{
    public IQueryable<Warehouse> GetWarehouseQuery(WarehouseParams warehouseParams)
    {
        ArgumentNullException.ThrowIfNull(warehouseParams);

        return storeContext.Warehouses.AsNoTracking()
            .Sort(warehouseParams.OrderBy)
            .Search(warehouseParams.SearchTerm)
            .Filter(warehouseParams.Locations)
            .AsQueryable();
    }

    public Task<Warehouse?> GetById(Guid id)
    {
        if(id == Guid.Empty) throw new ArgumentNullException(nameof(id));

        return storeContext.Warehouses.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
