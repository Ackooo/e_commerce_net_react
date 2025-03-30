namespace API.Controllers;

using Domain.DTOs.Order;
using Domain.Interfaces.Services;
using Domain.Shared.Enums;

using Infrastructure.Authentication;

using Localization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController(IOrderService orderService, IStringLocalizer<Resource> localizer) : ControllerBase
{
	#region GET

	[HttpGet]
	[Route("GetOrders", Name = "GetOrders")]
	[ProducesResponseType(typeof(List<OrderDto>), 200)]
	public async Task<ActionResult<List<OrderDto>>> GetOrdersAsync()
	{
		return await orderService.GetByBuyerIdAsync(User.Identity.Name);
	}

	[HttpGet]
	[Route("{id}", Name = "GetOrder")]
	[ProducesResponseType(typeof(OrderDto), 200)]

	public async Task<ActionResult<OrderDto>> GetOrderAsync(Guid id)
	{
		return await orderService.GetByIdAsync(id, User.Identity.Name);
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
