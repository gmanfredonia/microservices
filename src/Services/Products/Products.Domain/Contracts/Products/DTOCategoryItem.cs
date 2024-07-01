using Building.Base.DTOs;

namespace Admin.Domain.Contracts.Products;

public class DTOCategoryItem : DTOKeyValuePair<int>
{
    public bool Enabled { get; set; }
}