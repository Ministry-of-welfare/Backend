using System;
using System.Collections.Generic;

namespace server_pra.Models;

/// <summary>
/// טבלת סטטוסים של תבניות (טיוטה, פעיל, לא בשימוש)
/// </summary>
public partial class TemplateStatus
{
    /// <summary>
    /// מזהה סטטוס (1, 2, 3)
    /// </summary>
    public int TemplateStatusId { get; set; }

    /// <summary>
    /// קוד סטטוס (DRAFT, ACTIVE, DEPRECATED)
    /// </summary>
    public string StatusCode { get; set; }

    /// <summary>
    /// שם סטטוס לתצוגה (טיוטה, פעיל, לא בשימוש)
    /// </summary>
    public string StatusName { get; set; }

    /// <summary>
    /// תיאור חופשי על הסטטוס
    /// </summary>
    public string Description { get; set; }

    public virtual ICollection<Template> Templates { get; set; } = new List<Template>();
}
