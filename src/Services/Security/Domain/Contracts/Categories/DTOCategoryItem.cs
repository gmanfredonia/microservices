using Domain.Contracts.Base;

namespace Domain.Contracts.Categories;

public class DTOCategoryRow
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Dimensions { get; set; }
    public decimal Price { get; set; }
    
    public bool Enabled { get; set; }
}