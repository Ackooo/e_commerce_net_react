﻿namespace API.Controllers;

using Domain.DTOs.Product;
using Domain.Entities.Product;
using Domain.Extensions;
using Domain.Interfaces.Extensions;
using Domain.Interfaces.Services;
using Domain.RequestHelpers;
using Domain.Shared.Constants;
using Infrastructure.Authentication;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[ApiBase(Order = 1)]
public class ProductsController(IProductService productService, IImageService imageService,
    IApiLocalizer localizer) : ApiBaseController
{
    #region GET

    [HttpGet]
    [Route("", Name = "GetProducts")]
    [ProducesResponseType(typeof(PagedList<Product>), 200)]
    public async Task<ActionResult<PagedList<Product>>> GetProductsAsync([FromQuery] ProductParams productParams)
    {
        var products = await productService.GetProductsFromQueryPagedAsync(productParams);

        Response.AddPaginationHeader(products.MetaData);
        return products;
    }

    [HttpGet]
    [Route("{id}", Name = "GetProduct")]
    [ProducesResponseType(typeof(Product), 200)]
    public async Task<ActionResult<Product>> GetProductAsync(long id)
    {
        var product = await productService.GetProductAsync(id, false);
        return product == null ? NotFound() : product;
    }

    [HttpGet]
    [Route("filters", Name = "GetFilters")]
    [ProducesResponseType(typeof(ProductFiltersDto), 200)]
    public async Task<IActionResult> GetFiltersAsync()
    {
        var filters = await productService.GetProductFiltersAsync();
        return Ok(filters);
    }

    #endregion

    #region Upsert

    [HttpPost]
    [Route("", Name = "CreateProduct")]
    [Authorize(Roles = Roles.Vendor)]
    public async Task<ActionResult<Product>> CreateProductAsync([FromForm] CreateProductDto productDto)
    {
        var product = productDto.MapToProduct();

        if(productDto.File != null)
        {
            var imageResult = await imageService.AddImageAsync(productDto.File);
            if(imageResult.Error != null)
                return BadRequest(new ProblemDetails { Title = imageResult.Error.Message });

            product.PictureUrl = imageResult.SecureUrl.ToString();
            product.PublicId = imageResult.PublicId;

        }

        var result = await productService.AddProductAsync(product);
        if(result) return CreatedAtRoute("GetProduct", new { product.Id }, product);

        return BadRequest(new ProblemDetails { Title = localizer.Translate("Product_ProblemCreate") });
    }

    [HttpPut]
    [Route("", Name = "UpdateProduct")]
    [Authorize(Roles = Roles.Vendor)]
    [ProducesResponseType(typeof(Product), 200)]
    public async Task<ActionResult<Product>> UpdateProductAsync([FromForm] UpdateProductDto productDto)
    {
        var product = await productService.GetProductAsync(productDto.Id, true);
        if(product == null) return NotFound();

        productDto.MapToProduct(product);
        if(productDto.File != null)
        {
            var imageResult = await imageService.AddImageAsync(productDto.File);
            if(imageResult.Error != null)
                return BadRequest(new ProblemDetails { Title = imageResult.Error.Message });

            if(!string.IsNullOrEmpty(product.PublicId))
                await imageService.DeleteImageAsync(product.PublicId);

            product.PictureUrl = imageResult.SecureUrl.ToString();
            product.PublicId = imageResult.PublicId;
        }

        var result = await productService.UpdateProductAsync(product);
        if(result) return Ok(product);

        return BadRequest(new ProblemDetails { Title = localizer.Translate("Product_ProblemUpdate") });
    }

    [HttpDelete]
    [Route("{id}", Name = "DeleteProduct")]
    [Authorize(Roles = Roles.Vendor)]
    public async Task<ActionResult> DeleteProductAsync(long id)
    {
        var publicId = await productService.DeleteProductAsync(id);

        //TODO:
        //log somewhere if fail
        //return BadRequest(new ProblemDetails { Title = localizer["Product_ProblemDelete"] });
        //service to check reasons and delete from cloudinary
        if(!string.IsNullOrEmpty(publicId))
            await imageService.DeleteImageAsync(publicId);

        return Ok();
    }

    #endregion
}
