using WarehouseApi.Models;

namespace WarehouseApi.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task AddAsync(Product product);
    Task SaveChangesAsync();
}