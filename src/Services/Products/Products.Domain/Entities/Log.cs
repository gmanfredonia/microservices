using System;
using System.Collections.Generic;

namespace Admin.Domain.Entities;

public partial class Log
{
    public int LogId { get; set; }

    public string LogValues { get; set; }

    public DateTime LogCreated { get; set; }
}
