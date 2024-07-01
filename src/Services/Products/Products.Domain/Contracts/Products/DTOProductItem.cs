using Building.Base.DTOs;

namespace Admin.Domain.Contracts.Products;

public class DTOProductItem : DTOKeyValuePair<int>
{
    public bool Enabled { get; set; }
}