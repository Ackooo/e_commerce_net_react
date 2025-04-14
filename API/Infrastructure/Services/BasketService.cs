namespace Infrastructure.Services;

using Domain.DTOs.Basket;
using Domain.Entities.Basket;
using Domain.Entities.User;
using Domain.Extensions;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Services;
using Domain.Shared.Constants;
using Microsoft.AspNetCore.Http;

public class BasketService(IBasketRepository basketRepository, IProductService productService) : IBasketService
{
    public Task<Basket?> GetBasketAsync(Guid userId, bool isTracked)
    {
        if(userId == Guid.Empty) return Task.FromResult<Basket?>(null);
        return basketRepository.GetBasketAsync(userId, isTracked);
    }

    public async Task<bool> UpdateBasketAsync(Basket existingBasket)
    {
        ArgumentNullException.ThrowIfNull(existingBasket);
        return await basketRepository.UpdateBasketAsync(existingBasket);
    }

    public async Task<bool> DeleteBasketAsync(Guid id)
    {
        if(id == Guid.Empty) throw new ArgumentNullException(nameof(id));
        return await basketRepository.DeleteBasketAsync(id);
    }

    public async Task<BasketDto?> AddItemToBasket(HttpResponse response, Guid? buyerId,
    long productId, int quantity)
    {
        Basket basket;
        if(!buyerId.HasValue)
        {
            basket = await CreateAnonymousBasketAsync(response, buyerId);
        }
        else
        {
            var existingBasket = await GetBasketAsync(buyerId.Value, true);
            basket = existingBasket ?? await CreateAnonymousBasketAsync(response, buyerId);
        }

        var product = await productService.GetProductAsync(productId, true);
        if(product == null) return null;

        await basketRepository.AddItemAsync(basket, product, quantity);
        return basket.MapToBasketDto();
    }

    public async Task<bool> RemoveItemAsync(Basket existingBasket, long productId, int quantity)
    {
        ArgumentNullException.ThrowIfNull(existingBasket);
        return await basketRepository.RemoveItemAsync(existingBasket, productId, quantity);
    }

    #region Helper

    private async Task<Basket> CreateAnonymousBasketAsync(HttpResponse response, Guid? buyerId)
    {
        if(!buyerId.HasValue || buyerId == Guid.Empty)
        {
            buyerId = Guid.CreateVersion7();
            var cookieOptions = new CookieOptions
            {
                IsEssential = true,
                Expires = DateTime.Now.AddDays(30),
                HttpOnly = false
            };
            response.Cookies.Append(RequestConstants.CookiesBasketUserId, buyerId.Value.ToString(), cookieOptions);
        }

        var basket = new Basket { UserId = buyerId.Value };
        return await basketRepository.AddBasketAsync(basket);
    }

    #endregion

}