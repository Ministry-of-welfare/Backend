using System;
using System.Collections.Generic;

namespace server_pra.Models;

/// <summary>
/// טבלת רישומי Audit עבור תבניות (TemplateAuditLog)
/// </summary>
public partial class TemplateAuditLog
{
    /// <summary>
    /// מזהה רישום
    /// </summary>
    public int LogId { get; set; }

    /// <summary>
    /// מזהה תבנית (קישור ל- Templates)
    /// </summary>
    public int TemplateId { get; set; }

    /// <summary>
    /// סוג פעולה (View, Edit, Delete, Duplicate, PermissionChange)
    /// </summary>
    public string Action { get; set; }

    /// <summary>
    /// המשתמש שביצע פעולה (user@domain.gov.il)
    /// </summary>
    public string ActionBy { get; set; }

    /// <summary>
    /// תאריך ביצוע הפעולה
    /// </summary>
    public DateTime ActionAt { get; set; }

    /// <summary>
    /// ערך קודם (JSON)
    /// </summary>
    public string OldValue { get; set; }

    /// <summary>
    /// ערך חדש (JSON)
    /// </summary>
    public string NewValue { get; set; }

    public virtual Template Template { get; set; }
}
