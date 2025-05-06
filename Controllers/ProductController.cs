using e_commerce_backend.DTO;
using e_commerce_backend.Services;
using e_commerce_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetById(Guid productId)
        {
            var product = await _productService.GetProductById(productId);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddOrUpdate([FromBody] PostProduct product)
        {
            if (product == null)
            {
                return BadRequest("Product cannot be null");
            }
            var result = await _productService.AddOrUpdateProduct(product);
            if (result.Status)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
