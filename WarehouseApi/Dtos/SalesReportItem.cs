namespace WarehouseApi.Dtos;

public record SalesReportItem(
    string Name,
    string Variant,
    string Unit,
    decimal SoldQuantity,
    decimal Revenue
    );