using System;
using System.Collections.Generic;

namespace Admin.Domain.Entities;

public partial class Role
{
    public int RolId { get; set; }

    public string RolDescription { get; set; }

    public string RolCategory { get; set; }

    public DateOnly RolInsertDate { get; set; }

    public DateOnly? RolLastUpdate { get; set; }

    public DateOnly RolValidFrom { get; set; }

    public bool RolEnabled { get; set; }

    public virtual ICollection<ProfilesMenu> ProfilesMenus { get; set; } = new List<ProfilesMenu>();

    public virtual ICollection<ProfilesUser> ProfilesUsers { get; set; } = new List<ProfilesUser>();
}
