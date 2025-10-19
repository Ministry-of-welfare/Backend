using System;
using System.Collections.Generic;

namespace server_pra.Models;

/// <summary>
/// טבלת הרשאות עבור תבניות (TemplatePermissions)
/// </summary>
public partial class TemplatePermission
{
    /// <summary>
    /// מזהה הרשאה
    /// </summary>
    public int PermissionId { get; set; }

    /// <summary>
    /// מזהה תבנית (קישור לטבלת Templates)
    /// </summary>
    public int TemplateId { get; set; }

    /// <summary>
    /// סוג ישות (User או Group)
    /// </summary>
    public string PrincipalType { get; set; }

    /// <summary>
    /// שם משתמש או קבוצה (user@domain.gov.il או Developers)
    /// </summary>
    public string PrincipalName { get; set; }

    /// <summary>
    /// הרשאת צפייה (1 = מותר, 0 = אסור)
    /// </summary>
    public bool CanView { get; set; }

    /// <summary>
    /// הרשאת עריכה (1 = מותר, 0 = אסור)
    /// </summary>
    public bool CanEdit { get; set; }

    /// <summary>
    /// הרשאת מחיקה (1 = מותר, 0 = אסור)
    /// </summary>
    public bool CanDelete { get; set; }

    /// <summary>
    /// הרשאת שכפול (1 = מותר, 0 = אסור)
    /// </summary>
    public bool CanDuplicate { get; set; }

    public virtual Template Template { get; set; }
}
