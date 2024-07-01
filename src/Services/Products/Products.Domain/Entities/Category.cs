namespace Admin.Domain.Entities;

public partial class Category
{
    public int CatId { get; set; }

    public string CatName { get; set; }

    public DateTime CatInsertDate { get; set; }

    public DateTime? CatLastUpdate { get; set; }

    public DateTime CatValidFrom { get; set; }

    public bool CatEnabled { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
