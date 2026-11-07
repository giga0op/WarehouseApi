using Microsoft.AspNetCore.Mvc;
using WarehouseApi.Dtos;
using WarehouseApi.Services;

namespace WarehouseApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _products;
    public ProductsController(IProductService products) => _products = products;

    [HttpGet]
    public async Task<List<ProductDto>> GetAll() => await _products.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ProductDto> Get(int id) => await _products.GetAsync(id);

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create(CreateProductRequest req)
    {
        var created = await _products.CreateAsync(req);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPost("{id}/restock")]
    public async Task<ProductDto> Restock(int id, RestockRequest req) =>
        await _products.RestockAsync(id, req);
}