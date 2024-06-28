using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class ProfilesMenu
{
    public int PrmId { get; set; }

    public int RolId { get; set; }

    public int MnuId { get; set; }

    public DateTime PrmInsertDate { get; set; }

    public bool PrmEnabled { get; set; }

    public virtual Menu Mnu { get; set; }

    public virtual Role Rol { get; set; }
}
