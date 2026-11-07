using Microsoft.AspNetCore.Mvc;
using WarehouseApi.Dtos;
using WarehouseApi.Services;

namespace WarehouseApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly ISaleService _sales;
    public ReportsController(ISaleService sales) => _sales = sales;

    // GET /api/reports/sales?from=2026-11-01&to=2026-11-30
    [HttpGet("sales")]
    public async Task<List<SalesReportItem>> Sales(DateOnly from, DateOnly to) =>
        await _sales.GetReportAsync(from, to);
}