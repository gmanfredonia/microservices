using System;
using System.Collections.Generic;

namespace Products.Domain.Entities;

public partial class Product
{
    public int PrdId { get; set; }

    public string PrdName { get; set; }

    public string PrdDescription { get; set; }

    public double? PrdHeight { get; set; }

    public double? PrdWidth { get; set; }

    public double? PrdDepth { get; set; }

    public decimal PrdPrice { get; set; }

    public bool PrdEnabled { get; set; }

    public string PrdUseType { get; set; }

    public DateTime PrdInsertDate { get; set; }

    public DateTime? PrdLastUpdate { get; set; }

    public DateTime PrdValidFrom { get; set; }

    public int CatId { get; set; }

    public string PrdImage { get; set; }

    public string PrdThumbnail { get; set; }

    public virtual Category Cat { get; set; }
}
