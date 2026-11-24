namespace WarehouseApi.Dtos;

public record ProductDto(
    int Id, string Name, string Category, string Variant,
    string Unit, decimal Price, decimal Quantity);

public record CreateProductRequest(
    string Name, string Category, string Variant,
    string Unit, decimal Price, decimal Quantity);

// Quantity: რამდენი მოვიდა; NewPrice: ახალი ფასი ერთეულზე (არასავალდებულო)
public record RestockRequest(decimal Quantity, decimal? NewPrice = null);

public record UpdateProductRequest(
    string Name, string Category, string Variant,
    string Unit, decimal Price, decimal Quantity);