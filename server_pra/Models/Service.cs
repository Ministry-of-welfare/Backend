using System;
using System.Collections.Generic;

namespace server_pra.Models;

/// <summary>
/// טבלת השירותים (כגון PDF, MAIL)
/// </summary>
public partial class Service
{
    /// <summary>
    /// מזהה שירות (1, 2)
    /// </summary>
    public int ServiceId { get; set; }

    /// <summary>
    /// שם השירות (PDF, MAIL)
    /// </summary>
    public string ServiceName { get; set; }

    /// <summary>
    /// תיאור השירות
    /// </summary>
    public string Description { get; set; }

    public virtual ServiceBucket ServiceBucket { get; set; }

    public virtual ICollection<Template> Templates { get; set; } = new List<Template>();
}
