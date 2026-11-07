using Microsoft.EntityFrameworkCore;
using WarehouseApi.Data;
using WarehouseApi.Dtos;
using WarehouseApi.Models;

namespace WarehouseApi.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly WarehouseDbContext _db;
    public SaleRepository(WarehouseDbContext db) => _db = db;

    public async Task AddAsync(Sale sale) =>
        await _db.Sales.AddAsync(sale);

    public async Task<List<SalesReportItem>> GetReportAsync(DateTime fromUtc, DateTime toUtcExclusive)
    {
        // ბაზაში მხოლოდ დაჯგუფება და ჯამები
        var rows = await _db.Sales
            .Where(s => s.SoldAt >= fromUtc && s.SoldAt < toUtcExclusive)
            .GroupBy(s => new { s.Product!.Name, s.Product.Variant, s.Product.Unit })
            .Select(g => new
            {
                g.Key.Name,
                g.Key.Variant,
                g.Key.Unit,
                Sold = g.Sum(x => x.Quantity),
                Revenue = g.Sum(x => x.Quantity * x.UnitPrice)
            })
            .ToListAsync();

        // დალაგება და DTO-ში გადაქცევა უკვე კოდში
        return rows
            .OrderBy(r => r.Name).ThenBy(r => r.Variant)
            .Select(r => new SalesReportItem(r.Name, r.Variant, r.Unit, r.Sold, r.Revenue))
            .ToList();
    }
}