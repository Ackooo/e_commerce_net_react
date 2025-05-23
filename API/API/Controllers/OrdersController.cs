﻿namespace API.Controllers;

using Domain.DTOs.Order;
using Domain.Interfaces.Extensions;
using Domain.Interfaces.Services;
using Infrastructure.Authentication;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[ApiBase(Order = 1)]
public class OrdersController(IOrderService orderService, IApiLocalizer localizer) 
	: ApiBaseController
{
    #region GET

    [HttpGet]
    [Route("GetOrders", Name = "GetOrders")]
    [ProducesResponseType(typeof(List<OrderDto>), 200)]
    public async Task<ActionResult<List<OrderDto>>> GetOrdersAsync()
    {
        return await orderService.GetByBuyerIdAsync(CurrentUserId!.Value);
    }

    [HttpGet]
    [Route("{id}", Name = "GetOrder")]
    [ProducesResponseType(typeof(OrderDto), 200)]
    public async Task<ActionResult<OrderDto>> GetOrderAsync(Guid id)
    {
        return await orderService.GetByIdAsync(id, CurrentUserId!.Value);
    }

    #endregion

    #region POST

    [HttpPost]
	[Route("", Name = "CreateOrder")]
	//	[HasPermission(Permissions.OrderModify)]
	public async Task<ActionResult<Guid>> CreateOrderAsync(CreateOrderDto orderDto)
	{
		try
		{
            var result = await orderService.CreateOrder(CurrentUserId!.Value, orderDto);
			if (result != Guid.Empty)
				return CreatedAtRoute("GetOrder", new { id = result }, result);
			return BadRequest(localizer.Translate("Order_ProblemCreate"));
		}
		catch (Exception ex)
		{
			return BadRequest(new ProblemDetails { Title = ex.Message });
		}

	}

	#endregion

}
