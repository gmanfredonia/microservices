using System;
using System.Collections.Generic;

namespace Admin.Domain.Entities;

public partial class Rating
{
    public int RatId { get; set; }

    public int UsrId { get; set; }

    public int PrdId { get; set; }

    public int StoId { get; set; }

    public string RatDescription { get; set; }

    public int RatRating { get; set; }

    public decimal? RatPrice { get; set; }

    public DateTime RatInsertDate { get; set; }

    public DateTime? RatLastUpdate { get; set; }

    public virtual Product Prd { get; set; }

    public virtual Store Sto { get; set; }

    public virtual User Usr { get; set; }
}
