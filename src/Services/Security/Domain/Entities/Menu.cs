using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Menu
{
    public int MnuId { get; set; }

    public string MnuHierarchyKey { get; set; }

    public string MnuKey { get; set; }

    public string MnuCategory { get; set; }

    public string MnuDescription { get; set; }

    public string MnuImageUrl { get; set; }

    public DateTime MnuInsertDate { get; set; }

    public DateTime? MnuLastUpdate { get; set; }

    public DateTime MnuValidFrom { get; set; }

    public bool MnuEnabled { get; set; }

    public virtual ICollection<ProfilesMenu> ProfilesMenus { get; set; } = new List<ProfilesMenu>();
}
