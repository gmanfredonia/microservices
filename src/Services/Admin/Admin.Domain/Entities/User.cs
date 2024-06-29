using System;
using System.Collections.Generic;

namespace Admin.Domain.Entities;

public partial class User
{
    public int UsrId { get; set; }

    public string UsrEmail { get; set; }

    public string UsrPassword { get; set; }

    public string UsrDescription { get; set; }

    public DateTime UsrInsertDate { get; set; }

    public DateTime? UsrLastUpdate { get; set; }

    public DateTime UsrValidFrom { get; set; }

    public bool UsrEnabled { get; set; }

    public virtual ICollection<ProfilesUser> ProfilesUsers { get; set; } = new List<ProfilesUser>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
