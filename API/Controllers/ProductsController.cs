//using System.Data.Entity;


using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.RequestHelpers;
using API.Services;
using AutoMapper;
using Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        #region Fields

        private readonly StoreContext _context;
        private readonly IMapper _mapper;
        private readonly ImageService _imageService;
        private readonly IStringLocalizer<Resource> _localizer;

        #endregion

        #region Ctors

        public ProductsController(StoreContext context, IMapper mapper, ImageService imageService, IStringLocalizer<Resource> localizer)
        {
            _context = context;
            _mapper = mapper;
            _imageService = imageService;
            _localizer = localizer;
        }

        #endregion

        #region Get

        [HttpGet]
        public async Task<ActionResult<PagedList<Product>>> GetProducts([FromQuery]ProductParams productParams)
        {
            var query = _context.Products.Sort(productParams.OrderBy).Search(productParams.SearchTerm)
                .Filter(productParams.Brands, productParams.Types).AsQueryable();
            //return await query.ToListAsync();

            var products =
                await PagedList<Product>.ToPagedList(query, productParams.PageNumber, productParams.PageSize);

            Response.AddPaginationHeader(products.MetaData);

            return products;
        }

        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            return product == null ? NotFound() : product;
        }

        [HttpGet("filters")]
        public async Task<IActionResult> GetFilters()
        {
            var brands = await _context.Products.Select(p => p.Brand).Distinct().ToListAsync();
            var types = await _context.Products.Select(p => p.Type).Distinct().ToListAsync();

            return Ok(new { brands, types });
        }

        #endregion

        #region Upsert

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromForm]CreateProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);

            if (productDto.File != null)
            {
                var imageResult = await _imageService.AddImageAsync(productDto.File);
                if (imageResult.Error != null)
                    return BadRequest(new ProblemDetails { Title = imageResult.Error.Message });

                product.PictureUrl = imageResult.SecureUrl.ToString();
                product.PublicId = imageResult.PublicId;

            }

            _context.Products.Add(product);
            var result = await _context.SaveChangesAsync() > 0;
            if (result) return CreatedAtRoute("GetProduct", new { Id = product.Id }, product);

            return BadRequest(new ProblemDetails { Title = _localizer["Product_ProblemCreate"] });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<Product>> UpdateProduct([FromForm]UpdateProductDto productDto)
        {
            var product = await _context.Products.FindAsync(productDto.Id);

            if (product == null) return NotFound();

            _mapper.Map(productDto, product);
            if (productDto.File != null)
            {
                var imageResult = await _imageService.AddImageAsync(productDto.File);
                if (imageResult.Error != null)
                    return BadRequest(new ProblemDetails { Title = imageResult.Error.Message });

                if (!string.IsNullOrEmpty(product.PublicId))
                    await _imageService.DeleteImageAsync(product.PublicId);

                product.PictureUrl = imageResult.SecureUrl.ToString();
                product.PublicId = imageResult.PublicId;
            }
            var result = await _context.SaveChangesAsync() > 0;
            if (result) return Ok(product);

            return BadRequest(new ProblemDetails { Title = _localizer["Product_ProblemUpdate"] });

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            //TODO:
            //log somewhere if fail
            //service to check reasons and delete from cloudinary
            if (!string.IsNullOrEmpty(product.PublicId))
                await _imageService.DeleteImageAsync(product.PublicId);

            _context.Products.Remove(product);

            var result = await _context.SaveChangesAsync() > 0;
            
            if(result) return Ok();
            return BadRequest(new ProblemDetails { Title = _localizer["Product_ProblemDelete"] });
        }

        #endregion


    }
}
