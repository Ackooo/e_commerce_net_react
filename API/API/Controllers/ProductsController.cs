namespace API.Controllers;

using AutoMapper;

using Domain.DTOs.Product;
using Domain.Entities.Product;
using Domain.Extensions;
using Domain.Interfaces.Services;
using Domain.RequestHelpers;

using Infrastructure.Services;

using Localization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

public class ProductsController(IProductService productService, ImageService imageService,
    IMapper mapper, IStringLocalizer<Resource> localizer) : BaseController
{
    #region GET

    [HttpGet]
    [ProducesResponseType(typeof(PagedList<Product>), 200)]
    public async Task<ActionResult<PagedList<Product>>> GetProducts([FromQuery] ProductParams productParams)
    {
        var products = await productService.GetProductsFromQueryAsync(productParams);

        Response.AddPaginationHeader(products.MetaData);
        return products;
    }

    [HttpGet("{id}", Name = "GetProduct")]
    [ProducesResponseType(typeof(Product), 200)]
    public async Task<ActionResult<Product>> GetProduct(int id)
	{
        var product = await productService.GetProductAsync(id);
        return product == null ? NotFound() : product;
    }

    [HttpGet("filters")]
    [ProducesResponseType(typeof(ProductFiltersDto), 200)]
    public async Task<IActionResult> GetFilters()
    {
        var filters = await productService.GetProductFiltersAsync();
        return Ok(filters);
    }

    #endregion

    #region Upsert

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromForm] CreateProductDto productDto)
    {
        var product = mapper.Map<Product>(productDto);

        if (productDto.File != null)
        {
            var imageResult = await imageService.AddImageAsync(productDto.File);
            if (imageResult.Error != null)
                return BadRequest(new ProblemDetails { Title = imageResult.Error.Message });

            product.PictureUrl = imageResult.SecureUrl.ToString();
            product.PublicId = imageResult.PublicId;

        }

        var result = await productService.AddProductAsync(product);
        if (result) return CreatedAtRoute("GetProduct", new { product.Id }, product);

        return BadRequest(new ProblemDetails { Title = localizer["Product_ProblemCreate"] });
    }

    [Authorize(Roles = "Admin")]
    [HttpPut]
    [ProducesResponseType(typeof(Product), 200)]
    public async Task<ActionResult<Product>> UpdateProduct([FromForm] UpdateProductDto productDto)
    {
        var product = await productService.GetProductAsync(productDto.Id);
        if (product == null) return NotFound();

        mapper.Map(productDto, product);
        if (productDto.File != null)
        {
            var imageResult = await imageService.AddImageAsync(productDto.File);
            if (imageResult.Error != null)
                return BadRequest(new ProblemDetails { Title = imageResult.Error.Message });

            if (!string.IsNullOrEmpty(product.PublicId))
                await imageService.DeleteImageAsync(product.PublicId);

            product.PictureUrl = imageResult.SecureUrl.ToString();
            product.PublicId = imageResult.PublicId;
        }

        var result = await productService.UpdateProductAsync(product);
        if (result) return Ok(product);

        return BadRequest(new ProblemDetails { Title = localizer["Product_ProblemUpdate"] });
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await productService.GetProductAsync(id);
        if (product == null) return NotFound();

        //TODO:
        //log somewhere if fail
        //service to check reasons and delete from cloudinary
        if (!string.IsNullOrEmpty(product.PublicId))
            await imageService.DeleteImageAsync(product.PublicId);

        var result = await productService.DeleteProductAsync(product.Id);
        if (result) return Ok();

        return BadRequest(new ProblemDetails { Title = localizer["Product_ProblemDelete"] });
    }

    #endregion
}
