
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly StoreContext _context;
        private readonly IStringLocalizer<Resource> _localizer;

        public BasketController(StoreContext context, IStringLocalizer<Resource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        [HttpGet(Name = "GetBasket")]
        public async Task<ActionResult<BasketDto>> GetBasket()
        {
            var basket = await RetrieveBasket(GetBuyerId());

            if (basket == null) return NotFound();
            return basket.MapBasketToDto();
        }

        

        [HttpPost] // api/basket?productId=3&quantity=2
        public async Task<ActionResult<BasketDto>> AddItemToBasket(int productId, int quantity)
        {
            var basket = await RetrieveBasket(GetBuyerId());
            if (basket == null) basket = CreateBasket();
            var product = await _context.Products.FindAsync(productId);
            //handled in client
            //if (product == null) return NotFound();
            if (product == null) return BadRequest(new ProblemDetails{Title = _localizer["Product_NotFound"] });
            basket.AddItem(product, quantity);

            var result = await _context.SaveChangesAsync() > 0;
            if (result) return CreatedAtRoute("GetBasket", basket.MapBasketToDto());
            return BadRequest(new ProblemDetails { Title = _localizer["Basket_ProblemSave"] });
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveBasketItem(int productId, int quantity)
        {
            var basket = await RetrieveBasket(GetBuyerId());
            
            if(basket == null) return NotFound();

            basket.RemoveItem(productId, quantity);

            var result = await _context.SaveChangesAsync() >0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = _localizer["Basket_ProblemRemove"] });
        }

        private async Task<Basket> RetrieveBasket(string buyerId)
        {
            if (string.IsNullOrEmpty(buyerId))
            {
                Response.Cookies.Delete("buyerId");
                return null;
            }

            var basket = await _context.Baskets
                .Include(i => i.Items)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(x => x.BuyerId == buyerId);
            return basket;
        }

        private string GetBuyerId()
        {
            return User.Identity?.Name ?? Request.Cookies["buyerId"];
        }

        private Basket CreateBasket()
        {
            var buyerId = User.Identity?.Name;
            if (string.IsNullOrEmpty(buyerId))
            {
                buyerId = Guid.NewGuid().ToString();
                var cookieOptions = new CookieOptions
                    { IsEssential = true, Expires = DateTime.Now.AddDays(30), HttpOnly = false };
                Response.Cookies.Append("buyerId", buyerId, cookieOptions);
            }
            
            var basket = new Basket { BuyerId = buyerId };
            _context.Baskets.Add(basket);//start to track
            return basket;

        }

    }
}

