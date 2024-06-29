using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Product
{
    public int PrdId { get; set; }

    public string PrdName { get; set; }

    public string PrdDescription { get; set; }

    public int CatId { get; set; }

    public string PrdImage { get; set; }

    public string PrdThumbnail { get; set; }

    public DateTime PrdInsertDate { get; set; }

    public DateTime? PrdLastUpdate { get; set; }

    public DateTime PrdValidFrom { get; set; }

    public bool PrdEnabled { get; set; }

    public virtual Category Cat { get; set; }

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
