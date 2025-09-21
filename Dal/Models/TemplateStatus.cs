using System;
using System.Collections.Generic;

namespace Dal.Models;

public partial class TemplateStatus
{
    public int TemplateStatusId { get; set; }

    public string StatusCode { get; set; }

    public string StatusName { get; set; }

    public string Description { get; set; }

    public virtual ICollection<Template> Templates { get; set; } = new List<Template>();
}
