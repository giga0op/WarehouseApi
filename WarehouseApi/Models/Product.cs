namespace WarehouseApi.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";       // ფილა, პეტლი
    public string Category { get; set; } = "";   // ფილა, ფურნიტურა
    public string Variant { get; set; } = "";    // თეთრი 60x60123
    public string Unit { get; set; } = "ცალი";   // ცალი, კვ.მ
    public decimal Price { get; set; }           // ფასი 1 ერთეულზე
    public decimal Quantity { get; set; }        // მარაგი (საბაზო ერთეულით)
}
