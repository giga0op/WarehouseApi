using Microsoft.EntityFrameworkCore;
using WarehouseApi.Data;
using WarehouseApi.Models;

namespace WarehouseApi.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly WarehouseDbContext _db;
    public ProductRepository(WarehouseDbContext db) => _db = db;

    public Task<List<Product>> GetAllAsync() =>
        _db.Products.OrderBy(p => p.Name).ThenBy(p => p.Variant).ToListAsync();

    public Task<Product?> GetByIdAsync(int id) =>
        _db.Products.FirstOrDefaultAsync(p => p.Id == id);

    public async Task AddAsync(Product product) =>
        await _db.Products.AddAsync(product);

    // ინახავს ყველა მოლოდინში მყოფ ცვლილებას ერთად (ერთი ტრანზაქცია)
    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}