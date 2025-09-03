using System;
using System.Collections.Generic;

namespace server_pra.Dal.Models;

public partial class Environment
{
    public int EnvironmentId { get; set; }

    public string EnvironmentCode { get; set; }

    public string EnvironmentName { get; set; }

    public string Description { get; set; }
}
