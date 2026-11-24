using WarehouseApi.Dtos;
using WarehouseApi.Models;
using WarehouseApi.Repositories;

namespace WarehouseApi.Services;

public interface IProductService
{
    Task<List<ProductDto>> GetAllAsync();
    Task<ProductDto> GetAsync(int id);
    Task<ProductDto> CreateAsync(CreateProductRequest req);
    Task<ProductDto> RestockAsync(int id, RestockRequest req);
    Task<ProductDto> UpdateAsync(int id, UpdateProductRequest req);
}

public class ProductService : IProductService
{
    private readonly IProductRepository _products;
    public ProductService(IProductRepository products) => _products = products;

    public async Task<List<ProductDto>> GetAllAsync() =>
        (await _products.GetAllAsync()).Select(ToDto).ToList();

    public async Task<ProductDto> GetAsync(int id) =>
        ToDto(await GetOrThrowAsync(id));

    public async Task<ProductDto> CreateAsync(CreateProductRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name))
            throw new BusinessRuleException("სახელი სავალდებულოა");
        if (req.Price < 0 || req.Quantity < 0)
            throw new BusinessRuleException("ფასი და რაოდენობა არ შეიძლება იყოს უარყოფითი");

        var product = new Product
        {
            Name = req.Name.Trim(),
            Category = req.Category.Trim(),
            Variant = req.Variant.Trim(),
            Unit = string.IsNullOrWhiteSpace(req.Unit) ? "ცალი" : req.Unit.Trim(),
            Price = req.Price,
            Quantity = req.Quantity
        };

        await _products.AddAsync(product);
        await _products.SaveChangesAsync();
        return ToDto(product);
    }

    public async Task<ProductDto> RestockAsync(int id, RestockRequest req)
    {
        if (req.Quantity <= 0)
            throw new BusinessRuleException("რაოდენობა უნდა იყოს 0-ზე მეტი");
        if (req.NewPrice is < 0)
            throw new BusinessRuleException("ფასი არ შეიძლება იყოს უარყოფითი");

        var product = await GetOrThrowAsync(id);

        product.Quantity += req.Quantity;

        if (req.NewPrice.HasValue)
            product.Price = req.NewPrice.Value;

        await _products.SaveChangesAsync();
        return ToDto(product);
    }

    private async Task<Product> GetOrThrowAsync(int id) =>
        await _products.GetByIdAsync(id)
        ?? throw new NotFoundException("პროდუქტი ვერ მოიძებნა");

    private static ProductDto ToDto(Product p) =>
        new(p.Id, p.Name, p.Category, p.Variant, p.Unit, p.Price, p.Quantity);
    public async Task<ProductDto> UpdateAsync(int id, UpdateProductRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name))
            throw new BusinessRuleException("სახელი სავალდებულოა");
        if (req.Price < 0 || req.Quantity < 0)
            throw new BusinessRuleException("ფასი და რაოდენობა არ შეიძლება იყოს უარყოფითი");

        var product = await GetOrThrowAsync(id);

        product.Name = req.Name.Trim();
        product.Category = req.Category.Trim();
        product.Variant = req.Variant.Trim();
        product.Unit = string.IsNullOrWhiteSpace(req.Unit) ? "ცალი" : req.Unit.Trim();
        product.Price = req.Price;
        product.Quantity = req.Quantity;

        // თუ ასეთი სახელი+ვარიანტი უკვე არსებობს, ბაზა დააბრუნებს შეცდომას (409)
        await _products.SaveChangesAsync();
        return ToDto(product);
    }
}



