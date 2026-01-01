namespace API.Controllers;

using Domain.DTOs.Wms;
using Domain.Interfaces.Extensions;
using Domain.Interfaces.Services;
using Domain.RequestHelpers;
using Domain.Shared.Constants;
using Domain.Shared.Enums;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[ApiBase(Order = 1)]
public class WarehouseController(IWarehouseService warehouseService, IApiLocalizer localizer) : ApiBaseController
{
    #region GET

    [HttpGet]
    [Route("", Name = "GetWarehouses")]
    [Authorize(Roles = Roles.WarehouseManager)]
    [HasPermission([Permissions.WarehouseAccess])]    
    [ProducesResponseType(typeof(PagedList<WarehouseDto>), 200)]
    public async Task<ActionResult<PagedList<WarehouseDto>>> GetWarehousesAsync(WarehouseParams warehouseParams)
    {
        return await warehouseService.GetWarehousesFromQueryAsync(warehouseParams);
    }

    [HttpGet]
    [Route("{id}", Name = "GetWarehouseById")]   
    [HasPermission([Permissions.WarehouseAccess])]
    [ProducesResponseType(typeof(WarehouseDto), 200)]
    public async Task<ActionResult<WarehouseDto?>> GetWarehouseAsync(Guid id)
    {
        if(id == Guid.Empty) throw new ArgumentNullException(nameof(id));
        return await warehouseService.GetWarehouse(id);
    }

    #endregion

    #region Post

    [HttpPost]
    [Route("", Name = "CreateProduct")]    
    [HasPermission([Permissions.WarehouseAccess])]
    [ProducesResponseType(typeof(WarehouseDto), 200)]
    public async Task<ActionResult<WarehouseDto>> CreateWarehouseAsync([FromForm] CreateWarehouseDto warehouseDto)
    {
        throw new NotImplementedException();
        //TODO:
        return BadRequest(new ProblemDetails { Title = localizer.Translate("Product_ProblemCreate") });
    }

    #endregion

}
