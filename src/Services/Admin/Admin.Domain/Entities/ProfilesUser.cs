using System;
using System.Collections.Generic;

namespace Admin.Domain.Entities;

public partial class ProfilesUser
{
    public int PruId { get; set; }

    public int RolId { get; set; }

    public int UsrId { get; set; }

    public DateOnly PruInsertDate { get; set; }

    public bool PruEnabled { get; set; }

    public virtual Role Rol { get; set; }

    public virtual User Usr { get; set; }
}
