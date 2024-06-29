using Domain.Contracts.Base;

namespace Domain.Contracts.Products;

public class DTOCategoryItem : DTOKeyValuePair<int>
{
    public bool Enabled { get; set; }
}