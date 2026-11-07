using WarehouseApi.Dtos;
using WarehouseApi.Models;
using WarehouseApi.Repositories;

namespace WarehouseApi.Services;

public interface ISaleService
{
    Task<SellResult> SellAsync(SellRequest req);
    Task<List<SalesReportItem>> GetReportAsync(DateOnly from, DateOnly to);
}

public class SaleService : ISaleService
{
    private readonly IProductRepository _products;
    private readonly ISaleRepository _sales;

    public SaleService(IProductRepository products, ISaleRepository sales)
    {
        _products = products;
        _sales = sales;
    }

    public async Task<SellResult> SellAsync(SellRequest req)
    {
        if (req.Quantity <= 0)
            throw new BusinessRuleException("რაოდენობა უნდა იყოს 0-ზე მეტი");

        var product = await _products.GetByIdAsync(req.ProductId)
            ?? throw new NotFoundException("პროდუქტი ვერ მოიძებნა");

        // ყოველთვის საბაზო ერთეულში (ცალი / კვ.მ) ვითვლით
        var amount = req.Quantity;

        if (product.Quantity < amount)
            throw new BusinessRuleException(
                $"საწყობში მხოლოდ {product.Quantity} {product.Unit} არის");

        product.Quantity -= amount;

        await _sales.AddAsync(new Sale
        {
            ProductId = product.Id,
            Quantity = amount,
            UnitPrice = product.Price,
            SoldAt = DateTime.UtcNow
        });

        // ერთი შენახვა: მარაგის შემცირება და გაყიდვის ჩანაწერი ერთად
        await _products.SaveChangesAsync();

        return new SellResult(product.Id, amount, amount * product.Price, product.Quantity);
    }

    public Task<List<SalesReportItem>> GetReportAsync(DateOnly from, DateOnly to)
    {
        if (to < from)
            throw new BusinessRuleException("\"მდე\" თარიღი არ შეიძლება იყოს \"დან\" თარიღზე ადრე");

        // მომხმარებელი თარიღს თბილისის დროით ირჩევს, ბაზაში კი UTC ინახება
        var tz = GetTbilisiZone();
        var fromUtc = TimeZoneInfo.ConvertTimeToUtc(from.ToDateTime(TimeOnly.MinValue), tz);
        var toUtcExclusive = TimeZoneInfo.ConvertTimeToUtc(to.AddDays(1).ToDateTime(TimeOnly.MinValue), tz);

        return _sales.GetReportAsync(fromUtc, toUtcExclusive);
    }

    private static TimeZoneInfo GetTbilisiZone()
    {
        try { return TimeZoneInfo.FindSystemTimeZoneById("Asia/Tbilisi"); }
        catch (TimeZoneNotFoundException) { return TimeZoneInfo.FindSystemTimeZoneById("Georgian Standard Time"); }
    }
}