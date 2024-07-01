using System;
using System.Collections.Generic;

namespace Admin.Domain.Entities;

public partial class Menu
{
    public int MnuId { get; set; }

    public int? MnuParentId { get; set; }

    public string MnuKey { get; set; }

    public string MnuDescription { get; set; }

    public string MnuImageUrl { get; set; }

    public DateOnly MnuInsertDate { get; set; }

    public DateOnly? MnuLastUpdate { get; set; }

    public DateOnly MnuValidFrom { get; set; }

    public bool MnuEnabled { get; set; }

    public virtual ICollection<ProfilesMenu> ProfilesMenus { get; set; } = new List<ProfilesMenu>();
}
