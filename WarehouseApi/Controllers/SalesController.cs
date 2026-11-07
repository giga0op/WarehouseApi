using Microsoft.AspNetCore.Mvc;
using WarehouseApi.Dtos;
using WarehouseApi.Services;

namespace WarehouseApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly ISaleService _sales;
    public SalesController(ISaleService sales) => _sales = sales;

    [HttpPost]
    public async Task<SellResult> Sell(SellRequest req) => await _sales.SellAsync(req);
}