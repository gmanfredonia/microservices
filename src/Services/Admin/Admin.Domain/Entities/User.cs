using System;
using System.Collections.Generic;

namespace Admin.Domain.Entities;

public partial class User
{
    public int UsrId { get; set; }

    public string UsrEmail { get; set; }

    public string UsrPassword { get; set; }

    public DateOnly UsrInsertDate { get; set; }

    public DateOnly? UsrLastUpdate { get; set; }

    public DateOnly UsrValidFrom { get; set; }

    public bool UsrEnabled { get; set; }

    public string UsrDescription { get; set; }

    public virtual ICollection<ProfilesUser> ProfilesUsers { get; set; } = new List<ProfilesUser>();
}
