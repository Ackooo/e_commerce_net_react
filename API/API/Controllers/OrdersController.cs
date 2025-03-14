namespace API.Controllers;

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
    public async Task<ActionResult<List<OrderDto>>> GetOrders()
    {
		return await orderService.GetByBuyerIdAsync(User.Identity.Name);
    }

    [HttpGet("{id}", Name = "GetOrder")]
    public async Task<ActionResult<OrderDto>> GetOrder(int id)
    {
        return await orderService.GetByIdAsync(id, User.Identity.Name);
    }

    #endregion

    #region POST

    [HttpPost]
    public async Task<ActionResult<int>> CreateOrder(CreateOrderDto orderDto)
    {
        try
        {
            var result = await orderService.CreateOrder(User.Identity.Name, orderDto);
            if (result != 0) 
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
