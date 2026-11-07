using WarehouseApi.Dtos;
using WarehouseApi.Models;

namespace WarehouseApi.Repositories;

public interface ISaleRepository
{
    Task AddAsync(Sale sale);
    Task<List<SalesReportItem>> GetReportAsync(DateTime fromUtc, DateTime toUtcExclusive);
}