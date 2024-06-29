using Domain.Contracts.Base;

namespace Domain.Contracts.Products;

public class DTOProductItem : DTOKeyValuePair<int>
{
    public bool Enabled { get; set; }
}