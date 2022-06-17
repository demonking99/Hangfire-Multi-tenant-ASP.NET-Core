using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Multitenant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var productDetails = await _service.GetByIdAsync(id);
            return Ok(productDetails);
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var productDetails = await _service.GetAllAsync();
            return Ok(productDetails);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(ProductsDto request)
        {
            return Ok(await _service.CreateAsync(request.Name, request.Description, request.Rate));
        }
        [HttpPost("CreateWithBackgroundJob")]
        public IActionResult CreateWithBackgroundJob(List<ProductsDto> request)
        {
            return Ok(_service.CreateWithBackgroundJob(request));
        }
    }
    
}