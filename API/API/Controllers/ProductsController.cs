﻿namespace API.Controllers;

using API.Controllers.Common;
using API.Middleware;

using AutoMapper;

using Domain.DTOs.Product;
using Domain.Entities.Product;
using Domain.Extensions;
using Domain.Interfaces.Services;
using Domain.RequestHelpers;

using Localization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

[ApiController]
[Route("api/[controller]")]
//[HasPermission(Permissions.ProductAccess)]
[ApiBase(Order = 1)]
public class ProductsController(IProductService productService, IImageService imageService,
	IMapper mapper, IStringLocalizer<Resource> localizer) : ApiBaseController
{
	#region GET

	[HttpGet]
	[Route("GetProducts", Name = "GetProducts")]
	[ProducesResponseType(typeof(PagedList<Product>), 200)]
	public async Task<ActionResult<PagedList<Product>>> GetProductsAsync([FromQuery] ProductParams productParams)
	{
		var products = await productService.GetProductsFromQueryAsync(productParams);

		Response.AddPaginationHeader(products.MetaData);
		return products;
	}

	[HttpGet]
	[Route("{id}", Name = "GetProduct")]
	[ProducesResponseType(typeof(Product), 200)]
	public async Task<ActionResult<Product>> GetProductAsync(long id)
	{
		var product = await productService.GetProductAsync(id);
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
	[Route("CreateProduct", Name = "CreateProduct")]
	[Authorize(Roles = "Admin")]
	public async Task<ActionResult<Product>> CreateProductAsync([FromForm] CreateProductDto productDto)
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

	[HttpPut]
	[Route("UpdateProduct", Name = "UpdateProduct")]
	[Authorize(Roles = "Admin")]
	[ProducesResponseType(typeof(Product), 200)]
	public async Task<ActionResult<Product>> UpdateProductAsync([FromForm] UpdateProductDto productDto)
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

	[HttpDelete]
	[Route("{id}", Name = "DeleteProduct")]
	[Authorize(Roles = "Admin")]
	public async Task<ActionResult> DeleteProductAsync(long id)
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
