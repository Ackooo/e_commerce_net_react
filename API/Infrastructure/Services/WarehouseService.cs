namespace Infrastructure.Services;

using System;
using System.Threading.Tasks;
using Domain.DTOs.Wms;
using Domain.Extensions;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Services;
using Domain.RequestHelpers;

public class WarehouseService(IWarehouseRepository warehouseRepository) : IWarehouseService
{
    public async Task<PagedList<WarehouseDto>> GetWarehousesFromQueryAsync(WarehouseParams warehouseParams)
    {
        ArgumentNullException.ThrowIfNull(warehouseParams);

        //TODO: if admin all otherwise just connected, add to filter
        var query = warehouseRepository.GetWarehouseQuery(warehouseParams);
        var dtoQuery = query.Select(x => x.MapToDto());
        return await PagedList<WarehouseDto>.ToPagedList(dtoQuery,
            warehouseParams.PageNumber, warehouseParams.PageSize);
    }

    public async Task<WarehouseDto?> GetWarehouse(Guid id)
    {
        if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));

        //TODO: check permission for specific wh
        var warehouse = await warehouseRepository.GetById(id);
        return warehouse?.MapToDto();
        
    }
}
