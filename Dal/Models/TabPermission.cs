using System;
using System.Collections.Generic;

namespace server_pra.Dal.Models.ScaffoldEntities;

/// <summary>
/// רשימת ההרשאות במערכת (כגון צפייה, עריכה, אישור)
/// </summary>
public partial class TabPermission
{
    /// <summary>
    /// מזהה ייחודי להרשאה
    /// </summary>
    public int PermissionId { get; set; }

    /// <summary>
    /// שם ההרשאה (כגון ViewFiles, ApproveImport)
    /// </summary>
    public string PermissionName { get; set; }

    /// <summary>
    /// תיאור קצר של ההרשאה
    /// </summary>
    public string PermissionDesc { get; set; }

    /// <summary>
    /// שם המודול במערכת שבו ההרשאה רלוונטית
    /// </summary>
    public string ModuleName { get; set; }

    public virtual ICollection<TabRolePermission> TabRolePermissions { get; set; } = new List<TabRolePermission>();
}
