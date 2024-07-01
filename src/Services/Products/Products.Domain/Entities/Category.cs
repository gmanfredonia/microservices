using System;
using System.Collections.Generic;

namespace Products.Domain.Entities;

public partial class Category
{
    public int CatId { get; set; }

    public string CatName { get; set; }

    public DateOnly CatInsertDate { get; set; }

    public DateOnly? CatLastUpdate { get; set; }

    public DateOnly CatValidFrom { get; set; }

    public bool CatEnabled { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
