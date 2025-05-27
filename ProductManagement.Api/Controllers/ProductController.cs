using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Application;
using ProductManagement.Domain;
using System;

namespace ProductManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;
        private readonly IValidator<Product> _validator;

        public ProductController(ILogger<ProductController> logger, 
            IProductService productService,
            IValidator<Product> validator)
        {
            _logger = logger;
            _productService = productService;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IReadOnlyList<Product>> GetProducts()
        {
            return await _productService.GetProducts();
        }

        [HttpPost]
        public async Task<IActionResult> SaveProduct(Product product)
        {
            var validationResult = await _validator.ValidateAsync(product);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = await _productService.SaveProduct(product);
                
            if (result.Success)
            {
                return Ok(result.Data);
            }
            else
            {
                return Conflict(result.Message);
            }
        }
    }
}
