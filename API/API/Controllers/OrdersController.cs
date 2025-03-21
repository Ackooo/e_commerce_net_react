﻿namespace API.Controllers;

using Domain.DTOs.Order;
using Domain.Interfaces.Services;

using Localization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

[Authorize]
public class OrdersController(IOrderService orderService, IStringLocalizer<Resource> localizer) : BaseController
{
    #region GET

    [HttpGet]
    [ProducesResponseType(typeof(List<OrderDto>), 200)]
    public async Task<ActionResult<List<OrderDto>>> GetOrders()
    {
		return await orderService.GetByBuyerIdAsync(User.Identity.Name);
    }

    [ProducesResponseType(typeof(OrderDto), 200)]
    [HttpGet("{id}", Name = "GetOrder")]
    public async Task<ActionResult<OrderDto>> GetOrder(Guid id)
    {
        return await orderService.GetByIdAsync(id, User.Identity.Name);
    }

    #endregion

    #region POST

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateOrder(CreateOrderDto orderDto)
    {
        try
        {
            var result = await orderService.CreateOrder(User.Identity.Name, orderDto);
            if (result != Guid.Empty) 
                return CreatedAtRoute("GetOrder", new { id = result }, result);
			return BadRequest(localizer["Order_ProblemCreate"]);
        }
        catch (Exception ex)
        {
            return BadRequest(new ProblemDetails { Title = ex.Message });
        }

    }

    #endregion

}
