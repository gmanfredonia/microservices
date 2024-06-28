namespace Domain.Contracts.Products;

public class DTOProductRow
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Dimensions { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
}