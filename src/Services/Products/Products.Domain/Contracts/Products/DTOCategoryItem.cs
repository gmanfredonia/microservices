using Admin.Domain.Contracts.Base;

namespace Admin.Domain.Contracts.Products;

public class DTOCategoryItem : DTOKeyValuePair<int>
{
    public bool Enabled { get; set; }
}