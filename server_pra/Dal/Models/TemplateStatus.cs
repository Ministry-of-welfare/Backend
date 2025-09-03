using System;
using System.Collections.Generic;

namespace server_pra.Dal.Models;

public partial class TemplateStatus
{
    public int TemplateStatusId { get; set; }

    public string StatusCode { get; set; }

    public string StatusName { get; set; }

    public string Description { get; set; }
}
