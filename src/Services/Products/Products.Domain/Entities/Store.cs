using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Store
{
    public int StoId { get; set; }

    public string StoDescription { get; set; }

    public string StoAddress { get; set; }

    public string StoMobile { get; set; }

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
