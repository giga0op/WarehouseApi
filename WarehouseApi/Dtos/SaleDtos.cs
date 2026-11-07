namespace WarehouseApi.Dtos;

public record SellRequest(int ProductId, decimal Quantity);

public record SellResult(int ProductId, decimal SoldQuantity, decimal Total, decimal Remaining);